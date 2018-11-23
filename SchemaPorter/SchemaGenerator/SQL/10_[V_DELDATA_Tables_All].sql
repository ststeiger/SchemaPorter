
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_DELDATA_Tables_All]'))
DROP VIEW [dbo].[V_DELDATA_Tables_All]
GO





CREATE VIEW [dbo].[V_DELDATA_Tables_All] 
AS
WITH Fkeys AS (

    SELECT DISTINCT 
         OnTable       = OnTable.name
        ,AgainstTable  = AgainstTable.name 
    FROM sysforeignkeys fk 
        
        INNER JOIN sysobjects onTable 
            ON fk.fkeyid = onTable.id 
            
        INNER JOIN sysobjects againstTable  
            ON fk.rkeyid = againstTable.id 
            
    WHERE 1=1
        AND AgainstTable.TYPE = 'U'
        AND OnTable.TYPE = 'U'
        -- ignore self joins; they cause an infinite recursion
        AND OnTable.Name <> AgainstTable.Name
    )

,MyData AS (

    SELECT 
         OnTable = o.name 
        ,AgainstTable = FKeys.againstTable 
    FROM sys.objects o 
        
    LEFT JOIN FKeys
		ON o.name = FKeys.onTable 
		
    WHERE (1=1) 
        AND o.type = 'U' 
        AND o.name NOT LIKE 'sys%' 
    )

,MyRecursion AS (

    -- base case
    SELECT  
         TableName    = OnTable
        ,Lvl        = 1
    FROM MyData
    WHERE 1=1
        AND AgainstTable IS NULL 
        
    -- recursive case
    UNION ALL 
    
    SELECT 
         TableName = OnTable 
        ,Lvl       = r.Lvl + 1 
    FROM MyData d 
        INNER JOIN MyRecursion r 
            ON d.AgainstTable = r.TableName 
)


SELECT TOP 999999999999999999 
     Lvl = max(Lvl)
    ,TableName 
    ,'DELETE FROM [' + REPLACE(TableName, '''', '''''') + ']; ' AS DeleteCmd 
FROM 
    MyRecursion

GROUP BY
    TableName
    
    
ORDER BY lvl DESC, TableName

GO


