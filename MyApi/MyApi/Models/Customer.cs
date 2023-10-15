using System.ComponentModel.DataAnnotations;

namespace MyApi.Models
{
    public class Customer
    {
        [key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Display]
        public bool IsActive {  get; set; }
    }
}
