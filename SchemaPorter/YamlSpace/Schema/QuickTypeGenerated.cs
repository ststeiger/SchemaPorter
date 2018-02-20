
using System.Collections.Generic;

// https://app.quicktype.io/
namespace SchemaPorter.YamlSpace 
{

    public class RootNode
    {
        public DE de { get; set; }
    }

    public class DE
    {

        private Dictionary<string, string> dict = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);


        public string this[string index]
        {
            get
            {
                return this.dict[index];
            }
            set
            {
                this.dict[index] = value;
            }
        }


        public string Direction { get; set; }


        //public Date Date { get; set; }
        //public Time Time { get; set; }
        //public Datetime Datetime { get; set; }
        //public Number Number { get; set; }
        //public Support Support { get; set; }
        //public Activerecord Activerecord { get; set; }


        //public LabelX LabelXClosedIssuesAbbr { get; set; }
        //public LabelX LabelXComments { get; set; }
        //public LabelX LabelXIssues { get; set; }
        //public LabelX LabelXOpenIssuesAbbr { get; set; }
        //public LabelX LabelXProjects { get; set; }
    }
}