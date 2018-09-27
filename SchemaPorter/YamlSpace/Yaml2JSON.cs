
namespace SchemaPorter.YamlSpace
{

    
    public class TestConnectionString
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
    }

    
    public class Config
    {
        public System.Collections.Generic.List<TestConnectionString> ConnectionStrings { get; set; }
    }

    // https://stackoverflow.com/questions/40762139/how-to-serialize-and-deserialize-a-type-class-object-in-c-sharp-into-yaml
    // http://aaubry.net/pages/yamldotnet.html 
    // https://dotnetfiddle.net/rrR2Bb 
    public class Yaml2JSON
    {

        public static string TestSerialize()
        {
            Config cfg = new Config();
            cfg.ConnectionStrings= new System.Collections.Generic.List<TestConnectionString>();

            cfg.ConnectionStrings.Add(new TestConnectionString() { Name = "Test1", ConnectionString = "SSPI", ProviderName = "SqlClient" });
            cfg.ConnectionStrings.Add(new TestConnectionString() { Name = "Test2", ConnectionString = "Un;PW;", ProviderName = "SqlClient" });

            return TestSerialize(cfg);
        }

        public static string TestSerialize(object value)
        {
            System.Console.WriteLine(typeof(YamlDotNet.Serialization.SerializerBuilder).AssemblyQualifiedName);

            YamlDotNet.Serialization.ISerializer serializer = new YamlDotNet.Serialization.SerializerBuilder()
                .Build();

            // Note: Must be properties - doesn't work with public fields...
            string yaml = serializer.Serialize(value);
            System.Console.WriteLine(yaml);


            var writer = new System.IO.StringWriter();
            var obj = new TestConnectionString() { Name = "Test1", ConnectionString = "SSPI", ProviderName = "SqlClient" };

            new YamlDotNet.Serialization.SerializerBuilder()
                .EmitDefaults()
                .JsonCompatible()
                .Build()
                .Serialize(writer, obj, typeof(TestConnectionString)
            );

            string yamlTyped = writer.ToString();
            System.Console.WriteLine(yamlTyped);


            return yaml;
        }


        // convert string/file to YAML object
        public static void Test()
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(@"d:\username\Documents\Visual Studio 2017\Projects\BlueMine\BlueMine\Code\en.yml"))
            {
                //using (System.IO.TextReader r = new System.IO.StringReader(@""))
                using (System.IO.TextReader r = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {

                    YamlDotNet.Serialization.Deserializer deserializer =
                        new YamlDotNet.Serialization.Deserializer();

                    object yamlObject = deserializer.Deserialize(r);

                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                    // serializer.Serialize(Console.Out, yamlObject);

                    using (System.IO.Stream ws = System.IO.File.OpenWrite(@"d:\username\Documents\Visual Studio 2017\Projects\BlueMine\BlueMine\Code\en.json"))
                    {
                        using (System.IO.TextWriter sw = new System.IO.StreamWriter(ws, System.Text.Encoding.UTF8))
                        {
                            serializer.Serialize(sw, yamlObject);
                        } // End using sw 

                    } // End Using ws

                } // End Using r 

            } // End Using fr 

        } // End Sub Test 


    } // End Class Yaml2JSON 


} // End Namespace 
