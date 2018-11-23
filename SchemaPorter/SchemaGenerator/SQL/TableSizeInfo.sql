
--DELETE FROM T_RPT_LOG_ProzedurRun
--DELETE FROM VWSsessions
--DELETE FROM T_SessionValues



--sp_helptext 'sp_spaceused'

--sp_spaceused 'T_SYS_Geschossrechte' 


SELECT 
     TableProperties.name AS TableName 
    ,PartitionProperties.rows AS RowCounts 
    ,SUM(AllocUnits.total_pages) * 8 AS TotalSpaceKB 
    ,SUM(AllocUnits.used_pages) * 8 AS UsedSpaceKB 
    ,(SUM(AllocUnits.total_pages) - SUM(AllocUnits.used_pages)) * 8 AS UnusedSpaceKB 
FROM sys.tables AS TableProperties 

INNER JOIN sys.indexes AS Indices 
	ON TableProperties.OBJECT_ID = Indices.object_id 
	
INNER JOIN sys.partitions AS PartitionProperties 
	--ON PartitionProperties.OBJECT_ID = TableProperties.object_id 
	ON PartitionProperties.OBJECT_ID = Indices.object_id 
	AND Indices.index_id = PartitionProperties.index_id 
	
INNER JOIN sys.allocation_units AS AllocUnits 
	ON PartitionProperties.partition_id = AllocUnits.container_id 
	
WHERE 
    TableProperties.name NOT LIKE 'dt%' 
    AND TableProperties.is_ms_shipped = 0 
    --AND Indices.OBJECT_ID > 255 
    
GROUP BY 
    TableProperties.name, PartitionProperties.Rows 
    --TableProperties.is_ms_shipped -- Group for entire table 
    
ORDER BY 
	 TotalSpaceKB DESC 
	,TableName 
   