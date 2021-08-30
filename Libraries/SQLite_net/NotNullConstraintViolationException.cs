// Decompiled with JetBrains decompiler
// Type: SQLite.NotNullConstraintViolationException
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLite
{
  public class NotNullConstraintViolationException : SQLiteException
  {
    protected NotNullConstraintViolationException(SQLite3.Result r, string message)
      : this(r, message, (TableMapping) null, (object) null)
    {
    }

    protected NotNullConstraintViolationException(
      SQLite3.Result r,
      string message,
      TableMapping mapping,
      object obj)
      : base(r, message)
    {
      if (mapping == null || obj == null)
        return;
      this.Columns = ((IEnumerable<TableMapping.Column>) mapping.Columns).Where<TableMapping.Column>((Func<TableMapping.Column, bool>) (c => !c.IsNullable && c.GetValue(obj) == null));
    }

    public IEnumerable<TableMapping.Column> Columns { get; protected set; }

    public static NotNullConstraintViolationException New(
      SQLite3.Result r,
      string message)
    {
      return new NotNullConstraintViolationException(r, message);
    }

    public static NotNullConstraintViolationException New(
      SQLite3.Result r,
      string message,
      TableMapping mapping,
      object obj)
    {
      return new NotNullConstraintViolationException(r, message, mapping, obj);
    }

    public static NotNullConstraintViolationException New(
      SQLiteException exception,
      TableMapping mapping,
      object obj)
    {
      return new NotNullConstraintViolationException(exception.Result, exception.Message, mapping, obj);
    }
  }
}
