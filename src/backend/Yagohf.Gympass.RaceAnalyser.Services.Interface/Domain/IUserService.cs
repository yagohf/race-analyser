using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;

namespace Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain
{
    public interface IUserService
    {
        Task<TokenDTO> GenerateTokenAsync(AuthenticationDTO authentication);
        Task<UserDTO> RegisterAsync(RegistrationDTO registration);
        Task<UserDTO> GetByLoginAsync(string login);
    }
}
