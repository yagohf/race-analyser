using Yagohf.Gympass.RaceAnalyser.Data.Context;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Repositories
{
    public class RaceTypeRepository : RepositoryBase<RaceType>, IRaceTypeRepository
    {
        public RaceTypeRepository(RaceAnalyserContext context) : base(context)
        {
        }
    }
}
