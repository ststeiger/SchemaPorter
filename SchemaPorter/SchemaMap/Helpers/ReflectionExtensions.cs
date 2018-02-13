
using System.Linq.Expressions;


namespace SchemaPorter
{
    
    
    internal static class ReflectionExtensions
    {
        
        
        public static ExpressionInfo GetInfo<TModel>(Expression<System.Func<TModel, object>> expression)
        {
            // (expression.Body as MemberExpression).Member.Name

            System.Reflection.MemberInfo mem = 
                GetMemberExpression(expression.Body, true).Member;
            
            string name = mem.Name;
            
            if (mem.MemberType == System.Reflection.MemberTypes.Field)
            {
                System.Reflection.FieldInfo fi = (System.Reflection.FieldInfo)mem;
                
                // fi.DeclaringType
                
                
                
                return new ExpressionInfo() {
                    Name= name,
                    Type = fi.FieldType,
                    MemberType = mem.MemberType,
                    TableType = fi.DeclaringType
                    
                };
            }
            else if (mem.MemberType == System.Reflection.MemberTypes.Property)
            {
                System.Reflection.PropertyInfo pi = (System.Reflection.PropertyInfo)mem;
                
                return new ExpressionInfo()
                {
                    Name = name,
                    Type = pi.PropertyType,
                    MemberType = mem.MemberType,
                    TableType = pi.DeclaringType
                };
            }

            return null;
        }


        private static MemberExpression GetMemberExpression(Expression expression, bool enforceCheck)
        {
            MemberExpression memberExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
                memberExpression = ((UnaryExpression)expression).Operand as MemberExpression;
            else if (expression.NodeType == ExpressionType.MemberAccess)
                memberExpression = expression as MemberExpression;
            if (enforceCheck && memberExpression == null)
                throw new System.ArgumentException("Not a member access", nameof(expression));
            return memberExpression;
        }
        
        
    }
    
    
}
