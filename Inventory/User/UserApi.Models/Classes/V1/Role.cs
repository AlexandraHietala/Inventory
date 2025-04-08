using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Classes.V1
{
    public class Role
    {
        [Required]
        public required int Id { get; set; }

        public string? Description { get; set; }
    }
}
