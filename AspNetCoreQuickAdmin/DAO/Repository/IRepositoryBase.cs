using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增之后的数据</returns>
        bool Add(T entity);

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增之后的数据</returns>
        Task<bool> AddAsync(T entity);

        /// <summary>
        /// 新增List
        /// </summary>
        /// <param name="entityList">数据实体集合</param>
        /// <returns>是否新增成功</returns>
        bool Add(IEnumerable<T> entityList);

        /// <summary>
        /// 异步新增List
        /// </summary>
        /// <param name="entityList">数据实体集合</param>
        /// <returns>是否新增成功</returns>
        Task<bool> AddAsync(IEnumerable<T> entityList);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Delete(T entity);

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(T entity);


        /// <summary>
        /// 删除list
        /// </summary>
        /// <param name="entityList">数据实体list</param>
        bool Delete(IEnumerable<T> entityList);

        /// <summary>
        /// 异步删除list
        /// </summary>
        /// <param name="entityList">数据实体list</param>
        Task<bool> DeleteAsync(IEnumerable<T> entityList);

        /// <summary>
        /// 删除实体(根据条件删除)
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>是否成功</returns>
        bool Delete(Expression<Func<T, bool>> wherePredicate);

        /// <summary>
        /// 异步删除实体(根据条件删除)
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> wherePredicate);


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Update(T entity);

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entityList">数据实体</param>
        /// <returns>是否成功</returns>
        bool Update(IEnumerable<T> entityList);

        /// <summary>
        /// 异步批量更新数据
        /// </summary>
        /// <param name="entityList">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateAsync(IEnumerable<T> entityList);


        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>实体</returns>
        T GetEntity(Expression<Func<T, bool>> wherePredicate);

        /// <summary>
        /// 异步查询一条数据
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>实体</returns>
        Task<T> GetEntityAsync(Expression<Func<T, bool>> wherePredicate);

        /// <summary>
        /// 查找多条数据
        /// </summary>
        /// <param name="wherePredicate">wherePredicate</param>
        /// <param name="isTrack">是否追踪,if true,实体状态会在ef中缓存</param>
        /// <returns>实体 IQueryable</returns>
        IQueryable<T> GetList(Expression<Func<T, bool>> wherePredicate, bool isTrack = true);

        /// <summary>
        /// 异步查找多条数据
        /// </summary>
        /// <param name="wherePredicate">wherePredicate</param>
        /// <param name="isTrack">是否追踪,if true,实体状态会在ef中缓存</param>
        /// <returns>实体 IQueryable</returns>
        Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> wherePredicate, bool isTrack = true);

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
        IQueryable<T> GetPageList<TSort>(Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, TSort>> orderExpression, bool isAsc,
            int pageIndex, int pageSize, out int totalRecord);

        /// <summary>
        /// 查找分页数据列表【支持多级排序】
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderBy">排序字段,必须加上排序方式。【支持多级排序 比如 'ID DESC,CreateTime ASC'】</param>
        /// <returns>分页的IQueryable 数据</returns>
        IQueryable<T> GetPageList(Expression<Func<T, bool>> whereExpression,
            string orderBy, int pageIndex, int pageSize, out int totalRecord);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>布尔值</returns>
        bool IsExist(Expression<Func<T, bool>> wherePredicate);

        /// <summary>
        /// 异步是否存在
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>布尔值</returns>
        Task<bool> IsExistAsync(Expression<Func<T, bool>> wherePredicate);

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>记录数</returns>
        int Count(Expression<Func<T, bool>> wherePredicate);

        /// <summary>
        /// 异步查询记录数
        /// </summary>
        /// <param name="wherePredicate">条件表达式</param>
        /// <returns>记录数</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> wherePredicate);
    }
}
