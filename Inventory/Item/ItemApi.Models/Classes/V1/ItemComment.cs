using System.ComponentModel.DataAnnotations;
using ItemApi.Models.System;

namespace ItemApi.Models.Classes.V1
{
    public class ItemComment : Base
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required int ItemId { get; set; }

        [Required]
        public required string Comment { get; set; }
    }
}
