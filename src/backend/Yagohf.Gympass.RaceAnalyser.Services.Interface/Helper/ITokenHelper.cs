using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;

namespace Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper
{
    public interface ITokenHelper
    {
        TokenDTO Generate(string login, string name);
    }
}
