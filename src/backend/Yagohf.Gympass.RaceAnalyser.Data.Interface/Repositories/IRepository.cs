using System.Collections.Generic;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories
{
    public interface IRepository<T> where T : class
    {
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
    }
}
