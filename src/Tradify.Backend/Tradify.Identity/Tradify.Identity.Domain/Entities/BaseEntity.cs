﻿using System.ComponentModel.DataAnnotations;

namespace Tradify.Identity.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
