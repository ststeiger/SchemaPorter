
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
        } // End Sub Load 
        
        
    } // End Class RegistryConfigurationProvider 
    
    
} // End Namespace Microsoft.Extensions.Configuration.Registry 
