using System.ComponentModel.DataAnnotations;
using BrandApi.Models.System;

namespace BrandApi.Models.Classes.V1
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
