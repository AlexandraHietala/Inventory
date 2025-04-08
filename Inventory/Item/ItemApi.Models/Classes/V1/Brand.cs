using System.ComponentModel.DataAnnotations;
using ItemApi.Models.System;

namespace ItemApi.Models.Classes.V1
{
    public class Brand : Base
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string BrandName { get; set; }

        public string? Description { get; set; }

    }
}
