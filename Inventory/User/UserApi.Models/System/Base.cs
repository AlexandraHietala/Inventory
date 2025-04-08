using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.System
{
    public class Base
    {
        [Required]
        public required string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
