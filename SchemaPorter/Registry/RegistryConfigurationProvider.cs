
namespace Microsoft.Extensions.Configuration.Registry 
{ 


    public class RegistryConfigurationProvider 
        : Microsoft.Extensions.Configuration.ConfigurationProvider 
    { 
        
        
        protected RegistryConfigurationSource m_source; 
        
        
        public RegistryConfigurationProvider()
            :base()
        { } // End Constructor 
        
        
        public RegistryConfigurationProvider(RegistryConfigurationSource source)
            :this() 
        { 
            this.m_source = source; 
        } // End Constructor 
        
        
        /// <summary>Loads (or reloads) the data for this provider.</summary>
        public override void Load()
        {
            // "[classes:nfs::server:exports:0, /srv/share1]"
            // "[classes:nfs::server:exports:1, /srv/share3]"
            // "[environment, production]"

            System.Console.WriteLine(this.m_source);

            //if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            if(true)
            {
                string path = "/etc/COR/All";
                path = @"C:\Users\Administrator\Documents\Visual Studio 2017\Projects\aaaRegExplore\Comparers";
                this.Data = Microsoft.Extensions.Configuration.Registry.EtcAppliance.ReadEtc(path);
            } // End if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            else
            {
                this.Data = Microsoft.Extensions.Configuration.Registry.RegAppliance.RecursivelyListKeys(@"HKEY_CURRENT_USER\Software\COR\RedmineMailService");
            }

            // How to escape colon ? 
            //  ¯\_(ツ)_/¯

            // this.Set("key", "value");
            // this.TryGet("key", out value);
        } // End Sub Load 


    } // End Class RegistryConfigurationProvider 
    
    
} // End Namespace Microsoft.Extensions.Configuration.Registry 
