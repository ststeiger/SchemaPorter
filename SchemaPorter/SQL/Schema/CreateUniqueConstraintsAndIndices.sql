
-- CREATE TABLE dbo.T_User( USR_ID int NOT NULL, USR_User nvarchar(256) NULL, USR_Status int NOT NULL );
-- ALTER TABLE dbo.T_User ADD CONSTRAINT UC_T_User_USR_User UNIQUE(USR_User)

-- IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.T_User') AND name = N'IX_T_User_USR_User')
-- CREATE UNIQUE INDEX IX_T_User_USR_User ON dbo.T_User(USR_User)
-- WHERE (USR_User IS NOT NULL AND USR_Status=(1))

SELECT 
     sch.name 
    ,st.name 
    ,i.name 
    ,i.has_filter 
    ,i.filter_definition 
    ,i.is_unique 
    ,i.is_primary_key 
    ,i.is_unique_constraint 

    ,CASE WHEN i.is_unique_constraint = 1 
        THEN N'ALTER TABLE ' + QUOTENAME(sch.name) + N'.' + QUOTENAME(st.name) + N' DROP CONSTRAINT ' + QUOTENAME(i.name) + N'; ' 
        ELSE N'DROP INDEX ' + QUOTENAME(i.name)  + N' ON ' + QUOTENAME(sch.name)  + '.' + QUOTENAME(st.name) + '; ' 
    END AS sql 
FROM sys.indexes AS i

INNER JOIN sys.index_columns AS ic 
    ON i.index_id = ic.index_id 
    AND i.object_id = ic.object_id 

INNER JOIN sys.tables AS st 
    ON st.object_id = i.object_id 

INNER JOIN sys.objects AS syso 
    ON syso.object_id = st.object_id 
    AND syso.is_ms_shipped = 0 

INNER JOIN sys.schemas AS sch 
    ON sch.schema_id = st.schema_id 

WHERE (1=1) 
AND NOT EXISTS  
(
    SELECT * 
    FROM sys.extended_properties AS xp 
    WHERE xp.minor_id = 0 
    AND xp.major_id = st.object_id 
    AND xp.name = 'microsoft_database_tools_support' 
)

-- AND sch.name = 'dbo' 
-- AND st.name = 'T_Benutzer'

AND 
(
    i.is_unique_constraint = 1 
    OR 
    (i.is_unique = 1 AND i.is_primary_key = 0)
)
;
