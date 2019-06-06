using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Queries
{
    [TestClass]
    public class UserQueryTests
    {
        private readonly UserQuery _userQuery;

        public UserQueryTests()
        {
            this._userQuery = new UserQuery();
        }

        [TestMethod]
        public void Test_ByLogin()
        {
            //Arrange


            //Act
            var query = this._userQuery.ByLogin("testuser");

            //Assert
            Assert.IsNotNull(query);
            Assert.AreEqual(0, query.Includes.Count);
            Assert.AreEqual(1, query.Criteria.Count);
            Assert.AreEqual(0, query.SortExpressions.Count);
        }

        [TestMethod]
        public void Test_ByLoginWithRaces()
        {
            //Arrange


            //Act
            var query = this._userQuery.ByLoginWithRaces("testuser");

            //Assert
            Assert.IsNotNull(query);
            Assert.AreEqual(1, query.Includes.Count);
            Assert.AreEqual(1, query.Criteria.Count);
            Assert.AreEqual(0, query.SortExpressions.Count);
        }

        [TestMethod]
        public void Test_ByLoginAndPass()
        {
            //Arrange


            //Act
            var query = this._userQuery.ByLoginAndPass("testuser", "123mudar");

            //Assert
            Assert.IsNotNull(query);
            Assert.AreEqual(0, query.Includes.Count);
            Assert.AreEqual(1, query.Criteria.Count);
            Assert.AreEqual(0, query.SortExpressions.Count);
        }
    }
}
