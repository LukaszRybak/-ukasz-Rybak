using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PrescriptionHandler.Models.DTO.Responses;
using PrescriptionHandler.Models;
using PrescriptionHandler.Services;

namespace PrescriptionHandler.Controllers
{
    [Route("api/prescriptions")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {

        private IDatabaseService _databaseService;

        public PrescriptionsController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [Route("/api/prescriptions/{idPrescription}")]
        [HttpGet]
        public async Task<IActionResult> GetPrescriptionAsync(int idPrescription)
        {
            var result = await _databaseService.GetPrescriptionAsync(idPrescription);
            return Ok(result.Output);
        }


        [Route("/api/doctors")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync()
        {
            var result = await _databaseService.GetDoctorsAsync();
            return Ok(result.Output);
        }

        [Route("/api/doctors")]
        [HttpPost]
        public async Task<IActionResult> AddDoctorAsync([FromBody] DoctorRequestDto newDoctor)
        {
            DatabaseSingleObjectResponseDto result;
            try
            {
                result = await _databaseService.AddDoctorAsync(newDoctor);
            }
            catch (SqlException exc)
            {
                return StatusCode(500, exc.Message);
            }

            return StatusCode(result.StatusCode, result.Message);

        }

        [HttpPut("/api/doctors/{idDoctor}")]
        public async Task<IActionResult> UpdateDoctorAsync(int idDoctor, [FromBody] DoctorRequestDto updatedDoctor)
        {

            DatabaseSingleObjectResponseDto result;
            try
            {
                result = await _databaseService.UpdateDoctorAsync(idDoctor, updatedDoctor);
            }
            catch (SqlException exc)
            {
                return StatusCode(500, exc.Message);
            }

            return StatusCode(result.StatusCode, result.Message);

        }

        [HttpDelete("/api/doctors/{idDoctor}")]
        public async Task<IActionResult> DeleteDoctorAsync(int idDoctor)
        {

            DatabaseSingleObjectResponseDto result;
            try
            {
                result = await _databaseService.DeleteDoctorAsync(idDoctor);
            }
            catch (SqlException exc)
            {
                return StatusCode(500, exc.Message);
            }

            return StatusCode(result.StatusCode, result.Message);

        }
        

    }
}
