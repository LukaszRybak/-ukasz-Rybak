using System.ComponentModel.DataAnnotations;

namespace PrescriptionHandler.Models.DTO.Responses
{
    public class DoctorRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email adress  is required")]
        [EmailAddress(ErrorMessage = "Invalid email adress")]
        public string Email { get; set; }

        public DoctorRequestDto(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
