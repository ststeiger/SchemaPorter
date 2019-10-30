
DECLARE @routineType int 
-- https://docs.microsoft.com/en-us/sql/t-sql/language-elements/bitwise-operators-transact-sql?view=sql-server-ver15
SET @routineType = 4  

SELECT 
	 sch.name AS SPECIFIC_SCHEMA 
	,o.name AS SPECIFIC_NAME 
	 
	,sch.name AS ROUTINE_SCHEMA 
	,o.name AS ROUTINE_NAME 
	 
	,sc.ROUTINE_TYPE 
	,sc.DATA_TYPE 
	,sc.CHARACTER_MAXIMUM_LENGTH 
	,sc.CHARACTER_OCTET_LENGTH 
	,sc.NUMERIC_PRECISION 
	,sc.NUMERIC_SCALE 
	,sc.DATETIME_PRECISION 
	,sc.ROUTINE_BODY 
	,sc.IS_DETERMINISTIC 
	 
	,tParameters.params

	,CASE 
		WHEN asm.assembly_id IS NULL THEN m.definition 
		ELSE N'CREATE ' + sc.ROUTINE_TYPE + N' ' + QUOTENAME(sch.name) + N'.' + QUOTENAME(o.name) 
		+ COALESCE(N'( ' + NULLIF(tParameters.params, '')  + N' ) ', '') + '
RETURNS ' + tReturnType.DDL_NAME + N' WITH EXECUTE AS CALLER
AS EXTERNAL NAME ' + QUOTENAME(asm.name) + N'.' + QUOTENAME(am.assembly_class) + N'.' + QUOTENAME(am.assembly_method) + '' 
		COLLATE Latin1_General_CI_AS 
	END AS ROUTINE_DEFINITION 
	,asm.permission_set_desc
	,c.is_nullable AS IS_NULLABLE 
	 
	,t.name 
FROM sys.objects AS o 
LEFT JOIN sys.parameters AS c ON (c.object_id = o.object_id AND c.parameter_id = 0)  
LEFT JOIN sys.types AS t ON t.user_type_id = c.user_type_id 
INNER JOIN sys.schemas AS sch ON sch.schema_id = o.schema_id 
LEFT JOIN sys.sql_modules AS m on m.object_id = o.object_id
LEFT JOIN sys.assembly_modules AS am on am.object_id = o.object_id
LEFT JOIN sys.assemblies AS asm ON  asm.assembly_id = am.assembly_id

OUTER APPLY 
	(
		SELECT 
			 CONVERT(nvarchar(20), 
				CASE 
					WHEN o.type IN ('P','PC', 'X')  THEN 'PROCEDURE' 
					ELSE 'FUNCTION' 
				END 
			 ) AS ROUTINE_TYPE 
	 
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

	) AS sc 
	
OUTER APPLY 
	(
		SELECT 
			t.name 
			+ 
			CASE 
				WHEN t.name IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
					THEN '(' 
						+ 
						CASE WHEN sc.CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
							ELSE CONVERT
								(
									 varchar(4)
									,CHARACTER_MAXIMUM_LENGTH 
								)
						END 
						+ ')' 
				WHEN t.name IN ('decimal', 'numeric')
					THEN '(' + CONVERT(varchar(4), sc.NUMERIC_PRECISION)+', ' + CONVERT(varchar(4), sc.NUMERIC_SCALE) + ')' 
				WHEN t.name IN ('time', 'datetime2', 'datetimeoffset') 
					THEN N'(' + CAST(DATETIME_PRECISION AS national character varying(36)) + N')' 
				ELSE '' 
			END AS DDL_NAME 
	) AS tReturnType 
	


OUTER APPLY 
	(
	SELECT 
		COALESCE(
		STUFF(
		(
			SELECT 
				 N', ' 
				+ isp.PARAMETER_NAME + N' ' 
				+ CASE 
					WHEN isp.DATA_TYPE = 'table type' THEN userDefinedTypes.name 
					ELSE isp.DATA_TYPE 
				END 
				+ 
				CASE 
					WHEN isp.DATA_TYPE = 'table type' THEN ' readonly' 
					WHEN isp.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
						THEN N'(' 
							+ 
							CASE WHEN isp.CHARACTER_MAXIMUM_LENGTH = -1 THEN N'MAX'
								ELSE CONVERT
									(
											varchar(4)
										,isp.CHARACTER_MAXIMUM_LENGTH 
									)
							END 
							+ N')' 
					WHEN isp.DATA_TYPE IN ('decimal', 'numeric')
						THEN N'(' + CONVERT(varchar(4), isp.NUMERIC_PRECISION) + N', ' + CONVERT(varchar(4), isp.NUMERIC_SCALE) + N')'
					WHEN isp.DATA_TYPE IN ('time', 'datetime2', 'datetimeoffset') 
						THEN N'(' + CAST(isp.DATETIME_PRECISION AS national character varying(36)) + N')' 
					ELSE N'' 
				END 
				+ 
				CASE sc.ROUTINE_TYPE 
					WHEN 'PROCEDURE'
					THEN  
						CASE WHEN isp.PARAMETER_MODE <> 'IN'-- only for procedures
							THEN N' OUTPUT'
							ELSE N''
						END 
					ELSE ''
				END 
				AS [text()]  
			FROM INFORMATION_SCHEMA.PARAMETERS AS isp 
			
			INNER JOIN sys.objects AS oo
			INNER JOIN sys.schemas AS sch
				ON sch.schema_id = oo.schema_id 
				ON sch.name = isp.SPECIFIC_SCHEMA
				AND oo.name = isp.SPECIFIC_NAME
				
			INNER JOIN sys.parameters cc 
				ON cc.object_id = oo.object_id  
				AND cc.parameter_id = isp.ORDINAL_POSITION

			LEFT JOIN sys.types AS userDefinedTypes 
				ON userDefinedTypes.user_type_id = cc.user_type_id 
				AND userDefinedTypes.is_user_defined = 1
				
			WHERE (1=1) 
			AND asm.assembly_id IS NOT NULL -- only if we have a CLR-type 
			AND isp.PARAMETER_NAME <> N'' -- first parameter (return type) is unnamed 
			AND isp.SPECIFIC_NAME = o.name 
			AND isp.SPECIFIC_SCHEMA = sch.name 
			
			ORDER BY isp.ORDINAL_POSITION 

			FOR XML PATH(''), TYPE 
		).value('.', 'nvarchar(MAX)') 
		,1,2, '')
		, '') 
		AS params 
	) AS tParameters  

WHERE (1=1)  
AND 
(
	CAST
	(
		CASE 
			WHEN o.is_ms_shipped = 1 
				THEN 1 
			WHEN ( 
					SELECT major_id 
					FROM sys.extended_properties 
					WHERE major_id = o.object_id 
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

-- WHERE (@routineType & flag <> 0) 
AND 
(
	-- 0: All routines: 
	(
		(@routineType = 0)
		AND 
		o.type IN ('AF', 'FN', 'FS', 'FT', 'IF', 'IS', 'TF', 'P', 'PC', 'RF', 'X')  
	) 
	OR 
	-- 1: All functions: 
	(
		(@routineType & 1 <> 0)
		AND 
		o.type IN ('AF', 'FN', 'FS', 'FT', 'IF', 'IS', 'TF')  
	)
	OR 
	-- 2: All SQL-functions: 
	(
		(@routineType & 2 <> 0)
		AND 
		o.type IN ('FN', 'IF', 'IS', 'TF') 
	)
	OR 
	-- 4: All CLR functions: 
	(
		(@routineType & 4 <> 0)
		AND 
		o.type IN ('FS', 'FT')  
	)
	OR
	-- 8: All Aggregate functions: 
	(
		(@routineType & 8 <> 0)
		AND 
		o.type IN ('AF')  
	) 
	OR 
	-- 16: All table functions: 
	(
		(@routineType & 16 <> 0)
		AND 
		o.type IN ('FT', 'IF', 'TF')
	)
	OR 
	-- 32: All SQL table functions: 
	(
		(@routineType & 32 <> 0)
		AND 
		o.type IN ('IF', 'TF')
	) 
	OR 
	-- 64: All CLR table functions: 
	(
		(@routineType & 64 <> 0)
		AND 
		o.type IN ('FT')
	)
	OR 
	-- 128: ALL scalar functions: 
	(
		(@routineType & 128 <> 0)
		AND 
		o.type IN ('FN', 'FS', 'IS')
	)
	OR 
	-- 256: All SQL scalar functions: 
	(
		(@routineType & 256 <> 0)
		AND 
		o.type IN ('FN', 'IS')
	) 
	OR 
	-- 512: All CLR scalar functions: 
	(
		(@routineType & 512 <> 0)
		AND 
		o.type IN ('FS')
	)
	OR 
	-- 1024: All procedures: 
	(
		(@routineType & 1024 <> 0)
		AND 
		o.type IN ('P', 'PC', 'RF', 'X') 
	)
	OR 
	-- 2048: All SQL procedures: 
	( 
		(@routineType & 2048 <> 0) 
		AND 
		o.type IN ('P') 
	) 
	-- 4096: All CLR procedures: 
	OR 
	( 
		(@routineType & 4096 <> 0) 
		AND 
		o.type IN ('PC') 
	) 
	OR 
	-- 8192: All extended procedures: 
	( 
		(@routineType & 8192 <> 0) 
		AND 
		o.type IN ('RF', 'X') 
	) 
) 

-- SELECT * from master..spt_values WHERE type = 'O9T' 
-- https://docs.microsoft.com/en-us/sql/t-sql/statements/create-rule-transact-sql?view=sql-server-ver15
-- https://stackoverflow.com/questions/17227823/difference-between-constraints-and-rules-in-sql
-- https://arvindlounge.wordpress.com/2008/12/08/rules-in-sql-server/
-- https://stackoverflow.com/questions/22352026/is-it-possible-to-use-user-defined-aggregates-clr-with-window-functions-over
-- https://docs.microsoft.com/en-us/sql/t-sql/functions/system-functions-transact-sql?view=sql-server-ver15


/*
SELECT 
	 SPECIFIC_SCHEMA
	,SPECIFIC_NAME
	,ROUTINE_SCHEMA
	,ROUTINE_NAME
	,ROUTINE_DEFINITION -- note:maximum length 4000 
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
-- WHERE ROUTINE_TYPE = 'FUNCTION' AND DATA_TYPE <> 'TABLE' 
WHERE ROUTINE_NAME ='fu_CLR_GenerateQR' 
*/
