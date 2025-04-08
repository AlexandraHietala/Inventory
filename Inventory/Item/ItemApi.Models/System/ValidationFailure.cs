using System.ComponentModel.DataAnnotations;

namespace ItemApi.Models.System
{
    public class ValidationFailure
    {
        [Required]
        public required int Code { get; set; }

        [Required]
        public required string Message { get; set; }

    }
}
