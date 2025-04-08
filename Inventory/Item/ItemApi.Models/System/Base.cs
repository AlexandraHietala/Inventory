using System.ComponentModel.DataAnnotations;

namespace ItemApi.Models.System
{
    public class Base
    {
        [Required]
        public required string CreatedBy { get; set; }

        [Required]
        public required DateTime CreatedDate { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
