
namespace SchemaPorter.YamlSpace
{


    public class DeserializeJSON
    {


        public static void Test()
        {
            // SchemaPorter.YamlSpace.RootNode retVal = null;
            // SchemaPorter.foofoo.RootNode retVal = null;
            // SchemaPorter.VerySimpleYamlSchema.RootObject retVal = null;
            SchemaPorter.SimpleYamlSchema.RootObject retVal = null;

            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();

            using (System.IO.FileStream fs = System.IO.File.OpenRead(@"d:\username\Documents\Visual Studio 2017\Projects\BlueMine\BlueMine\Code\EN.json"))
            {
                // using (System.IO.TextReader sr = new System.IO.StringReader(@""))
                using (System.IO.TextReader sr = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    using (Newtonsoft.Json.JsonReader jr = new Newtonsoft.Json.JsonTextReader(sr))
                    {
                        // retVal = ser.Deserialize<SchemaPorter.YamlSpace.RootNode>(jr);
                        // retVal = ser.Deserialize<SchemaPorter.foofoo.RootNode>(jr);
                        // retVal = ser.Deserialize<SchemaPorter.VerySimpleYamlSchema.RootObject>(jr);
                        retVal = ser.Deserialize<SchemaPorter.SimpleYamlSchema.RootObject>(jr);
                    } // End Using jr 

                } // End Using sr 

            } // End Using fs 

            System.Collections.Generic.Dictionary<string, string> dict = 
                new System.Collections.Generic.Dictionary<string, string>();

            System.Type tt = typeof(SchemaPorter.SimpleYamlSchema.DE);

            System.Reflection.PropertyInfo[] pis = tt.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            using (System.Data.Common.DbConnection con = SchemaPorter.SqlFactory.GetConnection())
            {
                // System.Console.WriteLine(con.ConnectionString);

                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                foreach (System.Reflection.PropertyInfo pi in pis)
                {
                    if (!object.ReferenceEquals(pi.PropertyType, typeof(string)))
                        continue;

                    string pname = pi.Name;
                    string value = (string)pi.GetValue(retVal.en, null);

                    System.Console.WriteLine($"Name: {pname}, Value: {value}");

                    using (System.Data.Common.DbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO T_Translations(TR_Key, TR_Value) VALUES(@p1, @p2); ";

                        System.Data.Common.DbParameter p1 = cmd.CreateParameter();
                        p1.ParameterName = "@p1";
                        p1.Value = (pname == null ? (object)System.DBNull.Value : (object)pname);
                        p1.DbType = System.Data.DbType.String;
                        cmd.Parameters.Add(p1);

                        System.Data.Common.DbParameter p2 = cmd.CreateParameter();
                        p2.ParameterName = "@p2";
                        p2.Value = (value == null ? (object)System.DBNull.Value : (object)value);
                        p2.DbType = System.Data.DbType.String;
                        cmd.Parameters.Add(p2);

                        cmd.ExecuteNonQuery();
                    } // End Using cmd 

                } // Next pi 

                if (con.State != System.Data.ConnectionState.Closed)
                    con.Close();
            } // End Using con 

            System.Console.WriteLine(retVal);
        } // End Sub Test 


    } // End Class DeserializeJSON 


} // End Namespace SchemaPorter.YamlSpace 
