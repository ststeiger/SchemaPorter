 
 -- DROP all user defined views
DECLARE @view_schema nvarchar(255) 
DECLARE @view_name nvarchar(255) 
DECLARE @sql nvarchar(MAX) 


DECLARE cur cursor FOR (
	SELECT 
		 TABLE_SCHEMA
		,TABLE_NAME
	FROM INFORMATION_SCHEMA.VIEWS 
) 
OPEN cur 
FETCH NEXT FROM cur INTO @view_schema, @view_name 
WHILE @@fetch_status = 0 
BEGIN 
	SET @sql = N'DROP VIEW ' + QUOTENAME(@view_schema) + N'.' + QUOTENAME(@view_name) + N'; '
	PRINT @sql 
	EXECUTE(@sql) 
	FETCH NEXT FROM cur INTO @view_schema, @view_name 
END
CLOSE cur 
DEALLOCATE cur 
