using System;

namespace QuickAdmin.Model.Entities.Interface
{
    public interface ICreateAudited
    {
        string CreateUserId { get; set; }

        DateTime CreateTime { get; set; }
    }
}
