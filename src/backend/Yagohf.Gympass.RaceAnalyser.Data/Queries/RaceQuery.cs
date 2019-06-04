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

        public IQuery<Race> ByDescriptionOrUploader(string description, string uploader)
        {
            return new Query<Race>()
                .Filter(x =>
                    (string.IsNullOrEmpty(description) || x.Description.Contains(description))
                    &&
                    (string.IsNullOrEmpty(uploader) || x.Uploader.Login.Equals(uploader))
                    )
                .AddInclude(x => x.Uploader)
                .AddInclude(x => x.DriverResults);
        }
    }
}
