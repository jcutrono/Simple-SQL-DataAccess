-- Create the table
CREATE TABLE LotsOData(
	firstColumn INT,
	secondColumn VARCHAR(200)
)
GO

-- Insert the data
DECLARE @RowCount INT
DECLARE @RowString VARCHAR(10)
DECLARE @Random INT
DECLARE @Upper INT
DECLARE @Lower INT
DECLARE @InsertDate DATETIME

SET @Lower = -730
SET @Upper = -1
SET @RowCount = 0

BEGIN TRAN
WHILE @RowCount < 30000
BEGIN
	SET @RowString = CAST(@RowCount AS VARCHAR(10))
	SELECT @Random = ROUND(((@Upper - @Lower -1) * RAND() + @Lower), 0)
	SET @InsertDate = DATEADD(dd, @Random, GETDATE())
	
	INSERT INTO LotsOData
		(firstColumn
		,secondColumn)
	VALUES
		(REPLICATE('0', 10 - DATALENGTH(@RowString)) + @RowString
		, @InsertDate)

	SET @RowCount = @RowCount + 1
END
COMMIT

-- Look at TOP 10
SELECT firstColumn, secondColumn FROM ( SELECT *, ROW_NUMBER() OVER(ORDER BY firstColumn) as RowNum FROM ( SELECT firstColumn, secondColumn FROM LotsOData ) AS temp ) as DerivedTableName 
WHERE RowNum BETWEEN (1 - 1) * 10 + 1 AND 1 * 10