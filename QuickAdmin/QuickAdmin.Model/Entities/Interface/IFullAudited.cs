using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entities.Interface
{
    public interface IFullAudited : ICreateAudited, IModifyAudited, IDeleteAudited
    {

    }
}
