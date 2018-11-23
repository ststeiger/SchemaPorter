
SELECT DISTINCT 
	 c.column_name 
	,c.data_type 
	,c.is_nullable 
	,tc.constraint_type 
	,CONVERT(int, c.numeric_precision) AS numeric_precision 
	,c.numeric_scale 
	,c.character_maximum_length 
	,c.table_name 
	,c.ordinal_position 
	,tc.constraint_name 
	
	,COLUMNPROPERTY(OBJECT_ID(c.table_schema + '.' + c.table_name), c.column_name, 'IsIdentity') AS IsIdentity 

	,
	(
		SELECT 
			CASE WHEN COUNT(1) = 0 THEN 0 ELSE 1 END 
		FROM information_schema.table_constraints x 
		
		INNER JOIN information_schema.constraint_column_usage ccux 
			ON c.table_name = ccux.table_name 
			AND c.column_name = ccux.column_name 
			AND c.table_schema = ccux.table_schema 

		WHERE x.constraint_type = 'UNIQUE' 
		AND x.table_schema = ccux.table_schema 
		AND x.constraint_name = ccux.constraint_name 
	) IsUnique 
FROM information_schema.columns c 

LEFT JOIN 
(
	information_schema.constraint_column_usage ccu
	INNER JOIN information_schema.table_constraints tc 
	ON 
	(
		tc.table_schema = ccu.table_schema
		AND tc.constraint_name = ccu.constraint_name
		AND NOT tc.constraint_type IN ('CHECK','UNIQUE')
	)
) 
	ON (
		c.table_schema = ccu.table_schema
		AND c.table_name = ccu.table_name
		AND c.column_name = ccu.column_name
	)

WHERE c.table_name = 'T_ZO_Floor_DWG'
AND c.table_schema ='dbo'
ORDER BY c.table_name, c.ordinal_position
