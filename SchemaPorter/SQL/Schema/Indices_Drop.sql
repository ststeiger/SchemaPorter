
DECLARE @qry nvarchar(MAX);
SELECT @qry = (
	SELECT  
		N'IF EXISTS(SELECT * FROM sys.indexes WHERE name = ''' + REPLACE(i.name, N'''', N'''''') + N''' ' 
		+ N'AND object_id = OBJECT_ID(''' + QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) + N''')) ' + 
		+ N'     DROP INDEX ' + QUOTENAME(i.name) 
		+ ' ON ' + QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) + N'; ' AS [text()]
	FROM sys.indexes AS i 
	INNER JOIN sys.objects AS o ON i.object_id = o.object_id
	INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id
	WHERE o.type <> 'S' 
	AND i.is_primary_key <> 1 
	AND i.is_unique_constraint = 0 
	AND i.index_id > 0 
	AND s.name <> N'sys' 
	
	FOR XML PATH(''), TYPE 
).value('.', 'nvarchar(MAX)');


SELECT @qry 
-- EXECUTE @qry
