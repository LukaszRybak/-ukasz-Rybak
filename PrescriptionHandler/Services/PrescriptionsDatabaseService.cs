using Microsoft.EntityFrameworkCore;
using PrescriptionHandler.Models;
using PrescriptionHandler.Models.DTO.Responses;

namespace PrescriptionHandler.Services
{
    public class PrescriptionsDatabaseService : IDatabaseService
    {
        private readonly PrescriptionsDbContext _context;

        public PrescriptionsDatabaseService(PrescriptionsDbContext context)
        {
            _context = context;
        }

        public async Task<DatabaseSingleObjectResponseDto> GetDoctorsAsync()
        {
            var doctors = await _context.Doctors.Select(d => new
            {
                idDoctor = d.IdDoctor,
                firstName = d.FirstName,
                lastName = d.LastName,
                email = d.Email,
            }).ToListAsync();

            return new DatabaseSingleObjectResponseDto(200, "", doctors);
        }

        public async Task<DatabaseSingleObjectResponseDto> AddDoctorAsync(DoctorRequestDto newDoctor)
        {

            var doctor = new Doctor
            {
                FirstName = newDoctor.FirstName,
                LastName = newDoctor.LastName,
                Email = newDoctor.Email
            };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return new DatabaseSingleObjectResponseDto(200, "New doctor added successfully", null);
        }

        public async Task<DatabaseSingleObjectResponseDto> UpdateDoctorAsync(int idDoctor, DoctorRequestDto updatedDoctor)
        {
            var existingDoctor = await _context.Doctors.FindAsync(idDoctor);
            if (existingDoctor == null)
            {
                return new DatabaseSingleObjectResponseDto(404, "Doctor not found", null);
            }
            existingDoctor.FirstName = updatedDoctor.FirstName;
            existingDoctor.LastName = updatedDoctor.LastName;
            existingDoctor.Email = updatedDoctor.Email;

            await _context.SaveChangesAsync();

            return new DatabaseSingleObjectResponseDto(200, "Doctor updated successfully", null);
        }
        public async Task<DatabaseSingleObjectResponseDto> DeleteDoctorAsync(int idDoctor)
        {
            var existingDoctor = await _context.Doctors.Where(d => idDoctor == d.IdDoctor).SingleOrDefaultAsync();
            if (existingDoctor == null)
            {
                return new DatabaseSingleObjectResponseDto(404, "Doctor not found", null);
            }

            _context.Doctors.Remove(existingDoctor);
            await _context.SaveChangesAsync();

            return new DatabaseSingleObjectResponseDto(200, "Doctor deleted successfully", null);
        }

        public async Task<DatabaseSingleObjectResponseDto> GetPrescriptionAsync(int idPrescription)
        {
            var prescription = await _context.Prescriptions
                                .Where(p => idPrescription == p.IdPrescription)
                                .Include(p => p.Patient)
                                .Include(p => p.Doctor)
                                .Include(p => p.PrescriptionMedicaments)
                                .FirstOrDefaultAsync();

            if (prescription == null)
            {
                return new DatabaseSingleObjectResponseDto(404, "Prescription not found", null);
            }

            var medicaments = await _context.Medicaments
                                .Include(m => m.PrescriptionMedicaments)
                                .ToListAsync();

            var output = new
            {
                idPrescription = prescription.IdPrescription,
                date = prescription.Date,
                dueDate = prescription.DueDate,
                doctor = new
                {
                    idDoctor = prescription.Doctor.IdDoctor,
                    firstName = prescription.Doctor.FirstName,
                    lastName = prescription.Doctor.LastName,
                    email = prescription.Doctor.Email
                },
                patient = new
                {
                    idPatient = prescription.Patient.IdPatient,
                    firstName = prescription.Patient.FirstName,
                    lastName = prescription.Patient.LastName,
                    birthdate = prescription.Patient.Birthdate
                },
                Medicines = prescription.PrescriptionMedicaments
                    .Select(pm => medicaments.First(m => m.IdMedicament == pm.IdMedicament))
                    .Select(m => new
                    {
                        idMedicament = m.IdMedicament,
                        name = m.Name,
                        description = m.Description,
                        type = m.Type
                    })
            };

            return new DatabaseSingleObjectResponseDto(200, "", output);
        }
    }

}
