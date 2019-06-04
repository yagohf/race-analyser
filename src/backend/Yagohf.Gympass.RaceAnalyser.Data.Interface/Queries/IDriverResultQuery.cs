using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries
{
    public interface IDriverResultQuery
    {
        IQuery<DriverResult> ByRace(int raceId);
    }
}
