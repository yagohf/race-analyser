using AutoMapper;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Exception;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Model.DTO;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper;

namespace Yagohf.Gympass.RaceAnalyser.Services.Domain
{
    public class UserService : IUserService
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserRepository _userRepository;
        private readonly IUserQuery _userQuery;
        private readonly IMapper _mapper;

        public UserService(ITokenHelper tokenHelper, IUserRepository userRepository, IUserQuery userQuery, IMapper mapper)
        {
            this._tokenHelper = tokenHelper;
            this._userRepository = userRepository;
            this._userQuery = userQuery;
            this._mapper = mapper;
        }

        public async Task<TokenDTO> GenerateTokenAsync(AuthenticationDTO auth)
        {
            if (string.IsNullOrEmpty(auth.Login) || string.IsNullOrEmpty(auth.Password))
                throw new BusinessException("Usuário ou senha inválidos");

            User user = await this._userRepository.GetSingleAsync(this._userQuery.ByLoginAndPass(auth.Login, auth.Password));
            if (user == null)
                throw new BusinessException("Usuário ou senha inválidos");

            return this._tokenHelper.Generate(user.Login, user.Name);
        }

        public async Task<UserDTO> RegisterAsync(RegistrationDTO registration)
        {
            User newUser = this._mapper.Map<User>(registration);

            if (string.IsNullOrEmpty(registration.Login) || string.IsNullOrEmpty(registration.Password) || string.IsNullOrEmpty(registration.Name))
                throw new BusinessException("Dados incompletos para registrar o usuário");
            else if (await this._userRepository.ExistsAsync(this._userQuery.ByLogin(registration.Login)))
                throw new BusinessException("Esse nome de usuário não está disponível para registro");

            newUser.Password = newUser.Password.ToCipherText();
            await this._userRepository.InsertAsync(newUser);
            return this._mapper.Map<UserDTO>(newUser);
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            User user = await this._userRepository.GetSingleAsync(this._userQuery.ById(id));
            return this._mapper.Map<UserDTO>(user);
        }
    }
}