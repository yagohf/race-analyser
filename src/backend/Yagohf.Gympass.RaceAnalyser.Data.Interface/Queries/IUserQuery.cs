using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries
{
    public interface IUserQuery
    {
        IQuery<User> PorId(int id);
        IQuery<User> PorUsuario(string usuario);
        IQuery<User> PorUsuarioSenha(string usuario, string senha);
    }
}
