using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Queries
{
    public class UserQuery : IUserQuery
    {
        public IQuery<User> PorId(int id)
        {
            return new Query<User>()
                 .Filtrar(x => x.Id == id);
        }

        public IQuery<User> PorUsuario(string usuario)
        {
            return new Query<User>()
                .Filtrar(x => x.Login == usuario);
        }

        public IQuery<User> PorUsuarioSenha(string usuario, string senha)
        {
            return new Query<User>()
                .Filtrar(x => x.Login == usuario && x.Password == senha.ToCipherText());
        }
    }
}
