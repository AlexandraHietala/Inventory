using System.ComponentModel.DataAnnotations;
using ItemApi.Models.System;

namespace ItemApi.Models.Classes.V1
{
    public class ItemExpanded : Base
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string Status { get; set; }

        [Required]
        public required string Type { get; set; }

        public Brand? Brand { get; set; }

        public Series? Series { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public required string Format { get; set; }

        [Required]
        public required string Size { get; set; }

        public int? Year { get; set; }

        public string? Photo { get; set; }
    }
}
