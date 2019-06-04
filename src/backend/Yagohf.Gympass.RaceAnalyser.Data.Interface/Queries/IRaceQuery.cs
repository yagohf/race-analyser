using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries
{
    public interface IRaceQuery
    {
        IQuery<Race> ById(int id);
        IQuery<Race> ByDescriptionOrUploader(string description, string uploader);
    }
}
