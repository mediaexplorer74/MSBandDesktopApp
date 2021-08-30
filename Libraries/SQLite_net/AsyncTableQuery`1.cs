// Decompiled with JetBrains decompiler
// Type: SQLite.AsyncTableQuery`1
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SQLite
{
  public class AsyncTableQuery<T> where T : new()
  {
    private TableQuery<T> _innerQuery;

    public AsyncTableQuery(TableQuery<T> innerQuery) => this._innerQuery = innerQuery;

    public AsyncTableQuery<T> Where(Expression<Func<T, bool>> predExpr) => new AsyncTableQuery<T>(this._innerQuery.Where(predExpr));

    public AsyncTableQuery<T> Skip(int n) => new AsyncTableQuery<T>(this._innerQuery.Skip(n));

    public AsyncTableQuery<T> Take(int n) => new AsyncTableQuery<T>(this._innerQuery.Take(n));

    public AsyncTableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr) => new AsyncTableQuery<T>(this._innerQuery.OrderBy<U>(orderExpr));

    public AsyncTableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr) => new AsyncTableQuery<T>(this._innerQuery.OrderByDescending<U>(orderExpr));

    public Task<List<T>> ToListAsync() => Task.Factory.StartNew<List<T>>((Func<List<T>>) (() =>
    {
      using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
        return this._innerQuery.ToList<T>();
    }));

    public Task<int> CountAsync() => Task.Factory.StartNew<int>((Func<int>) (() =>
    {
      using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
        return this._innerQuery.Count();
    }));

    public Task<T> ElementAtAsync(int index) => Task.Factory.StartNew<T>((Func<T>) (() =>
    {
      using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
        return this._innerQuery.ElementAt(index);
    }));

    public Task<T> FirstAsync() => Task<T>.Factory.StartNew((Func<T>) (() =>
    {
      using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
        return this._innerQuery.First();
    }));

    public Task<T> FirstOrDefaultAsync() => Task<T>.Factory.StartNew((Func<T>) (() =>
    {
      using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
        return this._innerQuery.FirstOrDefault();
    }));
  }
}
