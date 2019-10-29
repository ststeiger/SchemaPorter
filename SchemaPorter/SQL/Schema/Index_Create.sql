
SELECT 
	 ind.object_id 
	,syso.name AS table_name 
	,ind.index_id 
	,ind.name AS index_name 
	, N'IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = ''' + REPLACE(ind.name, N'''', N'''''') + N''' ' 
	+ N'AND object_id = OBJECT_ID(''' + QUOTENAME(sch.name) + N'.' + QUOTENAME(syso.name) + N''')) ' 
	+ 
	CAST
	 ( 
		 CASE WHEN ind.type = 1 AND ind.is_unique = 1 THEN N'CREATE UNIQUE CLUSTERED INDEX ' 
			 WHEN ind.type = 1 AND ind.is_unique = 0 THEN N'CREATE CLUSTERED INDEX ' 
			 WHEN ind.type = 2 AND ind.is_unique = 1 THEN N'CREATE UNIQUE NONCLUSTERED INDEX ' 
			 WHEN ind.type = 2 AND ind.is_unique = 0 THEN N'CREATE NONCLUSTERED INDEX ' 
		END 
		+ QUOTENAME(ind.name) + N' ON ' + QUOTENAME(sch.name) + '.' + QUOTENAME(syso.name) + N' (' 
		+ STUFF
		  ( 
			 ( 
				 SELECT 
					  N',' + QUOTENAME( indcolobj.name ) 
					+ N' ' + CASE WHEN indcol.is_descending_key = 1 THEN N'DESC' ELSE N'ASC' END 
					AS [text()] 
				 FROM sys.index_columns AS indcol WITH (NOLOCK) 
				 INNER JOIN sys.columns indcolobj WITH (NOLOCK) 
					ON indcol.object_id = indcolobj.object_id 
					AND indcol.column_id = indcolobj.column_id 
				 WHERE indcol.object_id = ind.object_id 
				 AND indcol.index_id = ind.index_id 
				 AND indcol.is_included_column = 0 
				 ORDER BY indcol.key_Ordinal ASC 
				 FOR XML PATH(''), TYPE 
			 ).value('.', 'nvarchar(MAX)') 
			 , 1, 1, N'' 
		  ) 
		+ N') ' 

		+ 
		 CASE 
			WHEN ind.type = 1 THEN N'' 
			ELSE 
			COALESCE
			(
				 N'INCLUDE (' 
				 + STUFF
				 (
					 (
						 SELECT
							N',' + QUOTENAME( indcolobj.name ) AS [text()] 
						 FROM sys.index_columns AS indcol WITH (NOLOCK) 
						 INNER JOIN sys.columns indcolobj WITH (NOLOCK) 
							ON indcol.object_id = indcolobj.object_id 
							AND indcol.column_id = indcolobj.column_id 
						 WHERE indcol.object_id = ind.object_id 
						 AND indcol.index_id = ind.index_id 
						 AND indcol.is_included_column = 1 
						 ORDER BY indcol.index_column_id ASC 
						 FOR XML PATH(''), TYPE 
					 ).value('.','nvarchar(MAX)') 
					 , 1, 1, N'' 
				 ) 
				 + N') ' 
				 , N'' 
			 ) 
		END 
		  
		+ CASE WHEN ind.has_filter = 1 THEN N'WHERE ' + ind.filter_definition ELSE N'' END 
		+ N' WITH (' -- DROP_EXISTING = ON, 
		+ N'SORT_IN_TEMPDB = ON' 
		-- WHEN the same index exists you'd better to set the DROP_EXISTING = ON 
		-- SORT_IN_TEMPDB = ON is recommended but based on your own environment. 
		+ N', FILLFACTOR = ' + CAST(CASE WHEN fill_factor = 0 THEN 100 ELSE fill_factor END AS varchar(3) ) 
		  
		+ CASE WHEN ind.is_padded = 1 THEN N', PAD_INDEX = ON' ELSE N', PAD_INDEX = OFF' END 
		+ CASE WHEN stat.no_recompute = 1 THEN N', STATISTICS_NORECOMPUTE = ON' ELSE N', STATISTICS_NORECOMPUTE = OFF' END 
		+ CASE WHEN ind.ignore_dup_key = 1 THEN N', IGNORE_DUP_KEY = ON' ELSE N', IGNORE_DUP_KEY = OFF' END 
		+ CASE WHEN ind.ALLOW_ROW_LOCKS = 1 THEN N', ALLOW_ROW_LOCKS = ON' ELSE N', ALLOW_ROW_LOCKS = OFF' END 
		+ CASE WHEN ind.ALLOW_PAGE_LOCKS = 1 THEN N', ALLOW_PAGE_LOCKS = ON' ELSE N', ALLOW_PAGE_LOCKS = OFF' END 
		+ CASE 
			WHEN tPartitions.data_compression = 0 THEN N', DATA_COMPRESSION = NONE' 
			WHEN tPartitions.data_compression = 1 THEN N', DATA_COMPRESSION = ROW' 
			ELSE N', DATA_COMPRESSION = PAGE' 
		END 
		+ N') ON ' 
		+ 
		CASE 
			WHEN dsp.type = 'FG' THEN QUOTENAME(dsp.name) 
			ELSE QUOTENAME(dsp.name) + N'(' + tIndexColumns.partition_column + N')' 
		END + N';' -- if it uses partition scheme then need partition column 
		AS nvarchar(MAX) 
	) AS index_create_statement 
	 
	--,dsp.name AS file_group_name  
	--,N'DROP INDEX IF EXISTS ' + QUOTENAME(ind.Name) + N' ON ' + QUOTENAME(sch.name) 
	--+ N'.' + QUOTENAME(syso.name) 
	--+ N';' AS index_drop_statement_sql_2016


	, N'IF EXISTS(SELECT * FROM sys.indexes WHERE name = ''' + REPLACE(ind.name, N'''', N'''''') + N''' ' 
	+ N'AND object_id = OBJECT_ID(''' + QUOTENAME(sch.name) + N'.' + QUOTENAME(syso.name) + N''')) ' + 
	+ N'     DROP INDEX ' + QUOTENAME(ind.name) 
	+ ' ON ' + QUOTENAME(sch.name) + N'.' + QUOTENAME(syso.name) + N'; ' 
	AS index_drop_statement 

FROM sys.indexes AS ind WITH (NOLOCK) 

INNER JOIN sys.objects AS syso WITH (NOLOCK) 
	ON ind.object_id = syso.object_id 

INNER JOIN sys.schemas AS sch 
	ON syso.schema_id = sch.schema_id 

INNER JOIN sys.data_spaces AS dsp WITH (NOLOCK) 
	ON ind.data_space_id = dsp.data_space_id 

INNER JOIN sys.stats AS stat WITH (NOLOCK) 
	ON ind.object_id = stat.object_id 
	AND ind.index_id = stat.stats_id 

INNER JOIN 
 -- The below code is to find out what data compression type was used by the index. If an index is not partitioned, it is easy as only one data compression
 -- type can be used. If the index is partitioned, THEN each partition can be configued to use the different data compression. This is hard to generalize,
 -- for simplicity, I just use the data compression type used most for the index partitions for all partitions. You can later rebuild the index partition to
 -- the appropriate data compression type you want to use
	 (
		 SELECT 
			 part.object_id 
			,part.index_id 
			,part.data_compression 
			,ROW_NUMBER() OVER
			(
				PARTITION BY 
					 part.object_id 
					,part.index_id 
				ORDER BY 
					 COUNT(*) DESC 
			) AS main_compression 
		 FROM sys.partitions AS part WITH (NOLOCK) 
		 GROUP BY part.object_id, part.index_id, part.data_compression 
	 ) AS tPartitions 
	 ON tPartitions.object_id = ind.object_id 
	 AND tPartitions.index_id = ind.index_id 
	 AND tPartitions.main_compression = 1

 OUTER APPLY
	 (
		 SELECT 
			COL_NAME(ind.object_id, indcols.column_id) AS partition_column 
		 FROM sys.index_columns AS indcols WITH (NOLOCK) 
		 WHERE indcols.object_id = ind.object_id 
		 AND indcols.index_id = ind.index_id 
		 AND indcols.partition_ordinal = 1 
	 ) AS tIndexColumns 
WHERE ind.type IN (1,2) --clustered AND nonclustered
AND syso.Type != 'S'
-- AND OBJECT_NAME(ind.object_id) NOT LIKE 'queue_messages_%'
-- AND OBJECT_NAME(ind.object_id) NOT LIKE 'filestream_tombstone_%'
-- AND OBJECT_NAME(ind.object_id) NOT LIKE 'sys%' --if you have index start with sys THEN remove it 
AND ind.is_primary_key = 0 
AND ind.is_unique_constraint = 0 

AND 
( 
	CAST
	( 
		CASE 
			WHEN syso.is_ms_shipped = 1 THEN 1 
			WHEN 
				( 
					SELECT major_id 
					FROM sys.extended_properties 
					WHERE major_id = syso.object_id 
					AND minor_id = 0 
					AND class = 1 
					AND name = N'microsoft_database_tools_support'
				) IS NOT NULL 
				THEN 1 
			ELSE 0 
		END 
		AS bit 
	) = 'false' 
) 
