using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entities.Interface
{
    public interface IModifyAudited
    {
        string LastModifyUserId { get; set; }

        DateTime LastModifyTime { get; set; }
       
    }
}
