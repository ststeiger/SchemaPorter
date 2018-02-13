
-- http://thomaslarock.com/sql-server-system-tables/



SELECT 

	CAST
	( 
		CASE 
			WHEN tbl.is_ms_shipped = 1 
				THEN 1 
			WHEN 
				(
					SELECT major_id FROM sys.extended_properties 
					WHERE 
						major_id = tbl.object_id 
						AND minor_id = 0 
						AND class = 1 
						AND name = N'microsoft_database_tools_support' 
				)  
				IS NOT NULL 
			THEN 1 
		ELSE 0 
	END 
	AS bit) AS IsSystemObject 
	,* 
FROM sys.tables AS tbl 

ORDER BY name 


---------------------------------------------


SELECT 
	CASE WHEN extp.major_id IS NOT NULL OR tbl.is_ms_shipped = 1 
		THEN 1
		ELSE 0
	END AS IsSystemObject
	
	,tbl.name
	,tbl.object_id
	,tbl.principal_id
	,tbl.schema_id
	,tbl.parent_object_id
	,tbl.type
	,tbl.type_desc
	,tbl.create_date
	,tbl.modify_date
	,tbl.is_ms_shipped
	,tbl.is_published
	,tbl.is_schema_published
	,tbl.lob_data_space_id
	,tbl.filestream_data_space_id
	,tbl.max_column_id_used
	,tbl.lock_on_bulk_load
	,tbl.uses_ansi_nulls
	,tbl.is_replicated
	,tbl.has_replication_filter
	,tbl.is_merge_published
	,tbl.is_sync_tran_subscribed
	,tbl.has_unchecked_assembly_data
	,tbl.text_in_row_limit
	,tbl.large_value_types_out_of_row
	
	-- not in 2005
	--,tbl.is_tracked_by_cdc
	--,tbl.lock_escalation
	--,tbl.lock_escalation_desc
FROM sys.tables AS tbl 

LEFT JOIN sys.extended_properties AS extp 
	ON extp.major_id = tbl.object_id 
	AND extp.minor_id = 0 
	AND extp.class = 1 
	AND extp.name = N'microsoft_database_tools_support' 
	
ORDER BY tbl.name 
