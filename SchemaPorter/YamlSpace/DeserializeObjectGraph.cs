
namespace SchemaPorter.YamlSpace
{


    public class Order
    {
        public string Receipt { get; set; }

        // [YamlAlias("bill-to")]
        [YamlDotNet.Serialization.YamlMember(Alias = "bill-to")]
        public string BillTo { get; set; }

    } // End Class Order 


    public class DeserializeObjectGraph
    {


        public static void Test()
        {
            // SchemaPorter.YamlSpace.RootNode retVal = null;
            // SchemaPorter.foofoo.RootNode retVal = null;
            SchemaPorter.SimpleYamlSchema.RootObject retVal = null;

            YamlDotNet.Serialization.IDeserializer deserializer = 
                new YamlDotNet.Serialization.DeserializerBuilder()
                .WithNamingConvention(new YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention())
                //.IgnoreUnmatchedProperties()
                .Build();
            

            using (System.IO.Stream fs = System.IO.File.OpenRead(@"d:\username\Documents\Visual Studio 2017\Projects\BlueMine\BlueMine\Code\de.yml"))
            {
                //using (System.IO.TextReader r = new System.IO.StringReader(@""))
                using (System.IO.TextReader r = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    // retVal = deserializer.Deserialize<SchemaPorter.YamlSpace.RootNode>(r);
                    // retVal = deserializer.Deserialize<SchemaPorter.foofoo.RootNode>(r);
                    retVal = deserializer.Deserialize<SchemaPorter.SimpleYamlSchema.RootObject>(r);
                } // End using r 

            } // End Using fs 

            System.Console.WriteLine(retVal);
        } // End Sub Test 


    } // End Class DeserializeObjectGraph 


} // End Namespace SchemaPorter.YamlSpace 
