
SET NOCOUNT ON; 


DECLARE @tbl_benutzer table(   [BE_ID] int NOT NULL  ,[BE_Name] character varying(50)  ,[BE_Vorname] character varying(50)  ,[BE_User] character varying(50)  ,[BE_Passwort] character varying(50)  ,[BE_Language] character varying(5)  ,[BE_Hash] character varying(50)  ,[BE_Level] tinyint NOT NULL  ,[BE_isLDAPSync] bit NOT NULL  ,[BE_Domaene] character varying(255)  ,[BE_Hide] bit  ,[BE_Email] character varying(150)  ,[BE_TelNr] character varying(20)  ,[BE_CurrencyID] int  ,[BE_Status] int  ,[BE_IsCOR_Hash] character varying(50)  ,[BE_IsGuest] bit NOT NULL  ,[BE_Augen] int  ,[BE_usePRT] bit  ,[BE_useMOD] bit  ,[BE_useNA] bit  ,[BE_ActiveDirectoryUID] uniqueidentifier  ); 


INSERT INTO @tbl_benutzer 
(
     BE_ID 
	,BE_Name
	,BE_Vorname
	,BE_User
	,BE_Passwort
	,BE_Language
	,BE_Hash
	,BE_Level
	,BE_isLDAPSync
	,BE_Domaene
	,BE_Hide
	,BE_Email
	,BE_TelNr
	,BE_CurrencyID
	,BE_Status
	,BE_IsCOR_Hash
	,BE_IsGuest
	,BE_Augen
	,BE_usePRT
	,BE_useMOD
	,BE_useNA
	,BE_ActiveDirectoryUID
) 
SELECT 
	 BE_ID
	,BE_Name
	,BE_Vorname
	,BE_User
	,BE_Passwort
	,BE_Language
	,BE_Hash
	,BE_Level
	,BE_isLDAPSync
	,BE_Domaene
	,BE_Hide
	,BE_Email
	,BE_TelNr
	,BE_CurrencyID
	,BE_Status
	,BE_IsCOR_Hash
	,BE_IsGuest
	,BE_Augen
	,BE_usePRT
	,BE_useMOD
	,BE_useNA
	,BE_ActiveDirectoryUID
FROM T_Benutzer 


-- SELECT * FROM @tbl_benutzer 




MERGE INTO T_Benutzer AS A 
USING @tbl_benutzer AS CTE ON CTE.BE_ID = A.BE_ID 
WHEN MATCHED 
	THEN UPDATE 
		SET  A.BE_Name = CTE.BE_Name 
			,A.BE_Vorname = CTE.BE_Vorname 
			
WHEN NOT MATCHED THEN 
INSERT 
( 
	 BE_ID 
	,BE_Name 
	,BE_Vorname 
) 
VALUES 
( 
	 CTE.BE_ID 
	,CTE.BE_Name 
	,CTE.BE_Vorname 
); 


-- DELETE FROM T_Benutzer WHERE BE_ID = '123'; 
-- SELECT * FROM T_Benutzer; 


SET NOCOUNT OFF; 
