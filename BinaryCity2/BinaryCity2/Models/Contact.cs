using System.ComponentModel.DataAnnotations;

namespace BinaryCity2.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
