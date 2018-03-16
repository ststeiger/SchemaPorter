﻿

SELECT 
     sch.name
    ,tables.name 
    ,sc.name 
    ,sc.collation_name 
    ,sc.is_computed 
    ,col.[definition] AS Formula
    ,sc.is_filestream
    ,sc.is_identity 
    ,sc.is_nullable
    ,col.is_persisted

    ,*
FROM sys.columns AS sc 
INNER JOIN sys.tables AS tables ON sc.object_id = tables.object_id 
INNER JOIN sys.schemas AS sch ON sch.schema_id = tables.schema_id
INNER JOIN sys.computed_columns AS col ON col.object_id = sc.object_id
WHERE sc.is_computed = 1











--SELECT COLUMN_NAME ,
--COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'IsComputed'),
--COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'Formula'),
--COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'IsDeterministic')
--FROM INFORMATION_SCHEMA.COLUMNS 
-- WHERE TABLE_NAME = 'Event' AND COLUMN_NAME = 'CurrentAttendance'

SELECT st.name,sc.name 
-- COLUMNPROPERTY(OBJECT_ID(TABLE_SCHEMA+'.'+TABLE_NAME),COLUMN_NAME,'IsComputed') AS IS_COMPUTED,
-- COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'IsComputed'),
-- COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'Formula'),
-- COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'IsDeterministic')

,sc.*
FROM sys.columns AS sc 
INNER JOIN sys.tables AS st ON sc.object_id = st.object_id 
INNER JOIN sys.schemas AS sch ON sch.schema_id = st.schema_id

WHERE is_computed = 1

--CREATE THE SCRIPT TO DROP COMPUTED COLUMN
SELECT 'Alter table ' + st.name  + ' Drop column ' +  sc.name 
FROM sys.columns AS sc 
INNER JOIN sys.tables AS st 
  ON sc.object_id=st.object_id
WHERE is_computed = 1


SELECT [name], definition
FROM sys.computed_columns



SELECT 
   obj.[name] AS TableName
  ,col.[name] AS ColumnName
  ,col.[definition] AS Formula
FROM sys.computed_columns AS col
INNER JOIN sys.objects AS obj 
  ON col.object_id = obj.object_id
ORDER BY obj.name, col.name






SELECT * 
FROM sys.columns sc 
-- INNER JOIN sys.objects AS obj ON obj.object_id = sc.object_id 
INNER JOIN sys.tables AS st ON sc.object_id=st.object_id 
INNER JOIN sys.schemas AS sch ON sch.schema_id = st.schema_id

INNER JOIN sys.computed_columns AS col 
  ON col.object_id = sc.object_id 
  --INNER JOIN sys.objects AS obj ON col.object_id = obj.object_id

WHERE sc.is_computed = 1


-- ------------------------------------------------------------------



SELECT 
     sch.name AS schema_name 
    ,tables.name AS table_name 
    ,sc.name AS column_name 
    ,sc.column_id 
    ,sm.text AS default_value
    ,sc.collation_name AS collation_name 
    --,sc.
    ,sc.is_computed 
    ,col.[definition] AS computation_definition
     
    ,col.is_persisted
    ,sc.is_identity 
    ,sc.is_filestream 
    ,sc.is_nullable
    ,sc.is_sparse 
    ,t.name 
    --,t.is_user_defined
    --,t.is_table_type 
    --,t.is_assembly_type 
    ,sc.*
    -- sc.max_length, sc.precision, sc.scale

 , CASE 
    WHEN TYPE_NAME(sc.system_type_id) IN ('NCHAR','NVARCHAR') 
    THEN sc.max_length / 2 
    ELSE sc.max_length 
  END AS CHARACTER_MAXIMUM_LENGTH 


	,CONVERT(tinyint, CASE -- int/decimal/numeric/real/float/money
		WHEN sc.system_type_id IN (48, 52, 56, 59, 60, 62, 106, 108, 122, 127) THEN sc.precision
		END) AS NUMERIC_PRECISION

	,CONVERT(smallint, CASE	-- int/money/decimal/numeric
		WHEN sc.system_type_id IN (48, 52, 56, 60, 106, 108, 122, 127) THEN 10
		WHEN sc.system_type_id IN (59, 62) THEN 2 END)	AS NUMERIC_PRECISION_RADIX	-- real/float

	,CONVERT(int, CASE	-- datetime/smalldatetime
		WHEN sc.system_type_id IN (40, 41, 42, 43, 58, 61) THEN NULL
		ELSE ODBCSCALE(sc.system_type_id, sc.scale) END)	AS NUMERIC_SCALE 

    ,CONVERT(smallint, CASE -- datetime/smalldatetime
		WHEN sc.system_type_id IN (40, 41, 42, 43, 58, 61) THEN ODBCSCALE(sc.system_type_id, sc.scale) END
        )	AS DATETIME_PRECISION
	

FROM sys.columns AS sc 
INNER JOIN sys.tables AS tables ON sc.object_id = tables.object_id 
INNER JOIN sys.schemas AS sch ON sch.schema_id = tables.schema_id
LEFT JOIN sys.computed_columns AS col ON col.object_id = sc.object_id AND col.column_id = sc.column_id 
LEFT JOIN sys.syscomments AS sm ON sm.id = sc.default_object_id 
LEFT JOIN sys.types AS t ON sc.user_type_id = t.user_type_id

-- WHERE tables.name = 'T_Benutzer' 
WHERE tables.name = 'T_Admin' 
