using System.Collections.Generic;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories
{
    public interface IRepository<T> where T : class
    {
        #region [ Async ]

        Task<T> GetSingleAsync(IQuery<T> query);
        Task<IEnumerable<T>> ListAllAsync();
        Task<IEnumerable<T>> ListAsync(IQuery<T> query);
        Task<Listing<T>> ListPagingAsync(IQuery<T> query, int pageNumber, int itemsPerPage);
        Task<int> CountAsync(IQuery<T> query);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(IQuery<T> query);

        #endregion

        #region [ Sync ]

        T GetSingle(IQuery<T> query);
        IEnumerable<T> ListAll();
        IEnumerable<T> List(IQuery<T> query);
        Listing<T> ListPaging(IQuery<T> query, int pageNumber, int itemsPerPage);
        int Count(IQuery<T> query);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);
        bool Exists(IQuery<T> query);

        #endregion
    }
}
