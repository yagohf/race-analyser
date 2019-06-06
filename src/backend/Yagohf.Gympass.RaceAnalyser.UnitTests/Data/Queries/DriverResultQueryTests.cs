using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Queries
{
    [TestClass]
    public class DriverResultQueryTests
    {
        private readonly DriverResultQuery _driverResultQuery;

        public DriverResultQueryTests()
        {
            this._driverResultQuery = new DriverResultQuery();
        }

        [TestMethod]
        public void Test_ByRace()
        {
            //Arrange


            //Act
            var query = this._driverResultQuery.ByRace(1);

            //Assert
            Assert.IsNotNull(query);
            Assert.AreEqual(0, query.Includes.Count);
            Assert.AreEqual(1, query.Criteria.Count);
            Assert.AreEqual(1, query.SortExpressions.Count);
        }
    }
}
