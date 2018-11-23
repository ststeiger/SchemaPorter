
SELECT 
	 sch.name AS table_schema 
	,syso.name AS table_name 
    ,sysc.name AS column_name 
	,sysc.column_id AS ordinal_position 
	,sch.name AS constraint_schema 
	,sysi.name AS constraint_name 
    -- ,COLUMNPROPERTY(sysc.object_id, sysc.name, 'ordinal')  AS ORDINAL_POSITION 
    --,sysc.is_identity 
FROM sys.indexes AS sysi 

INNER JOIN sys.index_columns AS ic 
	ON ic.object_id = sysi.object_id 
	AND ic.index_id = sysi.index_id 

INNER JOIN sys.columns AS sysc 
	ON ic.object_id = sysc.object_id 
	AND ic.column_id = sysc.column_id 

INNER JOIN sys.objects AS syso 
	ON syso.object_id = sysc.object_id 

INNER JOIN sys.schemas AS sch
	ON sch.schema_id = syso.schema_id 

WHERE sysi.is_primary_key = 1 
-- AND sysc.is_identity = 1 
-- AND sysi.object_ID = OBJECT_ID('<schema>.<tablename>');
AND syso.name = 'T_COR_Objekte'

-- SELECT * FROM T_COR_Objekte 