
using System.Configuration;
using System.Collections.Specialized;


using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.SqlEnum;


// nuget: Microsoft.SqlServer.SqlManagementObjects
// https://docs.microsoft.com/en-us/sql/relational-databases/server-management-objects-smo/smo-license-terms
// https://www.mssqltips.com/sqlservertip/1826/getting-started-with-sql-server-management-objects-smo/
// https://stackoverflow.com/questions/11656324/how-can-i-generate-create-table-script-from-code

namespace SchemaPorter
{


    class TestSMO
    {
        
        
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

                    Server server = new Server(sc);
                    Database database; // = new Database();
                    //database = server.Databases["redmine"];
                    //string schemaName = @"dbo";
                    //string tableName = @"issues";

                    database = server.Databases["COR_Basic_Demo_V4"];
                    string schemaName = @"dbo";
                    string tableName = @"T_Benutzer";


                    Table table = database.Tables[tableName, schemaName];
                    StringCollection result = table.Script();

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
            Server srv = new Server();

            // really you would get these from config or elsewhere:
            srv.ConnectionContext.Login = "foo";
            srv.ConnectionContext.Password = "bar";

            srv.ConnectionContext.ServerInstance = "ServerName";
            string dbName = "DatabaseName";

            Database db = new Database();
            db = srv.Databases[dbName];

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (Table tbl in db.Tables)
            {
                ScriptingOptions options = new ScriptingOptions();
                options.ClusteredIndexes = true;
                options.Default = true;
                options.DriAll = true;
                options.Indexes = true;
                options.IncludeHeaders = true;

                StringCollection coll = tbl.Script(options);
                foreach (string str in coll)
                {
                    sb.Append(str);
                    sb.Append(System.Environment.NewLine);
                }

            }

            System.IO.StreamWriter fs = System.IO.File.CreateText("c:\\temp\\output.txt");
            fs.Write(sb.ToString());
            fs.Close();
        }

    }


}