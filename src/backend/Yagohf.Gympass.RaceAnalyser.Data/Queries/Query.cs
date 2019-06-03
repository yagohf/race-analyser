using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Sorting;

namespace Yagohf.Gympass.RaceAnalyser.Data.Queries
{
    public class Query<T> : IQuery<T> where T : class
    {
        public string Identificador { get; private set; }

        public List<Expression<Func<T, bool>>> Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; private set; }

        public List<string> IncludeStrings { get; private set; }

        public List<SortExpression<T>> OrderBy { get; private set; }

        public Query()
        {
            this.Identificador = Guid.NewGuid().ToString();
            this.Criteria = new List<Expression<Func<T, bool>>>();
            this.Includes = new List<Expression<Func<T, object>>>();
            this.IncludeStrings = new List<string>();
            this.OrderBy = new List<SortExpression<T>>();
        }

        public IQuery<T> Filtrar(Expression<Func<T, bool>> filtro)
        {
            this.Criteria.Add(filtro);
            return this;
        }

        public IQuery<T> AdicionarInclude(Expression<Func<T, object>> include)
        {
            this.Includes.Add(include);
            return this;
        }

        public IQuery<T> AdicionarInclude(string include)
        {
            this.IncludeStrings.Add(include);
            return this;
        }

        public IQuery<T> OrdenarPor(Expression<Func<T, object>> expression)
        {
            return this.Ordenar(expression, false);
        }

        public IQuery<T> OrdenarPorDescendente(Expression<Func<T, object>> expression)
        {
            return this.Ordenar(expression, true);
        }

        private IQuery<T> Ordenar(Expression<Func<T, object>> expression, bool descendente)
        {
            this.OrderBy.Add(new SortExpression<T>(expression, descendente));
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

            return (obj as Query<T>).Identificador.Equals(this.Identificador, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return 96025222 + EqualityComparer<string>.Default.GetHashCode(Identificador);
        }
    }
}
