using Microsoft.EntityFrameworkCore;
using Receptor.Models;

namespace Receptor.Data;

public class RecepetorDBContext : DbContext
{
   
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    
    //pamietac o tym bo inaczej nie bedzie dzialac
    public RecepetorDBContext(DbContextOptions<RecepetorDBContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().ToTable("Patients");
        modelBuilder.Entity<Doctor>().ToTable("Doctors");
        modelBuilder.Entity<Medicament>().ToTable("Medicaments");
        modelBuilder.Entity<Prescription>().ToTable("Prescriptions");
        modelBuilder.Entity<PrescriptionMedicament>().ToTable("PrescriptionMedicaments");

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pres => new { pres.IdPrescription, pres.IdMedicament });
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pres => pres.Prescription)
            .WithMany(pres => pres.PrescriptionMedicaments)
            .HasForeignKey(pres => pres.IdPrescription);
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(med => med.Medicament)  
            .WithMany(med => med.PrescriptionMedicaments)
            .HasForeignKey(med => med.IdMedicament);

            
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor);
            
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(p => p.Prescriptions)
            .HasForeignKey(p => p.IdPatient);
            
        // Dodanie przykładowych danych
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@example.com" },
            new Doctor { IdDoctor = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@example.com" }
        );
        
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Apap", Description = "Lek przeciwbólowy", Type = "Tabletki" },
            new Medicament { IdMedicament = 2, Name = "Ibuprom", Description = "Lek przeciwzapalny", Type = "Tabletki" },
            new Medicament { IdMedicament = 3, Name = "Rutinoscorbin", Description = "Lek na odporność", Type = "Tabletki" }
        );
    }
}