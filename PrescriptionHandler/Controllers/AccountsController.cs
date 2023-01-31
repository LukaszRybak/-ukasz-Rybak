using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrescriptionHandler.Models;
using PrescriptionHandler.Models.Authentication;
using PrescriptionHandler.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrescriptionHandler.Controllers
{
    [Authorize]
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly PrescriptionsDbContext _context;

        public AccountsController(IConfiguration configuration, PrescriptionsDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAccount()
        {

            string g = null;
            g.ToLower();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            AppUser user = _context.Users.Where(u => u.Login == loginRequest.Login).FirstOrDefault();

            string passwordHash = user.Password;
            string curHashedPassword = "";

            //Validating password
            //#####

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = Convert.FromBase64String(user.Salt);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            //Password based key derivation function
            string currentHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginRequest.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            //#####

            //Here we check the hash
            if (passwordHash != currentHashedPassword)
            {
                return Unauthorized();
            }


            Claim[] userclaim = new[] {
                    new Claim(ClaimTypes.Name, "s15339"),
                    new Claim(ClaimTypes.Role, "user"),
                    new Claim(ClaimTypes.Role, "admin")
                    //Add additional data here
                };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:7231",
                audience: "https://localhost:7231",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            string GenerateRefreshToken()
            {
                var randomNumber = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                    return Convert.ToBase64String(randomNumber);
                }
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
            _context.SaveChanges();

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = "refresh_token"
            });
        }


    }
}
