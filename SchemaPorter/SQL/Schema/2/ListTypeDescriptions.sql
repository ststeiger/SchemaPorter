
SELECT 
	 name
	 ,SUBSTRING(name, 1,2) AS shorty 
	,number
	,type
	,low
	,high
	,status
FROM master.dbo.spt_values
WHERE (1=1) 
-- AND name LIKE 'AF:%'
AND type = 'O9T' 
-- AND name LIKE '%function%'
-- AND name LIKE '%proc%'
-- AND name LIKE '%scala%'

-- ORDER BY shorty 
ORDER BY 
	 type 
	,name 
	,number  
	 

