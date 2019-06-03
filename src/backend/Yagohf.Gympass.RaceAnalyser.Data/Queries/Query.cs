using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Sorting;

namespace Yagohf.Gympass.RaceAnalyser.Data.Queries
{
    public class Query<T> : IQuery<T> where T : class
    {
        public string Identifier { get; private set; }

        public List<Expression<Func<T, bool>>> Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; private set; }

        public List<string> IncludeStrings { get; private set; }

        public List<SortExpression<T>> SortExpressions { get; private set; }

        public Query()
        {
            this.Identifier = Guid.NewGuid().ToString();
            this.Criteria = new List<Expression<Func<T, bool>>>();
            this.Includes = new List<Expression<Func<T, object>>>();
            this.IncludeStrings = new List<string>();
            this.SortExpressions = new List<SortExpression<T>>();
        }

        public IQuery<T> Filter(Expression<Func<T, bool>> filterExpression)
        {
            this.Criteria.Add(filterExpression);
            return this;
        }

        public IQuery<T> AddInclude(Expression<Func<T, object>> include)
        {
            this.Includes.Add(include);
            return this;
        }

        public IQuery<T> AddInclude(string include)
        {
            this.IncludeStrings.Add(include);
            return this;
        }

        public IQuery<T> SortBy(Expression<Func<T, object>> sortExpression)
        {
            return this.Ordenar(sortExpression, false);
        }

        public IQuery<T> SortByDescending(Expression<Func<T, object>> sortByDescendingExpression)
        {
            return this.Ordenar(sortByDescendingExpression, true);
        }

        private IQuery<T> Ordenar(Expression<Func<T, object>> expression, bool descending)
        {
            this.SortExpressions.Add(new SortExpression<T>(expression, descending));
            return this;
        }

        public override bool Equals(object obj)
        {
            /*
             * Foi necessário sobrescrever esse método, adicionando uma comparação
             * pelo identificador para poder mockar cenários em que o mesmo método
             * do repositório é chamado em vários lugares dentro do mesmo método de negócio,
             * sendo necessário retornar um valor diferente via Moq para cada uma
             * das chamadas.
             */

            if (obj == null)
                return false;

            return (obj as Query<T>).Identifier.Equals(this.Identifier, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return 96025222 + EqualityComparer<string>.Default.GetHashCode(Identifier);
        }
    }
}
