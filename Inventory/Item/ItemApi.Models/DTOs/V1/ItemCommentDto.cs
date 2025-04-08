using System.ComponentModel.DataAnnotations;
using ItemApi.Models.System;

namespace ItemApi.Models.DTOs.V1
{
    public class ItemCommentDto
    {
        [Required]
        public required int COMMENT_ID { get; set; }

        [Required]
        public required int ITEM_ID { get; set; }

        [Required]
        public required string COMMENT { get; set; }

        [Required]
        public required string COMMENT_CREATED_BY { get; set; }

        [Required]
        public required DateTime COMMENT_CREATED_DATE { get; set; }

        public string? COMMENT_LAST_MODIFIED_BY { get; set; }

        public DateTime? COMMENT_LAST_MODIFIED_DATE { get; set; }
    }
}
