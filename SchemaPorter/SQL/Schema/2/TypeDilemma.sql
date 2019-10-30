
SELECT * FROM sys.types AS tt 
WHERE tt.system_type_id IN 
(
	SELECT system_type_id FROM sys.types AS t 
	WHERE t.is_user_defined = 0 
	GROUP BY system_type_id
	HAVING COUNT(*) > 1 
)
ORDER BY system_type_id 


/*
SELECT * FROM sys.types 
WHERE name LIKe '%char%' OR name like '%sys%'

select * from sys.systypes 
WHERE name LIKe '%char%' OR name like '%sys%'
*/
