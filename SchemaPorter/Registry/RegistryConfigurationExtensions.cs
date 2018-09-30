
namespace Microsoft.Extensions.Configuration
{
    
    
    public static class RegistryConfigurationExtensions
    {
        
        
        public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddRegistryKey(
            this Microsoft.Extensions.Configuration.IConfigurationBuilder builder
            , string key 
        )
        {
            Microsoft.Extensions.Configuration.Registry.RegistryConfigurationSource rs = 
                new Microsoft.Extensions.Configuration.Registry.RegistryConfigurationSource(key);
            
            builder.Add(rs);
            return builder;
        }
        
        
    }
    
    
}
