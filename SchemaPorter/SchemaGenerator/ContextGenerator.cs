
using Dapper;
using System.Linq;
using System.Collections.Generic;


namespace SchemaPorter
{

    public class TableDefinitions
    {
        public string TABLE_CATALOG;
        public string TABLE_SCHEMA;
        public string TABLE_NAME;
        public string TABLE_TYPE;
    }

    public class IndexDefinition
    {
        public string TABLE_SCHEMA;
        public string TABLE_NAME;
        public string INDEX_NAME;
        public string KEY_COLUMNS;
        public bool IS_MULTICOLUMN_INDEX;
        public string INCLUDED_COLUMNS;
        public int INDEX_ORDINAL;
    }


    class ContextGenerator
    {

        public static void GenerateContext()
        {
            GenerateContext("BlueMine.Db");
        }


        public static void GenerateContext(string ns)
        {
            string table_prefix = "T_";

            string cs = @"
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace "+ ns + @"
{
    
    
    public partial class RedmineContext : DbContext
    {
        
";


            List<TableDefinitions> allTables = null;

            // LOOP Tables
            using (System.Data.Common.DbConnection connection = SqlFactory.GetConnection())
            {
                System.Console.WriteLine(connection.ConnectionString);

                allTables = connection.Query<TableDefinitions>("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_SCHEMA, TABLE_NAME ")
                    .ToList();
            }

            for (int i = 0; i < allTables.Count; ++i)
            {
                cs += @"        public virtual DbSet<" + table_prefix + allTables[i].TABLE_NAME + @"> "+ allTables[i].TABLE_NAME + @" { get; set; } 
";
            }


            // MSG from entity framwork on error:
            // Unable to generate entity type for table 'dbo.groups_users'. Please see the warning messages.

            cs += @"


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    $@""Server ={ System.Environment.MachineName}\SQLEXPRESS; Database = Redmine; Integrated Security = true; "");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

";


            string sql = SchemaGenerator.GetScript("columns.sql");
            System.Collections.Generic.List<ColumnDefinition> allColumns = null;
            System.Collections.Generic.List<IndexDefinition> allIndices = null;
            System.Collections.Generic.List<PrimaryKeyDefinition> allPrimaryKeys = null;

            using (System.Data.Common.DbConnection connection = SqlFactory.GetConnection())
            {
                allColumns = connection.Query<ColumnDefinition>(sql).ToList();
            }

            sql = SchemaGenerator.GetScript("indices.sql");

            using (System.Data.Common.DbConnection connection = SqlFactory.GetConnection())
            {
                allIndices = connection.Query<IndexDefinition>(sql).ToList();
            }

            sql = SchemaGenerator.GetScript("primary_keys.sql");

            using (System.Data.Common.DbConnection connection = SqlFactory.GetConnection())
            {
                allPrimaryKeys = connection.Query<PrimaryKeyDefinition>(sql).ToList();
            }


            for (int i = 0; i < allTables.Count; ++i)
            {

                List<PrimaryKeyDefinition> primarykeyInfo = (
               from x in allPrimaryKeys
               where x.TABLE_NAME == allTables[i].TABLE_NAME
               orderby x.PRIMARY_KEY_NAME ascending
               select x
               ).ToList();

                List<ColumnDefinition> columnInfo = (
                    from x in allColumns
                    where x.TABLE_NAME == allTables[i].TABLE_NAME
                    orderby x.ORDINAL_POSITION ascending
                    select x
                    ).ToList();


                List<IndexDefinition> indexInfo = (
                    from x in allIndices
                    where x.TABLE_NAME == allTables[i].TABLE_NAME
                    orderby x.INDEX_ORDINAL ascending
                    select x
                    ).ToList();




                cs += @"            modelBuilder.Entity<" + table_prefix + allTables[i].TABLE_NAME + @">(entity =>
            {
                 ";

                
                for (int l = 0; l < primarykeyInfo.Count; ++l)
                {
                    string indcoldef = primarykeyInfo[l].PRIMARY_KEY;

                    if (primarykeyInfo[l].IS_MULTICOLUMN_KEY)
                        indcoldef = "new { " + primarykeyInfo[l].PRIMARY_KEY + " } ";

                    cs += @"
                    entity.HasKey(e => " + indcoldef + @")";

                    if (!string.IsNullOrEmpty(primarykeyInfo[l].PRIMARY_KEY_NAME))
                        cs += @"
                        .HasName(""" + primarykeyInfo[l].PRIMARY_KEY_NAME + @""")";

                    cs += @";
";

                }

                    for (int k = 0; k < indexInfo.Count; ++k)
                {
                    string indcoldef = indexInfo[k].KEY_COLUMNS;
                    
                    if (indexInfo[k].IS_MULTICOLUMN_INDEX)
                        indcoldef = "new { " + indexInfo[k].KEY_COLUMNS + " } ";

                    cs += @"
                    entity.HasIndex(e => " + indcoldef + @")";

                    if (!string.IsNullOrEmpty(indexInfo[k].INDEX_NAME))
                        cs += @"
                        .HasName(""" + indexInfo[k].INDEX_NAME + @""")";

                    cs += @";
";
                }

                cs += @";

";

                for (int j = 0; j < columnInfo.Count; ++j)
                {
                    // Indices
                    //entity.HasIndex(e => e.NewStatusId)
                    //.HasName("index_workflows_on_new_status_id");
                    //entity.HasIndex(e => new { e.RootId, e.Lft, e.Rgt })
                    //         .HasName("index_issues_on_root_id_and_lft_and_rgt");
                    // .IsUnique();



                    // Columns
                    cs += @"
                    entity.Property(e => e.";



                    if (Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsReservedKeyword(
                        Microsoft.CodeAnalysis.CSharp.SyntaxFacts.GetKeywordKind(columnInfo[j].COLUMN_NAME)
                        ))
                        cs += "@";

                    cs += columnInfo[j].COLUMN_NAME + @")";

                    if(columnInfo[j].IS_REQUIRED)
                    cs += @"
                        .IsRequired()";

                    cs += @"
                        .HasColumnName(""" + columnInfo[j].COLUMN_NAME + @""")";

                    if (columnInfo[j].COLUMN_TYPE != null)
                        cs += @"
                        .HasColumnType(""" + columnInfo[j].COLUMN_TYPE + @""")";


                    if (columnInfo[j].MAX_LENGTH.HasValue && columnInfo[j].MAX_LENGTH != -1)
                        cs += @"
                        .HasMaxLength(" + columnInfo[j].MAX_LENGTH.ToString() + @")";

                    if (!string.IsNullOrEmpty(columnInfo[j].COLUMN_DEFAULT))
                    cs+=@"
                        .HasDefaultValueSql(""" + columnInfo[j].COLUMN_DEFAULT + @""")";


                    cs += @";

";

                    //columnInfo[j].COLUMN_NAME
                    //columnInfo[j].COLUMN_DEFAULT
                    // columnInfo[j].DOTNET_TYPE
                    //columnInfo[j].ORDINAL_POSITION
                    // columnInfo[j].SQL_TYPE
                } // Next j 

                cs += @"
            }); 
";
            } // Next i 


            cs += @"

        } // End Sub OnModelCreating 


    } // End partial class RedmineContext : DbContext


} // End namespace "+ ns + @"
";

            System.Console.WriteLine(cs);
            System.IO.File.WriteAllText("SomeContext.cs", cs, System.Text.Encoding.UTF8);
        }

    }
}
