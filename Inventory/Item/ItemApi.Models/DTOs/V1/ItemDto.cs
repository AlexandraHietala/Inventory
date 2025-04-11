using System.ComponentModel.DataAnnotations;
using ItemApi.Models.System;

namespace ItemApi.Models.DTOs.V1
{
    public class ItemDto
    {
        [Required]
        public required int ID { get; set; }

        [Required]
        public required string STATUS { get; set; } 

        [Required]
        public required string TYPE { get; set; }

        public string? BRAND { get; set; }

        public string? SERIES { get; set; }

        public string? NAME { get; set; }

        public string? DESCRIPTION { get; set; }

        [Required]
        public required string FORMAT { get; set; }

        [Required]
        public required string SIZE { get; set; }

        public int? YEAR { get; set; }

        public string? PHOTO { get; set; }

        [Required]
        public required string CREATED_BY { get; set; }

        [Required]
        public required DateTime CREATED_DATE { get; set; }

        public string? LAST_MODIFIED_BY { get; set; }

        public DateTime? LAST_MODIFIED_DATE { get; set; }
    }
}
