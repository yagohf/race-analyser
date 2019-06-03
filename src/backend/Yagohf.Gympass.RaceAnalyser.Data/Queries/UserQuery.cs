using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Queries
{
    public class UserQuery : IUserQuery
    {
        public IQuery<User> ByLogin(string login)
        {
            return new Query<User>()
                .Filter(x => x.Login == login);
        }

        public IQuery<User> ByLoginAndPass(string login, string password)
        {
            return new Query<User>()
                .Filter(x => x.Login == login && x.Password == password.ToCipherText());
        }
    }
}
