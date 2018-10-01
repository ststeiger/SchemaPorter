
using System.Collections.Generic;


namespace SchemaPorter
{
    
    
    public class SearchResult
    {
        public string SearchTerm;
        public string Line;
        public int LineNumber;
        public int CharPos;


        public SearchResult(string line, int lineNumber, int charPos)
        {
            this.Line = line;
            this.LineNumber = lineNumber;
            this.CharPos = charPos;
        }
    }
    
    
    public class FileSearch
    {
        
        
        /*
/root/github/RedmineMailService/RedmineMailService/Redmine/API.cs (132):   , SecretManager.GetSecret<string>("RedmineSuperUser")
/root/github/RedmineMailService/RedmineMailService/Redmine/API.cs (133):   , SecretManager.GetSecret<string>("RedmineSuperUserPassword")
/root/github/SchemaPorter/SchemaPorter/FileSearch.cs (55):                    if (line.IndexOf("RedmineSuperUser") != -1)
/root/github/CorMine/RedmineClient/RedmineFactory.cs (43):                 , TestPlotly.SecretManager.GetSecret<string>("RedmineSuperUser")
/root/github/CorMine/RedmineClient/RedmineFactory.cs (44):                 , TestPlotly.SecretManager.GetSecret<string>("RedmineSuperUserPassword")
/root/github/CorMine/CorMine/AppCode/RedmineFactory.cs (42):               , SecretManager.GetSecret<string>("RedmineSuperUser")
/root/github/CorMine/CorMine/AppCode/RedmineFactory.cs (43):               , SecretManager.GetSecret<string>("RedmineSuperUserPassword")
         */
        
        
        public static void Test()
        {
            string path = @"/root/github";
            string searchTerm = @"RedmineSuperUser";
            
            string[] filez = System.IO.Directory.GetFiles(path, "*.cs", System.IO.SearchOption.AllDirectories);

            for (int i = 0; i < filez.Length; ++i)
            {
                List<SearchResult> ls = SearchContent(filez[i], searchTerm);
                for (int j = 0; j < ls.Count; ++j)
                {
                    System.Console.WriteLine(filez[i] + " ("+ls[j].LineNumber.ToString()+"):\t" + ls[j].Line);
                } // Next j 
                
            } // Next i 
            
        } // End Sub Test 


        
        
        public static List<SearchResult> SearchContent(string file, string searchTerm)
        {
            List<SearchResult> ls = new List<SearchResult>();
            
            using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
            {
                for (int lineNumber = 1; !reader.EndOfStream; ++lineNumber)
                {
                    string line = reader.ReadLine();
                    int pos = line.IndexOf(searchTerm);
                    
                    if (pos != -1)
                    {
                        ls.Add(new SearchResult(line, lineNumber, pos));
                    } // End if (pos != -1)
                    
                } // Whend 
                
            } // End Using reader
            
            return ls;
        } // End Function SearchContent 
        
        
        public static bool ContainsGroup(string file)
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
            {
                bool hasAction = false;
                bool hasInput = false;
                bool hasResult = false;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!hasAction)
                    {
                        if (line.StartsWith("ACTION:"))
                            hasAction = true;
                    }
                    else if (!hasInput)
                    {
                        if (line.StartsWith("INPUT:"))
                            hasInput = true;
                    }
                    else if (!hasResult)
                    {
                        if (line.StartsWith("RESULT:"))
                            hasResult = true;
                    }

                    if (hasAction && hasInput && hasResult)
                        return true;
                } // Whend 

                return false;
            } // End Using reader
            
            
        } // End Sub 
        
        
    } // End Class 
    
    
} // End Namespace 
