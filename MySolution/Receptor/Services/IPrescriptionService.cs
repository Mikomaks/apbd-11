using Receptor.Models;
using Receptor.Models.DTOs;

namespace Receptor.Services;

public interface IPrescriptionService
{
    Task AddPrescription(PrescriptionAddDto prescription);
    Task<PatientPrescriptionsDTO> GetPrescriptionsFromID(int id);
}