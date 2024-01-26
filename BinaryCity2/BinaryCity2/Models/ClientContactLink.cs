using System.ComponentModel.DataAnnotations;

namespace BinaryCity2.Models
{
    public class ClientContactLink
    {
        [Key]
        public int ClientContactLinkId { get; set; }
        public int ClientId { get; set; }
        public int ContactId { get; set; }
    }
}
