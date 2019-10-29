
SELECT 
	 isr.SPECIFIC_SCHEMA 
	,isr.SPECIFIC_NAME 
	,isr.ROUTINE_TYPE 
	,isr.DATA_TYPE 
	,tParameters.params 
	 
	,CASE 
		WHEN isr.ROUTINE_TYPE = 'PROCEDURE'
			THEN 
				'CREATE PROCEDURE ' + QUOTENAME(isr.SPECIFIC_SCHEMA) + N'.' + QUOTENAME(isr.SPECIFIC_NAME) 
				+ COALESCE(N'( ' + NULLIF(tParameters.params, '')  + N' ) ', '') + '
AS
BEGIN PRINT ''Hello World !'' END ' 
		WHEN isr.ROUTINE_TYPE = 'FUNCTION' AND isr.DATA_TYPE = 'TABLE' 
			THEN 
				'CREATE FUNCTION ' + QUOTENAME(isr.SPECIFIC_SCHEMA) + N'.' + QUOTENAME(isr.SPECIFIC_NAME) + N'( ' + tParameters.params  + N' )
		RETURNS TABLE 
AS
RETURN 
( 
	SELECT ' + tColumns.dummyColumns + N'
)' 
		WHEN isr.ROUTINE_TYPE = 'FUNCTION' AND isr.DATA_TYPE <> 'TABLE' 
			THEN 
				'CREATE FUNCTION ' + QUOTENAME(isr.SPECIFIC_SCHEMA) + N'.' + QUOTENAME(isr.SPECIFIC_NAME) + N'( ' + tParameters.params  + N' )
RETURNS ' + isr.DATA_TYPE + tReturnDataType.returnDataType 
 + N' AS BEGIN RETURN NULL END 
'  
	END 
	AS dummy 

FROM INFORMATION_SCHEMA.ROUTINES AS isr 

LEFT JOIN sys.schemas AS sch  ON sch.name = isr.SPECIFIC_SCHEMA 
LEFT JOIN sys.objects AS syso
	ON syso.name = isr.SPECIFIC_NAME 
	AND syso.schema_id = sch.schema_id 

OUTER APPLY 
	(
		SELECT 
			CASE 
				WHEN isr.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
					THEN N'(' 
						+ 
						CASE WHEN isr.CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
							ELSE CONVERT
								(
									varchar(4)
									,isr.CHARACTER_MAXIMUM_LENGTH 
								)
						END 
						+ N')' 
				WHEN isr.DATA_TYPE IN ('decimal', 'numeric')
					THEN N'(' + CONVERT(varchar(4), isr.NUMERIC_PRECISION) + N', ' + CONVERT(varchar(4), isr.NUMERIC_SCALE) + N')'
				WHEN isr.DATA_TYPE IN ('time', 'datetime2', 'datetimeoffset') 
					THEN N'(' + CAST(isr.DATETIME_PRECISION AS national character varying(36)) + N')' 
				ELSE N'' 
			END AS returnDataType 
	) AS tReturnDataType
	
OUTER APPLY (
	SELECT 
		COALESCE(
		STUFF(
		(
			SELECT 
				 N', ' 
				+ isp.PARAMETER_NAME + N' ' 
				+ CASE 
					WHEN isp.DATA_TYPE = 'table type' THEN userDefinedTypes.name 
					ELSE isp.DATA_TYPE 
				END 
				+ 
				CASE 
					WHEN isp.DATA_TYPE = 'table type' THEN ' readonly' 
					WHEN isp.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
						THEN N'(' 
							+ 
							CASE WHEN isp.CHARACTER_MAXIMUM_LENGTH = -1 THEN N'MAX'
								ELSE CONVERT
									(
											varchar(4)
										,isp.CHARACTER_MAXIMUM_LENGTH 
									)
							END 
							+ N')' 
					WHEN isp.DATA_TYPE IN ('decimal', 'numeric')
						THEN N'(' + CONVERT(varchar(4), isp.NUMERIC_PRECISION) + N', ' + CONVERT(varchar(4), isp.NUMERIC_SCALE) + N')'
					WHEN isp.DATA_TYPE IN ('time', 'datetime2', 'datetimeoffset') 
						THEN N'(' + CAST(isp.DATETIME_PRECISION AS national character varying(36)) + N')' 
					ELSE N'' 
				END 
				+ 
				CASE isr.ROUTINE_TYPE 
					WHEN 'PROCEDURE'
					THEN  
						CASE WHEN isp.PARAMETER_MODE <> 'IN'-- only for procedures
							THEN N' OUTPUT'
							ELSE N''
						END 
					ELSE ''
				END 
				AS [text()]  
			FROM INFORMATION_SCHEMA.PARAMETERS AS isp 
			
			INNER JOIN sys.objects AS o
			INNER JOIN sys.schemas AS sch
				ON sch.schema_id = o.schema_id 
				ON sch.name = isp.SPECIFIC_SCHEMA
				AND o.name = isp.SPECIFIC_NAME
				
			INNER JOIN sys.parameters c 
				ON c.object_id = o.object_id  
				AND c.parameter_id = isp.ORDINAL_POSITION

			LEFT JOIN sys.types AS userDefinedTypes 
				ON userDefinedTypes.user_type_id = c.user_type_id 
				AND userDefinedTypes.is_user_defined = 1
				
			WHERE (1=1) 
			AND isp.PARAMETER_NAME <> N'' 
			AND isp.SPECIFIC_SCHEMA = isr.SPECIFIC_SCHEMA
			AND isp.SPECIFIC_NAME = isr.SPECIFIC_NAME 

			ORDER BY isp.ORDINAL_POSITION 

			FOR XML PATH(''), TYPE 
		).value('.', 'nvarchar(MAX)') 
		,1,2, '')
		, '') 
		AS params 
	) AS tParameters  


OUTER APPLY 
	(
		SELECT 
		(
			SELECT 
				CASE WHEN isrc.ORDINAL_POSITION = 1 THEN ' ' ELSE ',' END 
				+ N'CAST(NULL AS ' 
				+ isrc.DATA_TYPE 
				+ 
				CASE 
					WHEN isrc.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
						THEN N'(' 
							+ 
							CASE WHEN isrc.CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
								ELSE CONVERT
									(
										varchar(4)
										,isrc.CHARACTER_MAXIMUM_LENGTH 
									)
							END 
							+ N')' 
					WHEN isrc.DATA_TYPE IN ('decimal', 'numeric')
						THEN N'(' + CONVERT(varchar(4), isrc.NUMERIC_PRECISION) + N', ' + CONVERT(varchar(4), isrc.NUMERIC_SCALE) + N')'
					WHEN isrc.DATA_TYPE IN ('time', 'datetime2', 'datetimeoffset') 
						THEN N'(' + CAST(isrc.DATETIME_PRECISION AS national character varying(36)) + N')' 
					ELSE N'' 
				END 
				+ N') AS ' + QUOTENAME(isrc.COLUMN_NAME) 
				+ NCHAR(13) + NCHAR(10) 
				AS [text()]
			FROM INFORMATION_SCHEMA.ROUTINE_COLUMNS AS isrc 
			WHERE isrc.TABLE_NAME = 'tfu_RPT_FM_DATA_DIN277_Grob_Parkplaetze_OLD_20190611' 
			ORDER BY isrc.ORDINAL_POSITION
			FOR XML PATH(''), TYPE 
		).value('.', 'nvarchar(MAX)') AS dummyColumns  
	) AS tColumns 


	-- WHERE ROUTINE_TYPE = 'PROCEDURE' 
WHERE (1=1) 
-- AND ROUTINE_TYPE = 'FUNCTION' 
-- AND DATA_TYPE <> 'TABLE' 
-- AND DATA_TYPE = 'TABLE' 

-- AND SPECIFIC_SCHEMA = 'dbo'
-- AND SPECIFIC_NAME = 'stp_VWS_ObjektCopy'
-- AND SPECIFIC_NAME = 'tfu_RPT_MSEL_Geschossfilter' 

AND 
(
	CAST
	(
		CASE 
			WHEN syso.is_ms_shipped = 1
				THEN 1
			WHEN (
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
		AS BIT
	) = 'false' 
)
