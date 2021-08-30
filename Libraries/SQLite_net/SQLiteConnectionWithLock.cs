// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteConnectionWithLock
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;
using System.Threading;

namespace SQLite
{
  internal class SQLiteConnectionWithLock : SQLiteConnection
  {
    private readonly object _lockPoint = new object();

    public SQLiteConnectionWithLock(
      SQLiteConnectionString connectionString,
      SQLiteOpenFlags openFlags)
      : base(connectionString.DatabasePath, openFlags, connectionString.StoreDateTimeAsTicks)
    {
    }

    public IDisposable Lock() => (IDisposable) new SQLiteConnectionWithLock.LockWrapper(this._lockPoint);

    private class LockWrapper : IDisposable
    {
      private object _lockPoint;

      public LockWrapper(object lockPoint)
      {
        this._lockPoint = lockPoint;
        Monitor.Enter(this._lockPoint);
      }

      public void Dispose() => Monitor.Exit(this._lockPoint);
    }
  }
}
