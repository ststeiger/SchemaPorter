
namespace Microsoft.Extensions.Configuration.Registry
{


    public class EtcAppliance
    {


        private static string GetFullPathWithoutExtension(string path)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path));
        }


        public static System.Collections.Generic.Dictionary<string, string> ReadEtc(string basePath, System.IO.DirectoryInfo baseDirectory)
        {
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>(
                System.StringComparer.OrdinalIgnoreCase
            );


            System.IO.FileInfo[] fis = baseDirectory.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);

            foreach (System.IO.FileInfo fi in fis)
            {
                string key = fi.FullName.Substring(basePath.Length + 1);
                key = GetFullPathWithoutExtension(key);
                key = key.Replace(System.IO.Path.DirectorySeparatorChar, ':');

                using (System.IO.FileStream fs = fi.OpenRead()) 
                { 
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8, true))
                    {
                        dict[key] = sr.ReadToEnd().Trim(new char[]{'\r', '\n', '\v'});
                    } // End Using sr 

                } // End Using fs 

            } // Next fi 

            System.IO.DirectoryInfo[] dis = baseDirectory.GetDirectories("*.*", System.IO.SearchOption.TopDirectoryOnly); 
            foreach (System.IO.DirectoryInfo di in dis) 
            { 
                System.Collections.Generic.Dictionary<string, string> sdict = ReadEtc(basePath, di); 
                foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in sdict) 
                { 
                    dict[kvp.Key] = kvp.Value; 
                } // Next kvp 

            } // Next di 

            return dict; 
        } // End Function ReadEtc 


        // Microsoft.Extensions.Configuration.Registry.EtcAppliance.ReadEtc("/etc/COR/All"); 
        public static System.Collections.Generic.Dictionary<string, string> ReadEtc(string basePath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(basePath);
            if (!di.Exists)
                throw new System.ArgumentException("Invalid basePath: Directory \"" + basePath + "\" doesn't exist.");

            return ReadEtc(basePath, di);
        } // End Function ReadEtc 


    } // End Class EtcAppliance 


} // End Namespace Microsoft.Extensions.Configuration.Registry 
