using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entities.Interface
{
    public interface IDeleteAudited
    {
        bool IsDeleted { get; set; }

        string DeleteUserId { get; set; }

        DateTime DeleteTime { get; set; }
    }
}
