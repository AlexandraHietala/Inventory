using System.ComponentModel.DataAnnotations;
using UserApi.Models.System;

namespace UserApi.Models.Classes.V1
{
    public class User : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string PassSalt { get; set; }

        [Required]
        public required string PassHash { get; set; }

        public int? RoleId { get; set; }
    }
}
