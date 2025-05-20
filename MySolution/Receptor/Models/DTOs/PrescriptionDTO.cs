namespace Receptor.Models.DTOs;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    
    public List<MedicamentDto> Medicaments { get; set; }
    
    public DoctorDTO Doctor { get; set; }
}