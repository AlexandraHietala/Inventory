using System.ComponentModel.DataAnnotations;
using ItemApi.Models.System;

namespace ItemApi.Models.DTOs.V1
{
    public class BrandDto
    {
        [Required]
        public required int BRAND_ID { get; set; }

        [Required]
        public required string BRAND_NAME { get; set; }

        public string? BRAND_DESCRIPTION { get; set; }

        [Required]
        public required string BRAND_CREATED_BY { get; set; }

        [Required]
        public required DateTime BRAND_CREATED_DATE { get; set; }

        public string? BRAND_LAST_MODIFIED_BY { get; set; }

        public DateTime? BRAND_LAST_MODIFIED_DATE { get; set; }
    }
}
