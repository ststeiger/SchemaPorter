
using Dapper;
using System.Linq;

namespace SchemaPorter
{

    public class TableDeclaration
    {
        public string variable_name;
        public string table_declaration;
    }

    public class TableColumnDefinition
    {
        public string TABLE_SCHEMA;
        public string TABLE_NAME;
        public string COLUMN_NAME;
        public string QUOTED_COLUMN_NAME;
        public int ORDINAL_POSITION;
        public string IS_NULLABLE;
        public string IS_COMPUTED;
        public string DDL_NAME;
        public string ddl;
    }


    class TestDapper
    {

        public static string ObjectToInsertStringValue(object value)
        {
            if (value == null)
                return "NULL ";

            System.Type t = value.GetType();
            if (object.ReferenceEquals(t, typeof(System.DateTime)))
            {
                System.DateTime dt = (System.DateTime)value;
                return "'" + dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff", System.Globalization.CultureInfo.InvariantCulture) + "'";
            }

            return "'" + System.Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture).Replace("'", "''") + "'";
        }


        public static void Test()
        {
            string table_schema = "dbo";
            // string table_name = "T_VWS_Ref_Darstellung";
            string table_name = "T_VWS_Ref_PdfLegendenKategorie";
            

            using (System.Data.Common.DbConnection connection = SqlFactory.GetConnection())
            {

                string select_query = connection.QueryFirstEmbedded<string>("SELECT_Columns.sql", typeof(TestDapper), new
                {
                    in_schema = table_schema,
                    in_table = table_name,
                });




                TableDeclaration td = connection.QueryFirstEmbedded<TableDeclaration>("Upsert_Table.sql", typeof(TestDapper), new
                {
                    in_schema = table_schema,
                    in_table = table_name,
                });


                if (td == null)
                    return;



                System.Collections.Generic.List<System.Collections.Generic.IDictionary<string, object>> data_to_insert =
                    connection.Query(select_query).Select(x => (System.Collections.Generic.IDictionary<string, object>)x).ToList();





                System.Console.WriteLine(data_to_insert);


                System.Collections.Generic.IEnumerable<TableColumnDefinition> lsColumns = connection.QueryEmbedded<TableColumnDefinition>("Upsert_Columns.sql", typeof(TestDapper), new
                {
                    in_schema = table_schema,
                    in_table = table_name,
                });

                if (lsColumns == null || !System.Linq.Enumerable.Any(lsColumns))
                    return;



                string sql = @"
SET NOCOUNT ON;

" + td.table_declaration + @"

INSERT INTO  @" + td.variable_name + @" 
( 
";

                foreach (TableColumnDefinition cd in lsColumns)
                {
                    sql += "    ";

                    if (cd.ORDINAL_POSITION == 1)
                        sql += " ";
                    else
                        sql += ",";

                    sql += cd.QUOTED_COLUMN_NAME;
                    sql += System.Environment.NewLine;
                }




                sql += @") 
";




                for (int i = 0; i < data_to_insert.Count; ++i)
                {
                    if (i != 0)
                        sql += "UNION ALL ";

                    sql += "SELECT ";


                    foreach (TableColumnDefinition cd in lsColumns)
                    {
                        sql += "    ";


                        if (cd.ORDINAL_POSITION == 1)
                            sql += " ";
                        else
                            sql += ",";

                        object value = data_to_insert[i][cd.COLUMN_NAME];
                        sql += ObjectToInsertStringValue(value);

                        sql += System.Environment.NewLine;
                    }



                }


                sql += @"; 
-- SELECT * FROM @" + td.variable_name + @" 


MERGE INTO " + table_name + @" WITH (HOLDLOCK) AS A 
USING @" + td.variable_name + @" AS CTE ON CTE." + lsColumns.AsList()[0].QUOTED_COLUMN_NAME + " = A." + lsColumns.AsList()[0].QUOTED_COLUMN_NAME + @" 
WHEN MATCHED 
	THEN UPDATE 
";

                foreach (TableColumnDefinition cd in lsColumns)
                {
                    if (cd.ORDINAL_POSITION == 1)
                        continue;

                    sql += "    ";

                    if (cd.ORDINAL_POSITION == 2)
                        sql += "SET  ";
                    else
                        sql += "	,";


                    sql += "A." + cd.QUOTED_COLUMN_NAME + " = CTE." + cd.QUOTED_COLUMN_NAME + " ";
                    sql += System.Environment.NewLine;
                }


                sql += @"

WHEN NOT MATCHED THEN 
INSERT 
( 
";
                foreach (TableColumnDefinition cd in lsColumns)
                {
                    sql += "    ";

                    if (cd.ORDINAL_POSITION == 1)
                        sql += " ";
                    else
                        sql += ",";

                    sql += cd.QUOTED_COLUMN_NAME;
                    sql += System.Environment.NewLine;
                }

                sql += @"
) 
VALUES 
( 
";
                foreach (TableColumnDefinition cd in lsColumns)
                {
                    sql += "    ";

                    if (cd.ORDINAL_POSITION == 1)
                        sql += " CTE.";
                    else
                        sql += ",CTE.";

                    sql += cd.QUOTED_COLUMN_NAME;
                    sql += System.Environment.NewLine;
                }

                sql += @"
); 


-- DELETE FROM " + table_name + @" WHERE xxx = '123'; 
-- SELECT * FROM " + table_name + @"; 


SET NOCOUNT OFF; 


";





                System.Console.WriteLine(sql);

            }
        }


    }


}
