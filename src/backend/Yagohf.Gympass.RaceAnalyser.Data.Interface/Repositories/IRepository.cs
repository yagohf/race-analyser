using System.Collections.Generic;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> SelecionarUnicoAsync(IQuery<T> query);
        Task<IEnumerable<T>> ListarTodosAsync();
        Task<IEnumerable<T>> ListarAsync(IQuery<T> query);
        Task<Listing<T>> ListarPaginandoAsync(IQuery<T> query, int pagina, int qtdRegistrosPorPagina);
        Task<int> ContarAsync(IQuery<T> query);
        Task InserirAsync(T entidade);
        Task AtualizarAsync(T entidade);
        Task ExcluirAsync(int id);
        Task ExcluirAsync(T entidade);
        Task<bool> ExisteAsync(IQuery<T> query);
    }
}
