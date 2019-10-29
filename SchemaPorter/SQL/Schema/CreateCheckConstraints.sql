
SELECT 
	 con.name AS CONSTRAINT_NAME 
	,sch.name AS CONSTRAINT_SCHEMA 
	,t.name AS CONSTRAINT_TABLE 
	,col.name AS CONSTRAINT_COLUMN_NAME 
	,con.definition AS CONSTRAINT_DEFINITION 
	,CASE 
		WHEN con.is_disabled = 0
			THEN 'Active'
		ELSE 'Disabled'
	 END AS CONSTRAINT_STATUS 
	,N'ALTER TABLE ' + QUOTENAME(sch.name) + N'.' + QUOTENAME(t.name) + N' 
ADD CONSTRAINT ' + QUOTENAME(con.name) + N'
  CHECK (' + con.definition + '); ' AS SqlCreateCheck 
FROM sys.check_constraints AS con 
LEFT JOIN sys.objects AS t ON con.parent_object_id = t.object_id
LEFT JOIN sys.schemas AS sch ON sch.schema_id = t.schema_id 
LEFT JOIN sys.all_columns AS col 
	ON con.parent_column_id = col.column_id
	AND con.parent_object_id = col.object_id

ORDER BY 
	 con.name 
	,CONSTRAINT_SCHEMA 
	,CONSTRAINT_TABLE 
	 

-- ------------------------------------------------------------------------------------------------------------


SELECT 
	 ctu.TABLE_SCHEMA 
	,ctu.TABLE_NAME
	,cc.CONSTRAINT_SCHEMA
	,cc.CONSTRAINT_NAME
	,cc.CHECK_CLAUSE

	,N'ALTER TABLE ' + QUOTENAME(ctu.TABLE_SCHEMA) + N'.' + QUOTENAME(ctu.TABLE_NAME) + N' 
ADD CONSTRAINT ' + QUOTENAME(cc.CONSTRAINT_NAME) + N'
  CHECK (' + cc.CHECK_CLAUSE + '); ' AS SqlCreateCheck 
FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS AS cc 

INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS ctu 
	ON ctu.CONSTRAINT_NAME = cc.CONSTRAINT_NAME 
	AND ctu.CONSTRAINT_SCHEMA = cc.CONSTRAINT_SCHEMA 

ORDER BY
	 TABLE_SCHEMA
	,TABLE_NAME
	,CONSTRAINT_SCHEMA 
	,CONSTRAINT_NAME 
	 