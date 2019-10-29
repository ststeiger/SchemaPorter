
// https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/

using System.Collections.Generic;

namespace SchemaPorter
{


    public class DbDiff
    {
        // schemas
        // tables column, pk, fk, uc, uix, 
        // views, procedures, scalar_functions, table_functions, triggers, indices, 

        // hashset tables
        // dictionary table-hashset columns
        // dictionary table-columns
        // dictionary table-columns-properties
        // dictionary table-computed columns
        // dictionary table-non-computed columns

        // hashset views
        // dictionary view-hashset columns
        // dictionary views-columns
        // dictionary views tables
        // dictionary views dependencies
        // dictionary views tables
        // dictionary views scalar functions
        // dictionary views table functions


        public static void ListDbs()
        {
	        string sql = @"
SELECT 
     sch.name AS TABLE_SCHEMA  
    ,tables.name AS TABLE_NAME 
    ,sc.name AS COLUMN_NAME 
    -- ,sc.column_id AS ORDINAL_POSITION 
    ,COLUMNPROPERTY(sc.object_id, sc.name, 'ordinal') AS ORDINAL_POSITION 
    -- ,sm.text AS COLUMN_DEFAULT 
    ,CASE WHEN sc.is_nullable = 1 THEN 'YES' ELSE 'NO' END AS IS_NULLABLE 
    ,t.name AS DATA_TYPE  
    ,sc.is_identity 
    ,sc.is_computed AS IS_COMPUTED 
    ,col.is_persisted AS IS_PERSISTED 
        
    ,col.""definition"" AS COMPUTATION  
    ,COLUMNPROPERTY(sc.object_id, sc.name, 'charmaxlen') AS CHARACTER_MAXIMUM_LENGTH 
    ,COLUMNPROPERTY(sc.object_id, sc.name, 'octetmaxlen') AS CHARACTER_OCTET_LENGTH 
    
    ,CONVERT(tinyint, CASE -- int/decimal/numeric/real/float/money  
        WHEN sc.system_type_id IN (48, 52, 56, 59, 60, 62, 106, 108, 122, 127) THEN sc.precision  
        END) AS NUMERIC_PRECISION 

    ,CONVERT(int, CASE -- datetime/smalldatetime  
        WHEN sc.system_type_id IN (40, 41, 42, 43, 58, 61) THEN NULL  
        ELSE ODBCSCALE(sc.system_type_id, sc.scale) 
        END) AS NUMERIC_SCALE 
        
    ,CONVERT(smallint, CASE -- datetime/smalldatetime  
        WHEN sc.system_type_id IN (40, 41, 42, 43, 58, 61) 
        THEN ODBCSCALE(sc.system_type_id, sc.scale) 
    END) AS DATETIME_PRECISION 

    ,
    t.name 
    + 
    CASE 
        WHEN t.name IN ('char', 'varchar', 'nchar', 'nvarchar', 'binary', 'varbinary') 
            THEN '(' 
                + 
                CASE WHEN sc.max_length = -1 THEN 'MAX'
                    ELSE CONVERT
                        (
                                varchar(4)
                            ,CASE WHEN t.name IN ('nchar', 'nvarchar') THEN sc.max_length/2 ELSE sc.max_length END 
                        )
                END 
                + ')' 
        WHEN t.name IN ('decimal', 'numeric')
            THEN '(' + CONVERT(varchar(4), sc.precision)+', ' + CONVERT(varchar(4), sc.Scale) + ')'
        WHEN t.name IN ('time', 'datetime2', 'datetimeoffset') 
            THEN N'(' + CAST(ODBCSCALE(sc.system_type_id, sc.scale) AS national character varying(36)) + N')' 
        ELSE '' 
    END AS DDL_NAME  

    ,dc.name 
    ,dc.definition 
    -- ,sc.is_filestream 
    -- ,sc.is_identity 
    -- ,sc.is_sparse 
FROM sys.columns AS sc 
-- INNER JOIN sys.objects AS o ON o.object_id = sc.object_id 
INNER JOIN sys.tables AS tables ON sc.object_id = tables.object_id 
INNER JOIN sys.schemas AS sch ON sch.schema_id = tables.schema_id 
LEFT JOIN sys.computed_columns AS col ON col.object_id = sc.object_id AND col.column_id = sc.column_id 
INNER JOIN sys.types AS t ON sc.user_type_id = t.user_type_id 
LEFT JOIN sys.syscomments AS sm ON sm.id = sc.default_object_id

LEFT JOIN sys.default_constraints AS dc  
    ON dc.parent_object_id = tables.object_id  
    AND dc.parent_column_id = sc.object_id 
    
LEFT JOIN 
    (
            SELECt 
                    1 AS tbl_id
                ,sch.name AS table_schema
                ,tables.name AS table_name 
                ,sch.schema_id 
                ,tables.object_id 
            FROM sys.tables AS tables -- ON sc.object_id = tables.object_id 
            INNER JOIN sys.schemas AS sch ON sch.schema_id = tables.schema_id 
            INNER JOIN sys.extended_properties AS xp ON xp.major_id = tables.object_id AND xp.minor_id = 0 
            AND xp.name = 'microsoft_database_tools_support' 
            GROUP BY sch.schema_id, sch.name, tables.object_id, tables.name 
    ) AS systemTables 
    ON systemTables.schema_id = sch.schema_id  
    AND systemTables.object_id = tables.object_id  

WHERE (1=1) -- 12'846
AND tables.is_ms_shipped = 0 -- 12'839 
AND systemTables.tbl_id IS NULL --12'834


-- schemas1
-- schema2

-- tables1
-- tables2

-- columns1
-- columns2

-- defaults

-- primary keys 
-- foreign keys

-- views1
-- views2

-- functions1
-- functions2

-- tvf1
-- tvf2 

-- stored_procedures1
-- stored_procedures2


-- indices
-- unique indices
-- unique constraints
-- check constraints

-- assemblies1
-- assemblies2


-- version
	

-- https://www.sqlshack.com/sql-server-system-databases-the-master-database/
-- https://dba.stackexchange.com/questions/213494/how-to-locate-resource-system-database-in-ms-sql-server
SELECT 
     sysdb.name
    ,sysdb.compatibility_level
    ,sysdb.owner_sid 
    ,sysdb.collation_name 
    ,sysdb.snapshot_isolation_state
    ,sysdb.is_fulltext_enabled
    ,sysdb.state_desc -- = N'ONLINE'            -- is online
    ,sysdb.user_access_desc -- = N'MULTI_USER'  -- open for all users
    ,sysdb.is_read_only -- = 0 -- not read-only

    ,CAST
    (
        CASE 
            -- ReportServer, ReportServerTempDB
            WHEN sysdb.name in ('master','model','msdb','tempdb') THEN 1 
            WHEN sysdb.database_id BETWEEN 1 AND 4 THEN 1 -- master, tempdb, model, and msdb
            WHEN sysdb.name LIKE 'ReportServer$%' THEN 1 -- Report Server
            ELSE sysdb.is_distributor 
        END 
        AS bit
    ) AS IsSystemDatabase 
FROM sys.databases AS sysdb
-- WHERE name = 'WideWorldImporters';

-- SELECT * FROM sys.views 

-- SELECT * FROM sys.tables 
-- SELECT * FROM sys.procedures 




SELECT *
FROM sys.objects
WHERE type IN (N'FN', N'IF', N'TF', N'FS', N'FT')  -- FN: scalar, IF: inline table-valued, TF: table-valued, FS: Assembly (CLR) scalar-function, FT: Assembly (CLR) table-valued function

SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'FUNCTION'

SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE NOT IN ( 'FUNCTION', 'PROCEDURE')

";

        }


        public static void ListUserDefinedTypes() { }

        public static void ListAssemblies()
        {
            string sql = @"
SELECT 
	 name
	,clr_name
	,permission_set_desc
FROM sys.assemblies
";
        }
        public static void ListDatabases()
        {
            string sql = @"
SELECT 
	 name
	,compatibility_level
	,user_access_desc
	,state_desc
	,is_read_only
	,snapshot_isolation_state_desc
	,recovery_model_desc
	,is_quoted_identifier_on
	,is_fulltext_enabled
	,is_trustworthy_on
FROM sys.databases 
";
        }


        public static void ListSchemas()
        {
            string sql = @"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA";
        }
        public static void ListTables()
        {
            string sql = @"
SELECT 
	 TABLE_SCHEMA	
	,TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
";
        }


        public static void ListColumns()
        {
            string sql = @"
SELECT 
	 TABLE_SCHEMA
	,TABLE_NAME
	,COLUMN_NAME
	,ORDINAL_POSITION
	,COLUMN_DEFAULT
	,IS_NULLABLE
	,DATA_TYPE
	,CHARACTER_MAXIMUM_LENGTH
	,CHARACTER_OCTET_LENGTH
	,NUMERIC_PRECISION
	,NUMERIC_PRECISION_RADIX
	,NUMERIC_SCALE
	,DATETIME_PRECISION
	,CHARACTER_SET_CATALOG
	,CHARACTER_SET_SCHEMA
	,CHARACTER_SET_NAME
	,COLLATION_CATALOG
	,COLLATION_SCHEMA
	,COLLATION_NAME
	,DOMAIN_CATALOG
	,DOMAIN_SCHEMA
	,DOMAIN_NAME
FROM INFORMATION_SCHEMA.COLUMNS 
";
        }
        public static void ListViews()
        {
            string sql = @"
SELECT 
	 sch.name AS TABLE_SCHEMA 
	,v.name AS TABLE_NAME 
	,m.definition AS VIEW_DEFINITION 
	,'NO' AS IS_UPDATABLE 
	,CONVERT(varchar(7), 
		CASE v.with_check_option  
			WHEN 1 THEN 'CASCADE'  
			ELSE 'NONE' 
		END
	) AS CHECK_OPTION
FROM sys.views AS v 
INNER JOIN sys.schemas AS sch ON sch.schema_id = v.schema_id 
INNER JOIN sys.sql_modules AS m on m.object_id = v.object_id
/*
SELECT 
	 TABLE_SCHEMA
	,TABLE_NAME
	,VIEW_DEFINITION
	,OBJECT_DEFINITION(OBJECT_ID(TABLE_SCHEMA + '.' + TABLE_NAME)) AS object_defintion 
	,CHECK_OPTION
	,IS_UPDATABLE
FROM INFORMATION_SCHEMA.VIEWS 
*/
";
        }

        public static void ListScalarFunctions() {
            string sql = @"
SELECT 
	 sch.name AS SPECIFIC_SCHEMA 
	,o.name AS SPECIFIC_NAME 

	,sch.name AS ROUTINE_SCHEMA 
	,o.name AS ROUTINE_NAME 
	 
	,CONVERT(nvarchar(20), 
		CASE 
			WHEN o.type IN ('P','PC', 'X')  THEN 'PROCEDURE' 
			ELSE 'FUNCTION' 
		END 
	 ) AS ROUTINE_TYPE 
	 
	,m.definition AS ROUTINE_DEFINITION 


	,CONVERT(sysname, 
		CASE  
			WHEN o.type IN ('TF', 'IF', 'FT') THEN N'TABLE'  
			ELSE ISNULL(TYPE_NAME(c.system_type_id),  TYPE_NAME(c.user_type_id)) 
		END
	 ) AS DATA_TYPE 
	 
	,COLUMNPROPERTY(c.object_id, c.name, 'charmaxlen') AS CHARACTER_MAXIMUM_LENGTH 
	,COLUMNPROPERTY(c.object_id, c.name, 'octetmaxlen') AS CHARACTER_OCTET_LENGTH 


	,CONVERT(tinyint, 
		CASE -- int/decimal/numeric/real/float/money  
			WHEN c.system_type_id IN (48, 52, 56, 59, 60, 62, 106, 108, 122, 127) THEN c.precision  
		END
	 ) AS NUMERIC_PRECISION 
	 
	,CONVERT(int, 
		CASE -- datetime/smalldatetime  
			WHEN c.system_type_id IN (40, 41, 42, 43, 58, 61) THEN NULL  
			ELSE ODBCSCALE(c.system_type_id, c.scale) 
		END
	) AS NUMERIC_SCALE 
	 
	,CONVERT(smallint, 
		CASE -- datetime/smalldatetime  
			WHEN c.system_type_id IN (40, 41, 42, 43, 58, 61) THEN ODBCSCALE(c.system_type_id, c.scale) 
		END
	 ) AS DATETIME_PRECISION 
	 
	,CONVERT(nvarchar(30), 
		CASE  
			WHEN o.type IN ('P ', 'FN', 'TF', 'IF') THEN 'SQL'  
			ELSE 'EXTERNAL' 
		END
	) AS ROUTINE_BODY 
	
	,CONVERT(nvarchar(10), 
		CASE 
			WHEN ObjectProperty(o.object_id, 'IsDeterministic') = 1 THEN 'YES' 
			ELSE 'NO' 
		END
	 ) AS IS_DETERMINISTIC 
FROM sys.objects AS o 
LEFT JOIN sys.parameters AS c ON (c.object_id = o.object_id AND c.parameter_id = 0)  
INNER JOIN sys.schemas AS sch ON sch.schema_id = o.schema_id 
INNER JOIN sys.sql_modules AS m on m.object_id = o.object_id

  -- AF: Aggregate function (CLR)
-- WHERE o.type IN (  'AF', )
-- WHERE o.type IN ('P', 'PC', 'FN', 'TF', 'IF', 'FT', 'AF', 'IS', 'FS')  
-- P  = SQL Stored Procedure, PC = Assembly (CLR) stored-procedure
-- P : stored procedure, PC : assembly stored procedure, X : extended stored proc 
-- WHERE o.type IN ('P','PC', 'X')  -- PROCEDURE 
-- TF: table function, IF: inline table-valued function, FT: assembly table function
-- WHERE o.type IN ('TF', 'IF', 'FT') -- N'TABLE'  
-- FN: scalar function, IS: inline scalar function, FS: assembly scalar function
-- WHERE o.type IN ('FN', 'IS', 'FS')

-- SELECT * from master..spt_values WHERE type = 'O9T'

/*
SELECT 
	 SPECIFIC_SCHEMA
	,SPECIFIC_NAME
	,ROUTINE_SCHEMA
	,ROUTINE_NAME
	,*
	,DATA_TYPE
	,CHARACTER_MAXIMUM_LENGTH
	,CHARACTER_OCTET_LENGTH
	,NUMERIC_PRECISION
	,NUMERIC_SCALE
	,DATETIME_PRECISION
	,ROUTINE_BODY
	,IS_DETERMINISTIC
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_TYPE = 'FUNCTION' 
AND DATA_TYPE <> 'TABLE' 
*/
";
        }
        public static void ListScalarFunctionArguments() { }



        public static void ListTVFFunctions() { }

        public static void ListTVFFunctionsArguments() { }


        public static void Procedures() { }



        public static void ListParameters()
        {
            string sql = @"
SELECT 
	 SPECIFIC_SCHEMA
	,SPECIFIC_NAME
	,ORDINAL_POSITION
	,PARAMETER_MODE
	,IS_RESULT
	,AS_LOCATOR
	,PARAMETER_NAME
	,DATA_TYPE
	,CHARACTER_MAXIMUM_LENGTH
	,CHARACTER_OCTET_LENGTH
	,COLLATION_CATALOG
	,COLLATION_SCHEMA
	,COLLATION_NAME
	,CHARACTER_SET_CATALOG
	,CHARACTER_SET_SCHEMA
	,CHARACTER_SET_NAME
	,NUMERIC_PRECISION
	,NUMERIC_PRECISION_RADIX
	,NUMERIC_SCALE
	,DATETIME_PRECISION
	,INTERVAL_TYPE
	,INTERVAL_PRECISION
	,USER_DEFINED_TYPE_CATALOG
	,USER_DEFINED_TYPE_SCHEMA
	,USER_DEFINED_TYPE_NAME
	,SCOPE_CATALOG
	,SCOPE_SCHEMA
	,SCOPE_NAME 
FROM INFORMATION_SCHEMA.PARAMETERS 
";
        }



        public static void ListTvfColumns() {
            string sql = @"

SELECT 
	 TABLE_NAME
	,COLUMN_NAME
	,ORDINAL_POSITION
	,COLUMN_DEFAULT
	,IS_NULLABLE
	,DATA_TYPE
	,CHARACTER_MAXIMUM_LENGTH
	,CHARACTER_OCTET_LENGTH
	,NUMERIC_PRECISION
	,NUMERIC_PRECISION_RADIX
	,NUMERIC_SCALE
	,DATETIME_PRECISION
	,CHARACTER_SET_CATALOG
	,CHARACTER_SET_SCHEMA
	,CHARACTER_SET_NAME
	,COLLATION_CATALOG
	,COLLATION_SCHEMA
	,COLLATION_NAME
	,DOMAIN_CATALOG
	,DOMAIN_SCHEMA
	,DOMAIN_NAME
FROM INFORMATION_SCHEMA.ROUTINE_COLUMNS 
";
        }




        public static void ListTriggers() {
            string sql = @"
SELECT 
     sch.name AS trigger_table_schema 
    ,systbl.name AS trigger_table_name 
    ,systrg.name AS trigger_name 
    ,sysm.definition AS trigger_definition 
    ,systrg.is_instead_of_trigger



    -- https://stackoverflow.com/questions/5340638/difference-between-a-for-and-after-triggers
    -- Difference between a FOR and AFTER triggers?
    -- CREATE TRIGGER trgTable on dbo.Table FOR INSERT,UPDATE,DELETE
    -- Is the same as
    -- CREATE TRIGGER trgTable on dbo.Table AFTER INSERT,UPDATE,DELETE
    -- An INSTEAD OF trigger is different, and fires before and instead of the insert 
    -- and can be used on views, in order to insert the appropriate values into the underlying tables.
    -- AFTER specifies that the DML trigger is fired only when all operations 
    -- specified in the triggering SQL statement have executed successfully. 
    -- All referential cascade actions and constraint checks also must succeed before this trigger fires. 
    -- AFTER is the default when FOR is the only keyword specified.
    ,CASE WHEN systrg.is_instead_of_trigger = 1 THEN 0 ELSE 1 END AS is_after_trigger 

    ,systrg.is_not_for_replication 
    ,systrg.is_disabled
    ,systrg.create_date 
    ,systrg.modify_date

    ,CASE WHEN systrg.parent_class = 1 THEN 'TABLE' WHEN systrg.parent_class = 0 THEN 'DATABASE' END trigger_class 


    ,CASE 
        WHEN systrg.[type] = 'TA' then 'Assembly (CLR) trigger'
        WHEN systrg.[type] = 'TR' then 'SQL trigger' 
        ELSE '' 
    END AS trigger_type 

    -- https://dataedo.com/kb/query/sql-server/list-triggers 
    -- ,(CASE WHEN objectproperty(systrg.object_id, 'ExecIsUpdateTrigger') = 1
    --      THEN 'UPDATE ' ELSE '' END 
    -- + CASE WHEN objectproperty(systrg.object_id, 'ExecIsDeleteTrigger') = 1
    --      THEN 'DELETE ' ELSE '' END
    -- + CASE WHEN objectproperty(systrg.object_id, 'ExecIsInsertTrigger') = 1
    --      THEN 'INSERT' ELSE '' END
    -- ) AS trigger_event 

    ,
    ( 
        STUFF 
        ( 
            ( 
                SELECT 
                    ', ' + type_desc AS [text()]
                    -- STRING_AGG(type_desc, ', ') AS foo 
                FROM sys.events AS syse 
                WHERE syse.object_id = systrg.object_id
                FOR XML PATH(''), TYPE 
                -- GROUP BY syse.object_id 
            ).value('.[1]', 'nvarchar(MAX)') 
            , 1, 2, '' 
        ) 
    ) AS trigger_event_groups 

    -- ,CASE WHEN systrg.parent_class = 1 THEN 'TABLE' WHEN systrg.parent_class = 0 THEN 'DATABASE' END trigger_class  

    ,'DROP TRIGGER ""' + sch.name + '"".""' + systrg.name + '""; ' AS sql 
    -- ,systrg.*
FROM sys.triggers AS systrg 

LEFT JOIN sys.sql_modules AS sysm 
    ON sysm.object_id = systrg.object_id 

-- sys.objects for view triggers 
-- LEFT JOIN sys.objects AS systbl ON systbl.object_id = systrg.object_id 

-- INNER JOIN if you only want table-triggers 
LEFT JOIN sys.tables AS systbl ON systbl.object_id = systrg.parent_id 
-- INNER JOIN sys.views AS systbl ON systbl.object_id = systrg.parent_id 


LEFT JOIN sys.schemas AS sch 
    ON sch.schema_id = systbl.schema_id 

WHERE (1=1) 

-- AND sch.name IS NOT NULL 
-- AND sch.name IS NULL 
-- AND sch.name = 'dbo' 
-- And here, exclude some triggers with a certain naming schema 
/*  
AND 
(
    -- systbl.name IS NULL 
    -- OR 
    NOT 
    (
        systrg.name = 'TRG_' + systbl.name  + '_INSERT_History'
        OR 
        systrg.name = 'TRG_' + systbl.name  + '_UPDATE_History'
        OR 
        systrg.name = 'TRG_' + systbl.name  + '_DELETE_History'
    )
)
*/

ORDER BY 
     sch.name 
    ,systbl.name 
    ,systrg.name 
";
        }



        public static void ListIndices() { }


        public static void ListUniqueConstrains() { }


        public static void ListCheckConstrains() {
            string sql = @"
SELECT 
	 con.name AS CONSTRAINT_NAME 
	,sch.name AS CONSTRAINT_SCHEMA 
	,t.name AS CONSTRAINT_TABLE 
	,col.name AS CONSTRAINT_COLUMN_NAME 
	,con.definition AS CONSTRAINT_DEFINITION 
	,CASE 
		WHEN con.is_disabled = 0
			THEN 'Active'
		ELSE 'Disabled'
	 END AS CONSTRAINT_STATUS 
	,N'ALTER TABLE ' + QUOTENAME(sch.name) + N'.' + QUOTENAME(t.name) + N' 
ADD CONSTRAINT ' + QUOTENAME(con.name) + N'
  CHECK (' + con.definition + '); ' AS SqlCreateCheck 
FROM sys.check_constraints AS con 
LEFT JOIN sys.objects AS t ON con.parent_object_id = t.object_id
LEFT JOIN sys.schemas AS sch ON sch.schema_id = t.schema_id 
LEFT JOIN sys.all_columns AS col 
	ON con.parent_column_id = col.column_id
	AND con.parent_object_id = col.object_id

ORDER BY 
	 con.name 
	,CONSTRAINT_SCHEMA 
	,CONSTRAINT_TABLE 
";
        }


    }





    public static class Program
    {


        static void EnumRegistry()
        {
            Microsoft.Win32.RegistryKey key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            foreach (string subkeyName in key.GetSubKeyNames())
            {
                System.Console.WriteLine(subkeyName);

                Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subkeyName);
                if (subkey != null)
                {
                    foreach (var value in subkey.GetValueNames())
                    {
                        System.Console.WriteLine("\tValue:" + value);

                        // Check for the publisher to ensure it's our product
                        string keyValue = System.Convert.ToString(subkey.GetValue("Publisher"));
                        if (!keyValue.Equals("MyPublisherCompanyName", System.StringComparison.OrdinalIgnoreCase))
                            continue;

                        string productName = System.Convert.ToString(subkey.GetValue("DisplayName"));
                        if (!productName.Equals("MyProductName", System.StringComparison.OrdinalIgnoreCase))
                            return;

                        string uninstallPath = System.Convert.ToString(subkey.GetValue("InstallSource"));

                        // Do something with this valuable information
                    }
                }
            }

            System.Console.ReadLine();
        }

        public static System.DateTime GetColumnInfo()
        {
            return System.DateTime.MaxValue;
        }

        public static string GenerateInsertScript()
        {
            return null;
        }


        public static void TestStack()
        {
            System.Collections.Generic.Stack<string> stack = new Stack<string>();
            string isEmpty = stack.Peek();
            stack.Push("abc");
            string abc = stack.Pop();
        }



        private static System.IO.FileStream CreateInheritedFile(string file)
        {
            return new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read | System.IO.FileShare.Inheritable);
        }

        public static void ExecuteProcess()
        {
            string outputName = @"output.txt";
            string errorName = @"error.txt";
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            string cmd = @"""C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319\vbc.exe""  /t:library /utf8output+ /r:""d:\username\Documents\Visual Studio 2017\Projects\AnyWebReporting\AnyFormsReporting\bin\Debug\AspNetCore.ReportingServices.dll"" /out:""D:\username\Desktop\TestCode\ExpressionHost.dll"" /debug- /optimize+  /verbose ""D:\username\Desktop\TestCode\yofz523a.0.vb""";
            System.Console.WriteLine(cmd);

            System.IO.StreamWriter outputWriter = new System.IO.StreamWriter(CreateInheritedFile(outputName), System.Text.Encoding.UTF8);
            try
            {
                using (new System.IO.StreamWriter(CreateInheritedFile(errorName), System.Text.Encoding.UTF8))
                {
                    outputWriter.Write(currentDir);
                    outputWriter.Write("> ");
                    outputWriter.WriteLine(cmd);
                    outputWriter.WriteLine();
                    outputWriter.WriteLine();

                    // System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(cmd)
                    // System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd)
                    // System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + cmd)
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c \"" + cmd + "\"")
                    {
                        WorkingDirectory = currentDir,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false
                    };
                    using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo))
                    {
                        process.OutputDataReceived += delegate (object s, System.Diagnostics.DataReceivedEventArgs e)
                        {
                            if (e.Data != null)
                            {
                                outputWriter.WriteLine(e.Data);
                            }
                        };
                        process.BeginOutputReadLine();
                        try
                        {
                        }
                        catch
                        {
                        }
                        process.WaitForExit();
                        int ret = process.ExitCode;
                        System.Console.WriteLine(ret);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }


        public static void Main(string[] args)
        {
            // ExecuteProcess();
            SchemaGeneration();

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }



        public static void SchemaGeneration()
        {
            SchemaPorter.Settings.SettingsHelper.Test();
            // TestSMO.Test();


            // SchemaGenerator.GenerateSchema();
            // ContextGenerator.GenerateContext();

            // SomeTableMap xm = new SomeTableMap();
        }

        public static void ParsePath()
        {
            // Another way: https://github.com/thinksquirrel/nanosvg-csharp
            string d = @"M3.726 91.397h9.349v-17.64H3.726zM18.19 79.578l4.41-4.056 4.41 4.056v3l-3.175-3.176v8.82h-2.293v-8.82l-3.352 3.175zM35.83 78.344h.175l.177-.177H36.71v-.176h.176l.177-.176h.176v-.177l.177-.176.176-.177v-.352h.176V76.05l-.176-.177v-.352h-.176v-.177h-.177v-.176l-.176-.177h-.177l-.176-.176h-.176v-.176h-1.412l-.176.176h-.176l-.177.176h-.176v.177h-.176v.176h-.177v.353h-.176V77.109l.176.176v.177h.177v.176l.176.177h.176v.176h.177l.176.176h.353l.176.177h.177z";

            // var cp1 = new Svg.CoordinateParser(d);
            var segmentList = Svg.SvgPathBuilder.Parse(d);
            System.Console.WriteLine(segmentList);

            foreach (Svg.Pathing.SvgPathSegment seg in segmentList)
            {
                System.Console.WriteLine(seg.Start);
                System.Console.WriteLine(seg.End);
            }


        } // End Sub Main 


    } // End Class Program 


} // End Namespace SchemaPorter 
