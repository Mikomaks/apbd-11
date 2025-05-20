using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Receptor.Data;
using Receptor.Models;
using Receptor.Models.DTOs;

namespace Receptor.Services;

public class PrescriptionService : IPrescriptionService
{
    
    private readonly RecepetorDBContext _context;

    public PrescriptionService(RecepetorDBContext context)
    {
        _context = context;
    }


    public async Task AddPrescription(PrescriptionAddDto prescription)
    {

        if (prescription.Medicaments.Count > 10)
        {
            throw new Exception("Za duzo lekow w recepcie");
        }
        if (prescription.DueDate < prescription.Date)
        {
            throw new Exception("Recepta jest wygasla");
        }

        var doctor = await _context.Doctors.FindAsync(prescription.IdDoctor);
        if (doctor == null)
        {
            throw new Exception("Doktor nie istnieje");
        }
        
        var pacjent = await _context.Patients.FirstOrDefaultAsync(
            p => p.FirstName == prescription.Patient.FirstName &&
                 p.LastName == prescription.Patient.LastName &&
                 p.Birthdate == prescription.Patient.Birthdate);

        if (pacjent == null)
        {
            pacjent = new Patient
            {
                FirstName = prescription.Patient.FirstName,
                LastName = prescription.Patient.LastName,
                Birthdate = prescription.Patient.Birthdate
            };
            
            _context.Patients.Add(pacjent);
            await _context.SaveChangesAsync();
        }

        var idLekow = prescription.Medicaments.Select(m => m.IdMedicament).ToList();

        var istniejace_leki = await _context.Medicaments.Where(med => idLekow.Contains(med.IdMedicament))
            .Select(med => med.IdMedicament).ToListAsync();

        var brakujaceLeki = idLekow.Except(istniejace_leki).ToList();

        if (brakujaceLeki.Any())
        {
            throw new Exception("Uwaga brakuje lekow w systemie");
        }


        var recepta = new Prescription
        {
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            IdDoctor = prescription.IdDoctor,
            IdPatient = pacjent.IdPatient,
            PrescriptionMedicaments = prescription.Medicaments.Select(
                med => new PrescriptionMedicament
                {
                    IdMedicament = med.IdMedicament,
                    Dose = med.Dose,
                    Description = med.Description
                }).ToList()
        };
        
        _context.Prescriptions.Add(recepta);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientPrescriptionsDTO> GetPrescriptionsFromID(int id)
    {
        var patient = await _context.Patients
            .Include(pat => pat.Prescriptions)
            .ThenInclude(doc => doc.Doctor)
            .Include(prep => prep.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(med => med.Medicament)
            .FirstOrDefaultAsync(pat => pat.IdPatient == id);

        if (patient == null)
        {
            return null;
        }

        return new PatientPrescriptionsDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Prescriptions = patient.Prescriptions.OrderBy(pr => pr.DueDate)
                .Select(pr => new PrescriptionDTO
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = pr.IdDoctor,
                        FirstName = pr.Doctor.FirstName
                    },
                    Medicaments = pr.PrescriptionMedicaments.Select(med => new MedicamentDto
                    {
                        IdMedicament = med.IdMedicament,
                        Dose = med.Dose,
                        Description = med.Description
                    }).ToList()
                }).ToList(),
        };


    }
}