
SELECT  
	 SCHEM.name AS TABLE_SCHEMA 
	,T.name AS TABLE_NAME 
	,I.name AS INDEX_NAME 

	,tmp4.KeyColumns AS KEY_COLUMNS 
	,
	CASE WHEN temp_index_counter.cnt > 1 THEN 1
		ELSE 0
	END IS_MULTICOLUMN_INDEX 

	,tmp2.IncludedColumns AS INCLUDED_COLUMNS 
	,i.index_id AS INDEX_ORDINAL 
FROM sys.indexes AS I

CROSS APPLY 
	(
		SELECT COUNT(*) AS cnt FROM sys.index_columns AS MultiColumnIndex 
		WHERE MultiColumnIndex.object_id = I.object_id 
		AND MultiColumnIndex.index_id = I.index_id 
	) AS temp_index_counter 

INNER JOIN sys.tables AS T
    ON  T.object_id = I.object_id 

INNER JOIN sys.schemas AS SCHEM 
	ON SCHEM.schema_id = T.schema_id 


INNER JOIN sys.sysindexes AS SI
    ON  I.object_id = SI.id
    AND I.index_id = SI.indid

INNER JOIN 
	(
		SELECT *
		FROM   
		(
			SELECT 
				IC2.object_id,
				IC2.index_id,
				STUFF
				(
					(
						SELECT ' , e.' + C.name + CASE 
													WHEN MAX(CONVERT(INT, IC1.is_descending_key)) 
														= 1 THEN 
														-- ' DESC '
														''
													ELSE 
														--' ASC '
														'' 
												END
						FROM sys.index_columns AS IC1 

						INNER JOIN sys.columns AS C
							ON  C.object_id = IC1.object_id
							AND C.column_id = IC1.column_id
							AND IC1.is_included_column = 0

						WHERE  IC1.object_id = IC2.object_id
						AND IC1.index_id = IC2.index_id

						GROUP BY
							IC1.object_id,
							C.name,
							index_id
						ORDER BY
							MAX(IC1.key_ordinal) 
							FOR XML PATH('')
					),
					1,
					2,
					'' 
				) KeyColumns
			FROM sys.index_columns AS IC2 
			-- WHERE IC2.Object_id = object_id('Person.Address') --Comment for all tables
			GROUP BY
				IC2.object_id,
				IC2.index_id
		) AS tmp3
    ) AS tmp4
    ON I.object_id = tmp4.object_id
    AND I.Index_id = tmp4.index_id

INNER JOIN sys.stats AS ST
    ON ST.object_id = I.object_id
    AND ST.stats_id = I.index_id

INNER JOIN sys.data_spaces AS DS
    ON I.data_space_id = DS.data_space_id

INNER JOIN sys.filegroups AS FG
    ON I.data_space_id = FG.data_space_id

LEFT JOIN 
	(
        SELECT *
        FROM   
		(
			SELECT 
				IC2.object_id,
				IC2.index_id,
				STUFF
				(
					(
						SELECT 
							' , ' + C.name 
						FROM sys.index_columns AS IC1 
						INNER JOIN sys.columns AS C 
							ON C.object_id = IC1.object_id 
							AND C.column_id = IC1.column_id 
							AND IC1.is_included_column = 1 

						WHERE IC1.object_id = IC2.object_id 
						AND IC1.index_id = IC2.index_id 

						GROUP BY
								 IC1.object_id 
								,C.name 
								,index_id 
						FOR XML PATH('') 
					),
					1,
					2,
					''
				) AS IncludedColumns 
			FROM sys.index_columns AS IC2 
			-- WHERE IC2.Object_id = object_id('Person.Address') --Comment for all tables
			GROUP BY
					IC2.object_id,
					IC2.index_id
		) AS tmp1 
        WHERE IncludedColumns IS NOT NULL 
    ) AS tmp2 
    ON tmp2.object_id = I.object_id
    AND tmp2.index_id = I.index_id
WHERE (1=1) 
AND I.is_primary_key = 0
AND I.is_unique_constraint = 0
