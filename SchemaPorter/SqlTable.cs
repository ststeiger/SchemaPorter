
namespace SchemaPorter
{


    class SqlTable
    {

        public string Schema;
        public string Table;

        public System.Collections.Generic.Dictionary<string, SqlColumn> Columns;


        public string CreateTableScript()
        {
            return null;
        }



        public static void CreateAll()
        {

            System.Collections.Generic.List<SqlTable> tables = 
                new System.Collections.Generic.List<SqlTable>();

                // Pass assembly name via argument
                System.Reflection.Assembly a = System.Reflection.Assembly.LoadFile("args[0]");

                System.Type[] types = a.GetTypes();

                // Get Types in the assembly.
                foreach (System.Type t in types)
                {
                SqlTable tc = (SqlTable)System.Activator.CreateInstance(t);
                    tables.Add(tc);
                }

                // Create SQL for each table
                foreach (SqlTable table in tables)
                {
                    System.Console.WriteLine(table.CreateTableScript());
                    System.Console.WriteLine();
                }

                // Total Hacked way to find FK relationships! Too lazy to fix right now
                foreach (SqlTable table in tables)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, SqlColumn> field 
                    in table.Columns)
                    {
                        foreach (SqlTable t2 in tables)
                        {
                            // if (field.Value.Name == t2.ClassName)
                            {
                                // We have a FK Relationship!
                                System.Console.WriteLine("GO");
                                System.Console.WriteLine("ALTER TABLE " + table.Table + " WITH NOCHECK");
                                System.Console.WriteLine("ADD CONSTRAINT FK_" + field.Key + " FOREIGN KEY (" + field.Key + ") REFERENCES " + t2.ClassName + "(ID)");
                                System.Console.WriteLine("GO");

                            }
                        }
                    }
                }
            }
        

    }




}
