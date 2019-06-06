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

        #region [ Async ]
        [TestMethod]
        public async Task Test_CountAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
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
        public async Task Test_UpdateAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            string updatedDescription = "UPDATED_UNIT_TEST";

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
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
        public async Task Test_InsertAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
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
        public async Task Test_DeleteAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            await repositoryBase.InsertAsync(race);
            await repositoryBase.DeleteAsync(race);
            var entityAfterDeletion = await repositoryBase.GetSingleAsync(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsNull(entityAfterDeletion);
        }

        [TestMethod]
        public async Task Test_GetSingleAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
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
        public async Task Test_ListAsync()
        {
            //Arrange
            RepositoryBase<Race> raceRepositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < 10; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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
                        Name = $"{GetActualMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                await raceRepositoryBase.InsertAsync(r);
            }

            var result = await raceRepositoryBase.ListAsync(this._raceQuery.ByDescription(GetActualMethodName() + "_"));

            var todas = await raceRepositoryBase.ListAllAsync();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(races.Count, result.Count());
        }

        [TestMethod]
        public async Task Test_ListAsync_Sorting()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race1 = new Race()
            {
                Date = DateTime.Now,
                Description = $"{GetActualMethodName()}_A",
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
                Description = $"{GetActualMethodName()}_B",
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
                Description = $"{GetActualMethodName()}_CD",
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
                Description = $"{GetActualMethodName()}_CD",
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

            string racePrefix = GetActualMethodName() + "_";
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
        public async Task Test_ListPagingAsync_Page1()
        {
            //Arrange
            int itemsPerPage = 10;
            int searchPage = 1;
            int totalItems = 50;

            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < totalItems; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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
                        Name = $"{GetActualMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                await repositoryBase.InsertAsync(r);
            }

            var result = await repositoryBase.ListPagingAsync(this._raceQuery.ByDescription(GetActualMethodName()), searchPage, itemsPerPage);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Paging);
            Assert.IsNotNull(result.List);
            Assert.AreEqual(searchPage, result.Paging.CurrentPage);
            Assert.AreEqual(itemsPerPage, result.Paging.ItemsPerPage);
            Assert.AreEqual(totalItems, result.Paging.TotalItems);
            Assert.AreEqual(itemsPerPage, result.List.Count());
        }

        [TestMethod]
        public async Task Test_ListPagingAsync_Page5()
        {
            //Arrange
            int itemsPerPage = 10;
            int totalItemsLastPage = 9;
            int searchPage = 5;
            int totalItems = 49;

            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < totalItems; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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
                        Name = $"{GetActualMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                await repositoryBase.InsertAsync(r);
            }

            var result = await repositoryBase.ListPagingAsync(this._raceQuery.ByDescription(GetActualMethodName()), searchPage, itemsPerPage);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Paging);
            Assert.IsNotNull(result.List);
            Assert.AreEqual(searchPage, result.Paging.CurrentPage);
            Assert.AreEqual(itemsPerPage, result.Paging.ItemsPerPage);
            Assert.AreEqual(totalItems, result.Paging.TotalItems);
            Assert.AreEqual(totalItemsLastPage, result.List.Count());
        }

        [TestMethod]
        public async Task Test_ListAllAsync()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < 10; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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

            var dataInContext = this._context.Set<Race>().ToList();
            var result = await repositoryBase.ListAllAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataInContext.Count, result.Count());
        }

        [TestMethod]
        public async Task Test_ExistsAsync_Exists()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            await repositoryBase.InsertAsync(race);
            bool exists = await repositoryBase.ExistsAsync(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task Test_ExistsAsync_NotExists()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            //Act
            bool exists = await repositoryBase.ExistsAsync(this._raceQuery.ById(0));

            //Assert
            Assert.IsFalse(exists);
        }
        #endregion

        #region [ Helpers ]

        #region [ Sync ]

        [TestMethod]
        public void Test_Count()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            repositoryBase.Insert(race);
            int count = repositoryBase.Count(this._raceQuery.ById(race.Id));

            //Assert
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void Test_Update()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            string updatedDescription = "UPDATED_UNIT_TEST";

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
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
            repositoryBase.Insert(race);
            race.Description = updatedDescription;
            repositoryBase.Update(race);

            race = repositoryBase.GetSingle(this._raceQuery.ById(race.Id));

            //Assert
            Assert.AreEqual(updatedDescription, race.Description);
        }

        [TestMethod]
        public void Test_Insert()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            repositoryBase.Insert(race);

            //Assert
            Assert.IsTrue(race.Id > 0);
        }

        [TestMethod]
        public void Test_Delete()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            repositoryBase.Insert(race);
            repositoryBase.Delete(race);
            var entityAfterDeletion = repositoryBase.GetSingle(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsNull(entityAfterDeletion);
        }

        [TestMethod]
        public void Test_GetSingle()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
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
            repositoryBase.Insert(race);
            var entity = repositoryBase.GetSingle(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsNotNull(entity);
            Assert.AreEqual(entity.Id, race.Id);
        }

        [TestMethod]
        public void Test_List()
        {
            //Arrange
            RepositoryBase<Race> raceRepositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < 10; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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
                        Name = $"{GetActualMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                raceRepositoryBase.Insert(r);
            }

            var result = raceRepositoryBase.List(this._raceQuery.ByDescription(GetActualMethodName() + "_"));

            var todas = raceRepositoryBase.ListAll();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(races.Count, result.Count());
        }

        [TestMethod]
        public void Test_List_Sorting()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race1 = new Race()
            {
                Date = DateTime.Now,
                Description = $"{GetActualMethodName()}_A",
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
                Description = $"{GetActualMethodName()}_B",
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
                Description = $"{GetActualMethodName()}_CD",
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
                Description = $"{GetActualMethodName()}_CD",
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
            repositoryBase.Insert(race1);
            repositoryBase.Insert(race2);
            repositoryBase.Insert(race3);
            repositoryBase.Insert(race4);

            string racePrefix = GetActualMethodName() + "_";
            var result = (repositoryBase.List(
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
        public void Test_ListPaging_Page1()
        {
            //Arrange
            int itemsPerPage = 10;
            int searchPage = 1;
            int totalItems = 50;

            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < totalItems; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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
                        Name = $"{GetActualMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                repositoryBase.Insert(r);
            }

            var result = repositoryBase.ListPaging(this._raceQuery.ByDescription(GetActualMethodName()), searchPage, itemsPerPage);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Paging);
            Assert.IsNotNull(result.List);
            Assert.AreEqual(searchPage, result.Paging.CurrentPage);
            Assert.AreEqual(itemsPerPage, result.Paging.ItemsPerPage);
            Assert.AreEqual(totalItems, result.Paging.TotalItems);
            Assert.AreEqual(itemsPerPage, result.List.Count());
        }

        [TestMethod]
        public void Test_ListPaging_Page5()
        {
            //Arrange
            int itemsPerPage = 10;
            int totalItemsLastPage = 9;
            int searchPage = 5;
            int totalItems = 49;

            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < totalItems; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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
                        Name = $"{GetActualMethodName()}_{i}"
                    }
                };

                races.Add(race);
            }

            //Act
            foreach (var r in races)
            {
                repositoryBase.Insert(r);
            }

            var result = repositoryBase.ListPaging(this._raceQuery.ByDescription(GetActualMethodName()), searchPage, itemsPerPage);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Paging);
            Assert.IsNotNull(result.List);
            Assert.AreEqual(searchPage, result.Paging.CurrentPage);
            Assert.AreEqual(itemsPerPage, result.Paging.ItemsPerPage);
            Assert.AreEqual(totalItems, result.Paging.TotalItems);
            Assert.AreEqual(totalItemsLastPage, result.List.Count());
        }

        [TestMethod]
        public void Test_ListAll()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);
            List<Race> races = new List<Race>();
            for (int i = 0; i < 10; i++)
            {
                Race race = new Race()
                {
                    Date = DateTime.Now,
                    Description = $"{GetActualMethodName()}_{i}",
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
                repositoryBase.Insert(r);
            }

            var dataInContext = this._context.Set<Race>().ToList();
            var result = repositoryBase.ListAll();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataInContext.Count, result.Count());
        }

        [TestMethod]
        public void Test_Exists_Exists()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            Race race = new Race()
            {
                Date = DateTime.Now,
                Description = GetActualMethodName(),
                RaceTypeId = 1,
                TotalLaps = 10,
                UploadDate = DateTime.Now,
                UploaderId = 1
            };

            //Act
            repositoryBase.Insert(race);
            bool exists = repositoryBase.Exists(this._raceQuery.ById(race.Id));

            //Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void Test_Exists_NotExists()
        {
            //Arrange
            RepositoryBase<Race> repositoryBase = new RepositoryBase<Race>(this._context);

            //Act
            bool exists = repositoryBase.Exists(this._raceQuery.ById(0));

            //Assert
            Assert.IsFalse(exists);
        }

        #endregion

        static string GetActualMethodName([CallerMemberName]string name = null)
        {
            return name;
        }
        #endregion
    }
}
