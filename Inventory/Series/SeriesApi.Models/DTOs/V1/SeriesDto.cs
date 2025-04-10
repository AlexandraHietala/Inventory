using System.ComponentModel.DataAnnotations;
using SeriesApi.Models.System;

namespace SeriesApi.Models.DTOs.V1
{
    public class SeriesDto
    {
        [Required]
        public required int SERIES_ID { get; set; }

        [Required]
        public required string SERIES_NAME { get; set; }

        public string? SERIES_DESCRIPTION { get; set; }

        [Required]
        public required string SERIES_CREATED_BY { get; set; }

        [Required]
        public required DateTime SERIES_CREATED_DATE { get; set; }

        public string? SERIES_LAST_MODIFIED_BY { get; set; }

        public DateTime? SERIES_LAST_MODIFIED_DATE { get; set; }
    }
}
