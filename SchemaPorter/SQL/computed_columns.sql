
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
FROM sys.columns sc 
inner join sys.tables st on sc.object_id=st.object_id 
inner join sys.schemas sch on sch.schema_id = st.schema_id

WHERE is_computed = 1

--CREATE THE SCRIPT TO DROP COMPUTED COLUMN
SELECT 'Alter table ' + st.name  + ' Drop column ' +  sc.name FROM sys.columns sc inner join sys.tables st on sc.object_id=st.object_id
WHERE is_computed = 1
