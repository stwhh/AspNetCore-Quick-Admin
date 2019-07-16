using System;
using System.Collections.Generic;
using System.Text;
using Model.Entities.Interface;

namespace Model.Entities
{
    public class FullAudited : IFullAudited
    {
        public string CreateUserId { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string LastModifyUserId { get; set; }

        public DateTime LastModifyTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }

        public string DeleteUserId { get; set; }

        public DateTime DeleteTime { get; set; } = new DateTime(1970, 1, 1);
    }
}
