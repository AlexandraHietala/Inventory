using System.ComponentModel.DataAnnotations;

namespace BrandApi.Models.System
{
    public class Error
    {
        [Required]
        public required int Code { get; set; }

        [Required]
        public required string Message { get; set; }

    }
}
