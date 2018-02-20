
namespace SchemaPorter.YamlSpace
{


    // http://aaubry.net/pages/yamldotnet.html 
    // https://dotnetfiddle.net/rrR2Bb 
    public class Yaml2JSON
    {

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
