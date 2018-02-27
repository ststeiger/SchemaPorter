﻿
using System.Linq;
using Dapper;

namespace SchemaPorter 
{

    // https://github.com/zapadi/redmine-net-api/blob/master/xUnitTest-redmine-net45-api/Tests/Sync/ProjectMembershipTests.cs
    public class SchemaGenerator
    {


        public static string GetScript(string scriptName)
        {

            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();

            foreach (string resName in asm.GetManifestResourceNames())
            {
                
                if (resName.EndsWith(scriptName, System.StringComparison.OrdinalIgnoreCase))
                {
                    using (var strm = asm.GetManifestResourceStream(resName))
                    {
                        using (var sr = new System.IO.StreamReader(strm, System.Text.Encoding.UTF8))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }

            }

            return "";
        }


        // https://www.developerfusion.com/article/143896/understanding-the-basics-of-roslyn/
        public static void GenerateSchema()
        {
            string sql = GetScript("columns.sql");
            System.Collections.Generic.List<ColumnDefinition> allColumns = null;

            using (System.Data.Common.DbConnection connection = SqlFactory.GetConnection())
            {
                allColumns = connection.Query<ColumnDefinition>(sql).ToList();
            }

            //var ls = allColumns.GroupBy(p => new { p.TABLE_NAME })
            //    .Select(g => g.First().TABLE_NAME)
            //.ToList();

            System.Collections.Generic.List<string> ls = null;

            using (System.Data.Common.DbConnection connection = SqlFactory.GetConnection())
            {
                ls = connection.Query<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ").ToList();
            }


            foreach (string table in ls)
            {
                var tableColumns = allColumns
                    .Where(p => p.TABLE_NAME.Equals(table, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"
namespace BlueMine.Db 
{
    
    
    public partial class ");
                sb.Append("T_"); // filed-name may not be equal to class name, rename class
                sb.Append(table);
                sb.Append(@"
    {
");

foreach (ColumnDefinition cl in tableColumns)
{
    sb.Append(@"         public ");
    sb.Append(cl.DOTNET_TYPE);
    sb.Append(" ");

    // for keywords
    //if (!Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsValidIdentifier(cl.COLUMN_NAME))
    if (Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsReservedKeyword(
        Microsoft.CodeAnalysis.CSharp.SyntaxFacts.GetKeywordKind(cl.COLUMN_NAME)
        ))
        sb.Append("@");

    sb.Append(cl.COLUMN_NAME);
                    bool useProperties = true;
                    if(useProperties)
                        sb.Append(" { get; set; } // ");
                    else
                        sb.Append("; // ");
                   
        sb.AppendLine(cl.SQL_TYPE);
} // Next cl 

                sb.AppendLine(@"    }");
                sb.AppendLine(System.Environment.NewLine);
                sb.AppendLine(@"}");

                string fileContents = sb.ToString();

                string dir = System.IO.Path.Combine("Db", "Redmine", "BlueMappings", table + ".cs");
                string d = System.IO.Path.GetDirectoryName(dir);
                if (!System.IO.Directory.Exists(d))
                    System.IO.Directory.CreateDirectory(d);

                System.IO.File.WriteAllText(dir, fileContents, System.Text.Encoding.UTF8);
            }

        }


    }


}
