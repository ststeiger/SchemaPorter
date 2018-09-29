
using System.IO;
using Microsoft.Win32;

namespace Microsoft.Extensions.Configuration.Yaml
{


    public class RegAppliance()
    {

        protected System.Collections.Generic.IDictionary<string, string>  m_data = 
            new System.Collections.Generic.SortedDictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);


        public System.Collections.Generic.IDictionary<string, string> GetData()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                // "/etc/COR/All"
                var di = new System.IO.DirectoryInfo("path");
                di.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
                di.GetFiles("*.*", SearchOption.TopDirectoryOnly);
                System.IO.Directory.GetFiles("path", "*.*", SearchOption.TopDirectoryOnly);
            }

            Microsoft.Win32.Registry.CurrentConfig;
            
            // Microsoft.Win32.RegistryValueKind GetValueKind
            Microsoft.Win32.RegistryKey rk = null;
            rk.GetValueKind("name");
            rk.SetValue("name", "value", RegistryValueKind.String);
            
            
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
    
    
    public class YamlConfigurationProvider : Microsoft.Extensions.Configuration.FileConfigurationProvider
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
