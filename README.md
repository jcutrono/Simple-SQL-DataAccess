# Simple-SQL-DataAccess
Simple methods to encapsulate ad-hoc SQL. Includes SQL paging

----
## usage
1. ExecuteNonQuery
2. ExecuteScalar
3. GetData

----

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

----
## changelog
* 06-Oct-2015 Initial Commit