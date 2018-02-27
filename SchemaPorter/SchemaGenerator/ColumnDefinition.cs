
namespace SchemaPorter
{

    public class PrimaryKeyDefinition 
    {
        public string TABLE_SCHEMA;
        public string TABLE_NAME;
        public string PRIMARY_KEY_NAME;
        public string PRIMARY_KEY;
        public bool IS_MULTICOLUMN_KEY;
    }


    public class ColumnDefinition
    {

        public string TABLE_NAME;
        public string COLUMN_NAME;
        public int ORDINAL_POSITION;
        public string SQL_TYPE;
        public string COLUMN_DEFAULT;
        public string DOTNET_TYPE;

        public bool IS_REQUIRED;


        public int? MAX_LENGTH;
        public string COLUMN_TYPE;

    }

}