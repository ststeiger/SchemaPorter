SELECT 
	 sch.name AS SPECIFIC_SCHEMA 
	,o.name AS SPECIFIC_NAME 

	,sch.name AS ROUTINE_SCHEMA 
	,o.name AS ROUTINE_NAME 
	 
	,CONVERT(nvarchar(20), 
		CASE 
			WHEN o.type IN ('P','PC', 'X')  THEN 'PROCEDURE' 
			ELSE 'FUNCTION' 
		END 
	 ) AS ROUTINE_TYPE 
	 
	,m.definition AS ROUTINE_DEFINITION 


	,CONVERT(sysname, 
		CASE  
			WHEN o.type IN ('TF', 'IF', 'FT') THEN N'TABLE'  
			ELSE ISNULL(TYPE_NAME(c.system_type_id),  TYPE_NAME(c.user_type_id)) 
		END
	 ) AS DATA_TYPE 
	 
	,COLUMNPROPERTY(c.object_id, c.name, 'charmaxlen') AS CHARACTER_MAXIMUM_LENGTH 
	,COLUMNPROPERTY(c.object_id, c.name, 'octetmaxlen') AS CHARACTER_OCTET_LENGTH 


	,CONVERT(tinyint, 
		CASE -- int/decimal/numeric/real/float/money  
			WHEN c.system_type_id IN (48, 52, 56, 59, 60, 62, 106, 108, 122, 127) THEN c.precision  
		END
	 ) AS NUMERIC_PRECISION 
	 
	,CONVERT(int, 
		CASE -- datetime/smalldatetime  
			WHEN c.system_type_id IN (40, 41, 42, 43, 58, 61) THEN NULL  
			ELSE ODBCSCALE(c.system_type_id, c.scale) 
		END
	) AS NUMERIC_SCALE 
	 
	,CONVERT(smallint, 
		CASE -- datetime/smalldatetime  
			WHEN c.system_type_id IN (40, 41, 42, 43, 58, 61) THEN ODBCSCALE(c.system_type_id, c.scale) 
		END
	 ) AS DATETIME_PRECISION 
	 
	,CONVERT(nvarchar(30), 
		CASE  
			WHEN o.type IN ('P ', 'FN', 'TF', 'IF') THEN 'SQL'  
			ELSE 'EXTERNAL' 
		END
	) AS ROUTINE_BODY 
	
	,CONVERT(nvarchar(10), 
		CASE 
			WHEN ObjectProperty(o.object_id, 'IsDeterministic') = 1 THEN 'YES' 
			ELSE 'NO' 
		END
	 ) AS IS_DETERMINISTIC 
FROM sys.objects AS o 
LEFT JOIN sys.parameters AS c ON (c.object_id = o.object_id AND c.parameter_id = 0)  
INNER JOIN sys.schemas AS sch ON sch.schema_id = o.schema_id 
INNER JOIN sys.sql_modules AS m on m.object_id = o.object_id

  -- AF: Aggregate function (CLR)
-- WHERE o.type IN (  'AF', )
-- WHERE o.type IN ('P', 'PC', 'FN', 'TF', 'IF', 'FT', 'AF', 'IS', 'FS')  
-- P  = SQL Stored Procedure, PC = Assembly (CLR) stored-procedure
-- P : stored procedure, PC : assembly stored procedure, X : extended stored proc 
-- WHERE o.type IN ('P','PC', 'X')  -- PROCEDURE 
-- TF: table function, IF: inline table-valued function, FT: assembly table function
-- WHERE o.type IN ('TF', 'IF', 'FT') -- N'TABLE'  
-- FN: scalar function, IS: inline scalar function, FS: assembly scalar function
-- WHERE o.type IN ('FN', 'IS', 'FS')

-- SELECT * from master..spt_values WHERE type = 'O9T'

/*
SELECT 
	 SPECIFIC_SCHEMA
	,SPECIFIC_NAME
	,ROUTINE_SCHEMA
	,ROUTINE_NAME
	,*
	,DATA_TYPE
	,CHARACTER_MAXIMUM_LENGTH
	,CHARACTER_OCTET_LENGTH
	,NUMERIC_PRECISION
	,NUMERIC_SCALE
	,DATETIME_PRECISION
	,ROUTINE_BODY
	,IS_DETERMINISTIC
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_TYPE = 'FUNCTION' 
AND DATA_TYPE <> 'TABLE' 
*/
