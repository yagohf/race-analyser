using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Queries
{
    public class RaceTypeQuery : IRaceTypeQuery
    {
        public IQuery<RaceType> ById(int id)
        {
            return new Query<RaceType>()
                .Filter(x => x.Id == id);
        }
    }
}
