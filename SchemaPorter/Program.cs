
using Microsoft.Extensions.Configuration;

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




        public static System.Collections.Generic.Dictionary<string, string> RecursivelyListKeys(Microsoft.Win32.RegistryKey baseKey)
        {
            System.Collections.Generic.Dictionary<string, string> ls =
                new System.Collections.Generic.Dictionary<string, string>();

            string[] valueNames = baseKey.GetValueNames();
            System.Array.Sort(valueNames, delegate (string s1, string s2)
            {
                return s1.CompareTo(s2);
            });


            foreach (string valueName in valueNames)
            {
                string key = baseKey.Name + @"\" + valueName;

                try
                {
                    Microsoft.Win32.RegistryValueKind rvk = baseKey.GetValueKind(valueName);
                    string type = ToDataType(rvk);
                    object value = baseKey.GetValue(valueName);

                    // https://docs.microsoft.com/en-us/windows/desktop/sysinfo/registry-value-types
                    if (rvk == Microsoft.Win32.RegistryValueKind.String)
                    {
                        string stringValue = System.Convert.ToString(value);
                        ls[key] = stringValue;
                    }
                    else if (rvk == Microsoft.Win32.RegistryValueKind.DWord)
                    {
                        int dwordValue = System.Convert.ToInt32(value);
                        ls[key] = dwordValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (rvk == Microsoft.Win32.RegistryValueKind.QWord)
                    {
                        long qwordValue = System.Convert.ToInt64(value);
                        ls[key] = qwordValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (rvk == Microsoft.Win32.RegistryValueKind.Binary)
                    {
                        byte[] binaryValue = (byte[])value;
                        ls[key] = System.Convert.ToBase64String(binaryValue);
                    }
                    else if (rvk == Microsoft.Win32.RegistryValueKind.MultiString)
                    {
                        string[] multiStringValue = (string[])value;
                        ls[key] = Newtonsoft.Json.JsonConvert.SerializeObject(multiStringValue);
                    }
                    else if (rvk == Microsoft.Win32.RegistryValueKind.ExpandString)
                    {
                        string expString = System.Convert.ToString(value);
                        ls[key] = expString;
                    }
                    else if (rvk == Microsoft.Win32.RegistryValueKind.None)
                    {
                        ls[key] = null;
                    }
                    else  // if (rvk == Microsoft.Win32.RegistryValueKind.Unknown)
                    {
                        ls[key] = System.Convert.ToString(value);
                    }

                }
                catch (System.Exception ex)
                {
                    ls[key] = ex.Message;
                }

            } // Next valueName 


            string[] subkeys = baseKey.GetSubKeyNames();
            // System.Console.WriteLine(subkeys.Length);

            foreach (string subKey in subkeys)
            {
                string newBasePath = baseKey.Name + @"\" + subKey;

                try
                {
                    Microsoft.Win32.RegistryKey sk = baseKey.OpenSubKey(subKey);
                    // ls.Add(newBasePath);

                    System.Collections.Generic.Dictionary<string, string> lss = RecursivelyListKeys(sk);
                    // ls.AddRange(lss);

                    foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in lss)
                        ls[kvp.Key] = kvp.Value;

                    lss.Clear();
                    lss = null;
                }
                catch (System.Exception ex)
                {
                    // ls.Add(newBasePath);
                    ls[newBasePath] = ex.Message;
                }

            } // Next subKey 

            return ls;
        }

        public static string ToHiveName(this Microsoft.Win32.RegistryHive regHive)
        {
            switch (regHive)
            {
                case Microsoft.Win32.RegistryHive.CurrentUser:
                    return "HKEY_CURRENT_USER";
                case Microsoft.Win32.RegistryHive.LocalMachine:
                    return "HKEY_LOCAL_MACHINE";
                case Microsoft.Win32.RegistryHive.PerformanceData:
                    return "HKEY_PERFORMANCE_DATA";
                case Microsoft.Win32.RegistryHive.Users:
                    return "HKEY_USERS";
                case Microsoft.Win32.RegistryHive.ClassesRoot:
                    return "HKEY_CLASSES_ROOT";
                case Microsoft.Win32.RegistryHive.CurrentConfig:
                    return "HKEY_CURRENT_CONFIG";
                    // case Microsoft.Win32.RegistryHive.DynData: return "HKEY_DYN_DATA";

                    // default: return regHive.ToString();
            } // End switch (regHive) 

            string hiveName = regHive.ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("HKEY");

            foreach (char c in hiveName)
            {
                if (char.IsUpper(c))
                    sb.Append("_");

                sb.Append(c);
            } // Next c 

            hiveName = sb.ToString().ToUpper();
            sb.Clear();
            sb = null;

            return hiveName;
        } // End Function ToHiveName 


        public static System.Collections.Generic.Dictionary<string, string> RecursivelyListKeys(string path)
        {
            if (string.IsNullOrEmpty(path) || path.Trim() == string.Empty)
            {
                throw new System.ArgumentNullException("path");
            }

            if (path.StartsWith(@"Computer\"))
                path = path.Substring(@"Computer\".Length);

            string[] paths = null;

            if (path.IndexOf('\\') == -1)
                paths = new string[] { path };
            else
                paths = path.Split('\\', System.StringSplitOptions.RemoveEmptyEntries);

            // Microsoft.Win32.RegistryKey HKCC = Microsoft.Win32.Registry.CurrentConfig;
            // Microsoft.Win32.RegistryKey HKLM = Microsoft.Win32.Registry.LocalMachine;

            // Microsoft.Win32.RegistryKey hive = Microsoft.Win32.RegistryKey.OpenBaseKey(
            //     Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Default
            // );

            // hive.GetValue("name");


            Microsoft.Win32.RegistryKey baseKey = null;

            System.Array values = System.Enum.GetValues(typeof(Microsoft.Win32.RegistryHive));
            foreach (Microsoft.Win32.RegistryHive thisHive in values)
            {
                if (string.Equals(ToHiveName(thisHive), paths[0], System.StringComparison.OrdinalIgnoreCase))
                {
                    baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(
                        thisHive, Microsoft.Win32.RegistryView.Default
                    );
                    break;
                } // End if (string.Equals("HKEY" + thisHive.ToString(), paths[0].Replace("_", ""), System.StringComparison.OrdinalIgnoreCase)) 

            } // Next thisHive 

            for (int i = 1; i < paths.Length; ++i)
            {
                baseKey = baseKey.OpenSubKey(paths[i]);
            } // Next i 

            System.Collections.Generic.Dictionary<string, string> ls = RecursivelyListKeys(baseKey);
            return ls;
        } // End Function RecursivelyListKeys 


        public static string ToDataType(Microsoft.Win32.RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case Microsoft.Win32.RegistryValueKind.Binary:
                    return "REG_BINARY"; // Binärwert
                case Microsoft.Win32.RegistryValueKind.DWord:
                    return "REG_DWORD"; // DWord (32-Bit)
                case Microsoft.Win32.RegistryValueKind.ExpandString:
                    return "REG_EXPAND_SZ"; // Wert der erweiterbaren Zeichenfolge
                case Microsoft.Win32.RegistryValueKind.MultiString:
                    return "REG_MULTI_SZ"; // Wert der mehrteiligen Zeichenfolge
                case Microsoft.Win32.RegistryValueKind.QWord:
                    return "REG_QWORD"; // QWord (64-Bit) 
                case Microsoft.Win32.RegistryValueKind.String:
                    return "REG_SZ"; // Zeichenfolge
                case Microsoft.Win32.RegistryValueKind.Unknown:
                    return "REG_UNKNOWN";
                default:
                    return string.Empty;
            }
        }

        public static void foo()
        {
            System.Collections.Generic.Dictionary<string, string> ls = RecursivelyListKeys(@"Computer\HKEY_CURRENT_USER\Software\COR\SchemaPorter");
            System.Console.WriteLine(ls);

            // Microsoft.Win32.RegistryValueKind GetValueKind
            Microsoft.Win32.RegistryKey rk = null;
            Microsoft.Win32.RegistryValueKind rvk = rk.GetValueKind("name");
            rk.SetValue("name", "value", Microsoft.Win32.RegistryValueKind.String);


            // Microsoft.Win32.RegistryHive.CurrentConfig

        }


        public static void Main(string[] args)
        {
            foo();
            return;

            SchemaPorter.Settings.SettingsHelper.Test();
            string yaml = YamlSpace.Yaml2JSON.TestSerialize();
            System.Console.WriteLine(yaml);

            YamlSpace.Yaml2JSON.Test();
            // YamlSpace.DeserializeJSON.Test();

            // YamlSpace.DeserializeObjectGraph.Test();
            System.Console.WriteLine("hello");

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
