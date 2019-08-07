using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core; //可实现动态查询：https://github.com/StefH/System.Linq.Dynamic.Core 
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAO.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly QuickAdminDbContext _dbContext;

        public RepositoryBase(QuickAdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增之后的数据</returns>
        public virtual bool Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增之后的数据</returns>
        public virtual async Task<bool> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 新增List
        /// </summary>
        /// <param name="entityList">数据实体集合</param>
        /// <returns>是否新增成功</returns>
        public virtual bool Add(IEnumerable<T> entityList)
        {
            _dbContext.Set<T>().AddRange(entityList);
            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 异步新增List
        /// </summary>
        /// <param name="entityList">数据实体集合</param>
        /// <returns>是否新增成功</returns>
        public virtual async Task<bool> AddAsync(IEnumerable<T> entityList)
        {
            await _dbContext.Set<T>().AddRangeAsync(entityList);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion


        #region 删除

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public virtual bool Delete(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Deleted;

            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public virtual async Task<bool> DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Deleted;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除list
        /// </summary>
        /// <param name="entityList">数据实体list</param>
        /// <returns>是否成功</returns>
        public virtual bool Delete(IEnumerable<T> entityList)
        {
            foreach (var item in entityList)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Deleted;
            }

            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除list
        /// </summary>
        /// <param name="entityList">数据实体list</param>
        /// <returns>是否成功</returns>
        public virtual async Task<bool> DeleteAsync(IEnumerable<T> entityList)
        {
            foreach (var item in entityList)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Deleted;
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>是否成功</returns>
        public virtual bool Delete(Expression<Func<T, bool>> wherePredicate)
        {
            var list = GetList(wherePredicate);
            if (!list.Any())
            {
                return false;
            }

            foreach (var item in list)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Deleted;
            }

            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>是否成功</returns>
        public virtual async Task<bool> DeleteAsync(Expression<Func<T, bool>> wherePredicate)
        {
            var list = GetList(wherePredicate);
            if (!list.Any())
            {
                return false;
            }

            foreach (var item in list)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Deleted;
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion


        #region 修改

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public virtual bool Update(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entityList">数据实体</param>
        /// <returns>是否成功</returns>
        public virtual bool Update(IEnumerable<T> entityList)
        {
            if (!entityList.Any())
            {
                return false;
            }

            foreach (var item in entityList)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Modified;
            }

            return _dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 异步批量更新数据
        /// </summary>
        /// <param name="entityList">数据实体</param>
        /// <returns>是否成功</returns>
        public virtual async Task<bool> UpdateAsync(IEnumerable<T> entityList)
        {
            if (!entityList.Any())
            {
                return false;
            }

            foreach (var item in entityList)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Modified;
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion


        #region 查询

        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>实体</returns>
        public virtual T GetEntity(Expression<Func<T, bool>> wherePredicate)
        {
            var entity = _dbContext.Set<T>().FirstOrDefault(wherePredicate);
            return entity;
        }

        /// <summary>
        /// 异步查询一条数据
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>实体</returns>
        public virtual async Task<T> GetEntityAsync(Expression<Func<T, bool>> wherePredicate)
        {
            var entity = _dbContext.Set<T>().FirstOrDefaultAsync(wherePredicate);
            return await entity;
        }

        /// <summary>
        /// 查找多条数据
        /// </summary>
        /// <param name="wherePredicate">wherePredicate</param>
        /// <param name="isTrack">是否追踪,if true,实体状态会在ef中缓存</param>
        /// <returns>实体 IQueryable</returns>
        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> wherePredicate, bool isTrack = true)
        {
            return isTrack
                ? _dbContext.Set<T>().Where<T>(wherePredicate)
                : _dbContext.Set<T>().AsNoTracking().Where<T>(wherePredicate);
        }

        /// <summary>
        /// 异步查找多条数据
        /// </summary>
        /// <param name="wherePredicate">wherePredicate</param>
        /// <param name="isTrack">是否追踪,if true,实体状态会在ef中缓存</param>
        /// <returns>实体 IQueryable</returns>
        public virtual async Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> wherePredicate,
            bool isTrack = true)
        {
            return isTrack
                ? await Task.FromResult(_dbContext.Set<T>().Where<T>(wherePredicate))
                : await Task.FromResult(_dbContext.Set<T>().AsNoTracking().Where<T>(wherePredicate));
        }

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <typeparam name="TSort">排序</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <returns>分页的IQueryable 数据</returns>
        public virtual IQueryable<T> GetPageList<TSort>(Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, TSort>> orderExpression,
            bool isAsc, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = GetList(whereExpression);
            totalRecord = list.Count();
            list = isAsc
                ? list.OrderBy<T, TSort>(orderExpression).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize)
                : list.OrderByDescending<T, TSort>(orderExpression).Skip<T>((pageIndex - 1) * pageSize)
                    .Take<T>(pageSize);
            return list;
        }

        /// <summary>
        /// 查找分页数据列表【支持多级排序】
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderBy">排序字段,必须加上排序方式。【支持多级排序 比如 'ID DESC,CreateTime ASC'】</param>
        /// <returns>分页的IQueryable 数据</returns>
        public virtual IQueryable<T> GetPageList(Expression<Func<T, bool>> whereExpression,
            string orderBy, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = GetList(whereExpression);
            totalRecord = list.Count();
            list = list.OrderBy(orderBy) //System.Linq.Dynamic.Core 命名空间下的; //需要通过nuget安装
                .Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize)
                .Take<T>(pageSize);
            return list;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>布尔值</returns>
        public virtual bool IsExist(Expression<Func<T, bool>> wherePredicate)
        {
            return _dbContext.Set<T>().AsNoTracking().Any(wherePredicate);
        }

        /// <summary>
        /// 异步是否存在
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>布尔值</returns>
        public virtual async Task<bool> IsExistAsync(Expression<Func<T, bool>> wherePredicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().AnyAsync(wherePredicate);
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>记录数</returns>
        public virtual int Count(Expression<Func<T, bool>> wherePredicate)
        {
            return _dbContext.Set<T>().AsNoTracking().Count(wherePredicate);
        }

        /// <summary>
        /// 异步查询记录数
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>记录数</returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> wherePredicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().CountAsync(wherePredicate);
        }

        #endregion
    }
}
