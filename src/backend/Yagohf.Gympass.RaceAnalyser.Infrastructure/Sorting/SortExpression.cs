using System;
using System.Linq.Expressions;

namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Sorting
{
    public class SortExpression<T> where T : class
    {
        public SortExpression(Expression<Func<T, object>> expression, bool descending)
        {
            this.Expression = expression;
            this.Descending = descending;
        }

        public bool Descending { get; }
        public Expression<Func<T, object>> Expression { get; }
    }
}
