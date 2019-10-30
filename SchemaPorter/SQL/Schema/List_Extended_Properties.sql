

SELECT 
	 sch.name AS SCHEMA_NAME 
	,o.name AS OBJECT_NAME 
	,clmns.name AS COLUMN_NAME 
	,o.type_desc 
	,ep.* 
FROM sys.extended_properties AS ep 

LEFT JOIN sys.objects AS o 
	ON o.object_id = ep.major_id 

LEFT JOIN sys.schemas AS sch 
	ON sch.schema_id = o.schema_id 

LEFT JOIN sys.columns AS clmns 
    ON clmns.column_id = ep.minor_id 
	AND clmns.object_id = ep.major_id 
    -- AND ep.class = 1

WHERE (1=1) 
-- AND type_desc NOT IN ( 'VIEW', 'USER_TABLE' ) 
-- AND ep.name NOT IN ( 'microsoft_database_tools_support', 'Version', 'MS_DiagramPane1', 'MS_DiagramPane2', 'MS_DiagramPaneCount' )

-- sp_addextendedproperty 
-- https://gist.github.com/ststeiger/5264fb92f48f0050a67722f59afbf6b3
-- https://www.red-gate.com/simple-talk/sql/database-delivery/scripting-description-database-tables-using-extended-properties/
-- https://www.mssqltips.com/sqlservertip/5384/working-with-sql-server-extended-properties/
-- https://docs.microsoft.com/en-us/sql/relational-databases/system-catalog-views/extended-properties-catalog-views-sys-extended-properties?view=sql-server-ver15

ORDER BY 
	 type_desc 
	,SCHEMA_NAME 
	,OBJECT_NAME 
	,COLUMN_NAME 
	 