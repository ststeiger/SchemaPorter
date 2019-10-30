
SELECT 
	 sch.name AS CONSTRAINT_SCHEMA 
	,dc.name AS CONSTRAINT_NAME 

	,tsch.name AS CONSTRAINT_TABLE_SCHEMA 
	,t.name AS CONSTRAINT_TABLE 
	 
	,col.name AS COLUMN_NAME  
	,dc.definition AS CONSTRAINT_DEFINITION 


	,N'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''' + QUOTENAME(tsch.name) + N'.' + QUOTENAME(t.name) + N''') AND type in (N''U''))
ALTER TABLE ' + QUOTENAME(tsch.name) + N'.' + QUOTENAME(t.name) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name) + N'
;' AS cmdDrop 
	
	

	,N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''' + QUOTENAME(sch.name) + N'.' + QUOTENAME(dc.name) + N''') AND type = ''D'')
BEGIN
ALTER TABLE ' + QUOTENAME(tsch.name) + N'.' + QUOTENAME(t.name) + N' ADD CONSTRAINT ' + QUOTENAME(dc.name) + N' DEFAULT (' + dc.definition + N') FOR ' + QUOTENAME(col.name) + N' 
END
;
' AS cmdAlter 

FROM sys.default_constraints AS dc 

LEFT JOIN sys.schemas AS sch 
	ON sch.schema_id = dc.schema_id 

LEFT JOIN sys.tables AS t 
	ON t.object_id = dc.parent_object_id 

LEFT JOIN sys.schemas AS tsch 
	ON tsch.schema_id = t.schema_id 

LEFT JOIN sys.columns AS col 
	ON col.column_id = dc.parent_column_id 
	AND col.object_id = dc.parent_object_id 

WHERE (1=1)

/* 
-- simpler, but slower: 
AND 
(
	t.is_ms_shipped = 0  
	AND 
	( 
		SELECT ep.major_id 
		FROM sys.extended_properties AS ep 
		WHERE ep.major_id = t.object_id 
		AND ep.minor_id = 0 
		AND ep.class = 1 
		AND ep.name = N'microsoft_database_tools_support' 
	) IS NULL 
) 
*/

AND 
(
	CAST
	(
		CASE 
			WHEN t.is_ms_shipped = 1 
				THEN 1 
			WHEN ( 
					SELECT major_id 
					FROM sys.extended_properties 
					WHERE major_id = t.object_id 
					AND minor_id = 0 
					AND class = 1 
					AND name = N'microsoft_database_tools_support' 
				 ) IS NOT NULL 
				THEN 1 
			ELSE 0 
		END 
		AS bit 
	) = 'false' 
) 


ORDER BY 
	 sch.name 
	,dc.name 
	,tsch.name 
	,t.name 
	 