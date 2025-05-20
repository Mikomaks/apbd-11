using System.ComponentModel.DataAnnotations;

namespace Receptor.Models
{
    public class Doctor
    {
        [Key]
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; }
    }
}