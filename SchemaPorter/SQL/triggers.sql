
SELECT 
	 sch.name AS schema_name 
	,parent.name AS table_name 
	,obj.name AS trigger_name 
	,m.[definition] AS trigger_definition 
FROM sys.sql_modules AS m 
INNER JOIN sys.objects AS obj ON obj.object_id=m.object_id 
INNER JOIN sys.objects AS parent ON parent.object_id = obj.parent_object_id 
INNER JOIN sys.schemas AS sch ON sch.schema_id = parent.schema_id 

WHERE obj.type ='TR' 


