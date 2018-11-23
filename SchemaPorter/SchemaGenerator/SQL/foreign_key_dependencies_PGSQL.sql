
;WITH -- RECURSIVE 
CTE_ForeignKeys AS 
( 
    SELECT DISTINCT 
         KCU1.table_schema AS OnTableSchema 
        ,KCU1.table_name AS OnTable 
        ,KCU2.table_name AS AgainstTable 
    FROM information_schema.referential_constraints AS RC 
    
    INNER JOIN information_schema.key_column_usage AS KCU1 
        ON KCU1.constraint_catalog = RC.constraint_catalog 
        AND KCU1.constraint_schema = RC.constraint_schema 
        AND KCU1.constraint_name = RC.constraint_name 
        
    INNER JOIN information_schema.key_column_usage AS KCU2 
        ON KCU2.constraint_catalog =  RC.constraint_catalog 
        AND KCU2.constraint_schema = RC.unique_constraint_schema 
        AND KCU2.constraint_name = RC.unique_constraint_name 
        AND KCU2.ordinal_position = KCU1.ordinal_position 
) 
,CTE_AllTables AS 
( 
    SELECT 
         ist.table_schema AS OnTableSchema 
        ,ist.table_name AS OnTable 
        ,CTE_ForeignKeys.againstTable AS AgainstTable 
    FROM INFORMATION_SCHEMA.TABLES AS ist 
    
    LEFT JOIN CTE_ForeignKeys 
        ON table_name = CTE_ForeignKeys.OnTable 
        AND table_schema = CTE_ForeignKeys.OnTableSchema 
        
    WHERE (1=1) 
    AND table_type = 'BASE TABLE' 
    -- AND table_name NOT IN ('sysdiagrams', 'dtproperties') 
    -- AND table_schema = 'public' 
    AND table_schema NOT IN ('pg_catalog', 'information_schema') 
) 
,CTE_RecursiveDependencyResolution AS 
( 
    -- Tables with no dependencies first 
    SELECT 
		 OnTableSchema AS TableSchema 
        ,OnTable AS TableName 
        ,1 AS Lvl 
    FROM CTE_AllTables 
    WHERE (1=1) 
    AND AgainstTable IS NULL 
    
    
    -- Recursion to resolve dependencies 
    UNION ALL 
    
    
    SELECT 
         CTE_AllTables.OnTableSchema AS TableSchema 
        ,CTE_AllTables.OnTable AS TableName 
        ,CTE_RecursiveDependencyResolution.Lvl + 1 AS Lvl 
    FROM CTE_AllTables 
    
    INNER JOIN CTE_RecursiveDependencyResolution 
        ON CTE_RecursiveDependencyResolution.TableName = CTE_AllTables.AgainstTable 
) 

SELECT 
     MAX(Lvl) AS Lvl 
    ,TableSchema 
    ,TableName 
    --,'DELETE FROM ' || QUOTE_IDENT(TableSchema) || '.' || QUOTE_IDENT(TableName) AS cmdDelete 
FROM CTE_RecursiveDependencyResolution 

GROUP BY 
     TableSchema 
    ,TableName 
     
ORDER BY 
     lvl 
    ,TableSchema 
    ,TableName 
    
    
/*
ORDER BY 
     2 ASC 
    ,1 ASC 
*/

-- OPTION (MAXRECURSION 0) 
