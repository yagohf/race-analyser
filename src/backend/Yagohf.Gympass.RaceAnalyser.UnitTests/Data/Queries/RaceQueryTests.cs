using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Queries
{
    [TestClass]
    public class RaceQueryTests
    {
        private readonly RaceQuery _raceQuery;

        public RaceQueryTests()
        {
            this._raceQuery = new RaceQuery();
        }

        [TestMethod]
        public void Test_ById()
        {
            //Arrange


            //Act
            var query = this._raceQuery.ById(1);

            //Assert
            Assert.IsNotNull(query);
            Assert.AreEqual(1, query.Includes.Count);
            Assert.AreEqual(1, query.Criteria.Count);
            Assert.AreEqual(0, query.SortExpressions.Count);
        }

        [TestMethod]
        public void Test_ByDescription()
        {
            //Arrange


            //Act
            var query = this._raceQuery.ByDescription("Test");

            //Assert
            Assert.IsNotNull(query);
            Assert.AreEqual(3, query.Includes.Count);
            Assert.AreEqual(1, query.Criteria.Count);
            Assert.AreEqual(1, query.SortExpressions.Count);
        }
    }
}
