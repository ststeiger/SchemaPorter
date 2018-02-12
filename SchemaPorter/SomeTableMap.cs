
namespace SchemaPorter
{


    public class SomeTableMap : ClassMap<SomeTable>
    {


        public SomeTableMap()
        {
            this.Schema("xxx");
            this.Table("xxx");
            this.PrimaryKey(x => x.Schema, x => x.Ordinal);
            this.Column(x => x.Schema, "Schema").Length(255).Unicode(true).NotNullable();
            this.Column(x => x.TestField, "TestField");

            System.Console.WriteLine(this.MappingFor);
        }


    }


}
