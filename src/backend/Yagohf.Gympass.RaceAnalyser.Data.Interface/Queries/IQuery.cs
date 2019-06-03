﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Sorting;

namespace Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries
{
    public interface IQuery<T> where T : class
    {
        string Identificador { get; }
        List<Expression<Func<T, bool>>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        List<SortExpression<T>> OrderBy { get; }

        IQuery<T> Filtrar(Expression<Func<T, bool>> filtro);
        IQuery<T> AdicionarInclude(Expression<Func<T, object>> include);
        IQuery<T> AdicionarInclude(string include);
        IQuery<T> OrdenarPor(Expression<Func<T, object>> expression);
        IQuery<T> OrdenarPorDescendente(Expression<Func<T, object>> expression);
    }
}
