using System.ComponentModel.DataAnnotations;
namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [StringLength(125)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Department { get; set; }
        [Range(15000, 60000)]
        public decimal Salary { get; set; }
        [Required]
        [Range(18,60)]
        public string Age { get; set; }
    }
}
