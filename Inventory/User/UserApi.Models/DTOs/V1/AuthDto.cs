using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.DTOs.V1
{
    public class AuthDto
    {
        [Required]
        public required string PASS_SALT { get; set; }

        [Required]
        public required string PASS_HASH { get; set; }

        public int? ROLE_ID { get; set; }
    }
}
