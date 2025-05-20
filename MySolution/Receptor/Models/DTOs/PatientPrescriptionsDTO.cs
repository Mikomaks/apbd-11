namespace Receptor.Models.DTOs;

public class PatientPrescriptionsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
    
    public List<PrescriptionDTO> Prescriptions { get; set; }
}