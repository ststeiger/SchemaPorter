
namespace Microsoft.Extensions.Configuration.Yaml
{


    // https://andrewlock.net/converting-web-config-files-to-appsettings-json-with-a-net-core-global-tool/
    // https://raw.githubusercontent.com/andrewlock/NetEscapades.Configuration/master/src/NetEscapades.Configuration.Yaml/YamlConfigurationFileParser.cs
    internal class YamlConfigurationFileParser
    {
        private readonly System.Collections.Generic.IDictionary<string, string> _data = 
            new System.Collections.Generic.SortedDictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

        private readonly System.Collections.Generic.Stack<string> _context = 
            new System.Collections.Generic.Stack<string>();

        private string _currentPath;




        public System.Collections.Generic.IDictionary<string, string> Parse(
            System.IO.Stream input)
        {
            _data.Clear();
            _context.Clear();

            // https://dotnetfiddle.net/rrR2Bb
            YamlDotNet.RepresentationModel.YamlStream yaml = new YamlDotNet.RepresentationModel.YamlStream();
            yaml.Load(new System.IO.StreamReader(input, detectEncodingFromByteOrderMarks: true));


            foreach (YamlDotNet.RepresentationModel.YamlDocument doc in yaml.Documents)
            {
                YamlDotNet.RepresentationModel.YamlMappingNode mapping =
                    (YamlDotNet.RepresentationModel.YamlMappingNode)doc.RootNode;

                // The document node is a mapping node
                VisitYamlMappingNode(mapping);
            }

            return _data;
        }




        private void VisitYamlNodePair(System.Collections.Generic.KeyValuePair<
            YamlDotNet.RepresentationModel.YamlNode, YamlDotNet.RepresentationModel.YamlNode
            > yamlNodePair)
        {
            string context = ((YamlDotNet.RepresentationModel.YamlScalarNode)yamlNodePair.Key).Value;

            VisitYamlNode(context, yamlNodePair.Value);
        }




        private void VisitYamlNode(string context, YamlDotNet.RepresentationModel.YamlNode node)
        {
            if (node is YamlDotNet.RepresentationModel.YamlScalarNode)
            {
                VisitYamlScalarNode(context, (YamlDotNet.RepresentationModel.YamlScalarNode)node);
            }
            if (node is YamlDotNet.RepresentationModel.YamlMappingNode)
            {
                VisitYamlMappingNode(context, (YamlDotNet.RepresentationModel.YamlMappingNode)node);
            }
            if (node is YamlDotNet.RepresentationModel.YamlSequenceNode)
            {
                VisitYamlSequenceNode(context, (YamlDotNet.RepresentationModel.YamlSequenceNode)node);
            }
        }



        private void VisitYamlScalarNode(string context, YamlDotNet.RepresentationModel.YamlScalarNode yamlValue)
        {
            //a node with a single 1-1 mapping 
            EnterContext(context);
            string currentKey = _currentPath;

            if (_data.ContainsKey(currentKey))
            {
                throw new System.FormatException("Resources.FormatError_KeyIsDuplicated(currentKey)");
            }

            _data[currentKey] = yamlValue.Value;
            ExitContext();
        }



        private void VisitYamlMappingNode(YamlDotNet.RepresentationModel.YamlMappingNode node)
        {
            foreach (System.Collections.Generic.KeyValuePair<
                YamlDotNet.RepresentationModel.YamlNode, YamlDotNet.RepresentationModel.YamlNode
                > yamlNodePair in node.Children)
            {
                VisitYamlNodePair(yamlNodePair);
            }
        }


        private void VisitYamlMappingNode(string context, YamlDotNet.RepresentationModel.YamlMappingNode yamlValue)
        {
            //a node with an associated sub-document
            EnterContext(context);

            VisitYamlMappingNode(yamlValue);

            ExitContext();
        }


        private void VisitYamlSequenceNode(string context, YamlDotNet.RepresentationModel.YamlSequenceNode yamlValue)
        {
            //a node with an associated list
            EnterContext(context);

            VisitYamlSequenceNode(yamlValue);

            ExitContext();
        }


        private void VisitYamlSequenceNode(YamlDotNet.RepresentationModel.YamlSequenceNode node)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                VisitYamlNode(i.ToString(), node.Children[i]);
            }
        }


        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(System.Linq.Enumerable.Reverse(_context));
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(System.Linq.Enumerable.Reverse(_context));
        }


    }

}
