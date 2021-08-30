// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteConnection
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace SQLite
{
  public class SQLiteConnection : IDisposable
  {
    private bool _open;
    private TimeSpan _busyTimeout;
    private Dictionary<string, TableMapping> _mappings;
    private Dictionary<string, TableMapping> _tables;
    private Stopwatch _sw;
    private long _elapsedMilliseconds;
    private int _transactionDepth;
    private Random _rand = new Random();
    internal static readonly sqlite3 NullHandle;

    public SQLiteConnection(string databasePath, bool storeDateTimeAsTicks = true)
      : this(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks)
    {
    }

    public SQLiteConnection(
      string databasePath,
      SQLiteOpenFlags openFlags,
      bool storeDateTimeAsTicks = true)
    {
      this.DatabasePath = !string.IsNullOrEmpty(databasePath) ? databasePath : throw new ArgumentException("Must be specified", nameof (databasePath));
      sqlite3 db;
      SQLite3.Result r = SQLite3.Open(databasePath, out db, (int) openFlags, IntPtr.Zero);
      this.Handle = db;
      if (r != SQLite3.Result.OK)
        throw SQLiteException.New(r, string.Format("Could not open database file: {0} ({1})", new object[2]
        {
          (object) this.DatabasePath,
          (object) r
        }));
      this._open = true;
      this.StoreDateTimeAsTicks = storeDateTimeAsTicks;
      this.BusyTimeout = TimeSpan.FromSeconds(0.1);
    }

    public sqlite3 Handle { get; private set; }

    public string DatabasePath { get; private set; }

    public bool TimeExecution { get; set; }

    public bool Trace { get; set; }

    public bool StoreDateTimeAsTicks { get; private set; }

    public TimeSpan BusyTimeout
    {
      get => this._busyTimeout;
      set
      {
        this._busyTimeout = value;
        if (this.Handle == SQLiteConnection.NullHandle)
          return;
        int num = (int) SQLite3.BusyTimeout(this.Handle, (int) this._busyTimeout.TotalMilliseconds);
      }
    }

    public IEnumerable<TableMapping> TableMappings => this._tables != null ? (IEnumerable<TableMapping>) this._tables.Values : Enumerable.Empty<TableMapping>();

    public TableMapping GetMapping(Type type, CreateFlags createFlags = CreateFlags.None)
    {
      if (this._mappings == null)
        this._mappings = new Dictionary<string, TableMapping>();
      TableMapping tableMapping;
      if (!this._mappings.TryGetValue(type.FullName, out tableMapping))
      {
        tableMapping = new TableMapping(type, createFlags);
        this._mappings[type.FullName] = tableMapping;
      }
      return tableMapping;
    }

    public TableMapping GetMapping<T>() => this.GetMapping(typeof (T));

    public int DropTable<T>() => this.Execute(string.Format("drop table if exists \"{0}\"", new object[1]
    {
      (object) this.GetMapping(typeof (T)).TableName
    }));

    public int CreateTable<T>(CreateFlags createFlags = CreateFlags.None) => this.CreateTable(typeof (T), createFlags);

    public int CreateTable(Type ty, CreateFlags createFlags = CreateFlags.None)
    {
      if (this._tables == null)
        this._tables = new Dictionary<string, TableMapping>();
      TableMapping mapping;
      if (!this._tables.TryGetValue(ty.FullName, out mapping))
      {
        mapping = this.GetMapping(ty, createFlags);
        this._tables.Add(ty.FullName, mapping);
      }
      bool flag1 = (createFlags & CreateFlags.FullTextSearch3) != CreateFlags.None;
      bool flag2 = (createFlags & CreateFlags.FullTextSearch4) != CreateFlags.None;
      string str1 = !flag1 && !flag2 ? string.Empty : "virtual ";
      string str2 = !flag1 ? (!flag2 ? string.Empty : "using fts4 ") : "using fts3 ";
      int num = this.Execute("create " + str1 + "table if not exists \"" + mapping.TableName + "\" " + str2 + "(\n" + string.Join(",\n", ((IEnumerable<TableMapping.Column>) mapping.Columns).Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (p => Orm.SqlDecl(p, this.StoreDateTimeAsTicks))).ToArray<string>()) + ")");
      if (num == 0)
        this.MigrateTable(mapping);
      Dictionary<string, SQLiteConnection.IndexInfo> dictionary = new Dictionary<string, SQLiteConnection.IndexInfo>();
      foreach (TableMapping.Column column in mapping.Columns)
      {
        foreach (IndexedAttribute index in column.Indices)
        {
          string key = index.Name ?? mapping.TableName + "_" + column.Name;
          SQLiteConnection.IndexInfo indexInfo1;
          if (!dictionary.TryGetValue(key, out indexInfo1))
          {
            indexInfo1 = new SQLiteConnection.IndexInfo();
            SQLiteConnection.IndexInfo indexInfo2 = indexInfo1;
            indexInfo2.IndexName = key;
            indexInfo2.TableName = mapping.TableName;
            indexInfo2.Unique = index.Unique;
            indexInfo2.Columns = new List<SQLiteConnection.IndexedColumn>();
            indexInfo1 = indexInfo2;
            dictionary.Add(key, indexInfo1);
          }
          if (index.Unique != indexInfo1.Unique)
            throw new Exception("All the columns in an index must have the same value for their Unique property");
          indexInfo1.Columns.Add(new SQLiteConnection.IndexedColumn()
          {
            Order = index.Order,
            ColumnName = column.Name
          });
        }
      }
      foreach (string key in dictionary.Keys)
      {
        SQLiteConnection.IndexInfo indexInfo = dictionary[key];
        string[] array = indexInfo.Columns.OrderBy<SQLiteConnection.IndexedColumn, int>((Func<SQLiteConnection.IndexedColumn, int>) (i => i.Order)).Select<SQLiteConnection.IndexedColumn, string>((Func<SQLiteConnection.IndexedColumn, string>) (i => i.ColumnName)).ToArray<string>();
        num += this.CreateIndex(key, indexInfo.TableName, array, indexInfo.Unique);
      }
      return num;
    }

    public int CreateIndex(string indexName, string tableName, string[] columnNames, bool unique = false) => this.Execute(string.Format("create {2} index if not exists \"{3}\" on \"{0}\"(\"{1}\")", (object) tableName, (object) string.Join("\", \"", columnNames), !unique ? (object) string.Empty : (object) nameof (unique), (object) indexName));

    public int CreateIndex(string indexName, string tableName, string columnName, bool unique = false) => this.CreateIndex(indexName, tableName, new string[1]
    {
      columnName
    }, (unique ? 1 : 0) != 0);

    public int CreateIndex(string tableName, string columnName, bool unique = false) => this.CreateIndex(tableName + "_" + columnName, tableName, columnName, unique);

    public int CreateIndex(string tableName, string[] columnNames, bool unique = false) => this.CreateIndex(tableName + "_" + string.Join("_", columnNames), tableName, columnNames, unique);

    public void CreateIndex<T>(Expression<Func<T, object>> property, bool unique = false)
    {
      string name1 = ((property.Body.NodeType != ExpressionType.Convert ? property.Body as MemberExpression : ((UnaryExpression) property.Body).Operand as MemberExpression).Member as PropertyInfo ?? throw new ArgumentException("The lambda expression 'property' should point to a valid Property")).Name;
      TableMapping mapping = this.GetMapping<T>();
      string name2 = mapping.FindColumnWithPropertyName(name1).Name;
      this.CreateIndex(mapping.TableName, name2, unique);
    }

    public List<SQLiteConnection.ColumnInfo> GetTableInfo(string tableName) => this.Query<SQLiteConnection.ColumnInfo>("pragma table_info(\"" + tableName + "\")");

    private void MigrateTable(TableMapping map)
    {
      List<SQLiteConnection.ColumnInfo> tableInfo = this.GetTableInfo(map.TableName);
      List<TableMapping.Column> columnList = new List<TableMapping.Column>();
      foreach (TableMapping.Column column in map.Columns)
      {
        bool flag = false;
        foreach (SQLiteConnection.ColumnInfo columnInfo in tableInfo)
        {
          flag = string.Compare(column.Name, columnInfo.Name, StringComparison.OrdinalIgnoreCase) == 0;
          if (flag)
            break;
        }
        if (!flag)
          columnList.Add(column);
      }
      foreach (TableMapping.Column p in columnList)
        this.Execute("alter table \"" + map.TableName + "\" add column " + Orm.SqlDecl(p, this.StoreDateTimeAsTicks));
    }

    protected virtual SQLiteCommand NewCommand() => new SQLiteCommand(this);

    public SQLiteCommand CreateCommand(string cmdText, params object[] ps)
    {
      if (!this._open)
        throw SQLiteException.New(SQLite3.Result.Error, "Cannot create commands from unopened database");
      SQLiteCommand sqLiteCommand = this.NewCommand();
      sqLiteCommand.CommandText = cmdText;
      foreach (object p in ps)
        sqLiteCommand.Bind(p);
      return sqLiteCommand;
    }

    public int Execute(string query, params object[] args)
    {
      SQLiteCommand command = this.CreateCommand(query, args);
      if (this.TimeExecution)
      {
        if (this._sw == null)
          this._sw = new Stopwatch();
        this._sw.Reset();
        this._sw.Start();
      }
      int num = command.ExecuteNonQuery();
      if (this.TimeExecution)
      {
        this._sw.Stop();
        this._elapsedMilliseconds += this._sw.ElapsedMilliseconds;
      }
      return num;
    }

    public T ExecuteScalar<T>(string query, params object[] args)
    {
      SQLiteCommand command = this.CreateCommand(query, args);
      if (this.TimeExecution)
      {
        if (this._sw == null)
          this._sw = new Stopwatch();
        this._sw.Reset();
        this._sw.Start();
      }
      T obj = command.ExecuteScalar<T>();
      if (this.TimeExecution)
      {
        this._sw.Stop();
        this._elapsedMilliseconds += this._sw.ElapsedMilliseconds;
      }
      return obj;
    }

    public List<T> Query<T>(string query, params object[] args) where T : new() => this.CreateCommand(query, args).ExecuteQuery<T>();

    public IEnumerable<T> DeferredQuery<T>(string query, params object[] args) where T : new() => this.CreateCommand(query, args).ExecuteDeferredQuery<T>();

    public List<object> Query(TableMapping map, string query, params object[] args) => this.CreateCommand(query, args).ExecuteQuery<object>(map);

    public IEnumerable<object> DeferredQuery(
      TableMapping map,
      string query,
      params object[] args)
    {
      return this.CreateCommand(query, args).ExecuteDeferredQuery<object>(map);
    }

    public TableQuery<T> Table<T>() where T : new() => new TableQuery<T>(this);

    public T Get<T>(object pk) where T : new() => this.Query<T>(this.GetMapping(typeof (T)).GetByPrimaryKeySql, pk).First<T>();

    public T Get<T>(Expression<Func<T, bool>> predicate) where T : new() => this.Table<T>().Where(predicate).First();

    public T Find<T>(object pk) where T : new() => this.Query<T>(this.GetMapping(typeof (T)).GetByPrimaryKeySql, pk).FirstOrDefault<T>();

    public object Find(object pk, TableMapping map) => this.Query(map, map.GetByPrimaryKeySql, pk).FirstOrDefault<object>();

    public T Find<T>(Expression<Func<T, bool>> predicate) where T : new() => this.Table<T>().Where(predicate).FirstOrDefault();

    public T FindWithQuery<T>(string query, params object[] args) where T : new() => this.Query<T>(query, args).FirstOrDefault<T>();

    public bool IsInTransaction => this._transactionDepth > 0;

    public void BeginTransaction()
    {
      if (Interlocked.CompareExchange(ref this._transactionDepth, 1, 0) != 0)
        throw new InvalidOperationException("Cannot begin a transaction while already in a transaction.");
      try
      {
        this.Execute("begin transaction");
      }
      catch (Exception ex)
      {
        if (ex is SQLiteException sqLiteException1)
        {
          switch (sqLiteException1.Result)
          {
            case SQLite3.Result.Busy:
            case SQLite3.Result.NoMem:
            case SQLite3.Result.Interrupt:
            case SQLite3.Result.IOError:
            case SQLite3.Result.Full:
              this.RollbackTo((string) null, true);
              break;
          }
        }
        else
          Interlocked.Decrement(ref this._transactionDepth);
        throw;
      }
    }

    public string SaveTransactionPoint()
    {
      int num = Interlocked.Increment(ref this._transactionDepth) - 1;
      string str = "S" + (object) this._rand.Next((int) short.MaxValue) + "D" + (object) num;
      try
      {
        this.Execute("savepoint " + str);
      }
      catch (Exception ex)
      {
        if (ex is SQLiteException sqLiteException1)
        {
          switch (sqLiteException1.Result)
          {
            case SQLite3.Result.Busy:
            case SQLite3.Result.NoMem:
            case SQLite3.Result.Interrupt:
            case SQLite3.Result.IOError:
            case SQLite3.Result.Full:
              this.RollbackTo((string) null, true);
              break;
          }
        }
        else
          Interlocked.Decrement(ref this._transactionDepth);
        throw;
      }
      return str;
    }

    public void Rollback() => this.RollbackTo((string) null, false);

    public void RollbackTo(string savepoint) => this.RollbackTo(savepoint, false);

    private void RollbackTo(string savepoint, bool noThrow)
    {
      try
      {
        if (string.IsNullOrEmpty(savepoint))
        {
          if (Interlocked.Exchange(ref this._transactionDepth, 0) <= 0)
            return;
          this.Execute("rollback");
        }
        else
          this.DoSavePointExecute(savepoint, "rollback to ");
      }
      catch (SQLiteException ex)
      {
        if (noThrow)
          return;
        throw;
      }
    }

    public void Release(string savepoint) => this.DoSavePointExecute(savepoint, "release ");

    private void DoSavePointExecute(string savepoint, string cmd)
    {
      int num = savepoint.IndexOf('D');
      int result;
      if (num < 2 || savepoint.Length <= num + 1 || !int.TryParse(savepoint.Substring(num + 1), out result) || 0 > result || result >= this._transactionDepth)
        throw new ArgumentException("savePoint is not valid, and should be the result of a call to SaveTransactionPoint.", "savePoint");
      Volatile.Write(ref this._transactionDepth, result);
      this.Execute(cmd + savepoint);
    }

    public void Commit()
    {
      if (Interlocked.Exchange(ref this._transactionDepth, 0) == 0)
        return;
      this.Execute("commit");
    }

    public void RunInTransaction(Action action)
    {
      try
      {
        string savepoint = this.SaveTransactionPoint();
        action();
        this.Release(savepoint);
      }
      catch (Exception ex)
      {
        this.Rollback();
        throw;
      }
    }

    public int InsertAll(IEnumerable objects, bool runInTransaction = true)
    {
      int c = 0;
      if (runInTransaction)
      {
        this.RunInTransaction((Action) (() =>
        {
          foreach (object obj in objects)
            c += this.Insert(obj);
        }));
      }
      else
      {
        foreach (object obj in objects)
          c += this.Insert(obj);
      }
      return c;
    }

    public int InsertAll(IEnumerable objects, string extra, bool runInTransaction = true)
    {
      int c = 0;
      if (runInTransaction)
      {
        this.RunInTransaction((Action) (() =>
        {
          foreach (object obj in objects)
            c += this.Insert(obj, extra);
        }));
      }
      else
      {
        foreach (object obj in objects)
          c += this.Insert(obj);
      }
      return c;
    }

    public int InsertAll(IEnumerable objects, Type objType, bool runInTransaction = true)
    {
      int c = 0;
      if (runInTransaction)
      {
        this.RunInTransaction((Action) (() =>
        {
          foreach (object obj in objects)
            c += this.Insert(obj, objType);
        }));
      }
      else
      {
        foreach (object obj in objects)
          c += this.Insert(obj, objType);
      }
      return c;
    }

    public int Insert(object obj) => obj == null ? 0 : this.Insert(obj, string.Empty, obj.GetType());

    public int InsertOrReplace(object obj) => obj == null ? 0 : this.Insert(obj, "OR REPLACE", obj.GetType());

    public int Insert(object obj, Type objType) => this.Insert(obj, string.Empty, objType);

    public int InsertOrReplace(object obj, Type objType) => this.Insert(obj, "OR REPLACE", objType);

    public int Insert(object obj, string extra) => obj == null ? 0 : this.Insert(obj, extra, obj.GetType());

    public int Insert(object obj, string extra, Type objType)
    {
      if (obj == null || (object) objType == null)
        return 0;
      TableMapping mapping = this.GetMapping(objType);
      TypeInfo typeInfo;
      if (mapping.PK != null && mapping.PK.IsAutoGuid)
      {
        for (; (object) objType != null; objType = typeInfo.BaseType)
        {
          typeInfo = objType.GetTypeInfo();
          PropertyInfo declaredProperty = typeInfo.GetDeclaredProperty(mapping.PK.PropertyName);
          if ((object) declaredProperty != null)
          {
            if (declaredProperty.GetValue(obj, (object[]) null).Equals((object) Guid.Empty))
            {
              declaredProperty.SetValue(obj, (object) Guid.NewGuid(), (object[]) null);
              break;
            }
            break;
          }
        }
      }
      TableMapping.Column[] columnArray = string.Compare(extra, "OR REPLACE", StringComparison.OrdinalIgnoreCase) != 0 ? mapping.InsertColumns : mapping.InsertOrReplaceColumns;
      object[] source = new object[columnArray.Length];
      for (int index = 0; index < source.Length; ++index)
        source[index] = columnArray[index].GetValue(obj);
      PreparedSqlLiteInsertCommand insertCommand = mapping.GetInsertCommand(this, extra);
      int num;
      lock ((object) insertCommand)
      {
        try
        {
          num = insertCommand.ExecuteNonQuery(source);
        }
        catch (SQLiteException ex)
        {
          if (SQLite3.ExtendedErrCode(this.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
            throw NotNullConstraintViolationException.New(ex.Result, ex.Message, mapping, obj);
          throw;
        }
        if (mapping.HasAutoIncPK)
        {
          long id = SQLite3.LastInsertRowid(this.Handle);
          mapping.SetAutoIncPK(obj, id);
        }
      }
      if (num > 0)
        this.OnTableChanged(mapping, NotifyTableChangedAction.Insert);
      return num;
    }

    public int Update(object obj) => obj == null ? 0 : this.Update(obj, obj.GetType());

    public int Update(object obj, Type objType)
    {
      if (obj == null || (object) objType == null)
        return 0;
      TableMapping mapping = this.GetMapping(objType);
      TableMapping.Column pk = mapping.PK;
      if (pk == null)
        throw new NotSupportedException("Cannot update " + mapping.TableName + ": it has no PK");
      IEnumerable<TableMapping.Column> source = ((IEnumerable<TableMapping.Column>) mapping.Columns).Where<TableMapping.Column>((Func<TableMapping.Column, bool>) (p => p != pk));
      List<object> objectList = new List<object>(source.Select<TableMapping.Column, object>((Func<TableMapping.Column, object>) (c => c.GetValue(obj))));
      objectList.Add(pk.GetValue(obj));
      string query = string.Format("update \"{0}\" set {1} where {2} = ? ", new object[3]
      {
        (object) mapping.TableName,
        (object) string.Join(",", source.Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (c => "\"" + c.Name + "\" = ? ")).ToArray<string>()),
        (object) pk.Name
      });
      int num;
      try
      {
        num = this.Execute(query, objectList.ToArray());
      }
      catch (SQLiteException ex)
      {
        if (ex.Result == SQLite3.Result.Constraint && SQLite3.ExtendedErrCode(this.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
          throw NotNullConstraintViolationException.New(ex, mapping, obj);
        throw ex;
      }
      if (num > 0)
        this.OnTableChanged(mapping, NotifyTableChangedAction.Update);
      return num;
    }

    public int UpdateAll(IEnumerable objects, bool runInTransaction = true)
    {
      int c = 0;
      if (runInTransaction)
      {
        this.RunInTransaction((Action) (() =>
        {
          foreach (object obj in objects)
            c += this.Update(obj);
        }));
      }
      else
      {
        foreach (object obj in objects)
          c += this.Update(obj);
      }
      return c;
    }

    public int Delete(object objectToDelete)
    {
      TableMapping mapping = this.GetMapping(objectToDelete.GetType());
      TableMapping.Column pk = mapping.PK;
      if (pk == null)
        throw new NotSupportedException("Cannot delete " + mapping.TableName + ": it has no PK");
      int num = this.Execute(string.Format("delete from \"{0}\" where \"{1}\" = ?", new object[2]
      {
        (object) mapping.TableName,
        (object) pk.Name
      }), pk.GetValue(objectToDelete));
      if (num > 0)
        this.OnTableChanged(mapping, NotifyTableChangedAction.Delete);
      return num;
    }

    public int Delete<T>(object primaryKey)
    {
      TableMapping mapping = this.GetMapping(typeof (T));
      int num = this.Execute(string.Format("delete from \"{0}\" where \"{1}\" = ?", new object[2]
      {
        (object) mapping.TableName,
        (object) (mapping.PK ?? throw new NotSupportedException("Cannot delete " + mapping.TableName + ": it has no PK")).Name
      }), primaryKey);
      if (num > 0)
        this.OnTableChanged(mapping, NotifyTableChangedAction.Delete);
      return num;
    }

    public int DeleteAll<T>()
    {
      TableMapping mapping = this.GetMapping(typeof (T));
      int num = this.Execute(string.Format("delete from \"{0}\"", new object[1]
      {
        (object) mapping.TableName
      }));
      if (num > 0)
        this.OnTableChanged(mapping, NotifyTableChangedAction.Delete);
      return num;
    }

    ~SQLiteConnection() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing) => this.Close();

    public void Close()
    {
      if (!this._open)
        return;
      if (this.Handle == SQLiteConnection.NullHandle)
        return;
      try
      {
        if (this._mappings != null)
        {
          foreach (TableMapping tableMapping in this._mappings.Values)
            tableMapping.Dispose();
        }
        SQLite3.Result r = SQLite3.Close(this.Handle);
        if (r != SQLite3.Result.OK)
        {
          string errmsg = SQLite3.GetErrmsg(this.Handle);
          throw SQLiteException.New(r, errmsg);
        }
      }
      finally
      {
        this.Handle = SQLiteConnection.NullHandle;
        this._open = false;
      }
    }

    private void OnTableChanged(TableMapping table, NotifyTableChangedAction action)
    {
      EventHandler<NotifyTableChangedEventArgs> tableChanged = this.TableChanged;
      if (tableChanged == null)
        return;
      tableChanged((object) this, new NotifyTableChangedEventArgs(table, action));
    }

    public event EventHandler<NotifyTableChangedEventArgs> TableChanged;

    private struct IndexedColumn
    {
      public int Order;
      public string ColumnName;
    }

    private struct IndexInfo
    {
      public string IndexName;
      public string TableName;
      public bool Unique;
      public List<SQLiteConnection.IndexedColumn> Columns;
    }

    public class ColumnInfo
    {
      [Column("name")]
      public string Name { get; set; }

      public int notnull { get; set; }

      public override string ToString() => this.Name;
    }
  }
}
