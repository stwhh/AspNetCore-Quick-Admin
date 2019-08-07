using System;
using System.Collections.Generic;
using System.Text;

namespace DAO.Repository
{
    /// <summary>
    /// 自定义仓储接口，有需要新的方法在里面实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuickAdminRepository<T> : RepositoryBase<T>,  IQuickAdminRepository<T> where T : class
    {
        private readonly QuickAdminDbContext _dbContext;

       
        public QuickAdminRepository(QuickAdminDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
