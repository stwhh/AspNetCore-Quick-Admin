using System;

namespace QuickAdmin.Model.Entities.Interface
{
    public interface IDeleteAudited
    {
        bool IsDeleted { get; set; }

        string DeleteUserId { get; set; }

        DateTime DeleteTime { get; set; }
    }
}
