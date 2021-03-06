﻿using DapperExtensions;
using LogDashboard.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace LogDashboard.Repository.Dapper
{
    public class DapperRepository<T> : IRepository<T> where T : class, ILogModel
    {
        private readonly SqlConnection _conn;

        public DapperRepository(IUnitOfWork unitOfWork)
        {
            _conn = (unitOfWork as DapperUnitOfWork)?.GetConnection();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            return GetList(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return _conn.GetList<T>(predicate?.ToPredicateGroup());
        }


        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            return _conn.Count<T>(predicate?.ToPredicateGroup());
        }

        public IEnumerable<T> GetPageList(int page, int size, Expression<Func<T, bool>> predicate = null, params ISort[] sorts)
        {
            return _conn.GetPage<T>(predicate?.ToPredicateGroup(), sorts, page == 0 ? page : page-1, size).ToList();

        }

    }
}
