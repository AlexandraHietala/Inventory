using System.ComponentModel.DataAnnotations;
using ItemApi.Models.System;

namespace ItemApi.Models.Classes.V1
{
    public class Series : Base
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string SeriesName { get; set; }

        public string? Description { get; set; }
    }
}
