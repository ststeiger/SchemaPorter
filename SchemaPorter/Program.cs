
// https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/

namespace SchemaPorter
{
    
    
    public static class Program
    {
        
        
        static void EnumRegistry()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            foreach (string subkeyName in key.GetSubKeyNames())
            {
                System.Console.WriteLine(subkeyName);

                Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subkeyName);
                if (subkey != null)
                {
                    foreach (var value in subkey.GetValueNames())
                    {
                        System.Console.WriteLine("\tValue:" + value);

                        // Check for the publisher to ensure it's our product
                        string keyValue = System.Convert.ToString(subkey.GetValue("Publisher"));
                        if (!keyValue.Equals("MyPublisherCompanyName", System.StringComparison.OrdinalIgnoreCase))
                            continue;

                        string productName = System.Convert.ToString(subkey.GetValue("DisplayName"));
                        if (!productName.Equals("MyProductName", System.StringComparison.OrdinalIgnoreCase))
                            return;

                        string uninstallPath = System.Convert.ToString(subkey.GetValue("InstallSource"));

                        // Do something with this valuable information
                    }
                }
            }

            System.Console.ReadLine();
        }
        
        
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
