
namespace SchemaPorter
{



    public class SqlFactory
    {
        private static string s_cs;
        private static System.Data.Common.DbProviderFactory s_fac;
        
        
        static SqlFactory()
        {
            s_cs = GetConnectionString();
            s_fac = Mono.Sucks.DbProviderFactories.GetFactory(
                typeof(System.Data.SqlClient.SqlClientFactory)
            );
        }
        
        
        public static System.Data.Common.DbConnection GetConnection()
        {
            System.Data.Common.DbConnection con = s_fac.CreateConnection();
            con.ConnectionString = s_cs;
            
            return con;
        }
        
        
        public static string ConnectionString
        {
            get { return GetConnectionString(); }
        }

        public static System.Data.SqlClient.SqlConnection LocalConntection
        {
            get
            {
                return new System.Data.SqlClient.SqlConnection(LocalConnection());
            }
        }


        public static string LocalConnection()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = 
                new System.Data.SqlClient.SqlConnectionStringBuilder();

            csb.IntegratedSecurity = true;
            csb.DataSource = System.Environment.MachineName;

            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                csb.IntegratedSecurity = false;
            }
            else
            {
                csb.DataSource = System.Environment.MachineName + @"\SQLEXPRESS";
            }
                


            csb.InitialCatalog = "COR_Basic_Demo_V4";
            
            if (!csb.IntegratedSecurity)
            {
                string asm = "BlueMine";
                csb.UserID = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbUser", asm);
                csb.Password = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbPassword", asm);
            }

            csb.PacketSize = 4096;
            csb.PersistSecurityInfo = false;
            csb.ApplicationName = "SqlFactory";
            csb.ConnectTimeout = 15;
            csb.Pooling = true;
            csb.MinPoolSize = 1;
            csb.MaxPoolSize = 100;
            csb.MultipleActiveResultSets = false;
            csb.WorkstationID = System.Environment.MachineName;



            return csb.ConnectionString;
        }



        public static string GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();

            string asm = "SchemaPorter";

            csb.DataSource = SchemaPorter.SecretManager.GetSecret<string>("DataSource", asm);
            csb.InitialCatalog = "COR_Basic_Demo_V4";

            // csb.UserID = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbUser");
            // csb.Password = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbPassword");

            csb.UserID = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbUser", asm);
            csb.Password = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbPassword", asm);

            csb.PacketSize = 4096;
            csb.PersistSecurityInfo = false;
            csb.ApplicationName = "SchemaPorter";
            csb.ConnectTimeout = 15;
            csb.Pooling = true;
            csb.MinPoolSize = 1;
            csb.MaxPoolSize = 100;
            csb.MultipleActiveResultSets = false;
            csb.WorkstationID = System.Environment.MachineName;

            return csb.ConnectionString;
        }

            public static string GetBlueMineConnectionStringGetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = 
                new System.Data.SqlClient.SqlConnectionStringBuilder();

            string asm = "BlueMine";

            csb.DataSource = SchemaPorter.SecretManager.GetSecret<string>("DataSource", asm);
            csb.InitialCatalog = "BlueMine";
            csb.InitialCatalog = "Redmine";

            // csb.UserID = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbUser");
            // csb.Password = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbPassword");

            csb.UserID = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbUser", asm);
            csb.Password = SchemaPorter.SecretManager.GetSecret<string>("DefaultDbPassword", asm);

            csb.PacketSize = 4096;
            csb.PersistSecurityInfo = false;
            csb.ApplicationName = "BlueMine";
            csb.ConnectTimeout = 15;
            csb.Pooling = true;
            csb.MinPoolSize = 1;
            csb.MaxPoolSize = 100;
            csb.MultipleActiveResultSets = false;
            csb.WorkstationID = System.Environment.MachineName;
            
            return csb.ConnectionString;
        }
        
        
    }
    
    
}
