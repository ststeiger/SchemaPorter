
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace SchemaPorter
{

    public class Mandant
    {
        public int Id;
        public int Customer;
    }


    public class SomeTableMap : ClassMap<SomeTable>
    {
        
        
        public SomeTableMap()
        {
            this.Schema("xxx");
            this.Table("xxx");
            this.PrimaryKey(x => x.Id, x => x.Sort);
            this.Column(x => x.Text, "Text").Length(255).Unicode(true).NotNullable();
            this.Column(x => x.TestField, "TestField");

            this.AddForeignKey("name")
                .Referencing(x => x.MDT_ID, x => x.MDT_Customer)
                .Referenced<Mandant>(x => x.Id, x => x.Customer)
            ;
            
            this.AddForeignKey<Mandant>("name", src => src.MDT_Customer, fk => fk.Customer);
            
            System.Console.WriteLine(this.MappingFor);
        }
        
        
    }
    
    
}
