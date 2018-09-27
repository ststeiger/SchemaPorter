
namespace Microsoft.Extensions.Configuration.Yaml
{


    public class YamlConfigurationSource : Microsoft.Extensions.Configuration.FileConfigurationSource
    {


        public override Microsoft.Extensions.Configuration.IConfigurationProvider 
            Build(
            Microsoft.Extensions.Configuration.IConfigurationBuilder builder)
        {
            // FileProvider = FileProvider ?? builder.GetFileProvider();

            FileProvider = FileProvider ?? Microsoft.Extensions.Configuration
                .FileConfigurationExtensions.GetFileProvider(builder);

            return new YamlConfigurationProvider(this);
        }


    }


}
