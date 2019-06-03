using Yagohf.Gympass.RaceAnalyser.Data.Context;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Repositories
{
    public class LapRepository : RepositoryBase<Lap>, ILapRepository
    {
        public LapRepository(RaceAnalyserContext context) : base(context)
        {
        }
    }
}
