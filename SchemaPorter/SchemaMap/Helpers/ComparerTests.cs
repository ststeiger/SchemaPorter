
namespace SchemaPorter.SchemaMap.Helpers
{
    class ComparerTests
    {

        public enum SortDirection : int
        {
            Ascending = 1,
            Descending = -1
        };

        public static void Test()
        {
            System.Collections.Generic.List<string> ls = 
                new System.Collections.Generic.List<string>();
            ls.Add("A");
            ls.Add("B");
            ls.Add(null);
            ls.Add("C");
            ls.Add("ASA");
            ls.Add("AAA");
            ls.Add("a");
            ls.Add("BAB");
            ls.Add(null);
            ls.Add("CCC");
            ls.Add("D");


            ls.Sort(delegate (string x, string y)
            {
                SortDirection direction = SortDirection.Descending;

                if (x == null && y == null)
                    return 0;

                if (x == null || y == null)
                {
                    // return (int)direction * (x == null ? -1 : 1); // NULL-Values at top when ASC, bottom when DESC
                    // return (int)direction * (x == null ? 1 : -1); // NULL-Values at bottom when ASC, top when DESC

                    // return (x == null ? -1 : 1); // NULL-Values at top, indep. of search dir
                    return (x == null ? 1 : -1); // NULL-Values at bottom, indep. of search dir
                }

                return (int)direction * x.CompareTo(y); ;
            });
            

            for (int i = 0; i < ls.Count; ++i)
            {
                System.Console.WriteLine($"{i}: \"{ls[i]}\"");
            }


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }


        public class TestClassNonGeneric : System.IComparable
        {
            public string Name;

            public int CompareTo(object obj)
            {
                return this.Name.CompareTo(((TestClassNonGeneric)obj).Name);
            }
        }


        public class TestClassGeneric : System.IComparable<TestClassGeneric>
        {
            public string Name;

            public int CompareTo(TestClassGeneric other)
            {
                return System.StringComparer.InvariantCultureIgnoreCase.Compare(this.Name, other.Name);
            }
        }


        public class TestClassGenericStatic : System.Collections.Generic.IComparer<TestClassGenericStatic>
        {
            public int Compare(TestClassGenericStatic x, TestClassGenericStatic y)
            {
                if (x == y)
                    return 0;

                if (1 == System.Math.Pow(-1, 3))
                    return 1;

                return -1;
            }
        }


    }
}
