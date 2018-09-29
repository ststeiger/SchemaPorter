
using Microsoft.Extensions.Configuration;

// https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/

namespace SchemaPorter
{
    
    
    public static class Program
    {
        
        
        public static void Main(string[] args)
        {
            SchemaPorter.Settings.SettingsHelper.Test();
            string yaml = YamlSpace.Yaml2JSON.TestSerialize();
            System.Console.WriteLine(yaml);
            
            YamlSpace.Yaml2JSON.Test();
            // YamlSpace.DeserializeJSON.Test();
            
            // YamlSpace.DeserializeObjectGraph.Test();
            System.Console.WriteLine("hello");
            
            // TestSMO.Test();
            
            
            // SchemaGenerator.GenerateSchema();
            // ContextGenerator.GenerateContext();
            
            // SomeTableMap xm = new SomeTableMap();
            
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 
        
        
    } // End Class Program 
    
    
} // End Namespace SchemaPorter 
