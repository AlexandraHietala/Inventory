using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.DTOs.V1
{
    public class RoleDto
    {
        [Required]
        public required int ROLE_ID { get; set; }

        public string? ROLE_DESCRIPTION { get; set; }
    }
}
