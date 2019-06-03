using Yagohf.Gympass.RaceAnalyser.Data.Context;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RaceAnalyserContext context) : base(context)
        {
        }
    }
}
