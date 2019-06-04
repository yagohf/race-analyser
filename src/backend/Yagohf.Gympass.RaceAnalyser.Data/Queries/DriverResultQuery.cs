using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Queries
{
    public class DriverResultQuery : IDriverResultQuery
    {
        public IQuery<DriverResult> ByRace(int raceId)
        {
            return new Query<DriverResult>()
                .Filter(x => x.RaceId == raceId)
                .SortBy(x => x.Position);
        }
    }
}
