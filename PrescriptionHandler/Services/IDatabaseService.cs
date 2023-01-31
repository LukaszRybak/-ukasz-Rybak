using PrescriptionHandler.Models;
using PrescriptionHandler.Models.DTO.Responses;

namespace PrescriptionHandler.Services
{
    public interface IDatabaseService
    {
        Task<DatabaseSingleObjectResponseDto> GetPrescriptionAsync(int idPrescription);
        Task<DatabaseSingleObjectResponseDto> GetDoctorsAsync();
        Task<DatabaseSingleObjectResponseDto> AddDoctorAsync(DoctorRequestDto newDoctor);
        Task<DatabaseSingleObjectResponseDto> UpdateDoctorAsync(int idDoctor, DoctorRequestDto updatedDoctor);
        Task<DatabaseSingleObjectResponseDto> DeleteDoctorAsync(int idDoctor);
    }
}
