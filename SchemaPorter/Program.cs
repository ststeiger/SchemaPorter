
// https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/

using System.Collections.Generic;

namespace SchemaPorter
{


    public static class Program
    {


        static void EnumRegistry()
        {
            Microsoft.Win32.RegistryKey key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
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

        public static System.DateTime GetColumnInfo()
        {
            return System.DateTime.MaxValue;
        }

        public static string GenerateInsertScript()
        {
            return null;
        }


        public static void TestStack()
        {
            System.Collections.Generic.Stack<string> stack = new Stack<string>();
            string isEmpty = stack.Peek();
            stack.Push("abc");
            string abc = stack.Pop();
        }



        private static System.IO.FileStream CreateInheritedFile(string file)
        {
            return new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read | System.IO.FileShare.Inheritable);
        }

        public static void ExecuteProcess()
        {
            string outputName = @"output.txt";
            string errorName = @"error.txt";
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            string cmd = @"""C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319\vbc.exe""  /t:library /utf8output+ /r:""d:\username\Documents\Visual Studio 2017\Projects\AnyWebReporting\AnyFormsReporting\bin\Debug\AspNetCore.ReportingServices.dll"" /out:""D:\username\Desktop\TestCode\ExpressionHost.dll"" /debug- /optimize+  /verbose ""D:\username\Desktop\TestCode\yofz523a.0.vb""";
            System.Console.WriteLine(cmd);

            System.IO.StreamWriter outputWriter = new System.IO.StreamWriter(CreateInheritedFile(outputName), System.Text.Encoding.UTF8);
            try
            {
                using (new System.IO.StreamWriter(CreateInheritedFile(errorName), System.Text.Encoding.UTF8))
                {
                    outputWriter.Write(currentDir);
                    outputWriter.Write("> ");
                    outputWriter.WriteLine(cmd);
                    outputWriter.WriteLine();
                    outputWriter.WriteLine();

                    // System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(cmd)
                    // System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd)
                    // System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + cmd)
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c \"" + cmd + "\"")
                    {
                        WorkingDirectory = currentDir,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false
                    };
                    using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo))
                    {
                        process.OutputDataReceived += delegate (object s, System.Diagnostics.DataReceivedEventArgs e)
                        {
                            if (e.Data != null)
                            {
                                outputWriter.WriteLine(e.Data);
                            }
                        };
                        process.BeginOutputReadLine();
                        try
                        {
                        }
                        catch
                        {
                        }
                        process.WaitForExit();
                        int ret = process.ExitCode;
                        System.Console.WriteLine(ret);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    

        public static void Main(string[] args)
        {
            // ExecuteProcess();
            SchemaGeneration();

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }



        public static void SchemaGeneration()
        {
            SchemaPorter.Settings.SettingsHelper.Test();
            // TestSMO.Test();


            // SchemaGenerator.GenerateSchema();
            // ContextGenerator.GenerateContext();

            // SomeTableMap xm = new SomeTableMap();
        }

        public static void ParsePath()
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
            
            
        } // End Sub Main 
        
        
    } // End Class Program 
    
    
} // End Namespace SchemaPorter 
