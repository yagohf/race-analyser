using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Exception;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;
using Yagohf.Gympass.RaceAnalyser.Services.Domain;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper;
using Yagohf.Gympass.RaceAnalyser.Services.MappingProfiles;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Services.Domain
{
    [TestClass]
    public class UserServiceTests
    {
        private readonly Mock<ITokenHelper> _tokenHelperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserQuery> _userQueryMock;
        private readonly IMapper _mapper;
        private UserService _userService;

        public UserServiceTests()
        {
            this._tokenHelperMock = new Mock<ITokenHelper>();
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._userQueryMock = new Mock<IUserQuery>();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(mConfig =>
            {
                mConfig.AddProfile(new ServiceMapperProfile());
            });

            this._mapper = mapperConfiguration.CreateMapper();
        }

        [TestInitialize]
        public void Initialize()
        {
            this._userService = new UserService(
                this._tokenHelperMock.Object,
                this._userRepositoryMock.Object,
                this._userQueryMock.Object,
                this._mapper
                );
        }

        #region [ Gerar token ]
        [TestMethod]
        [ExpectedException(typeof(BusinessException))]
        public async Task Test_GenerateTokenAsync_NoDataProvided()
        {
            //Arrange.
            AuthenticationDTO authentication = new AuthenticationDTO()
            {
                Login = "",
                Password = ""
            };

            //Act.
            var token = await this._userService.GenerateTokenAsync(authentication);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessException))]
        public async Task Test_GenerateTokenAsync_IncorrectPassword()
        {
            //Arrange.
            AuthenticationDTO authentication = new AuthenticationDTO()
            {
                Login = "yagohf",
                Password = "123mudar"
            };

            //Mockar query de usuário por login e senha.
            var userByLoginPasswordQuery = new Query<User>();
            this._userQueryMock.Setup(x => x.ByLoginAndPass(authentication.Login, authentication.Password))
                  .Returns(userByLoginPasswordQuery);

            this._userRepositoryMock.Setup(x => x.GetSingleAsync(It.Is<Query<User>>(it => it.Equals(userByLoginPasswordQuery))))
                  .Returns(Task.FromResult<User>(null));

            //Act.
            var token = await this._userService.GenerateTokenAsync(authentication);
        }

        [TestMethod]
        public async Task Test_GenerateTokenAsync_Valid()
        {
            //Arrange.
            AuthenticationDTO authentication = new AuthenticationDTO()
            {
                Login = "yagohf",
                Password = "123mudar"
            };

            //Mockar query de usuário por login e senha.
            var userByLoginPasswordQuery = new Query<User>();
            this._userQueryMock.Setup(x => x.ByLoginAndPass(authentication.Login, authentication.Password))
                  .Returns(userByLoginPasswordQuery);

            //Mockar usuário por login e senha.
            var mockUser = new User()
            {
                Id = 1,
                Login = authentication.Login,
                Name = "Yago"
            };

            this._userRepositoryMock.Setup(x => x.GetSingleAsync(It.Is<Query<User>>(it => it.Equals(userByLoginPasswordQuery))))
                  .Returns(Task.FromResult(mockUser));

            //Mockar gerador de token.
            var tokenMock = new TokenDTO()
            {
                Login = mockUser.Login,
                Name = mockUser.Name,
                Token = "123456"
            };
            this._tokenHelperMock.Setup(x => x.Generate(mockUser.Login, mockUser.Name))
                .Returns(tokenMock);

            //Act.
            var token = await this._userService.GenerateTokenAsync(authentication);

            //Assert.
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Token);
            Assert.AreEqual(authentication.Login, token.Login);
        }
        #endregion

        #region [ Registrar ]
        [TestMethod]
        [ExpectedException(typeof(BusinessException))]
        public async Task Test_RegisterAsync_NoDataProvided()
        {
            //Arrange.
            RegistrationDTO registration = new RegistrationDTO()
            {
                Login = "",
                Password = "",
                Name = ""
            };

            //Act.
            var createdUser = await this._userService.RegisterAsync(registration);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessException))]
        public async Task Test_RegisterAsync_UserAlreadyExist()
        {
            //Arrange.
            RegistrationDTO registration = new RegistrationDTO()
            {
                Login = "yagohf",
                Password = "123mudar",
                Name = "Yago"
            };

            //Mockar query de usuário existente por login.
            var userByLoginQuery = new Query<User>();
            this._userQueryMock.Setup(x => x.ByLogin(registration.Login))
                  .Returns(userByLoginQuery);

            this._userRepositoryMock.Setup(x => x.ExistsAsync(It.Is<Query<User>>(it => it.Equals(userByLoginQuery))))
                  .Returns(Task.FromResult(true));

            //Act.
            var createdUser = await this._userService.RegisterAsync(registration);
        }

        [TestMethod]
        public async Task Test_RegisterAsync_Valido()
        {
            //Arrange.
            RegistrationDTO registration = new RegistrationDTO()
            {
                Login = "yagohf",
                Password = "123mudar",
                Name = "Yago"
            };

            //Mockar query de usuário existente por login.
            var userByLoginQuery = new Query<User>();
            this._userQueryMock.Setup(x => x.ByLogin(registration.Login))
                  .Returns(userByLoginQuery);

            this._userRepositoryMock.Setup(x => x.ExistsAsync(It.Is<Query<User>>(it => it.Equals(userByLoginQuery))))
                  .Returns(Task.FromResult(false));

            //Mockar usuário criado.
            var createdUserMock = new User()
            {
                Id = 1,
                Login = "yagohf",
                Name = "Yago"
            };

            //Act.
            var createdUser = await this._userService.RegisterAsync(registration);

            //Assert.
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(createdUserMock.Name, createdUser.Name);
            this._userRepositoryMock.Verify(rep => rep.InsertAsync(It.IsAny<User>()), Times.Once);
        }
        #endregion

        #region [ Selecionar por ID ]
        [TestMethod]
        public async Task Test_GetByLoginAsync()
        {
            //Arrange.
            string login = "yagohf";
            User mockUser = new User()
            {
                Id = 1,
                Name = "User 1",
                Login = login
            };

            this._userRepositoryMock
                .Setup(rep => rep.GetSingleAsync(It.IsAny<IQuery<User>>()))
                .Returns(Task.FromResult(mockUser));

            //Act.
            var result = await this._userService.GetByLoginAsync(login);

            //Assert.
            Assert.IsNotNull(result);
            Assert.AreEqual(mockUser.Id, result.Id);
        }
        #endregion
    }
}
