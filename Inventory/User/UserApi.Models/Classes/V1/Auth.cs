using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Classes.V1
{
    public class Auth
    {
        [Required]
        public required string PassSalt { get; set; }

        [Required]
        public required string PassHash { get; set; }

        public int? RoleId { get; set; }
    }
}
