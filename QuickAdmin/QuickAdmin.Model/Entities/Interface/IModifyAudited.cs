using System;

namespace QuickAdmin.Model.Entities.Interface
{
    public interface IModifyAudited
    {
        string LastModifyUserId { get; set; }

        DateTime LastModifyTime { get; set; }
       
    }
}
