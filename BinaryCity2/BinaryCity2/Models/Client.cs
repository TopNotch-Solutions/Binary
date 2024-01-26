using System.ComponentModel.DataAnnotations;

namespace BinaryCity2.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientCode { get; set; }
    }
}
