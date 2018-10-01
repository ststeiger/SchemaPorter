
// https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/

namespace SchemaPorter
{
    
    
    public static class Program
    {
        
        
        static void EnumRegistry()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            foreach (string subkeyName in key.GetSubKeyNames())
            {
                System.Console.WriteLine(subkeyName);

                Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subkeyName);
                if (subkey != null)
                {
                    foreach (var value in subkey.GetValueNames())
                    {
                        System.Console.WriteLine("\tValue:" + value);

                        // Check for the publisher to ensure it's our product
                        string keyValue = System.Convert.ToString(subkey.GetValue("Publisher"));
                        if (!keyValue.Equals("MyPublisherCompanyName", System.StringComparison.OrdinalIgnoreCase))
                            continue;

                        string productName = System.Convert.ToString(subkey.GetValue("DisplayName"));
                        if (!productName.Equals("MyProductName", System.StringComparison.OrdinalIgnoreCase))
                            return;

                        string uninstallPath = System.Convert.ToString(subkey.GetValue("InstallSource"));

                        // Do something with this valuable information
                    }
                }
            }

            System.Console.ReadLine();
        }
        
        
        public static void Main(string[] args)
        {
            // Another way: https://github.com/thinksquirrel/nanosvg-csharp
            string d = @"M3.726 91.397h9.349v-17.64H3.726zM18.19 79.578l4.41-4.056 4.41 4.056v3l-3.175-3.176v8.82h-2.293v-8.82l-3.352 3.175zM35.83 78.344h.175l.177-.177H36.71v-.176h.176l.177-.176h.176v-.177l.177-.176.176-.177v-.352h.176V76.05l-.176-.177v-.352h-.176v-.177h-.177v-.176l-.176-.177h-.177l-.176-.176h-.176v-.176h-1.412l-.176.176h-.176l-.177.176h-.176v.177h-.176v.176h-.177v.353h-.176V77.109l.176.176v.177h.177v.176l.176.177h.176v.176h.177l.176.176h.353l.176.177h.177z";

            // var cp1 = new Svg.CoordinateParser(d);
            var segmentList = Svg.SvgPathBuilder.Parse(d);
            System.Console.WriteLine(segmentList);

            foreach (Svg.Pathing.SvgPathSegment seg in segmentList)
            {
                System.Console.WriteLine(seg.Start);
                System.Console.WriteLine(seg.End);
            }



            SchemaPorter.FileSearch.Test();
            return;
            SchemaPorter.Settings.SettingsHelper.Test();
            // TestSMO.Test();
            
            
            // SchemaGenerator.GenerateSchema();
            // ContextGenerator.GenerateContext();
            
            // SomeTableMap xm = new SomeTableMap();
            
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 
        
        
    } // End Class Program 
    
    
} // End Namespace SchemaPorter 
