using System.IO;
using System;
using YamlDotNet.Serialization;

namespace SchemaPorter.YamlSpace
{


    // http://aaubry.net/pages/yamldotnet.html
    // https://dotnetfiddle.net/rrR2Bb

    public class Yaml2JSON
    {
        public static void Test()
        {
            // convert string/file to YAML object

            using (System.IO.FileStream fs = System.IO.File.OpenRead(@"d:\username\Documents\Visual Studio 2017\Projects\BlueMine\BlueMine\Code\de.yml"))
            {
                //using (var r = new StringReader(@""))
                using (var r = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {

                    var deserializer = new Deserializer();
                    var yamlObject = deserializer.Deserialize(r);

                    var serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                    // serializer.Serialize(Console.Out, yamlObject);

                    using (System.IO.FileStream fs2 = System.IO.File.OpenWrite(@"d:\username\Documents\Visual Studio 2017\Projects\BlueMine\BlueMine\Code\de.json"))
                    {
                        using (var r2 = new System.IO.StreamWriter(fs2, System.Text.Encoding.UTF8))
                        {
                            serializer.Serialize(r2, yamlObject);
                        }
                    }


                }
            }

  


        }
    }


}