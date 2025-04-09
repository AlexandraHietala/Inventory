using System.ComponentModel.DataAnnotations;
using CollectionApi.Models.System;

namespace CollectionApi.Models.DTOs.V1
{
    public class CollectionDto
    {
        [Required]
        public required int COLLECTION_ID { get; set; }

        [Required]
        public required string COLLECTION_NAME { get; set; }

        public string? COLLECTION_DESCRIPTION { get; set; }

        [Required]
        public required string COLLECTION_CREATED_BY { get; set; }

        [Required]
        public required DateTime COLLECTION_CREATED_DATE { get; set; }

        public string? COLLECTION_LAST_MODIFIED_BY { get; set; }

        public DateTime? COLLECTION_LAST_MODIFIED_DATE { get; set; }
    }
}
