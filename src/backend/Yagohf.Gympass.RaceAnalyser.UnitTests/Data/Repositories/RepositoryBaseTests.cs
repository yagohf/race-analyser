using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Data.Context;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Repositories;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Repositories
{
    [TestClass]
    public class RepositoryBaseTests
    {
        private readonly RaceAnalyserContext _context;
        private readonly IRaceQuery _raceQuery;

        public RepositoryBaseTests()
        {
            var options = new DbContextOptionsBuilder<RaceAnalyserContext>()
            .UseInMemoryDatabase(databaseName: "REPOSITORY_BASE_TESTS")
            .Options;

            this._context = new RaceAnalyserContext(options);

            this._raceQuery = new RaceQuery();
        }

        [TestMethod]
        public async Task Testar_CountAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualAsyncMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            await repositoryBase.InsertAsync(race);
            int count = await repositoryBase.CountAsync(this._raceQuery.ById(race.Id));

            //Assert
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public async Task Testar_UpdateAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            string updatedDescription = "BATATA_UNIT_TEST";

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualAsyncMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                Uploader = new User()
                {
                    Login = "logintest",
                    Name = "Login Test",
                    Password = "123mudar"
                }
            };

            //Act
            await repositoryBase.InsertAsync(race);
            race.Description = updatedDescription;
            await repositoryBase.UpdateAsync(race);

            race = await repositoryBase.GetSingleAsync(this._raceQuery.ById(race.Id));

            //Assert
            Assert.AreEqual(updatedDescription, race.Description);
        }

        [TestMethod]
        public async Task Testar_InsertAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualAsyncMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            await repositoryBase.InsertAsync(race);

            //Assert
            Assert.IsTrue(race.Id > 0);
        }

        [TestMethod]
        public async Task Testar_DeleteAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualAsyncMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            await repositoryBase.InsertAsync(race);
            await repositoryBase.DeleteAsync(race);
            var entityAposExclusao = await repositoryBase.GetSingleAsync(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsNull(entityAposExclusao);
        }

        [TestMethod]
        public async Task Testar_GetSingleAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualAsyncMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                Uploader = new User()
                {
                    Login = "logintest",
                    Name = "Teste",
                    Password = "123mudar"
                }
            };

            //Act
            await repositoryBase.InsertAsync(race);
            var entity = await repositoryBase.GetSingleAsync(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsNotNull(entity);
            Assert.AreEqual(entity.Id, race.Id);
        }

        [TestMethod]
        public async Task Testar_ListAsync()
        {
            //Arrange
            RepositoryBase<Race> raceRepositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < 10; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualAsyncMethodName()}_{i}",
                    TotalLaps = 10,
                    UploadDate = DateTime.Now,
                    Uploader = new User()
                    {
                        Login = "logintest",
                        Name = "Login Test",
                        Password = "123mudar"
                    },
                    RaceType = new RaceType()
                    {
                        Name = $"{GetActualAsyncMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                await raceRepositoryBase.InsertAsync(r);
            }

            var result = await raceRepositoryBase.ListAsync(this._raceQuery.ByDescription(GetActualAsyncMethodName() + "_"));

            var todas = await raceRepositoryBase.ListAllAsync();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(races.Count, result.Count());
        }

        [TestMethod]
        public async Task Testar_ListAsync_Sorting()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race1 = new Race()
            {
                Date = DateTime.Now,
                Description = $"{GetActualAsyncMethodName()}_A",
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                Uploader = new User()
                {
                    Login = "logintest",
                    Name = "Login Test",
                    Password = "123mudar"
                }
            };

            Race race2 = new Race()
            {
                Date = DateTime.Now,
                Description = $"{GetActualAsyncMethodName()}_B",
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                Uploader = new User()
                {
                    Login = "logintest",
                    Name = "Login Test",
                    Password = "123mudar"
                }
            };

            Race race3 = new Race()
            {
                Date = DateTime.Now,
                Description = $"{GetActualAsyncMethodName()}_CD",
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                Uploader = new User()
                {
                    Login = "logintest",
                    Name = "Login Test",
                    Password = "123mudar"
                }
            };

            Race race4 = new Race()
            {
                Date = DateTime.Now,
                Description = $"{GetActualAsyncMethodName()}_CD",
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                Uploader = new User()
                {
                    Login = "logintest",
                    Name = "Login Test",
                    Password = "123mudar"
                }
            };

            //Act
            await repositoryBase.InsertAsync(race1);
            await repositoryBase.InsertAsync(race2);
            await repositoryBase.InsertAsync(race3);
            await repositoryBase.InsertAsync(race4);

            string racePrefix = GetActualAsyncMethodName() + "_";
            var result = (await repositoryBase.ListAsync(
                new Query<Race>()
                .Filter(x => x.Description.StartsWith(racePrefix))
                .SortBy(x => x.Description)
                .SortByDescending(x => x.Id))
                ).ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(race1.Id, result[0].Id);
            Assert.AreEqual(race2.Id, result[1].Id);
            Assert.AreEqual(race4.Id, result[2].Id); //A4 deve vir antes de A3 por conta da ordenação por ID na descendente como desempate.
            Assert.AreEqual(race3.Id, result[3].Id);
        }

        [TestMethod]
        public async Task Testar_ListPagingAsync_Pagina1()
        {
            //Arrange
            int quantidadeRegistrosPorPagina = 10;
            int paginaPesquisar = 1;
            int totalRegistros = 50;

            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < totalRegistros; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualAsyncMethodName()}_{i}",
                    RaceTypeId = 1,
                    TotalLaps = 10,
                    UploadDate = DateTime.Now,
                    Uploader = new User()
                    {
                        Login = "logintest",
                        Name = "Login Test",
                        Password = "123mudar"
                    },
                    RaceType = new RaceType()
                    {
                        Name = $"{GetActualAsyncMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                await repositoryBase.InsertAsync(r);
            }

            var result = await repositoryBase.ListPagingAsync(this._raceQuery.ByDescription(GetActualAsyncMethodName()), paginaPesquisar, quantidadeRegistrosPorPagina);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Paging);
            Assert.IsNotNull(result.List);
            Assert.AreEqual(paginaPesquisar, result.Paging.CurrentPage);
            Assert.AreEqual(quantidadeRegistrosPorPagina, result.Paging.ItemsPerPage);
            Assert.AreEqual(totalRegistros, result.Paging.TotalItems);
            Assert.AreEqual(quantidadeRegistrosPorPagina, result.List.Count());
        }

        [TestMethod]
        public async Task Testar_ListPagingAsync_Pagina5()
        {
            //Arrange
            int quantidadeRegistrosPorPagina = 10;
            int quantidadeRegistrosUltimaPagina = 9;
            int paginaPesquisar = 5;
            int totalRegistros = 49;

            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < totalRegistros; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualAsyncMethodName()}_{i}",
                    RaceTypeId = 1,
                    TotalLaps = 10,
                    UploadDate = DateTime.Now,
                    Uploader = new User()
                    {
                        Login = "logintest",
                        Name = "Login Test",
                        Password = "123mudar"
                    },
                    RaceType = new RaceType()
                    {
                        Name = $"{GetActualAsyncMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                await repositoryBase.InsertAsync(r);
            }

            var result = await repositoryBase.ListPagingAsync(this._raceQuery.ByDescription(GetActualAsyncMethodName()), paginaPesquisar, quantidadeRegistrosPorPagina);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Paging);
            Assert.IsNotNull(result.List);
            Assert.AreEqual(paginaPesquisar, result.Paging.CurrentPage);
            Assert.AreEqual(quantidadeRegistrosPorPagina, result.Paging.ItemsPerPage);
            Assert.AreEqual(totalRegistros, result.Paging.TotalItems);
            Assert.AreEqual(quantidadeRegistrosUltimaPagina, result.List.Count());
        }

        [TestMethod]
        public async Task Testar_ListAllAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < 10; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualAsyncMethodName()}_{i}",
                    RaceTypeId = 1,
                    TotalLaps = 10,
                    UploadDate = DateTime.Now,
                    UploaderId = 1
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                await repositoryBase.InsertAsync(r);
            }

            var registrosDiretamenteContext = this._context.Set<Race>().ToList();
            var result = await repositoryBase.ListAllAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(registrosDiretamenteContext.Count, result.Count());
        }

        [TestMethod]
        public async Task Testar_ExistsAsync_Existe()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualAsyncMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            await repositoryBase.InsertAsync(race);
            bool existe = await repositoryBase.ExistsAsync(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task Testar_ExistsAsync_NaoExiste()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            //Act
            bool existe = await repositoryBase.ExistsAsync(this._raceQuery.ById(0));

            //Assert
            Assert.IsFalse(existe);
        }

        #region [ Helpers ]
        static string GetActualAsyncMethodName([CallerMemberName]string name = null)
        {
            return name;
        }
        #endregion
    }
}
