using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Api.Controllers;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Model;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Api
{
    [TestClass]
    public class RacesControllerTests
    {
        private readonly Mock<IRaceService> _raceServiceMock;
        private RacesController _racesController;

        public RacesControllerTests()
        {
            this._raceServiceMock = new Mock<IRaceService>();
        }

        [TestInitialize]
        public void Initialize()
        {
            this._racesController = new RacesController(this._raceServiceMock.Object);
        }

        [TestMethod]
        public async Task Test_Get_Paging()
        {
            //Arrange.
            int page = 1;
            int itemsPerPage = 10;

            var summariesList = new List<RaceSummaryDTO>();
            for (int i = 0; i < itemsPerPage; i++)
            {
                summariesList.Add(new RaceSummaryDTO()
                {
                    RaceId = i,
                    RaceDate = DateTime.Now.AddMonths(-i),
                    RaceDescription = $"Race_{i}",
                    RaceTypeDescription = $"RaceType_{i}",
                    RaceTypeId = i,
                    Uploader = $"Uploader_{i}",
                    Winner = $"Winner_{i}"
                });
            }

            var paginacao = new Paging(page, summariesList.Count, itemsPerPage);
            var mockListaPaginada = new Listing<RaceSummaryDTO>(summariesList, paginacao);

            this._raceServiceMock
                .Setup(srv => srv.ListSummaryAsync(It.IsAny<string>(), page))
                .Returns(Task.FromResult(mockListaPaginada));

            //Act.
            var result = await this._racesController.Get(null, page);
            var okResult = result as OkObjectResult;

            //Assert.
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(Listing<RaceSummaryDTO>));
            Assert.AreEqual(mockListaPaginada, okResult.Value as Listing<RaceSummaryDTO>);
        }

        [TestMethod]
        public async Task Test_Get_Without_Paging()
        {
            //Arrange.
            var summariesList = new List<RaceSummaryDTO>();
            for (int i = 0; i < 5; i++)
            {
                summariesList.Add(new RaceSummaryDTO()
                {
                    RaceId = i,
                    RaceDate = DateTime.Now.AddMonths(-i),
                    RaceDescription = $"Race_{i}",
                    RaceTypeDescription = $"RaceType_{i}",
                    RaceTypeId = i,
                    Uploader = $"Uploader_{i}",
                    Winner = $"Winner_{i}"
                });
            }

            var summariesListMock = new Listing<RaceSummaryDTO>(summariesList);
            this._raceServiceMock
                .Setup(srv => srv.ListSummaryAsync(It.IsAny<string>(), null))
                .Returns(Task.FromResult(summariesListMock));

            //Act.
            var result = await this._racesController.Get(null, null);
            var okResult = result as OkObjectResult;

            //Assert.
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(Listing<RaceSummaryDTO>));
            Assert.AreEqual(summariesListMock, okResult.Value as Listing<RaceSummaryDTO>);
        }

        [TestMethod]
        public async Task Test_GetRaceTypes()
        {
            //Arrange.
            var raceTypesMock = new List<RaceTypeDTO>();
            for (int i = 0; i < 5; i++)
            {
                raceTypesMock.Add(new RaceTypeDTO()
                {
                    Id = i,
                    Name = $"RaceType_{i}"
                });
            }

            this._raceServiceMock
                .Setup(srv => srv.ListRaceTypesAsync())
                .Returns(Task.FromResult(raceTypesMock.AsEnumerable()));

            //Act.
            var result = await this._racesController.GetRaceTypes();
            var okResult = result as OkObjectResult;

            //Assert.
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<RaceTypeDTO>));
            Assert.AreEqual(raceTypesMock, okResult.Value as IEnumerable<RaceTypeDTO>);
        }

        [TestMethod]
        public async Task Testar_GetResultById()
        {
            //Arrange.
            int raceId = 1;
            RaceResultDTO raceResultMock = new RaceResultDTO()
            {
                BestLap = new BestLapDTO()
                {
                    Driver = "10 - Test Driver",
                    Time = TimeSpan.FromSeconds(60)
                },
                RaceDate = DateTime.Now.AddDays(-1),
                RaceDescription = "TestRace",
                RaceId = raceId,
                TotalLaps = 10,
                UploadDate = DateTime.Now.Date,
                Uploader = "yagohf",
                Winner = "10 - Test Driver",
                Results = new List<DriverResultDTO>()
                {
                    new DriverResultDTO()
                    {
                        AverageSpeed = 40,
                        BestLap = TimeSpan.FromSeconds(60),
                        DriverName = "Test Driver",
                        DriverNumber = 10,
                        Gap = null,
                        Laps = 10,
                        Position = 1,
                        TotalRaceTime = TimeSpan.FromMinutes(15)
                    },
                    new DriverResultDTO()
                    {
                        AverageSpeed = 38,
                        BestLap = TimeSpan.FromSeconds(70),
                        DriverName = "Test Driver 2",
                        DriverNumber = 15,
                        Gap = TimeSpan.FromSeconds(60),
                        Laps = 10,
                        Position = 2,
                        TotalRaceTime = TimeSpan.FromMinutes(16)
                    }
                }
            };

            this._raceServiceMock
                .Setup(srv => srv.GetResultByIdAsync(raceId))
                .Returns(Task.FromResult(raceResultMock));

            //Act.
            var result = await this._racesController.GetResultById(raceId);
            var okResult = result as OkObjectResult;

            //Assert.
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(RaceResultDTO));
            Assert.AreEqual(raceResultMock, okResult.Value as RaceResultDTO);
        }

        [TestMethod]
        public async Task Test_Post()
        {
            //Arrange.
            CreateRaceDTO createRaceMock = new CreateRaceDTO()
            {
                Date = DateTime.Now.AddDays(-1),
                Description = "2019 F-1 Racing",
                RaceTypeId = 1,
                TotalLaps = 10
            };

            //Arrange.
            int raceId = 1;
            RaceResultDTO raceResultMock = new RaceResultDTO()
            {
                BestLap = new BestLapDTO()
                {
                    Driver = "10 - Test Driver",
                    Time = TimeSpan.FromSeconds(60)
                },
                RaceDate = DateTime.Now.AddDays(-1),
                RaceDescription = "2019 F-1 Racing",
                RaceId = raceId,
                TotalLaps = 10,
                UploadDate = DateTime.Now.Date,
                Uploader = "yagohf",
                Winner = "10 - Test Driver",
                Results = new List<DriverResultDTO>()
                {
                    new DriverResultDTO()
                    {
                        AverageSpeed = 40,
                        BestLap = TimeSpan.FromSeconds(60),
                        DriverName = "Test Driver",
                        DriverNumber = 10,
                        Gap = null,
                        Laps = 10,
                        Position = 1,
                        TotalRaceTime = TimeSpan.FromMinutes(15)
                    },
                    new DriverResultDTO()
                    {
                        AverageSpeed = 38,
                        BestLap = TimeSpan.FromSeconds(70),
                        DriverName = "Test Driver 2",
                        DriverNumber = 15,
                        Gap = TimeSpan.FromSeconds(60),
                        Laps = 10,
                        Position = 2,
                        TotalRaceTime = TimeSpan.FromMinutes(16)
                    }
                }
            };

            //Mockar arquivo.
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            string fileName = "NO-ERRORS.txt";
            string fileAssemblyPath = $"Yagohf.Gympass.RaceAnalyser.UnitTests.Embedded.{fileName}";
            using (var fileAssemblyStream = Assembly.GetAssembly(typeof(RacesControllerTests)).GetManifestResourceStream(fileAssemblyPath))
            {
                fileAssemblyStream.Position = 0;
                fileMock.Setup(x => x.OpenReadStream()).Returns(fileAssemblyStream);
                fileMock.Setup(x => x.FileName).Returns(fileName);
                fileMock.Setup(x => x.Length).Returns(fileAssemblyStream.Length);
                fileMock.Setup(x => x.ContentType).Returns("text/plain");
            }

            //Mockar usuário logado.
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "yagohf"),
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(x => x.Identity).Returns(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);

            this._racesController.ControllerContext.HttpContext = mockHttpContext.Object;

            this._raceServiceMock
                .Setup(srv => srv.AnalyseAsync(It.IsAny<CreateRaceDTO>(), It.IsAny<FileDTO>(), It.IsAny<string>()))
                .Returns(Task.FromResult(raceResultMock));

            //Act.
            var result = await this._racesController.Post(createRaceMock, fileMock.Object);
            var createdAtResult = result as CreatedAtActionResult;

            //Assert.
            Assert.IsNotNull(createdAtResult);
            Assert.IsInstanceOfType(createdAtResult.Value, typeof(RaceResultDTO));
            Assert.AreEqual(raceResultMock, createdAtResult.Value as RaceResultDTO);

            //Testar URL de redirecionamento.
            Assert.AreEqual(createdAtResult.ActionName, nameof(this._racesController.GetResultById));
            Assert.AreEqual(createdAtResult.RouteValues["id"], raceResultMock.RaceId);
        }

        [TestMethod]
        public async Task Test_GetExample()
        {
            //Arrange.
            FileDTO fileDownload = new FileDTO()
            {
                Name = "EXAMPLE.txt",
                ContentType = "text/plain",
                Extension = "txt",
                Content = new MemoryStream(new byte[] { 125, 141, 13, 27 })
            };

            this._raceServiceMock
                .Setup(srv => srv.GetExampleFileAsync())
                .Returns(Task.FromResult(fileDownload));

            //Act.
            var result = await this._racesController.GetExample();

            //Assert.
            Assert.IsInstanceOfType(result, typeof(FileStreamResult));
            var resultFile = result as FileStreamResult;
            Assert.AreEqual(fileDownload.Name, resultFile.FileDownloadName);
            Assert.AreEqual(fileDownload.ContentType, resultFile.ContentType);
            Assert.AreEqual(fileDownload.Content, resultFile.FileStream);
        }
    }
}
