
namespace Microsoft.Extensions.Configuration
{


    public static class YamlConfigurationExtensions
    {
        public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddYamlFile(
              this Microsoft.Extensions.Configuration.IConfigurationBuilder builder
            , string path)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }


        public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddYamlFile(
              this Microsoft.Extensions.Configuration.IConfigurationBuilder builder
            , string path
            , bool optional)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }


        public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddYamlFile(
              this Microsoft.Extensions.Configuration.IConfigurationBuilder builder
            , string path
            , bool optional
            , bool reloadOnChange)
        {
            return AddYamlFile(builder
                , provider: null
                , path: path, optional: optional
                , reloadOnChange: reloadOnChange
            );
        }



        public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddRegistryKey(
            this Microsoft.Extensions.Configuration.IConfigurationBuilder builder
            , string key 
            )
        {
            Microsoft.Extensions.Configuration.Yaml.RegistryConfiguratonSource rs = 
                new Microsoft.Extensions.Configuration.Yaml.RegistryConfiguratonSource(key);
            
            builder.Add(rs);
            return builder;
        }
        
        
        public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddYamlFile(
              this Microsoft.Extensions.Configuration.IConfigurationBuilder builder
            , Microsoft.Extensions.FileProviders.IFileProvider provider
            , string path
            , bool optional
            , bool reloadOnChange)
        {
            if (provider == null && System.IO.Path.IsPathRooted(path))
            {
                provider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    System.IO.Path.GetDirectoryName(path)
                );
                path = System.IO.Path.GetFileName(path);
            }

            Microsoft.Extensions.Configuration.Yaml.YamlConfigurationSource source = 
                new Microsoft.Extensions.Configuration.Yaml.YamlConfigurationSource
            {
                FileProvider = provider,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };
            builder.Add(source);
            return builder;
        }


    }


}
