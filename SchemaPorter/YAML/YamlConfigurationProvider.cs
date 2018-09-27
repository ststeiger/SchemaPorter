
namespace Microsoft.Extensions.Configuration.Yaml
{


    public class YamlConfigurationProvider : Microsoft.Extensions.Configuration.FileConfigurationProvider
    {

        public YamlConfigurationProvider(YamlConfigurationSource source) 
            : base(source)
        { }


        public override void Load(System.IO.Stream stream)
        {
            YamlConfigurationFileParser parser = new YamlConfigurationFileParser();

            Data = parser.Parse(stream);
        }


    }


}
