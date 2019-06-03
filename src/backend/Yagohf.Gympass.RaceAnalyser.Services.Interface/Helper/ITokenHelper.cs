using Yagohf.Gympass.RaceAnalyser.Model.DTO;

namespace Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper
{
    public interface ITokenHelper
    {
        TokenDTO Generate(string login, string name);
    }
}
