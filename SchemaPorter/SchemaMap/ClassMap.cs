
using System.Linq.Expressions;


namespace SchemaPorter
{


    
    public abstract class SqlTable
    {
        public System.Type MappingFor;
        public System.Collections.Generic.Dictionary<string, SqlColumn> Columns;
        
        
        public System.Collections.Generic.Dictionary<string, ForeignKey> ForeignKeys;
        
        
        public SqlTable()
        {
            this.Columns = new System.Collections.Generic.Dictionary<string, SqlColumn>(
                System.StringComparer.InvariantCultureIgnoreCase
            );
            
            this.ForeignKeys = new System.Collections.Generic.Dictionary<string, ForeignKey>(
                System.StringComparer.InvariantCultureIgnoreCase
            );
        }
        
        
        /// <summary>
        /// Sets the schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        public SqlTable Schema(string schema)
        {
            return this;
        }
        
        
        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        public SqlTable Table(string tableName)
        {
            return this;
        }
        
        
        public string CreateTableScript()
        {
            return null;
        }
        
    }

    
    public class ClassMap<T>
    : SqlTable
    {

        
        public ClassMap()
            :base()
        {
            this.MappingFor = typeof(T);
        }
        
        
        public virtual SqlTable PrimaryKey(string name, params Expression<System.Func<T, object>>[] memberExpressions)
        {
            System.Console.WriteLine("Primary key: ");
            
            for (int i = 0; i < memberExpressions.Length; ++i)
            {
                ExpressionInfo ei = ReflectionExtensions.GetInfo(memberExpressions[i]);
                System.Console.Write("    - ");
                System.Console.WriteLine(ei.Name);
            }
            
            return this;
        }
        
        
        public virtual SqlTable PrimaryKey(params Expression<System.Func<T, object>>[] memberExpressions)
        {
            return PrimaryKey(null, memberExpressions);
        }


        public System.Collections.Generic.List<ColumnReference> FieldList<TTable>
            (params Expression<System.Func<TTable, object>>[] memberExpressions)
        {
            
            // // Func<TEntity, T> func = propertyExpression.Compile();
            // System.Func<TTable, object> func = memberExpressions[0].Compile();
            
            return null;
        }



        public virtual ForeignKeyMap<T> AddForeignKey(string name)
        {
            return new ForeignKeyMap<T>(name);
        }
        
        
        public ForeignKeyMap<T> AddForeignKey<TTable>(
            string name,
            Expression<System.Func<T, object>> memberExpression1,
            Expression<System.Func<TTable, object>> memberExpression2
            
            )
        {
            return this.AddForeignKey(name)
                .Referencing(memberExpression1)
                .Referenced(memberExpression2);
        }

        
        
        public SqlColumn Column(Expression<System.Func<T, object>> memberExpression, string columnName)
        {
            ExpressionInfo ei = ReflectionExtensions.GetInfo(memberExpression);

            return new SqlColumn()
            {
                ColumnName = columnName
            };
        }
        
    }


}
