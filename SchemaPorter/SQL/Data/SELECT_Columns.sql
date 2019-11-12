
-- DECLARE @in_schema national character varying(256)
-- DECLARE @in_table national character varying(256)
-- SET @in_schema = N'dbo' 
-- SET @in_table = N'T_Benutzer'



DECLARE @outputtype varchar(255)
SET @outputtype = 'ms' 
-- SET @outputtype = 'pg' 

SELECT
	N'SELECT ' 
 + 
(
SELECT 
	 CASE WHEN ORDINAL_POSITION = 1 THEN N' ' ELSE N',' END 
	+ QUOTENAME(COLUMN_NAME)  + N' '
	+ NCHAR(13) + NCHAR(10) 
	AS [text()] 

-- ,NUMERIC_PRECISION_RADIX 
-- ,COLUMN_DEFAULT 
-- FROM INFORMATION_SCHEMA.COLUMNS AS isc 
FROM 
(
	SELECT 
		 sch.name AS TABLE_SCHEMA  
		,tables.name AS TABLE_NAME 
		,sc.name AS COLUMN_NAME 
		,sc.column_id AS ORDINAL_POSITION 
		,sm.text AS COLUMN_DEFAULT 
		,CASE WHEN sc.is_nullable = 1 THEN N'YES' ELSE N'NO' END AS IS_NULLABLE 
		,col.is_persisted AS IS_PERSISTED 
		,t.name AS DATA_TYPE  
		,sc.is_computed AS IS_COMPUTED 
		,col."definition" AS COMPUTATION  
		,COLUMNPROPERTY(sc.object_id, sc.name, 'charmaxlen') AS CHARACTER_MAXIMUM_LENGTH 
		,COLUMNPROPERTY(sc.object_id, sc.name, 'octetmaxlen') AS CHARACTER_OCTET_LENGTH 
				
		,CONVERT(tinyint, CASE -- int/decimal/numeric/real/float/money  
			WHEN sc.system_type_id IN (48, 52, 56, 59, 60, 62, 106, 108, 122, 127) THEN sc.precision  
			END) AS NUMERIC_PRECISION 

		,CONVERT(int, CASE -- datetime/smalldatetime  
			WHEN sc.system_type_id IN (40, 41, 42, 43, 58, 61) THEN NULL  
			ELSE ODBCSCALE(sc.system_type_id, sc.scale) 
			END) AS NUMERIC_SCALE 
				 
		,CONVERT(smallint, CASE -- datetime/smalldatetime  
			WHEN sc.system_type_id IN (40, 41, 42, 43, 58, 61) 
			THEN ODBCSCALE(sc.system_type_id, sc.scale) 
		END) AS DATETIME_PRECISION 

		,
		CASE 
			WHEN @outputtype = 'ms' THEN 
				CASE 
					WHEN t.name  = 'nvarchar' THEN N'national character varying' 
					WHEN t.name  = 'varchar' THEN N'character varying' 
					ELSE t.name  
				END 
			WHEN @outputtype = 'pg' THEN 
				CASE 
					WHEN t.name  = 'nvarchar' THEN N'national character varying' 
					WHEN t.name  = 'varchar' THEN N'character varying' 
					WHEN t.name  = 'ntext' THEN 'text' 
					WHEN t.name  = 'uniqueidentifier' THEN 'uuid' 
					WHEN t.name  = 'bit' THEN 'boolean' 
					WHEN t.name  = 'datetime' THEN 'timestamp without time zone' 
					WHEN t.name  = 'smalldatetime' THEN 'timestamp without time zone' 
					WHEN t.name  = 'datetime2' THEN 'timestamp with time zone' 
					WHEN t.name  = 'datetimeoffset' THEN 'timestamp with time zone' 
					WHEN t.name  = 'money' THEN 'decimal(19, 4)' 
					WHEN t.name  IN ('varbinary', 'image', 'binary') THEN 'bytea' 
					WHEN t.name  = 'tinyint' THEN 'smallint' 
					ELSE t.name  
				END 

			ELSE t.name  
		END  
		
		-- t.name 
		+ 

		CASE 
			WHEN t.name IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
				THEN '(' 
					+ 
					CASE WHEN sc.max_length = -1 THEN 'MAX'
						ELSE CONVERT
							(
									varchar(4)
								,CASE WHEN t.name IN ('nchar', 'nvarchar') THEN sc.max_length/2 ELSE sc.max_length END 
							)
					END 
					+ ')' 
			WHEN t.name IN ('decimal', 'numeric')
				THEN '(' + CONVERT(varchar(4), sc.precision)+', ' + CONVERT(varchar(4), sc.Scale) + ')'
			WHEN t.name IN ('time', 'datetime2', 'datetimeoffset') 
				THEN N'(' + CAST(ODBCSCALE(sc.system_type_id, sc.scale) AS national character varying(36)) + N')' 
			ELSE '' 
		END 
		AS DDL_NAME  
	 

		-- ,sc.is_filestream 
		-- ,sc.is_identity 
		-- ,sc.is_sparse 

		,dc.name AS DEFAULT_CONSTRAINT_NAME 
		,dc.definition AS DEFAULT_CONSTRAINT_DEFINITION 
	FROM sys.columns AS sc 
	-- INNER JOIN sys.objects AS o ON o.object_id = sc.object_id 
	INNER JOIN sys.tables AS tables ON tables.object_id = sc.object_id 
	INNER JOIN sys.schemas AS sch ON sch.schema_id = tables.schema_id 
	LEFT JOIN sys.computed_columns AS col ON col.object_id = sc.object_id AND col.column_id = sc.column_id 
	INNER JOIN sys.types AS t ON sc.user_type_id = t.user_type_id 
	LEFT JOIN sys.syscomments AS sm ON sm.id = sc.default_object_id 

	LEFT JOIN sys.default_constraints AS dc 
		ON sc.column_id = dc.parent_column_id 
		AND sc.object_id = dc.parent_object_id 

	WHERE (1=1) 
		
	AND 
	(
		tables.is_ms_shipped = 0 
		AND 
		( 
			SELECT ep.major_id 
			FROM sys.extended_properties AS ep 
			WHERE ep.major_id = tables.object_id 
			AND ep.minor_id = 0 
			AND ep.class = 1 
			AND ep.name = N'microsoft_database_tools_support' 
		) IS NULL 
	) 

) AS isc 
WHERE (1=1) 

AND IS_COMPUTED = 0 
AND TABLE_SCHEMA = @in_schema 
AND TABLE_NAME = @in_table 


ORDER BY 
	 TABLE_SCHEMA 
	,TABLE_NAME 
	,ORDINAL_POSITION 
	 
	FOR XML PATH(N''), TYPE 
	).value(N'.', N'nvarchar(MAX)')
	+ N' FROM ' + @in_schema + '.' + @in_table 

	AS select_query  
