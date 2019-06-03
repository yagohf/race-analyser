using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;

namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions
{
    public static class ListingExtensions
    {
        public static Listing<TDestino> Map<TOrigem, TDestino>(this Listing<TOrigem> listaOriginal, IMapper mapper)
            where TDestino : class
            where TOrigem : class
        {
            Listing<TDestino> mapped = new Listing<TDestino>(
                listaOriginal.List.Map<TOrigem, TDestino>(mapper),
                listaOriginal.Paging);

            return mapped;
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IEnumerable<TSource> originalList, IMapper mapper)
           where TDestination : class
           where TSource : class
        {
            return originalList.Select(x => mapper.Map<TDestination>(x)).AsEnumerable();
        }
    }
}
