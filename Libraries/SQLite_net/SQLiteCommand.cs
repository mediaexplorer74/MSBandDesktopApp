// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteCommand
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SQLite
{
  public class SQLiteCommand
  {
    private SQLiteConnection _conn;
    private List<SQLiteCommand.Binding> _bindings;
    internal static IntPtr NegativePointer = new IntPtr(-1);

    internal SQLiteCommand(SQLiteConnection conn)
    {
      this._conn = conn;
      this._bindings = new List<SQLiteCommand.Binding>();
      this.CommandText = string.Empty;
    }

    public string CommandText { get; set; }

    public int ExecuteNonQuery()
    {
      if (!this._conn.Trace)
        ;
      sqlite3_stmt stmt = this.Prepare();
      SQLite3.Result r = SQLite3.Step(stmt);
      this.Finalize(stmt);
      switch (r)
      {
        case SQLite3.Result.Error:
          string errmsg = SQLite3.GetErrmsg(this._conn.Handle);
          throw SQLiteException.New(r, errmsg);
        case SQLite3.Result.Constraint:
          if (SQLite3.ExtendedErrCode(this._conn.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
            throw NotNullConstraintViolationException.New(r, SQLite3.GetErrmsg(this._conn.Handle));
          break;
        case SQLite3.Result.Done:
          return SQLite3.Changes(this._conn.Handle);
      }
      throw SQLiteException.New(r, r.ToString());
    }

    public IEnumerable<T> ExecuteDeferredQuery<T>() => this.ExecuteDeferredQuery<T>(this._conn.GetMapping(typeof (T)));

    public List<T> ExecuteQuery<T>() => this.ExecuteDeferredQuery<T>(this._conn.GetMapping(typeof (T))).ToList<T>();

    public List<T> ExecuteQuery<T>(TableMapping map) => this.ExecuteDeferredQuery<T>(map).ToList<T>();

    protected virtual void OnInstanceCreated(object obj)
    {
    }

    [DebuggerHidden]
    public IEnumerable<T> ExecuteDeferredQuery<T>(TableMapping map)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SQLiteCommand.\u003CExecuteDeferredQuery\u003Ec__Iterator0<T> deferredQueryCIterator0 = new SQLiteCommand.\u003CExecuteDeferredQuery\u003Ec__Iterator0<T>()
      {
        map = map,
        \u0024this = this
      };
      // ISSUE: reference to a compiler-generated field
      deferredQueryCIterator0.\u0024PC = -2;
      return (IEnumerable<T>) deferredQueryCIterator0;
    }

    public T ExecuteScalar<T>()
    {
      if (!this._conn.Trace)
        ;
      T obj = default (T);
      sqlite3_stmt stmt = this.Prepare();
      try
      {
        SQLite3.Result r = SQLite3.Step(stmt);
        switch (r)
        {
          case SQLite3.Result.Row:
            SQLite3.ColType type = SQLite3.ColumnType(stmt, 0);
            obj = (T) this.ReadCol(stmt, 0, type, typeof (T));
            break;
          case SQLite3.Result.Done:
            break;
          default:
            throw SQLiteException.New(r, SQLite3.GetErrmsg(this._conn.Handle));
        }
      }
      finally
      {
        this.Finalize(stmt);
      }
      return obj;
    }

    public void Bind(string name, object val) => this._bindings.Add(new SQLiteCommand.Binding()
    {
      Name = name,
      Value = val
    });

    public void Bind(object val) => this.Bind((string) null, val);

    public override string ToString()
    {
      string[] strArray = new string[1 + this._bindings.Count];
      strArray[0] = this.CommandText;
      int index = 1;
      foreach (SQLiteCommand.Binding binding in this._bindings)
      {
        strArray[index] = string.Format("  {0}: {1}", new object[2]
        {
          (object) (index - 1),
          binding.Value
        });
        ++index;
      }
      return string.Join(Environment.NewLine, strArray);
    }

    private sqlite3_stmt Prepare()
    {
      sqlite3_stmt stmt = SQLite3.Prepare2(this._conn.Handle, this.CommandText);
      this.BindAll(stmt);
      return stmt;
    }

    private void Finalize(sqlite3_stmt stmt)
    {
      int num = (int) SQLite3.Finalize(stmt);
    }

    private void BindAll(sqlite3_stmt stmt)
    {
      int num = 1;
      foreach (SQLiteCommand.Binding binding in this._bindings)
      {
        binding.Index = binding.Name == null ? num++ : SQLite3.BindParameterIndex(stmt, binding.Name);
        SQLiteCommand.BindParameter(stmt, binding.Index, binding.Value, this._conn.StoreDateTimeAsTicks);
      }
    }

    internal static void BindParameter(
      sqlite3_stmt stmt,
      int index,
      object value,
      bool storeDateTimeAsTicks)
    {
      switch (value)
      {
        case null:
          SQLite3.BindNull(stmt, index);
          break;
        case int val:
          SQLite3.BindInt(stmt, index, val);
          break;
        case string _:
          SQLite3.BindText(stmt, index, (string) value, -1, SQLiteCommand.NegativePointer);
          break;
        case byte _:
        case ushort _:
        case sbyte _:
        case short _:
          SQLite3.BindInt(stmt, index, Convert.ToInt32(value));
          break;
        case bool flag:
          SQLite3.BindInt(stmt, index, !flag ? 0 : 1);
          break;
        case uint _:
        case long _:
          SQLite3.BindInt64(stmt, index, Convert.ToInt64(value));
          break;
        case float _:
        case double _:
        case Decimal _:
          SQLite3.BindDouble(stmt, index, Convert.ToDouble(value));
          break;
        case TimeSpan timeSpan:
          SQLite3.BindInt64(stmt, index, timeSpan.Ticks);
          break;
        case DateTime _:
          if (storeDateTimeAsTicks)
          {
            SQLite3.BindInt64(stmt, index, ((DateTime) value).Ticks);
            break;
          }
          SQLite3.BindText(stmt, index, ((DateTime) value).ToString("yyyy-MM-dd HH:mm:ss"), -1, SQLiteCommand.NegativePointer);
          break;
        case DateTimeOffset dateTimeOffset:
          SQLite3.BindInt64(stmt, index, dateTimeOffset.UtcTicks);
          break;
        default:
          if (value.GetType().GetTypeInfo().IsEnum)
          {
            SQLite3.BindInt(stmt, index, Convert.ToInt32(value));
            break;
          }
          switch (value)
          {
            case byte[] _:
              SQLite3.BindBlob(stmt, index, (byte[]) value, ((byte[]) value).Length, SQLiteCommand.NegativePointer);
              return;
            case Guid guid2:
              SQLite3.BindText(stmt, index, guid2.ToString(), 72, SQLiteCommand.NegativePointer);
              return;
            default:
              throw new NotSupportedException("Cannot store type: " + (object) value.GetType());
          }
      }
    }

    private object ReadCol(sqlite3_stmt stmt, int index, SQLite3.ColType type, Type clrType)
    {
      if (type == SQLite3.ColType.Null)
        return (object) null;
      if ((object) clrType == (object) typeof (string))
        return (object) SQLite3.ColumnString(stmt, index);
      if ((object) clrType == (object) typeof (int))
        return (object) SQLite3.ColumnInt(stmt, index);
      if ((object) clrType == (object) typeof (bool))
        return (object) (SQLite3.ColumnInt(stmt, index) == 1);
      if ((object) clrType == (object) typeof (double))
        return (object) SQLite3.ColumnDouble(stmt, index);
      if ((object) clrType == (object) typeof (float))
        return (object) (float) SQLite3.ColumnDouble(stmt, index);
      if ((object) clrType == (object) typeof (TimeSpan))
        return (object) new TimeSpan(SQLite3.ColumnInt64(stmt, index));
      if ((object) clrType == (object) typeof (DateTime))
        return this._conn.StoreDateTimeAsTicks ? (object) new DateTime(SQLite3.ColumnInt64(stmt, index)) : (object) DateTime.Parse(SQLite3.ColumnString(stmt, index));
      if ((object) clrType == (object) typeof (DateTimeOffset))
        return (object) new DateTimeOffset(SQLite3.ColumnInt64(stmt, index), TimeSpan.Zero);
      if (clrType.GetTypeInfo().IsEnum)
        return (object) SQLite3.ColumnInt(stmt, index);
      if ((object) clrType == (object) typeof (long))
        return (object) SQLite3.ColumnInt64(stmt, index);
      if ((object) clrType == (object) typeof (uint))
        return (object) (uint) SQLite3.ColumnInt64(stmt, index);
      if ((object) clrType == (object) typeof (Decimal))
        return (object) (Decimal) SQLite3.ColumnDouble(stmt, index);
      if ((object) clrType == (object) typeof (byte))
        return (object) (byte) SQLite3.ColumnInt(stmt, index);
      if ((object) clrType == (object) typeof (ushort))
        return (object) (ushort) SQLite3.ColumnInt(stmt, index);
      if ((object) clrType == (object) typeof (short))
        return (object) (short) SQLite3.ColumnInt(stmt, index);
      if ((object) clrType == (object) typeof (sbyte))
        return (object) (sbyte) SQLite3.ColumnInt(stmt, index);
      if ((object) clrType == (object) typeof (byte[]))
        return (object) SQLite3.ColumnByteArray(stmt, index);
      if ((object) clrType == (object) typeof (Guid))
        return (object) new Guid(SQLite3.ColumnString(stmt, index));
      throw new NotSupportedException("Don't know how to read " + (object) clrType);
    }

    private class Binding
    {
      public string Name { get; set; }

      public object Value { get; set; }

      public int Index { get; set; }
    }
  }
}
