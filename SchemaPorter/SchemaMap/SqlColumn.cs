
namespace SchemaPorter
{


    public class SqlColumn
    {

        public string ColumnName;
        // public int Ordinal; // Irrelevant
        public string Type;
        public bool IsNullable;
        public string ColumnDefault;
        public string CollationName;

        public int Size;
        public int Character_Maximum_Length;
        public int Character_Octet_Length;

        public int Numeric_Precision;
        public int Numeric_Scale;
        public int Datetime_Precision;

        public bool Unique;

        // Constraints

        public SqlColumn Column(string name)
        {
            this.ColumnName = name;
            
            return this;
        }


        public SqlColumn Length(int? length)
        {
            return this;
        }
        
        
        public SqlColumn Encoding(System.Text.Encoding enc)
        {
            return this;
        }


        public SqlColumn Unicode(bool isUnicode)
        {
            return this;
        }


        public SqlColumn Nullable(bool isNullable)
        {
            return this;
        }


        public SqlColumn Nullable()
        {
            return this.Nullable(true);
        }


        public SqlColumn NotNullable()
        {
            return this.Nullable(false);
        }
        
        
        public SqlColumn Default(string name)
        {
            return this;
        }
        
        
        public SqlColumn Computed(bool isNullable)
        {
            return this;
        }
        
        
        public SqlColumn Computed()
        {
            return this.Computed(true);
        }
        
        
    }
    
    
}
