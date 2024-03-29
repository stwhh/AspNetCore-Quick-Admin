﻿using System.ComponentModel.DataAnnotations;

namespace QuickAdmin.Model.Entities
{
    public class User : FullAudited
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]

        public string EnUserName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Phone { get; set; } = string.Empty;

    }
}
