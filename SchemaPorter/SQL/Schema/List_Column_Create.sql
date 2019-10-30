
DECLARE @outputtype varchar(255)
SET @outputtype = 'ms' 
-- SET @outputtype = 'pg' 

SELECT 
	 TABLE_SCHEMA 
	,TABLE_NAME 
	,COLUMN_NAME 
	,ORDINAL_POSITION 
	,IS_NULLABLE 
	,IS_COMPUTED 
	,DDL_NAME 
	,COLUMN_DEFAULT 
	,DEFAULT_CONSTRAINT_NAME 
	,DEFAULT_CONSTRAINT_DEFINITION 


	-- CASE WHEN ORDINAL_POSITION = 1 THEN N'     ' ELSE N'    ,' END 
	, N'    ,'
	+ N'"' + COLUMN_NAME + N'"' + N' ' 
	+ 
	CASE 
		WHEN IS_COMPUTED = 1 THEN N'AS ' 
		WHEN @outputtype = 'ms' THEN 
			CASE 
				WHEN DATA_TYPE = 'nvarchar' THEN N'national character varying' 
				WHEN DATA_TYPE = 'varchar' THEN N'character varying' 
				ELSE DATA_TYPE 
			END 
		WHEN @outputtype = 'pg' THEN 
			CASE 
				WHEN DATA_TYPE = 'nvarchar' THEN N'national character varying' 
				WHEN DATA_TYPE = 'varchar' THEN N'character varying' 
				WHEN DATA_TYPE = 'ntext' THEN 'text' 
				WHEN DATA_TYPE = 'uniqueidentifier' THEN 'uuid' 
				WHEN DATA_TYPE = 'bit' THEN 'boolean' 
				WHEN DATA_TYPE = 'datetime' THEN 'timestamp without time zone' 
				WHEN DATA_TYPE = 'smalldatetime' THEN 'timestamp without time zone' 
				WHEN DATA_TYPE = 'datetime2' THEN 'timestamp with time zone' 
				WHEN DATA_TYPE = 'datetimeoffset' THEN 'timestamp with time zone' 
				WHEN DATA_TYPE = 'money' THEN 'decimal(19, 4)' 
				WHEN DATA_TYPE IN ('varbinary', 'image', 'binary') THEN 'bytea' 
				WHEN DATA_TYPE = 'tinyint' THEN 'smallint' 
				ELSE DATA_TYPE 
			END 

		ELSE DATA_TYPE 
	END 
	+ 
	CASE 
		WHEN IS_COMPUTED = 1 
			THEN COMPUTATION + CASE WHEN IS_PERSISTED = 1 THEN N' PERSISTED' ELSE N'' END 
		WHEN @outputtype = 'pg' AND DATA_TYPE IN ('binary', 'varbinary') 
			THEN '' 
		WHEN DATA_TYPE IN ('char', 'nchar', 'binary') 
			THEN 
					N'(' 
				+ CAST(CHARACTER_MAXIMUM_LENGTH AS nvarchar(36)) 
				+ N')' 

		WHEN DATA_TYPE IN ('varchar', 'nvarchar', 'varbinary') 
			THEN 
				CASE 
					WHEN @outputtype = 'pg' THEN 
						CASE WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN '' ELSE N'(' + CAST(CHARACTER_MAXIMUM_LENGTH AS nvarchar(36)) + N')' END  
					ELSE 
						N'(' + 
						CASE WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX' ELSE CAST(CHARACTER_MAXIMUM_LENGTH AS nvarchar(36)) END 
						+ N')' 
				END 

		WHEN DATA_TYPE IN ('datetimeoffset', 'datetime2', 'time', 'smalldatetime') THEN 
			CASE 
				WHEN @outputtype = 'ms' THEN 
						N'(' 
					+ CAST(DATETIME_PRECISION AS nvarchar(36)) 
					+ N')' 
				ELSE N'' 
			END 
					
		WHEN DATA_TYPE IN ('decimal', 'numeric') 
			THEN N'(' 
				+ CAST(NUMERIC_PRECISION AS nvarchar(36)) 
				+ N',' 
				+ CAST(NUMERIC_SCALE AS nvarchar(36)) 
				+ N')' 

		ELSE N'' -- N'(default)' 
	END
			
	-- https://docs.microsoft.com/en-us/sql/relational-databases/tables/specify-computed-columns-in-a-table?view=sql-server-2017
	-- Great: computed column values can be non-nullable, but on insert, they are NULL ! 
	-- So we need to add computed columns and exclude them from the insert. 
	-- Also, we don't drop columns that don't exist in basic, so they need to be nullable 
	-- + CASE WHEN IS_NULLABLE = 'YES' OR IS_COMPUTED = 1 THEN N' ' ELSE N' NOT NULL ' END 
	+ CHAR(13) + CHAR(10) 
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
		,CASE WHEN sc.is_nullable = 1 THEN 'YES' ELSE 'NO' END AS IS_NULLABLE 
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
		END AS DDL_NAME  
	 

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
-- AND IS_COMPUTED = 0 
-- AND IS_COMPUTED = 1 
-- AND DDL_NAME = 'text' 
-- AND COLUMN_DEFAULT <> DEFAULT_CONSTRAINT_DEFINITION 
-- AND IS_NULLABLE = 'YES' 
-- AND IS_NULLABLE = 'NO' 

ORDER BY 
	 TABLE_SCHEMA 
	,TABLE_NAME 
	,ORDINAL_POSITION 
	 