
using Microsoft.Extensions.Configuration;


namespace SchemaPorter.Settings
{


    class SettingsHelper
    {

        public static void TestYaml()
        {
            string yaml = YamlSpace.Yaml2JSON.TestSerialize();
            System.Console.WriteLine(yaml);

            YamlSpace.Yaml2JSON.Test();
            // YamlSpace.DeserializeJSON.Test();

            // YamlSpace.DeserializeObjectGraph.Test();
            System.Console.WriteLine("Finished.");
        }


        public static void Test()
        {
            Settings_t env = new Settings_t();

            Microsoft.Extensions.Configuration.IConfigurationBuilder builder = 
                new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(env.ProjectRootPath)
                // .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Microsoft.Extensions.Configuration.Json.dll
                // .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                // .AddJsonFile("settings.json")
                // .AddYamlFile("settings.yml", optional: false) // Microsoft.Extensions.Configuration.Yaml.dll
                .AddRegistryKey(@"HKEY_CURRENT_USER\Software\COR\All", "/etc/COR/All")
                .AddRegistryKey(@"HKEY_CURRENT_USER\Software\COR\SchemaPorter", "/etc/COR/SchemaPorter")
                // .AddEnvironmentVariables() // Microsoft.Extensions.Configuration.EnvironmentVariables.dll
            ;

            Microsoft.Extensions.Configuration.IConfigurationRoot Configuration = builder.Build();
            
            // IConfigurationSection thisSection = Configuration.GetSection("AppSettings:token");
            // thisSection.Key
            // thisSection.Value;
            // services.Configure<StorageOptions>(Configuration.GetSection("AzureStorageConfig"));
            
            // Just the value of the token
            // string configValue = Configuration["AppSettings:token"];
            
            
            // https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-config-json-in-asp-net-core
            Newtonsoft.Json.Linq.JObject conf = GetConfigAsJObject(Configuration);
            string configValues = Newtonsoft.Json.JsonConvert.SerializeObject(conf, Newtonsoft.Json.Formatting.Indented);
            System.Console.WriteLine(configValues);
        } // End Sub Test 


        public static void ReadOldSettings()
        {
            // Add reference to System.Configuration.ConfigurationManager
            System.Configuration.ConnectionStringSettings a = System.Configuration.ConfigurationManager.ConnectionStrings["COR-W10-112"];
            System.Console.WriteLine(a);
        } // End Sub ReadOldSettings 


        // https://andrewlock.net/converting-web-config-files-to-appsettings-json-with-a-net-core-global-tool/
        static Newtonsoft.Json.Linq.JObject GetConfigAsJObject(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            Newtonsoft.Json.Linq.JObject root = new Newtonsoft.Json.Linq.JObject();

            foreach (Microsoft.Extensions.Configuration.IConfigurationSection child in config.GetChildren())
            {
                // Not strictly correct, but we'll go with it.
                bool isSection = (child.Value == null);
                if (isSection)
                {
                    // call the function again, passing in the section-children only
                    root.Add(child.Key, GetConfigAsJObject(child));
                }
                else
                {
                    root.Add(child.Key, child.Value);
                }

            } // Next child 

            return root;
        } // End Function GetConfigAsJObject 


    } // End Class SettingsHelper 


} // End Namespace SchemaPorter.Settings 
