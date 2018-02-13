
using System.Linq.Expressions;


namespace SchemaPorter
{
    
    
    public class ColumnReference
    {
        public string Schema;
        public string Table;
        public string Column;
    }
    
    
    public abstract class ForeignKey
    {
        public string Name;

        public System.Collections.Generic.List<ColumnReference> ReferencingColumns;
        public System.Collections.Generic.List<ColumnReference> ReferencedColumns;
        
        
        public ForeignKey()
        {
            this.ReferencingColumns = new System.Collections.Generic.List<ColumnReference>();
            this.ReferencedColumns = new System.Collections.Generic.List<ColumnReference>();
        }
        
        
    }
    
    
    public class ForeignKeyMap<T>
        : ForeignKey
    {
        
        
        public ForeignKeyMap(string name)
            :base()
        {
            this.Name = name;
        }
        
        
        public ForeignKeyMap()
            :this(null)
        { }




        public ForeignKeyMap<T> Referencing(params Expression<System.Func<T, object>>[] memberExpressions)
        {
            return this;
        }
        
        
        public ForeignKeyMap<T> Referenced<TTable>(params Expression<System.Func<TTable, object>>[] memberExpressions)
        {
            return this;
        }
        
        
    }
    
    
}
