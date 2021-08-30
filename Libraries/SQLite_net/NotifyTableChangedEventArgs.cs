// Decompiled with JetBrains decompiler
// Type: SQLite.NotifyTableChangedEventArgs
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;

namespace SQLite
{
  public class NotifyTableChangedEventArgs : EventArgs
  {
    public NotifyTableChangedEventArgs(TableMapping table, NotifyTableChangedAction action)
    {
      this.Table = table;
      this.Action = action;
    }

    public TableMapping Table { get; private set; }

    public NotifyTableChangedAction Action { get; private set; }
  }
}
