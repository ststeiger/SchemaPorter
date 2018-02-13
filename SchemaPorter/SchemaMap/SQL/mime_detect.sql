-- SELECT CAST('GIF89a' AS varbinary(6))
-- SELECT CAST('GIF87a' AS varbinary(6))

SELECT TOP 10 Thumbnail

,SUBSTRING(Thumbnail, DATALENGTH(Thumbnail)-1, 2) AS LastTwo
,Thumbnail

--,master.dbo.fn_varbintohexstr(Thumbnail) AS luul 

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


    , CASE WHEN SUBSTRING(Thumbnail, DATALENGTH(Thumbnail)-1, 2) = 0xFFD9
        THEN 1
        ELSE 0
    END

    ,CASE 
        WHEN CONVERT(varbinary(2), Thumbnail)=0x424D THEN 'BMP' 
        WHEN CONVERT(varbinary(2), Thumbnail)=0xFFD8 THEN 'JPG' 
        WHEN CONVERT(varbinary(2), Thumbnail)=0x2550 THEN 'PDF' 
        WHEN CONVERT(varbinary(2), Thumbnail)=0xD0CF THEN 'DOC' 
    END AS foo 

FROM T_File 
WHERE Thumbnail IS NOT NULL 
AND CASE WHEN SUBSTRING(Thumbnail, DATALENGTH(Thumbnail)-1, 2) = 0xFFD9
    THEN 1
    ELSE 0
END = 0 
