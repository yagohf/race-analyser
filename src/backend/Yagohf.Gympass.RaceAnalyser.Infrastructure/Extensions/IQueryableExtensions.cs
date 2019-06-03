using System.Linq;

namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> PrepareQueryToPaging<T>(this IQueryable<T> queryable, int page, int itemsPerPage)
        {
            return queryable.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
        }
    }
}
