
namespace SchemaPorter
{


    class Program
    {


        static void Main(string[] args)
        {
            YamlSpace.DeserializeJSON.Test();
            // YamlSpace.DeserializeObjectGraph.Test();
            // YamlSpace.Yaml2JSON.Test();

            // SomeTableMap xm = new SomeTableMap();

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


    } // End Class Program 


} // End Namespace SchemaPorter 
