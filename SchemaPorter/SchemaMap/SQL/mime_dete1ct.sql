
-- SELECT CAST('GIF89a' AS varbinary(6))
-- SELECT CAST('GIF87a' AS varbinary(6))

;WITH CTE AS 
(
    SELECT TOP 10 Thumbnail
    ,
    CONVERT
    (
        -- hex string to varbinary 
        varbinary(max),'0x' +  
            REPLACE
            (
                RTRIM
                (
                    REPLACE
                    (
                        -- varbinary to hex string 
                        CONVERT(varchar(max), Thumbnail, 2)
                        ,'0'
                        ,' '
                    )
                )
                ,' '
                ,'0'
            )
        ,1
    ) as lolz 

    FROM T_File 
    WHERE Thumbnail IS NOT NULL 
    AND CASE WHEN SUBSTRING(Thumbnail, DATALENGTH(Thumbnail)-1, 2) = 0xFFD9
        THEN 1
        ELSE 0
    END = 0 
)
SELECT 
     * 
    ,SUBSTRING(lolz, DATALENGTH(lolz)-1, 2) AS LastTwo 
    ,
    CASE  
        WHEN SUBSTRING(lolz, DATALENGTH(lolz)-1, 2) = 0xFFD9 THEN 1 
        ELSE 0 
    END AS foo 

FROM CTE 
