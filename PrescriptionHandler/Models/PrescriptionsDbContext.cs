using Microsoft.EntityFrameworkCore;
using PrescriptionHandler.Models.Authentication;

namespace PrescriptionHandler.Models
{
    public class PrescriptionsDbContext : DbContext
    {

        private IConfiguration _configuration;
        public PrescriptionsDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public PrescriptionsDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }

        //>to do: test if its still required
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ProductionDb"));
        }

        //<
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AppUser>(opt =>
            {
                opt.HasKey(u => u.IdUser);
                opt.Property(u => u.IdUser).ValueGeneratedOnAdd();

                opt.Property(u => u.Login).IsRequired().HasMaxLength(100);
                opt.Property(u => u.Email).IsRequired().HasMaxLength(100);
                opt.Property(u => u.Password).IsRequired().HasMaxLength(100);
                opt.Property(u => u.Salt).IsRequired().HasMaxLength(100);
                opt.Property(u => u.RefreshToken).IsRequired().HasMaxLength(255);
                opt.Property(u => u.RefreshTokenExp).HasColumnType("datetime");


                opt.HasData(
                        new AppUser {
                            IdUser = 1,
                            Login = "JonDoe",
                            Email = "Jon@wp.pl",
                            Password = "ToBeHashed",
                            Salt = "generatedSalt",
                            RefreshToken = "generatedRefreshToken",
                            RefreshTokenExp = new DateTime(2000, 4, 20)
                        }
                    );

            });







            modelBuilder.Entity<Doctor>(opt =>
            {
                opt.HasKey(e => e.IdDoctor);
                opt.Property(e => e.IdDoctor).ValueGeneratedOnAdd();

                opt.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                opt.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                opt.Property(e => e.Email).IsRequired().HasMaxLength(100);

                opt.HasData(
                        new Doctor { IdDoctor = 1, FirstName = "Jon", LastName = "Doe", Email = "Jon@wp.pl" },
                        new Doctor { IdDoctor = 2, FirstName = "Doe", LastName = "Jon", Email = "Doe@wp.pl" }
                    );

            });

            modelBuilder.Entity<Patient>(opt =>
            {
                opt.HasKey(e => e.IdPatient);
                opt.Property(e => e.IdPatient).ValueGeneratedOnAdd();

                opt.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                opt.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                opt.Property(e => e.Birthdate).IsRequired().HasColumnType("datetime");

                opt.HasData(
                        new Patient { IdPatient = 1, FirstName = "Jon", LastName = "Doe", Birthdate = new DateTime(2000, 4, 20) },
                        new Patient { IdPatient = 2, FirstName = "Doe", LastName = "Jon", Birthdate = new DateTime(2000, 4, 20) }
                    );

            });

            modelBuilder.Entity<Prescription>(opt =>
            {
                opt.HasKey(e => e.IdPrescription);
                opt.Property(e => e.IdPrescription).ValueGeneratedOnAdd();

                opt.Property(e => e.Date).IsRequired().HasColumnType("datetime");
                opt.Property(e => e.DueDate).IsRequired().HasColumnType("datetime");

                opt.HasOne(e => e.Doctor)
                   .WithMany(p => p.Prescriptions)
                   .HasForeignKey(p => p.IdDoctor);

                opt.HasOne(e => e.Patient)
                   .WithMany(p => p.Prescriptions)
                   .HasForeignKey(p => p.IdPatient);

                opt.HasData(
                        new Prescription { IdPrescription = 1, Date = new DateTime(2021, 4, 20), DueDate = new DateTime(2021, 4, 21), IdDoctor = 1, IdPatient = 2 },
                        new Prescription { IdPrescription = 2, Date = new DateTime(2021, 4, 20), DueDate = new DateTime(2021, 4, 21), IdDoctor = 2, IdPatient = 1 }
                    );

            });

            modelBuilder.Entity<PrescriptionMedicament>(opt =>
            {
                opt.HasKey(e => new { e.IdMedicament, e.IdPrescription });

                opt.Property(e => e.Dose);
                opt.Property(e => e.Details).IsRequired().HasMaxLength(100);

                opt.HasOne(e => e.Medicament)
                   .WithMany(pm => pm.PrescriptionMedicaments)
                   .HasForeignKey(pm => pm.IdMedicament);

                opt.HasOne(e => e.Prescription)
                   .WithMany(pm => pm.PrescriptionMedicaments)
                   .HasForeignKey(pm => pm.IdPrescription);

                opt.HasData(
                        new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 1, Dose = 2, Details = "Nie popijać herbatą" },
                        new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 2, Details = "Nie popijać herbatą" },
                        new PrescriptionMedicament { IdPrescription = 2, IdMedicament = 1, Dose = 2, Details = "Nie popijać herbatą" },
                        new PrescriptionMedicament { IdPrescription = 2, IdMedicament = 2, Details = "Nie popijać herbatą" },
                        new PrescriptionMedicament { IdPrescription = 2, IdMedicament = 3, Details = "Nie popijać herbatą" }
                    ); ;

            });

            modelBuilder.Entity<Medicament>(opt =>
            {
                opt.HasKey(e => e.IdMedicament);

                opt.Property(e => e.Name).IsRequired().HasMaxLength(100);
                opt.Property(e => e.Description).IsRequired().HasMaxLength(100);
                opt.Property(e => e.Type).IsRequired().HasMaxLength(100);

                opt.HasData(
                        new Medicament { IdMedicament = 1, Name = "Apap", Description = "W tabletkach", Type = "Painkiller" },
                        new Medicament { IdMedicament = 2, Name = "Rosół", Description = "Taki dobry", Type = "Zupa" },
                        new Medicament { IdMedicament = 3, Name = "Panadol", Description = "Dla dzieci", Type = "Syrop" }
                    ); ;

            });

        }
    }
}
