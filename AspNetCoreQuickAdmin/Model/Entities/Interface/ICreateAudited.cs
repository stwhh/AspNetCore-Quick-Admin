using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Entities.Interface
{
    public interface ICreateAudited
    {
        string CreateUserId { get; set; }

        DateTime CreateTime { get; set; }
    }
}
