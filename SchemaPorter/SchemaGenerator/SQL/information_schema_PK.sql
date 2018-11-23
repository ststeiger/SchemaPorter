
SELECT 
	 Tab.TABLE_SCHEMA 
	,Tab.TABLE_NAME 
	,Col.COLUMN_NAME 
	,ic.ORDINAL_POSITION 
	,Col.CONSTRAINT_SCHEMA
	,Col.CONSTRAINT_NAME 
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS Tab 

INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
	ON Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY' 

INNER JOIN INFORMATION_SCHEMA.COLUMNS AS ic 
	ON ic.TABLE_SCHEMA = Tab.TABLE_SCHEMA 
	AND ic.TABLE_NAME = Tab.TABLE_NAME 
	AND ic.COLUMN_NAME = Col.Column_Name 

WHERE (1=1) 
-- AND Col.Table_Name = '<your table name>'
-- AND Col.Table_Name = 'T_AP_Anlage' 
AND Col.Table_Name = 'T_COR_Objekte' 
