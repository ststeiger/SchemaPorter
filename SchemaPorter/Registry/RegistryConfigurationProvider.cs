
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

            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                if(!string.IsNullOrWhiteSpace( this.m_source.EtcDirectory))
                    this.Data = Microsoft.Extensions.Configuration.Registry.EtcAppliance.ReadEtc(this.m_source.EtcDirectory);
            } // End if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            else
            {
                if (!string.IsNullOrWhiteSpace(this.m_source.RegistryKey))
                    this.Data = Microsoft.Extensions.Configuration.Registry.RegAppliance.RecursivelyListKeys(this.m_source.RegistryKey);
            }

            // if (this.Data == null) this.Data = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

            // How to escape colon ? 
            //  ¯\_(ツ)_/¯

            // this.Set("key", "value");
            // this.TryGet("key", out value);
        } // End Sub Load 


    } // End Class RegistryConfigurationProvider 
    
    
} // End Namespace Microsoft.Extensions.Configuration.Registry 
