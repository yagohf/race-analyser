using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Queries
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void Test_Filter()
        {
            //Arrange.
            var query = new Query<User>();

            //Act.
            query.Filter(x => x.Name.Equals("UNIT_TEST"));

            //Assert.
            Assert.IsNotNull(query);
            Assert.AreEqual(1, query.Criteria.Count);
        }

        [TestMethod]
        public void Test_AddInclude_Object()
        {
            //Arrange.
            var query = new Query<User>();

            //Act.
            query.AddInclude(x => x.Races);

            //Assert.
            Assert.IsNotNull(query);
            Assert.IsNotNull(query.Includes);
            Assert.AreEqual(1, query.Includes.Count);
        }

        [TestMethod]
        public void Test_AddInclude_String()
        {
            //Arrange.
            var query = new Query<User>();

            //Act.
            query.AddInclude("Races");

            //Assert.
            Assert.IsNotNull(query);
            Assert.IsNotNull(query.IncludeStrings);
            Assert.AreEqual(1, query.IncludeStrings.Count);
        }

        [TestMethod]
        public void Test_SortBy()
        {
            //Arrange.
            var query = new Query<User>();

            //Act.
            query.SortBy(x => x.Name);

            //Assert.
            Assert.IsNotNull(query);
            Assert.IsNotNull(query.SortExpressions);
            Assert.AreEqual(1, query.SortExpressions.Count);
        }

        [TestMethod]
        public void Test_SortByDescending()
        {
            //Arrange.
            var query = new Query<User>();

            //Act.
            query.SortByDescending(x => x.Name);

            //Assert.
            Assert.IsNotNull(query);
            Assert.IsNotNull(query.SortExpressions);
            Assert.AreEqual(1, query.SortExpressions.Count);
        }

        [TestMethod]
        public void Test_SortBy_Multiple()
        {
            //Arrange.
            var query = new Query<User>();

            //Act.
            query.SortByDescending(x => x.Name).SortBy(x => x.Id);

            //Assert.
            Assert.IsNotNull(query);
            Assert.IsNotNull(query.SortExpressions);
            Assert.AreEqual(2, query.SortExpressions.Count);
        }

        [TestMethod]
        public void Test_Equals_TwoInstances()
        {
            //Arrange.
            var query1 = new Query<User>();
            var query2 = new Query<User>();

            //Act.
            bool saoIguais = query1.Equals(query2);

            //Assert.
            Assert.IsFalse(saoIguais);
        }

        [TestMethod]
        public void Test_Equals_OneInstanceOneNullObject()
        {
            //Arrange.
            Query<User> query1 = new Query<User>();
            Query<User> query2 = null;

            //Act.
            bool saoIguais = query1.Equals(query2);

            //Assert.
            Assert.IsFalse(saoIguais);
        }

        [TestMethod]
        public void Test_GetHashCode()
        {
            //Arrange.
            Query<User> query1 = new Query<User>();
            Query<User> query2 = new Query<User>();

            //Act.
            int hash1 = query1.GetHashCode();
            int hash2 = query2.GetHashCode();

            //Assert.
            Assert.IsNotNull(hash1);
            Assert.IsNotNull(hash2);
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
