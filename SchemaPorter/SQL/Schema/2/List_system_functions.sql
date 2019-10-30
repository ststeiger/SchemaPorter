
SELECT *
FROM sys.all_objects AS o 
WHERE (1=1) 
AND o.type IN ('FN', 'IS', 'FS', 'TF', 'IF', 'FT', 'AF')  -- scalar, inline table-valued, table-valued
AND 
(
	o.is_ms_shipped = 1 
	OR 
	( 
		SELECT ep.major_id 
		FROM sys.extended_properties AS ep 
		WHERE ep.major_id = o.object_id 
		AND ep.minor_id = 0 
		AND ep.class = 1 
		AND ep.name = N'microsoft_database_tools_support' 
	) IS NOT NULL 
) 

ORDER BY 
	 type_desc 
	,name 
	 