
using Microsoft.Extensions.Configuration;


namespace SchemaPorter.Settings
{


    class SettingsHelper
    {


        public static void Test()
        {
            Settings_t env = new Settings_t();

            Microsoft.Extensions.Configuration.IConfigurationBuilder builder = 
                new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(env.ProjectRootPath)
                .AddRegistryKey("foo")
                .AddYamlFile("settings.yml", optional: false) // Microsoft.Extensions.Configuration.Yaml.dll
                                                              // .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Microsoft.Extensions.Configuration.Json.dll
                                                              // .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                                                              // .AddEnvironmentVariables() // Microsoft.Extensions.Configuration.EnvironmentVariables.dll
            ;

            Microsoft.Extensions.Configuration.IConfigurationRoot Configuration = builder.Build();
            Newtonsoft.Json.Linq.JObject conf = GetConfigAsJObject(Configuration);
            string configValues = Newtonsoft.Json.JsonConvert.SerializeObject(conf, Newtonsoft.Json.Formatting.Indented);
            System.Console.WriteLine(configValues);
        }


        public static void ReadOldSettings()
        {
            // Add reference to System.Configuration.ConfigurationManager
            System.Configuration.ConnectionStringSettings a = System.Configuration.ConfigurationManager.ConnectionStrings["COR-W10-112"];
            System.Console.WriteLine(a);
        }


        // https://andrewlock.net/converting-web-config-files-to-appsettings-json-with-a-net-core-global-tool/
        static Newtonsoft.Json.Linq.JObject GetConfigAsJObject(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            Newtonsoft.Json.Linq.JObject root = new Newtonsoft.Json.Linq.JObject();

            foreach (Microsoft.Extensions.Configuration.IConfigurationSection child in config.GetChildren())
            {
                //not strictly correct, but we'll go with it.
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
            }

            return root;
        }


    }
}
