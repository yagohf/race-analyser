using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries
{
    public interface IRaceTypeQuery
    {
        IQuery<RaceType> ById(int id);
    }
}
