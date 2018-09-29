
namespace Microsoft.Extensions.Configuration.Yaml
{


    public class RegAppliance 
    {


        protected System.Collections.Generic.IDictionary<string, string>  m_data = 
            new System.Collections.Generic.SortedDictionary<string, string>(
                System.StringComparer.OrdinalIgnoreCase
        );


        public static void SplitKey(string key, out string hive, out string branch)
        {
            int index = key.IndexOf('\\');
            hive = string.Empty;
            branch = string.Empty;

            if (index == -1)
                hive = key;
            else
            {
                hive = key.Substring(0, index);
                branch = key.Substring(index + 1);
            }
        }


        public static Microsoft.Win32.RegistryKey ParseRootKey(string path)
        {
            Microsoft.Win32.RegistryKey key;
            
            switch (path)
            {
                case "HKCR":
                case "HKEY_CLASSES_ROOT":
                    key = Microsoft.Win32.Registry.ClassesRoot;
                    break;
                case "HKCU":
                case "HKEY_CURRENT_USER":
                    key = Microsoft.Win32.Registry.CurrentUser;
                    break;
                case "HKLM":
                case "HKEY_LOCAL_MACHINE":
                    key = Microsoft.Win32.Registry.LocalMachine;
                    break;
                case "HKU":
                case "HKEY_USERS":
                    key = Microsoft.Win32.Registry.Users;
                    break;
                case "HKPD":
                case "HKEY_PERFORMANCE_DATA":
                    key = Microsoft.Win32.Registry.PerformanceData;
                    break;
                default:
                    key = Microsoft.Win32.Registry.CurrentConfig;
                    break;
            }
            return key;
        }
        

        public System.Collections.Generic.IDictionary<string, string> GetData()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                // "/etc/COR/All"
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("path");
                di.GetDirectories("*.*", System.IO.SearchOption.TopDirectoryOnly);
                di.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);
                System.IO.Directory.GetFiles("path", "*.*", System.IO.SearchOption.TopDirectoryOnly);
            }



            return this.m_data;
        }

    }



    public class RegistryConfiguratonSource
        :Microsoft.Extensions.Configuration.IConfigurationSource
    {

        public string Key;
        

        public RegistryConfiguratonSource()
        { }


        public RegistryConfiguratonSource(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Builds the <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" /> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.Configuration.IConfigurationBuilder" />.</param>
        /// <returns>An <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" /></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RegistryConfigurationProvider(this);
        }
        
    }
    
    
    public class RegistryConfigurationProvider
        : Microsoft.Extensions.Configuration.ConfigurationProvider
    {

        protected RegistryConfiguratonSource m_source;
        
        
        public RegistryConfigurationProvider()
            :base()
        { } // End Constructor 
        
        
        public RegistryConfigurationProvider(RegistryConfiguratonSource source)
            :this()
        {
            this.m_source = source;
        }
        

        /// <summary>Loads (or reloads) the data for this provider.</summary>
        public override void Load()
        {
            /*
"[classes:nfs::server:exports:0, /srv/share1]"
"[classes:nfs::server:exports:1, /srv/share3]"
"[environment, production]"
            */
            
            // How to escape colon ? 
            //  ¯\_(ツ)_/¯
            
            this.Set("key", "value");
            string value;
            this.TryGet("key", out value);
        }
        
        
    }
    
    
    public class YamlConfigurationProvider 
        : Microsoft.Extensions.Configuration.FileConfigurationProvider
    {
        
        
        public YamlConfigurationProvider(YamlConfigurationSource source) 
            : base(source)
        { }


        public override void Load(System.IO.Stream stream)
        {
            YamlConfigurationFileParser parser = new YamlConfigurationFileParser();

            Data = parser.Parse(stream);
            System.Console.WriteLine(this.Data);
        }


    }


}
