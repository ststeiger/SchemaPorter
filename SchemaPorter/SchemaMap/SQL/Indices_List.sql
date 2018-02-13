
SELECT 
	 s.name AS [schema] 
	,T.name AS [table] 
	,I.name AS [index] 
	,I.filter_definition

	,I.is_unique_constraint
	,I.is_unique
	,I.is_primary_key
	
	 
	,CASE 
		WHEN T.is_ms_shipped = 1 
			THEN 1 
		WHEN 
			(
				SELECT major_id FROM sys.extended_properties 
				WHERE 
					major_id = T.object_id 
					AND minor_id = 0 
					AND class = 1 
					AND name = N'microsoft_database_tools_support' 
			)  
			IS NOT NULL 
		THEN 1 
		ELSE 0 
	END AS is_ms_shipped 
FROM sys.indexes AS I

INNER JOIN sys.tables AS T 
	ON T.object_id = I.object_id 

LEFT JOIN sys.schemas s 
	ON s.schema_id = t.schema_id

WHERE I.is_primary_key = 0 
AND I.is_unique = 1 
AND is_unique_constraint = 0 
-- AND I.filter_definition IS NOT NULL 
