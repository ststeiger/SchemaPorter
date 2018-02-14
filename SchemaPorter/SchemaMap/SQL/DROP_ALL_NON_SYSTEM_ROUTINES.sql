
 -- DROP all routines 
DECLARE @routine_schema nvarchar(255) 
DECLARE @routine_name nvarchar(255) 
DECLARE @routine_type nvarchar(255) 

DECLARE @sql nvarchar(MAX) 


DECLARE cur cursor FOR 
(
	SELECT 
		 SPECIFIC_SCHEMA 
		,SPECIFIC_NAME
		,ROUTINE_TYPE
		--,DATA_TYPE 
	FROM INFORMATION_SCHEMA.ROUTINES 
	WHERE (1=1) 
	-- AND ROUTINE_TYPE = 'PROCEDURE' 
	-- AND ROUTINE_TYPE = 'FUNCTION' 
	-- AND DATA_TYPE = 'TABLE' 
	AND 
	(
		(
			SELECT major_id FROM sys.extended_properties 
			WHERE major_id = OBJECT_ID(QUOTENAME(SPECIFIC_SCHEMA) + N'.' + QUOTENAME(SPECIFIC_NAME)) 
			AND minor_id = 0 
			AND class = 1 
			AND name = N'microsoft_database_tools_support' 
		) IS NULL 
	)
) 
OPEN cur 
FETCH NEXT FROM cur INTO @routine_schema, @routine_name, @routine_type 
WHILE @@fetch_status = 0 
BEGIN 
	SET @sql = N'DROP ' + @routine_type + ' ' + QUOTENAME(@routine_schema) + N'.' + QUOTENAME(@routine_name) + N'; '
	PRINT @sql 
	EXECUTE(@sql) 
	FETCH NEXT FROM cur INTO @routine_schema, @routine_name, @routine_type  
END
CLOSE cur 
DEALLOCATE cur 
