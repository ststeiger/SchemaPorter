
// using System.Configuration;
// using System.Collections.Specialized;


using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.SqlEnum;


// nuget: Microsoft.SqlServer.SqlManagementObjects
// https://stackoverflow.com/questions/11656324/how-can-i-generate-create-table-script-from-code

// https://docs.microsoft.com/en-us/sql/relational-databases/server-management-objects-smo/smo-license-terms
// https://www.mssqltips.com/sqlservertip/1826/getting-started-with-sql-server-management-objects-smo/
// https://www.mssqltips.com/sqlservertip/1833/generate-scripts-for-database-objects-with-smo-for-sql-server/
// https://support.microsoft.com/en-us/help/914288/how-to-generate-a-script-of-the-necessary-database-metadata-to-create

// https://stackoverflow.com/questions/2750278/sql-server-2008-create-database-script-schema-data-with-command-line
// https://blogs.technet.microsoft.com/dataplatforminsider/2017/05/17/try-new-sql-server-command-line-tools-to-generate-t-sql-scripts-and-monitor-dynamic-management-views/

namespace SchemaPorter
{


    public class TestSMO
    {

        public class foo
        {
            public foo(params int[] u)
            { }

            System.Collections.Generic.Dictionary<string, int> dict = 
                new System.Collections.Generic.Dictionary<string, int>(
                    System.StringComparer.OrdinalIgnoreCase
                    );


            public string this[string a]
            {
                get { return ""; }
                set { }
            }

        }


        public static void Test()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            using (System.Data.SqlClient.SqlConnection con = (System.Data.SqlClient.SqlConnection)
                //SqlFactory.GetConnection()
                SqlFactory.LocalConntection
            )
            {
                Microsoft.SqlServer.Management.Common.ServerConnection sc =
                    new Microsoft.SqlServer.Management.Common.ServerConnection(con);

                lock (con)
                {
                    sc.Connect();

                    Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(sc);
                    Microsoft.SqlServer.Management.Smo.Database database; // = new Microsoft.SqlServer.Management.Smo.Database();
                    //database = server.Databases["redmine"];
                    //string schemaName = @"dbo";
                    //string tableName = @"issues";

                    database = server.Databases["COR_Basic_Demo_V4"];
                    string schemaName = @"dbo";
                    string tableName = @"T_Benutzer";


                    Microsoft.SqlServer.Management.Smo.Table table = database.Tables[tableName, schemaName];
                    System.Collections.Specialized.StringCollection result = table.Script();


                    // table.Script(new ScriptingOptions() { ScriptForAlter = true });

                    Microsoft.SqlServer.Management.Smo.StoredProcedure proc = database.StoredProcedures["procname", "schema"];
                    // proc.Script(new ScriptingOptions() { ScriptForAlter = true });
                    // string alterText = proc.ScriptHeader(true) + proc.TextBody;

                    foreach (string line in result)
                    {
                        sb.AppendLine(line);
                    } // Next line 

                    sc.Disconnect();
                } // End Lock con 

            } // End Using con 

            using (System.IO.FileStream fs = System.IO.File.OpenWrite(@"d:\testScriptSAQl.sql"))
            {
                using (System.IO.TextWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.Write(sb.ToString());
                    sw.Flush();
                    fs.Flush();
                } // End Using sw 

            } // End Using fs 


        } // End Sub 


        public static void SlightlyMoreComplete()
        {
            Microsoft.SqlServer.Management.Smo.Server srv = new Microsoft.SqlServer.Management.Smo.Server();

            // really you would get these from config or elsewhere:
            srv.ConnectionContext.Login = "foo";
            srv.ConnectionContext.Password = "bar";

            srv.ConnectionContext.ServerInstance = "ServerName";
            string dbName = "DatabaseName";

            Microsoft.SqlServer.Management.Smo.Database db = new Microsoft.SqlServer.Management.Smo.Database();
            db = srv.Databases[dbName];

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (Microsoft.SqlServer.Management.Smo.Table tbl in db.Tables)
            {
                Microsoft.SqlServer.Management.Smo.ScriptingOptions options = new Microsoft.SqlServer.Management.Smo.ScriptingOptions();
                options.ClusteredIndexes = true;
                options.Default = true;
                options.DriAll = true;
                options.Indexes = true;
                options.IncludeHeaders = true;

                System.Collections.Specialized.StringCollection coll = tbl.Script(options);
                foreach (string str in coll)
                {
                    sb.Append(str);
                    sb.Append(System.Environment.NewLine);
                } // Next str 

            } // Next tbl 

            System.IO.StreamWriter fs = System.IO.File.CreateText("c:\\temp\\output.txt");
            fs.Write(sb.ToString());
            fs.Close();
        } // End Sub SlightlyMoreComplete 


    } //End Class TestSMO 


} // End Namespace SchemaPorter
