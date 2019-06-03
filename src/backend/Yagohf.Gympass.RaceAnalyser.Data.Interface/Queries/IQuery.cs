using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Sorting;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries
{
    public interface IQuery<T> where T : class
    {
        string Identifier { get; }
        List<Expression<Func<T, bool>>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        List<SortExpression<T>> SortExpressions { get; }

        IQuery<T> Filter(Expression<Func<T, bool>> filterExpression);
        IQuery<T> AddInclude(Expression<Func<T, object>> include);
        IQuery<T> AddInclude(string include);
        IQuery<T> SortBy(Expression<Func<T, object>> sortExpression);
        IQuery<T> SortByDescending(Expression<Func<T, object>> sortByDescendingExpression);
    }
}
