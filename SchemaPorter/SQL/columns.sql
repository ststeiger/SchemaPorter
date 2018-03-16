﻿
SELECT 
	 TABLE_NAME
	,COLUMN_NAME 
	,ORDINAL_POSITION
	,
	DATA_TYPE + 

	CASE
		WHEN DATA_TYPE LIKE '%varchar' AND CHARACTER_MAXIMUM_LENGTH = -1 THEN '(MAX)' 
		WHEN DATA_TYPE = 'varbinary' AND CHARACTER_MAXIMUM_LENGTH = -1 THEN '(MAX)' 
		ELSE ISNULL('(' + CAST(CHARACTER_MAXIMUM_LENGTH AS varchar(20)) + ')', '') 
	END 
	+
	CASE 
		WHEN IS_NULLABLE = 'YES' THEN ' NULL'
		ELSE ' not null'
		-- ELSE ''
	END 
	AS SQL_TYPE  

	,NULLIF(COLUMN_DEFAULT, '(NULL)') AS COLUMN_DEFAULT 

	--,CHARACTER_MAXIMUM_LENGTH

	,
	CASE
		WHEN DATA_TYPE LIKE '%char' THEN 'string' 
		WHEN DATA_TYPE = 'bigint' THEN 'long' 
		WHEN DATA_TYPE = 'int' THEN 'int' 
		WHEN DATA_TYPE = 'bit' THEN 'bool' 
		WHEN DATA_TYPE = 'datetime' THEN 'System.DateTime' 
		WHEN DATA_TYPE = 'datetime2' THEN 'System.DateTime' 
		WHEN DATA_TYPE = 'date' THEN 'System.DateTime' 
		--WHEN DATA_TYPE = 'float' THEN 'System.Decimal' -- Nono, entity doesn't like that...
		WHEN DATA_TYPE = 'float' THEN 'double' 
		WHEN DATA_TYPE = 'decimal' THEN 'System.Decimal' 
		WHEN DATA_TYPE = 'varbinary' THEN 'byte[]' 
		ELSE 'unknown' 
	END 
	+ 
	CASE 
		WHEN DATA_TYPE LIKE '%char' THEN '' 
		WHEN DATA_TYPE LIKE '%binary' THEN '' 
		WHEN IS_NULLABLE = 'YES' THEN '?'
		ELSE ''
	END AS DOTNET_TYPE   


	,CASE 
		WHEN IS_NULLABLE = 'YES' THEN 0 
		ELSE 1 
	END 
	AS IS_REQUIRED 


	,CASE
		WHEN DATA_TYPE LIKE '%char' THEN CHARACTER_MAXIMUM_LENGTH
		WHEN DATA_TYPE = '%binary' THEN CHARACTER_OCTET_LENGTH  
		ELSE CAST(NULL AS integer) 
	END AS MAX_LENGTH  
	
	,CASE
		WHEN DATA_TYPE = 'datetime' THEN 'datetime' 
		WHEN DATA_TYPE = 'date' THEN 'date' 
		ELSE CAST(NULL AS national character varying(255)) 
	END AS COLUMN_TYPE 

	--,*
FROM INFORMATION_SCHEMA.COLUMNS 
ORDER BY TABLE_NAME, ORDINAL_POSITION 



-- SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS 