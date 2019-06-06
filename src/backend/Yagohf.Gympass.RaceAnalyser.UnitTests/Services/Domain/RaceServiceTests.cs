using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Configuration;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;
using Yagohf.Gympass.RaceAnalyser.Services.Domain;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper;
using Yagohf.Gympass.RaceAnalyser.Services.MappingProfiles;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Services.Domain
{
    [TestClass]
    public class RaceServiceTests
    {
        private readonly Mock<IRaceFileReader> _raceFileReaderMock;
        private readonly Mock<IRaceRepository> _raceRepositoryMock;
        private readonly Mock<IRaceQuery> _raceQueryMock;
        private readonly Mock<IRaceTypeRepository> _raceTypeRepositoryMock;
        private readonly Mock<IRaceTypeQuery> _raceTypeQueryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserQuery> _userQueryMock;
        private readonly Mock<ILapRepository> _lapRepositoryMock;
        private readonly Mock<IDriverResultRepository> _driverResultRepositoryMock;
        private readonly Mock<IDriverResultQuery> _driverResultQueryMock;
        private readonly Mock<IOptions<FileServerSettings>> _fileServerSettingsOptionsMock;
        private readonly IMapper _mapper;
        private RaceService _raceService;

        public RaceServiceTests()
        {
            this._raceFileReaderMock = new Mock<IRaceFileReader>();
            this._raceRepositoryMock = new Mock<IRaceRepository>();
            this._raceQueryMock = new Mock<IRaceQuery>();
            this._raceTypeRepositoryMock = new Mock<IRaceTypeRepository>();
            this._raceTypeQueryMock = new Mock<IRaceTypeQuery>();
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._userQueryMock = new Mock<IUserQuery>();
            this._lapRepositoryMock = new Mock<ILapRepository>();
            this._driverResultRepositoryMock = new Mock<IDriverResultRepository>();
            this._driverResultQueryMock = new Mock<IDriverResultQuery>();

            //Configurar FileServerSettings.
            this._fileServerSettingsOptionsMock = new Mock<IOptions<FileServerSettings>>();
            FileServerSettings fileServerSettings = new FileServerSettings()
            {
                Path = TestUtil.GetConfiguration().GetSection("FileServer:Path").Value
            };
            this._fileServerSettingsOptionsMock.Setup(x => x.Value).Returns(fileServerSettings);

            MapperConfiguration mapperConfiguration = new MapperConfiguration(mConfig =>
            {
                mConfig.AddProfile(new ServiceMapperProfile());
            });

            this._mapper = mapperConfiguration.CreateMapper();
        }

        [TestInitialize]
        public void Initialize()
        {
            this._raceService = new RaceService(
                this._raceFileReaderMock.Object,
                this._raceRepositoryMock.Object,
                this._raceQueryMock.Object,
                this._raceTypeRepositoryMock.Object,
                this._raceTypeQueryMock.Object,
                this._userRepositoryMock.Object,
                this._userQueryMock.Object,
                this._lapRepositoryMock.Object,
                this._driverResultRepositoryMock.Object,
                this._driverResultQueryMock.Object,
                this._fileServerSettingsOptionsMock.Object,
                this._mapper
                );
        }

        [TestMethod]
        public async Task Test_GetResultByIdAsync()
        {
            //Arrange.
            Race raceMock = new Race()
            {
                Id = 1,
                Date = DateTime.Now.Date,
                Description = "Race Unit Test",
                RaceTypeId = 1,
                TotalLaps = 1,
                UploadDate = DateTime.Now.Date,
                Uploader = new User()
                {
                    Id = 1,
                    Login = "yagohf",
                    Name = "Yago",
                    Password = "123mudar"
                }
            };

            //Mockar query.
            var raceByIdQuery = new Query<Race>();
            this._raceQueryMock.Setup(x => x.ById(raceMock.Id))
                .Returns(raceByIdQuery);

            //Mockar retorno do repositório quando usamos a query criada.
            this._raceRepositoryMock
               .Setup(rep => rep.GetSingleAsync(It.Is<IQuery<Race>>(q => q.Equals(raceByIdQuery))))
               .Returns(Task.FromResult(raceMock));

            List<DriverResult> driverResultsMock = new List<DriverResult>()
            {
                new DriverResult()
                {
                    Id = 1,
                    AverageSpeed = 10,
                    BestLap = TimeSpan.FromSeconds(10),
                    DriverName = "Yago",
                    DriverNumber = 1,
                    Gap = null,
                    Laps = 1,
                    Position = 1,
                    RaceId = raceMock.Id,
                    TotalRaceTime = TimeSpan.FromSeconds(10)
                },
                new DriverResult()
                {
                    Id = 2,
                    AverageSpeed = 8,
                    BestLap = TimeSpan.FromSeconds(15),
                    DriverName = "Henrique",
                    DriverNumber = 2,
                    Gap = TimeSpan.FromSeconds(5),
                    Laps = 1,
                    Position = 1,
                    RaceId = raceMock.Id,
                    TotalRaceTime = TimeSpan.FromSeconds(15)
                }
            };

            //Mockar query.
            var driverResultsByRaceIdQuery = new Query<DriverResult>();
            this._driverResultQueryMock.Setup(x => x.ByRace(raceMock.Id))
                .Returns(driverResultsByRaceIdQuery);

            //Mockar retorno do repositório quando usamos a query criada.
            this._driverResultRepositoryMock
               .Setup(rep => rep.ListAsync(It.Is<IQuery<DriverResult>>(q => q.Equals(driverResultsByRaceIdQuery))))
               .Returns(Task.FromResult(driverResultsMock.AsEnumerable()));

            //Act.
            var result = await this._raceService.GetResultByIdAsync(raceMock.Id);

            //Assert.
            Assert.IsNotNull(result);
            Assert.AreEqual(raceMock.Id, result.RaceId);
            Assert.AreEqual($"{ driverResultsMock[0].DriverNumber } - {driverResultsMock[0].DriverName}", result.Winner);
            Assert.IsNotNull(result.BestLap);
            Assert.AreEqual($"{ driverResultsMock[0].DriverNumber } - {driverResultsMock[0].DriverName}", result.BestLap.Driver);
            Assert.AreEqual(driverResultsMock[0].BestLap, result.BestLap.Time);
            Assert.IsNotNull(result.Results);
            Assert.AreEqual(driverResultsMock.Count, result.Results.Count());
            CollectionAssert.AreEquivalent(driverResultsMock.Select(x => x.DriverNumber).ToList(), result.Results.Select(x => x.DriverNumber).ToList());
        }

        [TestMethod]
        public async Task Test_ListSummaryAsync_NoPaging()
        {
            //Arrange.
            int totalRaces = 100;
            string description = "Race Unit Test";
            List<Race> summariesMock = new List<Race>();

            for (int i = 0; i < totalRaces; i++)
            {
                summariesMock.Add(new Race()
                {
                    Id = i,
                    Date = DateTime.Now.Date.AddDays(-i),
                    Description = $"{ description } {i}",
                    RaceTypeId = i,
                    TotalLaps = 1,
                    UploadDate = DateTime.Now.Date,
                    UploaderId = 1,
                    DriverResults = new List<DriverResult>()
                    {
                        new DriverResult()
                        {
                            Id = i * 1000,
                            AverageSpeed = 10,
                            BestLap = TimeSpan.FromSeconds(10),
                            DriverName = "Yago",
                            DriverNumber = 1,
                            Gap = null,
                            Laps = 1,
                            Position = 1,
                            RaceId = 1,
                            TotalRaceTime = TimeSpan.FromSeconds(10)
                        },
                        new DriverResult()
                        {
                            Id = (i * 1000) + 1,
                            AverageSpeed = 8,
                            BestLap = TimeSpan.FromSeconds(15),
                            DriverName = "Henrique",
                            DriverNumber = 2,
                            Gap = TimeSpan.FromSeconds(5),
                            Laps = 1,
                            Position = 1,
                            RaceId = 1,
                            TotalRaceTime = TimeSpan.FromSeconds(15)
                        }
                    },
                    Uploader = new User()
                    {
                        Id = 1,
                        Login = "yagohf",
                        Name = "Yago",
                        Password = "123mudar"
                    },
                    RaceType = new RaceType()
                    {
                        Id = 1,
                        Name = "Tipo 1"
                    }
                });
            }

            //Mockar query.
            var raceByDescriptionQuery = new Query<Race>();
            this._raceQueryMock.Setup(x => x.ByDescription(description))
                .Returns(raceByDescriptionQuery);

            //Mockar retorno do repositório quando usamos a query criada.
            this._raceRepositoryMock
               .Setup(rep => rep.ListAsync(It.Is<IQuery<Race>>(q => q.Equals(raceByDescriptionQuery))))
               .Returns(Task.FromResult(summariesMock.AsEnumerable()));

            //Act.
            var result = await this._raceService.ListSummaryAsync(description, null);

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.List);
            Assert.IsNull(result.Paging);
            Assert.AreEqual(summariesMock.Count(), result.List.Count());
            CollectionAssert.AreEquivalent(summariesMock.Select(x => x.Id).ToList(), result.List.Select(x => x.RaceId).ToList());
            Assert.AreEqual($"{summariesMock[0].DriverResults.First().DriverNumber} - {summariesMock[0].DriverResults.First().DriverName}", result.List.First().Winner);
        }

        [TestMethod]
        public async Task Test_ListSummaryAsync_Paging()
        {
            //Arrange.
            int page = 1;
            int itemsPerPage = 10;
            string description = "Race Unit Test";
            var summariesMock = new List<Race>();
            for (int i = 0; i < itemsPerPage; i++)
            {
                summariesMock.Add(new Race()
                {
                    Id = i,
                    Date = DateTime.Now.Date.AddDays(-i),
                    Description = $"{ description } {i}",
                    RaceTypeId = i,
                    TotalLaps = 1,
                    UploadDate = DateTime.Now.Date,
                    UploaderId = 1,
                    DriverResults = new List<DriverResult>()
                    {
                        new DriverResult()
                        {
                            Id = i * 1000,
                            AverageSpeed = 10,
                            BestLap = TimeSpan.FromSeconds(10),
                            DriverName = "Yago",
                            DriverNumber = 1,
                            Gap = null,
                            Laps = 1,
                            Position = 1,
                            RaceId = 1,
                            TotalRaceTime = TimeSpan.FromSeconds(10)
                        },
                        new DriverResult()
                        {
                            Id = (i * 1000) + 1,
                            AverageSpeed = 8,
                            BestLap = TimeSpan.FromSeconds(15),
                            DriverName = "Henrique",
                            DriverNumber = 2,
                            Gap = TimeSpan.FromSeconds(5),
                            Laps = 1,
                            Position = 1,
                            RaceId = 1,
                            TotalRaceTime = TimeSpan.FromSeconds(15)
                        }
                    },
                    Uploader = new User()
                    {
                        Id = 1,
                        Login = "yagohf",
                        Name = "Yago",
                        Password = "123mudar"
                    },
                    RaceType = new RaceType()
                    {
                        Id = 1,
                        Name = "Tipo 1"
                    }
                });
            }

            var paging = new Paging(page, summariesMock.Count, itemsPerPage);
            var pagedListMock = new Listing<Race>(summariesMock, paging);
            var pagedDTOListMock = new Listing<RaceSummaryDTO>(summariesMock.Map<Race, RaceSummaryDTO>(this._mapper), paging);

            //Mockar query.
            var raceByDescriptionQuery = new Query<Race>();
            this._raceQueryMock.Setup(x => x.ByDescription(description))
                .Returns(raceByDescriptionQuery);

            //Mockar retorno do repositório quando usamos a query criada.
            this._raceRepositoryMock
               .Setup(rep => rep.ListPagingAsync(It.Is<IQuery<Race>>(q => q.Equals(raceByDescriptionQuery)), page, itemsPerPage))
               .Returns(Task.FromResult(pagedListMock));

            //Act.
            var result = await this._raceService.ListSummaryAsync(description, page);

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.List);
            Assert.IsNotNull(result.Paging);
            CollectionAssert.AreEquivalent(pagedDTOListMock.List.Select(x => x.RaceId).ToList(), result.List.Select(x => x.RaceId).ToList());
            Assert.AreEqual(pagedDTOListMock.List.Count(), result.List.Count());
            Assert.AreEqual($"{pagedListMock.List.First().DriverResults.First().DriverNumber} - {pagedListMock.List.First().DriverResults.First().DriverName}", result.List.First().Winner);
        }
    }
}
