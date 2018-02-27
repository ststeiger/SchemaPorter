
;WITH CTE AS 
(
	SELECT  
		 SCHEM.name AS TABLE_SCHEMA 
		,T.name AS TABLE_NAME 
		,I.name AS INDEX_NAME 
		--,I.has_filter
		--,I.is_unique_constraint
		--,I.is_primary_key
		--,I.is_unique

		,tmp4.KeyColumns AS KEY_COLUMNS 
		,
		CASE WHEN temp_index_counter.cnt > 1 THEN 1
			ELSE 0
		END IS_MULTICOLUMN_INDEX 
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
	AND I.is_unique = 1 

)

SELECT 
	 it.TABLE_SCHEMA
	,it.TABLE_NAME
	,COALESCE(tc.CONSTRAINT_NAME, CTE.INDEX_NAME) AS PRIMARY_KEY_NAME 
	
	,
	COALESCE
	(
		STUFF
		(
			(
				SELECT
					', e.' + COLUMN_NAME
				FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS kcu 
				WHERE kcu.CONSTRAINT_CATALOG = tc.CONSTRAINT_CATALOG
				AND kcu.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
				AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
				FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(MAX)') 
			,1
			, 2
			,''
		)
		,KEY_COLUMNS 
	)
	AS PRIMARY_KEY  


	,COALESCE 
	(
		CASE WHEN 
		(
			SELECT
				COUNT(*) 
			FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS kcu 
			WHERE kcu.CONSTRAINT_CATALOG = tc.CONSTRAINT_CATALOG
			AND kcu.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
			AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
		) > 1 THEN 1 
		ELSE CTE.IS_MULTICOLUMN_INDEX 
		END
		,0
	) AS IS_MULTICOLUMN_KEY   

FROM INFORMATION_SCHEMA.TABLES AS it 

LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS  AS tc 
	ON tc.TABLE_CATALOG = it.TABLE_CATALOG 
	AND tc.TABLE_SCHEMA = it.TABLE_SCHEMA 
	AND tc.TABLE_NAME = it.TABLE_NAME 
	AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY' 

LEFT JOIN CTE 
	ON CTE.TABLE_SCHEMA = it.TABLE_SCHEMA 
	AND CTE.TABLE_NAME = it.TABLE_NAME 

WHERE (1=1) 
AND TABLE_TYPE = 'BASE TABLE' 
-- AND tc.CONSTRAINT_NAME IS NULL -- Alle tabellen ohne PK 
-- AND COALESCE(tc.CONSTRAINT_NAME, CTE.INDEX_NAME) IS NULL -- Alle Tabellen ohne PK + unique index  

-- changeset_parents, issues_history

-- modifications:
-- changeset_parents -- i set composite primary-key 

-- issues_history

-- IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[issues_history]') AND type in (N'U'))
-- ALTER TABLE [dbo].[issues_history] DROP CONSTRAINT IF EXISTS [DF_issues_history_isshist_uid]
-- GO

-- ALTER TABLE [dbo].[issues_history] ADD [isshist_uid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_issues_history_isshist_uid]  DEFAULT (newid()) 
