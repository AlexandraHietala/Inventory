using System.ComponentModel.DataAnnotations;
using CollectionApi.Models.System;

namespace CollectionApi.Models.Classes.V1
{
    public class Collection : Base
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string CollectionName { get; set; }

        public string? Description { get; set; }

    }
}
