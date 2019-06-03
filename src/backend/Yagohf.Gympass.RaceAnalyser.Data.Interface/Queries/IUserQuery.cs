using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries
{
    public interface IUserQuery
    {
        IQuery<User> ByLogin(string login);
        IQuery<User> ByLoginAndPass(string login, string password);
    }
}
