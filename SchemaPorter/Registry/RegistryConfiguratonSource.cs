
namespace Microsoft.Extensions.Configuration.Registry
{
    
    public class RegistryConfigurationSource
        :Microsoft.Extensions.Configuration.IConfigurationSource
    {

        public string RegistryKey;
        public string EtcDirectory;


        public RegistryConfigurationSource()
        { }
        
        
        public RegistryConfigurationSource(string registryKey, string etcDirectory)
        {
            this.RegistryKey = registryKey;
            this.EtcDirectory = etcDirectory;
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
        
        
    } // End Class RegistryConfigurationSource
    
    
}
