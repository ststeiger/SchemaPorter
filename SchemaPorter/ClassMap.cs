
using System.Collections.Generic;
using System.Linq.Expressions;


namespace SchemaPorter
{


    public class ExpressionInfo
    {
        public System.Reflection.MemberTypes MemberType;
        public System.Type Type;
        public string Name;
    }


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
                
                return new ExpressionInfo() {
                    Name= name,
                    Type = fi.FieldType,
                    MemberType = mem.MemberType
                };
            }
            else if (mem.MemberType == System.Reflection.MemberTypes.Property)
            {
                System.Reflection.PropertyInfo pi = (System.Reflection.PropertyInfo)mem;
                
                return new ExpressionInfo()
                {
                    Name = name,
                    Type = pi.PropertyType,
                    MemberType = mem.MemberType
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


    public class ClassMap<T>
    {

        public System.Type MappingFor;


        public ClassMap()
        {
            this.MappingFor = typeof(T);
        }


        public virtual ClassMap<T> PrimaryKey(string name, params Expression<System.Func<T, object>>[] memberExpressions)
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


        public virtual ClassMap<T> PrimaryKey(params Expression<System.Func<T, object>>[] memberExpressions)
        {
            return PrimaryKey(null, memberExpressions);
        }


        public SqlColumn Column(Expression<System.Func<T, object>> memberExpression, string columnName)
        {
            ExpressionInfo ei = ReflectionExtensions.GetInfo(memberExpression);

            return new SqlColumn()
            {
                ColumnName = columnName
            };
        }


        /// <summary>
        /// Sets the schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        public ClassMap<T> Schema(string schema)
        {
            return this;
        }

        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        public ClassMap<T> Table(string tableName)
        {
            return this;
        }


    }


}
