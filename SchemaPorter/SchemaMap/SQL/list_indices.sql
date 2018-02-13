SELECT
  [schema] = OBJECT_SCHEMA_NAME([object_id]),
  [table]  = OBJECT_NAME([object_id]),
  [index]  = name, 
  is_unique_constraint,
  is_unique,
  is_primary_key
FROM sys.indexes
WHERE is_primary_key = 0 
AND is_unique = 1 
AND name IS NOT NULL 
AND OBJECT_NAME([object_id]) IN ( 
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'
)