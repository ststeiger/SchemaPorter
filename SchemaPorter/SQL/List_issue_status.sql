
SELECT 
	 TR_Key 
	,TR_Value 
FROM T_Translations 
WHERE TR_Key LIKE 'default_issue_status_%'

ORDER BY TR_Value 
