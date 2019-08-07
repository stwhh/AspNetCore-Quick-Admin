using System;
using System.Collections.Generic;
using System.Text;

namespace DAO.Repository
{
    /// <summary>
    /// 自定义仓储接口，有需要新的方法这里定义，QuickAdminRepository里面实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuickAdminRepository<T> : IRepositoryBase<T> where T : class
    {
    }
}
