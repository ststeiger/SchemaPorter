
namespace SchemaPorter.SchemaGeneratorPocos
{

    // interface list structure

    // functions: table (single-statement, multi-statement), scalar, clr

    // 0. dummy assemblies
    // 1. dummy views, functions
    // 2. tables, create columns, computed_columns
    // 3. primary_keys
    // 4. unique constraints
    // 5. foreign_keys
    // 6. real views, functions, procedures
    // 7. triggers



    // insert data (no computed columns)

    // Problems:
    // uid, filestream/bfile, cte, hierarchyid, geography


    public class Assembly
    { }


    public class CheckConstraint
    { }


    public class FunctionArgument
    {
        public string name;
        public string data_type;
        public string length;
        public bool nullable;
        public bool @in;
        public bool @out;

    }


    public class Function
    {

    }


    public class ForeignKey
    {
        public string name;

        public string source_table;
        public string target_table;

        public System.Collections.Generic.List<string> SourceColumns;
        public System.Collections.Generic.List<string> TargetColumns;

    }


    public class DataTypeInfo
    {
        public string sql_type;
        public string dotnet_type;
        public string dotnet_assembly;
        public long min_range;
        public long max_range;

    }


    class MyPOCOs
    {
        public string schema_name;
        public string table_name;
        public string column_name;
        public long length;
        public string data_type;
        public bool is_nullable;
        public bool is_identity;
        public bool is_computed;

        public int ordinal_position;

        public string column_default;

    }
}
