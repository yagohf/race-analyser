using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Queries
{
    [TestClass]
    public class RaceTypeQueryTests
    {
        private readonly RaceTypeQuery _raceTypeQuery;

        public RaceTypeQueryTests()
        {
            this._raceTypeQuery = new RaceTypeQuery();
        }

        [TestMethod]
        public void Test_ById()
        {
            //Arrange


            //Act
            var query = this._raceTypeQuery.ById(1);

            //Assert
            Assert.IsNotNull(query);
            Assert.AreEqual(0, query.Includes.Count);
            Assert.AreEqual(1, query.Criteria.Count);
            Assert.AreEqual(0, query.SortExpressions.Count);
        }
    }
}
