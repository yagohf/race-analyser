using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Api.Controllers;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Api
{
    [TestClass]
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private UsersController _usersController;

        public UsersControllerTests()
        {
            this._userServiceMock = new Mock<IUserService>();
        }

        [TestInitialize]
        public void Inicializar()
        {
            this._usersController = new UsersController(this._userServiceMock.Object);
        }

        [TestMethod]
        public async Task Test_GetByLogin()
        {
            //Arrange.
            string userLogin = "yagohf";
            UserDTO userMock = new UserDTO()
            {
                Id = 1,
                Name = "Usuario",
                Login = "yagohf",
                UploadedRaces = 0
            };

            this._userServiceMock
                .Setup(bsn => bsn.GetByLoginAsync(userLogin))
                .Returns(Task.FromResult(userMock));

            //Act.
            var result = await this._usersController.GetByLogin(userLogin);
            var okResult = result as OkObjectResult;

            //Assert.
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(UserDTO));
            Assert.AreEqual(userMock, okResult.Value as UserDTO);
        }

        [TestMethod]
        public async Task Test_Post()
        {
            //Arrange.
            UserDTO userMock = new UserDTO()
            {
                Login = "yagohf",
                Name = "Yago",
                Id = 1
            };

            RegistrationDTO registrationData = new RegistrationDTO()
            {
                Login = "yagohf",
                Name = "Yago",
                Password = "123mudar"
            };

            this._userServiceMock
                .Setup(bsn => bsn.RegisterAsync(registrationData))
                .Returns(Task.FromResult(userMock));

            //Act.
            var result = await this._usersController.Post(registrationData);
            var createdAtResult = result as CreatedAtActionResult;

            //Assert.
            Assert.IsNotNull(createdAtResult);
            Assert.IsInstanceOfType(createdAtResult.Value, typeof(UserDTO));
            Assert.AreEqual(userMock, createdAtResult.Value as UserDTO);

            //Testar URL de redirecionamento.
            Assert.AreEqual(createdAtResult.ActionName, nameof(this._usersController.GetByLogin));
            Assert.AreEqual(createdAtResult.RouteValues["login"], userMock.Login);
        }

        [TestMethod]
        public async Task Test_PostAuth()
        {
            //Arrange.
            TokenDTO mockToken = new TokenDTO()
            {
                Login = "yagohf",
                Name = "Yago",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InlhZ29oZiIsIm5iZiI6MTU0NzYwNjQ5NywiZXhwIjoxNTQ3NjkyODk3LCJpYXQiOjE1NDc2MDY0OTd9.qzmJSEvtoHphSpOkFJ81mN2FqeiyXk47zo3euVFxACk"
            };

            AuthenticationDTO authenticationData = new AuthenticationDTO()
            {
                Login = "yagohf",
                Password = "123mudar"
            };

            this._userServiceMock
                .Setup(bsn => bsn.GenerateTokenAsync(authenticationData))
                .Returns(Task.FromResult(mockToken));

            //Act.
            var result = await this._usersController.PostAuth(authenticationData);
            var okResult = result as OkObjectResult;

            //Assert.
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(TokenDTO));
            Assert.AreEqual(mockToken, okResult.Value as TokenDTO);
        }
    }
}
