﻿
-- http://www.sqlines.com/sql-server-to-oracle/create_type
-- https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
/*
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'email_address' AND ss.name = N'dbo')
CREATE TYPE [dbo].email_address FROM varchar(254) NULL -- NOT NULL 
GO

CREATE TABLE dbo.test 
(
	 abc int 
	,def [email_address]
); 
GO
*/





SELECT 
	  sch.name AS UDT_SCHEMA_NAME 
	 ,userDefinedTypes.name AS UDT_TYPE_NAME 
	  
	,
	N'IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N''' + REPLACE(userDefinedTypes.name, '''', '''''') + N''' AND ss.name = N''' + REPLACE(sch.name, '''', '''''') + N''') '
	+ NCHAR(13) + NCHAR(10) 
	+
	CASE WHEN userDefinedTypeProperties.IsTableType = 1 
		THEN N'CREATE TYPE ' + QUOTENAME(sch.name) + '.' + QUOTENAME(userDefinedTypes.name) + ' AS TABLE ( 
	' + tAllColumns.column_definition  + N'
); ' 
		ELSE 
			+ N'CREATE TYPE ' + QUOTENAME(sch.name) + '.' + QUOTENAME(userDefinedTypes.name) 
			+ N' FROM ' 
			+ tBaseTypeComputation.baseTypeName 
			+ CASE WHEN userDefinedTypeProperties.is_nullable = 0 THEN N' NOT NULL' ELSE N'' END 
			+ N'; ' 
	END AS SqlCreateUdt

FROM sys.types AS userDefinedTypes 

INNER JOIN sys.schemas AS sch 
	ON sch.schema_id = userDefinedTypes.schema_id 

LEFT JOIN sys.table_types AS userDefinedTableTypes 
	ON userDefinedTableTypes.user_type_id = userDefinedTypes.user_type_id 

-- join the type alias (in the scalar case) 
LEFT JOIN sys.types AS systemType 
	ON systemType.system_type_id = userDefinedTypes.system_type_id 
	AND systemType.is_user_defined = 0 

OUTER APPLY 
	(
		SELECT 
			 userDefinedTypes.is_nullable 
			,userDefinedTypes.precision AS NUMERIC_PRECISION 
			,userDefinedTypes.scale AS NUMERIC_SCALE 
	 		,userDefinedTypes.max_length AS CHARACTER_MAXIMUM_LENGTH
			,CASE WHEN userDefinedTableTypes.user_type_id IS NULL THEN 0 ELSE 1 END AS IsTableType 
			,CONVERT(smallint, 
					CASE -- datetime/smalldatetime  
					WHEN userDefinedTypes.system_type_id IN (40, 41, 42, 43, 58, 61) THEN ODBCSCALE(userDefinedTypes.system_type_id, userDefinedTypes.scale) 
					END
			) AS DATETIME_PRECISION 
	) AS userDefinedTypeProperties 


OUTER APPLY 
	(
		SELECT 
			systemType.name 
			+ 
			CASE 
				WHEN systemType.name IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
					THEN N'(' 
						+ 
						CASE WHEN userDefinedTypeProperties.CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
							ELSE CONVERT
								(
									varchar(4)
									,userDefinedTypeProperties.CHARACTER_MAXIMUM_LENGTH 
								)
						END 
						+ N')' 
				WHEN systemType.name IN ('decimal', 'numeric')
					THEN N'(' + CONVERT(varchar(4), userDefinedTypeProperties.NUMERIC_PRECISION) + N', ' + CONVERT(varchar(4), userDefinedTypeProperties.NUMERIC_SCALE) + N')'
				WHEN systemType.name IN ('time', 'datetime2', 'datetimeoffset') 
					THEN N'(' + CAST(userDefinedTypeProperties.DATETIME_PRECISION AS national character varying(36)) + N')' 
				ELSE N'' 
			END AS baseTypeName  
	) AS tBaseTypeComputation 

OUTER APPLY 
	(
		SELECT 
			(
				SELECT 
					-- ,clmns.is_nullable 
					-- ,tComputedProperties.ORDINAL_POSITION
					-- ,tComputedProperties.COLUMN_DEFAULT
					
					  CASE WHEN tComputedProperties.ORDINAL_POSITION = 1 THEN N' ' ELSE N',' END 
					+ QUOTENAME(clmns.name) 
					+ N' '
					+ tComputedProperties.DATA_TYPE 
					+ 
					CASE 
						WHEN tComputedProperties.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
							THEN N'(' 
								+ 
								CASE WHEN tComputedProperties.CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
									ELSE CONVERT
										(
											varchar(4)
											,tComputedProperties.CHARACTER_MAXIMUM_LENGTH 
										)
								END 
								+ N')' 
						WHEN tComputedProperties.DATA_TYPE IN ('decimal', 'numeric')
							THEN N'(' + CONVERT(varchar(4), tComputedProperties.NUMERIC_PRECISION) + N', ' + CONVERT(varchar(4), tComputedProperties.NUMERIC_SCALE) + N')'
						WHEN tComputedProperties.DATA_TYPE IN ('time', 'datetime2', 'datetimeoffset') 
							THEN N'(' + CAST(tComputedProperties.DATETIME_PRECISION AS national character varying(36)) + N')' 
						ELSE N'' 
					END 
					+ CASE WHEN tComputedProperties.is_nullable = 0 THEN N' NOT NULL' ELSE N'' END 
					+ NCHAR(13) + NCHAR(10) 
					AS [text()]
				FROM sys.columns AS clmns 
				INNER JOIN sys.types AS t ON t.system_type_id = clmns.system_type_id 
				LEFT JOIN sys.types ut ON ut.user_type_id = clmns.user_type_id 

				OUTER APPLY 
					(
						SELECT 
							 33 As bb 
							,COLUMNPROPERTY(clmns.object_id, clmns.name, 'ordinal')  AS ORDINAL_POSITION 
							,COLUMNPROPERTY(clmns.object_id, clmns.name, 'charmaxlen') AS CHARACTER_MAXIMUM_LENGTH 
							,COLUMNPROPERTY(clmns.object_id, clmns.name, 'octetmaxlen') AS CHARACTER_OCTET_LENGTH 
							,CONVERT(nvarchar(4000), OBJECT_DEFINITION(clmns.default_object_id)) AS COLUMN_DEFAULT 

							,clmns.is_nullable 
							,t.name AS DATA_TYPE

							,CONVERT(tinyint, 
								CASE -- int/decimal/numeric/real/float/money  
									WHEN clmns.system_type_id IN (48, 52, 56, 59, 60, 62, 106, 108, 122, 127) THEN clmns.precision  
								END
							 ) AS NUMERIC_PRECISION 

							,CONVERT(int, 
								CASE -- datetime/smalldatetime  
									WHEN clmns.system_type_id IN (40, 41, 42, 43, 58, 61) THEN NULL  
									ELSE ODBCSCALE(clmns.system_type_id, clmns.scale) 
								END
							 ) AS NUMERIC_SCALE

							,CONVERT(smallint, 
								 CASE -- datetime/smalldatetime  
									WHEN clmns.system_type_id IN (40, 41, 42, 43, 58, 61) THEN ODBCSCALE(clmns.system_type_id, clmns.scale) 
								 END
							) AS DATETIME_PRECISION 
					) AS tComputedProperties  

				WHERE clmns.object_id = userDefinedTableTypes.type_table_object_id 
				ORDER BY tComputedProperties.ORDINAL_POSITION 
				
				FOR XML PATH(''), TYPE 
			).value('.', 'nvarchar(MAX)') AS column_definition 
	) AS tAllColumns  

WHERE userDefinedTypes.is_user_defined = 1 
