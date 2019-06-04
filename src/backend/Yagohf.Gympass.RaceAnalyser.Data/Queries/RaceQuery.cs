using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Queries
{
    public class RaceQuery : IRaceQuery
    {
        public IQuery<Race> ById(int id)
        {
            return new Query<Race>()
                .Filter(x => x.Id == id)
                .AddInclude(x => x.Uploader);
        }

        public IQuery<Race> ByDescription(string description)
        {
            return new Query<Race>()
                .Filter(x => string.IsNullOrEmpty(description) || x.Description.Contains(description))
                .AddInclude(x => x.Uploader)
                .AddInclude(x => x.RaceType)
                .AddInclude(x => x.DriverResults);
        }
    }
}
