using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Context;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : EntityBase
    {
        protected RaceAnalyserContext _context;

        public RepositoryBase(RaceAnalyserContext context)
        {
            this._context = context;
        }

        public async Task UpdateAsync(T entity)
        {
            this._context.Entry<T>(entity).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
            this._context.Entry<T>(entity).State = EntityState.Detached;
        }

        public async Task<int> CountAsync(IQuery<T> query)
        {
            var preparedQuery = this.PrepareQuery(query);
            return await preparedQuery.CountAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await this._context.Set<T>().AddAsync(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await this._context.Set<T>().FindAsync(id);
            this._context.Set<T>().Remove(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            await this.DeleteAsync(entity.Id);
        }

        public async Task<IEnumerable<T>> ListAsync(IQuery<T> query)
        {
            return await this.PrepareQuery(query).ToListAsync();
        }

        public async Task<Listing<T>> ListPagingAsync(IQuery<T> query, int pageNumber, int itemsPerPage)
        {
            var totalItems = this.CountAsync(query);
            var items = this.PrepareQuery(query, pageNumber, itemsPerPage).ToListAsync();

            return new Listing<T>(await items, new Paging(pageNumber, await totalItems, itemsPerPage));
        }

        private IQueryable<T> PrepareQuery(IQuery<T> query)
        {
            IQueryable<T> mainQuery = this._context.Set<T>().AsNoTracking();
            if (query != null)
            {
                if (query.Includes.Any())
                {
                    // Montar um IQueryable<T> com todos os includes explícitos.
                    mainQuery = query.Includes
                    .Aggregate(this._context.Set<T>().AsQueryable(),
                        (current, include) => current.Include(include));
                }

                if (query.IncludeStrings.Any())
                {
                    // Adicionar os includes de strings.
                    mainQuery = query.IncludeStrings
                        .Aggregate(mainQuery,
                            (current, include) => current.Include(include));
                }

                //Tratar where.
                if (query.Criteria != null)
                {
                    foreach (var criteria in query.Criteria)
                    {
                        mainQuery = mainQuery.Where(criteria);
                    }
                }

                //Tratar ordenação.
                if (query.SortExpressions != null)
                {
                    mainQuery = PrepareSorting(query, mainQuery);
                }
            }

            return mainQuery;
        }

        private IQueryable<T> PrepareSorting(IQuery<T> query, IQueryable<T> mainQuery)
        {
            if (query.SortExpressions != null && query.SortExpressions.Any())
            {
                IOrderedQueryable<T> queryToSort = mainQuery as IOrderedQueryable<T>;
                for (int i = 0; i < query.SortExpressions.Count; i++)
                {
                    var order = query.SortExpressions[i];
                    if (order.Descending)
                    {
                        queryToSort = i == 0 ? queryToSort.OrderByDescending(order.Expression) : queryToSort.ThenByDescending(order.Expression);
                    }
                    else
                    {
                        queryToSort = i == 0 ? queryToSort.OrderBy(order.Expression) : queryToSort.ThenBy(order.Expression);
                    }
                }

                return queryToSort as IQueryable<T>;
            }

            return mainQuery;
        }

        private IQueryable<T> PrepareQuery(IQuery<T> query, int pageNumber, int itemsPerPage)
        {
            IQueryable<T> preparedQuery = this.PrepareQuery(query);
            return preparedQuery.PrepareQueryToPaging(pageNumber, itemsPerPage);
        }

        public async Task<T> GetSingleAsync(IQuery<T> query)
        {
            return await this.PrepareQuery(query).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await this.ListAsync(null);
        }

        public async Task<bool> ExistsAsync(IQuery<T> query)
        {
            return await this.PrepareQuery(query).AnyAsync();
        }
    }
}
