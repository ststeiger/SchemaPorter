
SELECT 
     sch.name AS trigger_table_schema 
    ,systbl.name AS trigger_table_name 
    ,systrg.name AS trigger_name 
    ,sysm.definition AS trigger_definition 
    ,systrg.is_instead_of_trigger



    -- https://stackoverflow.com/questions/5340638/difference-between-a-for-and-after-triggers
    -- Difference between a FOR and AFTER triggers?
    -- CREATE TRIGGER trgTable on dbo.Table FOR INSERT,UPDATE,DELETE
    -- Is the same as
    -- CREATE TRIGGER trgTable on dbo.Table AFTER INSERT,UPDATE,DELETE
    -- An INSTEAD OF trigger is different, and fires before and instead of the insert 
    -- and can be used on views, in order to insert the appropriate values into the underlying tables.
    -- AFTER specifies that the DML trigger is fired only when all operations 
    -- specified in the triggering SQL statement have executed successfully. 
    -- All referential cascade actions and constraint checks also must succeed before this trigger fires. 
    -- AFTER is the default when FOR is the only keyword specified.
    ,CASE WHEN systrg.is_instead_of_trigger = 1 THEN 0 ELSE 1 END AS is_after_trigger 

    ,systrg.is_not_for_replication 
    ,systrg.is_disabled
    ,systrg.create_date 
    ,systrg.modify_date

    ,CASE WHEN systrg.parent_class = 1 THEN 'TABLE' WHEN systrg.parent_class = 0 THEN 'DATABASE' END trigger_class 


    ,CASE 
        WHEN systrg.[type] = 'TA' then 'Assembly (CLR) trigger'
        WHEN systrg.[type] = 'TR' then 'SQL trigger' 
        ELSE '' 
    END AS trigger_type 

    -- https://dataedo.com/kb/query/sql-server/list-triggers 
    -- ,(CASE WHEN objectproperty(systrg.object_id, 'ExecIsUpdateTrigger') = 1
    --      THEN 'UPDATE ' ELSE '' END 
    -- + CASE WHEN objectproperty(systrg.object_id, 'ExecIsDeleteTrigger') = 1
    --      THEN 'DELETE ' ELSE '' END
    -- + CASE WHEN objectproperty(systrg.object_id, 'ExecIsInsertTrigger') = 1
    --      THEN 'INSERT' ELSE '' END
    -- ) AS trigger_event 

    ,
    ( 
        STUFF 
        ( 
            ( 
                SELECT 
                    ', ' + type_desc AS [text()]
                    -- STRING_AGG(type_desc, ', ') AS foo 
                FROM sys.events AS syse 
                WHERE syse.object_id = systrg.object_id
                FOR XML PATH(''), TYPE 
                -- GROUP BY syse.object_id 
            ).value('.[1]', 'nvarchar(MAX)') 
            , 1, 2, '' 
        ) 
    ) AS trigger_event_groups 

    -- ,CASE WHEN systrg.parent_class = 1 THEN 'TABLE' WHEN systrg.parent_class = 0 THEN 'DATABASE' END trigger_class  

    ,'DROP TRIGGER "' + sch.name + '"."' + systrg.name + '"; ' AS sql 
    -- ,systrg.*
FROM sys.triggers AS systrg 

LEFT JOIN sys.sql_modules AS sysm 
    ON sysm.object_id = systrg.object_id 

-- sys.objects for view triggers 
-- LEFT JOIN sys.objects AS systbl ON systbl.object_id = systrg.object_id 

-- INNER JOIN if you only want table-triggers 
LEFT JOIN sys.tables AS systbl ON systbl.object_id = systrg.parent_id 
-- INNER JOIN sys.views AS systbl ON systbl.object_id = systrg.parent_id 


LEFT JOIN sys.schemas AS sch 
    ON sch.schema_id = systbl.schema_id 

WHERE (1=1) 

-- AND sch.name IS NOT NULL 
-- AND sch.name IS NULL 
-- AND sch.name = 'dbo' 
-- And here, exclude some triggers with a certain naming schema 
/*  
AND 
(
    -- systbl.name IS NULL 
    -- OR 
    NOT 
    (
        systrg.name = 'TRG_' + systbl.name  + '_INSERT_History'
        OR 
        systrg.name = 'TRG_' + systbl.name  + '_UPDATE_History'
        OR 
        systrg.name = 'TRG_' + systbl.name  + '_DELETE_History'
    )
)
*/

ORDER BY 
     sch.name 
    ,systbl.name 
    ,systrg.name 
