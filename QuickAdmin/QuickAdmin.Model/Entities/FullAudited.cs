using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Model.Entities.Interface;

namespace Model.Entities
{
    public class FullAudited : IFullAudited
    {
        [Required]
        [StringLength(100)]
        public string CreateUserId { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string LastModifyUserId { get; set; } = string.Empty;

        public DateTime LastModifyTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(100)]
        public string DeleteUserId { get; set; } = string.Empty;

        public DateTime DeleteTime { get; set; } = new DateTime(1970, 1, 1);
    }
}
