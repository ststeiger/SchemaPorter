
using System.Collections.Generic;


namespace SchemaPorter
{
    public enum RoutineEnum_t : int
    {
         All_Routines = 0

        ,All_Functions = 1
        ,All_SQL_Functions = 2
        ,All_CLR_Functions = 4
        ,All_Aggregate_Functions = 8 

        ,All_Table_Functions = 16
        ,All_SQL_Table_Functions = 32
        ,All_CLR_Table_Functions = 64

        ,ALL_scalar_Functions = 128
        ,All_SQL_scalar_Functions = 256
        ,All_CLR_scalar_Functions = 512

        ,All_Procedures = 1024
        ,All_SQL_Procedures = 2048
        ,All_CLR_Procedures = 4096
        ,All_Extended_Procedures = 8192 
    }


    public class DataStructures
    {
        
    }
    
        
        
    // views, tables, routines, synonyms
    public class ObjectSchemaContainer 
    {
        public string ObjectSchema;
        public string ObjectName;
        public string Type;
    }

    public class Synonym 
    {
        public ObjectSchemaContainer Location;
        public ObjectSchemaContainer Referenced;
    }


    public class Assembly
    {
        public string Name;
        public string Namespace;
        public string Base64;
        public string Permissions;
    }

    public class UserDefinedDataType
    {
        public int Kind;
        public string Name;
        public string Type;
    }


    public class RoutineData
    {
        public ObjectSchemaContainer Routine;
        public string Type; // procedure, tfv, scalar, 
        public string Definition;
        public string Assemlby;
        public bool? Deterministic;
        public bool? SchemaBinding;
        public bool? Recompile;
        public List<ObjectSchemaContainer> Dependencies;
    }



    // unique c&i, check, default, 
    public class ConstraintContainer 
    {
        public ObjectSchemaContainer Object;
        public ObjectSchemaContainer Constraint;
        public string Condition;
    }


    public class Index
    {
        public ObjectSchemaContainer @Object;
        public ObjectSchemaContainer Constraint;
        public ObjectSchemaContainer @Object2;
        public List<string> Columns;
    }


    public class ColumnProperty 
    {
        public string ColumnName;
        public string TableSchema;
        public string TableName;
        public int OrdinalPosition;
        public bool Nullable;
        public int Length;
        public int OctetLength;
        public int Precision;
        public int Scale;
        public int DateTimeScale;
        public string Type;
        public string DotNetType;
        public string ComputationFormula;
        public bool? Persisted;
        public bool? HasTimeZone;
    }
    
    
    public class ReferencedColumn
    {
        public string TableSchema;
        public string TableName;
        public string ColumnName;
        public int Ordinal;
    }


    public class PrimaryKey
    {
        public string ConstraintSchema;
        public string ConstraintName;

        public string TableSchema;
        public string TableName;


        public List<ReferencedColumn> KeyColumns;
    }



    public class ForeignKey1 
    {
        public ObjectSchemaContainer DomesticTable;
        public ObjectSchemaContainer ForeignTable;
    }




    public class ForeignKey2
    {
        public string BaseSchemaName;
        public string BaseTableName;

        public string ReferencedSchemaName;
        public string ReferencedTableName;

        public List<ReferencedColumn> BaseColumns;
        public List<ReferencedColumn> ReferencedColumns;
    }



    
    
}