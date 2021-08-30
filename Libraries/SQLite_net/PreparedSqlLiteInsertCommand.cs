// Decompiled with JetBrains decompiler
// Type: SQLite.PreparedSqlLiteInsertCommand
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using SQLitePCL;
using System;

namespace SQLite
{
  public class PreparedSqlLiteInsertCommand : IDisposable
  {
    internal static readonly sqlite3_stmt NullStatement;

    internal PreparedSqlLiteInsertCommand(SQLiteConnection conn) => this.Connection = conn;

    public bool Initialized { get; set; }

    protected SQLiteConnection Connection { get; set; }

    public string CommandText { get; set; }

    protected sqlite3_stmt Statement { get; set; }

    public int ExecuteNonQuery(object[] source)
    {
      if (!this.Connection.Trace)
        ;
      if (!this.Initialized)
      {
        this.Statement = this.Prepare();
        this.Initialized = true;
      }
      if (source != null)
      {
        for (int index = 0; index < source.Length; ++index)
          SQLiteCommand.BindParameter(this.Statement, index + 1, source[index], this.Connection.StoreDateTimeAsTicks);
      }
      SQLite3.Result r = SQLite3.Step(this.Statement);
      switch (r)
      {
        case SQLite3.Result.Error:
          string errmsg = SQLite3.GetErrmsg(this.Connection.Handle);
          int num1 = (int) SQLite3.Reset(this.Statement);
          throw SQLiteException.New(r, errmsg);
        case SQLite3.Result.Constraint:
          if (SQLite3.ExtendedErrCode(this.Connection.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
          {
            int num2 = (int) SQLite3.Reset(this.Statement);
            throw NotNullConstraintViolationException.New(r, SQLite3.GetErrmsg(this.Connection.Handle));
          }
          break;
        case SQLite3.Result.Done:
          int num3 = SQLite3.Changes(this.Connection.Handle);
          int num4 = (int) SQLite3.Reset(this.Statement);
          return num3;
      }
      int num5 = (int) SQLite3.Reset(this.Statement);
      throw SQLiteException.New(r, r.ToString());
    }

    protected virtual sqlite3_stmt Prepare() => SQLite3.Prepare2(this.Connection.Handle, this.CommandText);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this.Statement == PreparedSqlLiteInsertCommand.NullStatement)
        return;
      try
      {
        int num = (int) SQLite3.Finalize(this.Statement);
      }
      finally
      {
        this.Statement = PreparedSqlLiteInsertCommand.NullStatement;
        this.Connection = (SQLiteConnection) null;
      }
    }

    ~PreparedSqlLiteInsertCommand() => this.Dispose(false);
  }
}
