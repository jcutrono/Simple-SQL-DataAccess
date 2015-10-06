using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Tests
{
    public class DataAccessTests
    {
        private TestDataAccess _dal;

        [SetUp]
        public void SetUp()
        {
            _dal = new TestDataAccess();
        }

        [Test]
        public void Should_Call_SQL_Paged()
        {
            IList<string> values = new List<string>();

            string sql = _dal.GenerateSqlPagedDataString("firstColumn, secondColumn", "firstColumn", "SELECT firstColumn, secondColumn FROM LotsOData");
            var parameters = new[] { new SqlParameter("@PageNum", SqlDbType.Int) { Value = 1 }, new SqlParameter("@PageSize", SqlDbType.Int) { Value = 10 } };

            _dal.GetData(sql, parameters,
                reader =>
                {
                    while (reader.Read())
                    {
                        values.Add(reader.GetString(1));
                    }
                });

            Assert.That(values.Count, Is.EqualTo(10));
        }
    }

    public class TestDataAccess : DataAccess
    {
        public TestDataAccess() : base("Data Source=localhost;Initial Catalog=DummyData;User ID=testuser;Password=password;")
        {

        }
    }
}
