SELECT 
	 istu.TABLE_SCHEMA 
	,istu.TABLE_Name 
	,isc.CONSTRAINT_CATALOG
	,isc.CONSTRAINT_SCHEMA
	,isc.CONSTRAINT_NAME
	,isc.CHECK_CLAUSE
FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS AS isc 

INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS istu 
	ON istu.CONSTRAINT_SCHEMA = isc.CONSTRAINT_SCHEMA 
	AND istu.CONSTRAINT_NAME = isc.CONSTRAINT_NAME 


SELECT 
	 sch.name AS table_schema 
	,syso.name as table_name 
	,cc.name AS constraint_name 
	,cc.definition AS constraint_definition 
	-- ,cc.object_id 
	,cc.create_date
	,cc.modify_date
FROM sys.check_constraints AS cc

LEFT JOIN sys.objects AS syso 
	ON syso.object_id = cc.parent_object_id 

LEFT JOIN sys.schemas AS sch 
	ON sch.schema_id = syso.schema_id 

/*
WHERE name = (select cu.CONSTRAINT_NAME
              from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu
              inner join INFORMATION_SCHEMA.CHECK_CONSTRAINTS c on c.CONSTRAINT_NAME=cu.CONSTRAINT_NAME 
              where COLUMN_NAME = 'FieldsOptions<Your Column Name>')
*/
