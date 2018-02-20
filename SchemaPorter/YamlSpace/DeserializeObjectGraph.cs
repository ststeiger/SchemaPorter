
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace SchemaPorter.YamlSpace
{


    public class Order
    {
        public string Receipt { get; set; }

        // [YamlAlias("bill-to")]
        [YamlDotNet.Serialization.YamlMember(Alias = "bill-to")]
        public string BillTo { get; set; }

    }


    public class DeserializeObjectGraph
    {


        public static void Test()
        {
            // SchemaPorter.YamlSpace.RootNode retVal = null;
            // SchemaPorter.foofoo.RootNode retVal = null;
            SchemaPorter.SimpleYamlSchema.RootObject retVal = null;

            // var r = new StringReader("Document");

            // Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                //.IgnoreUnmatchedProperties()
                .Build();
            

            using (System.IO.FileStream fs = System.IO.File.OpenRead(@"d:\username\Documents\Visual Studio 2017\Projects\BlueMine\BlueMine\Code\de.yml"))
            {
                //using (var r = new StringReader(@""))
                using (var r = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    // retVal = deserializer.Deserialize<SchemaPorter.YamlSpace.RootNode>(r);
                    // retVal = deserializer.Deserialize<SchemaPorter.foofoo.RootNode>(r);
                    retVal = deserializer.Deserialize<SchemaPorter.SimpleYamlSchema.RootObject>(r);
                }
            }


            System.Console.WriteLine(retVal);
        }

    }


}