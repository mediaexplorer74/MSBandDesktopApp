// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Streaming.LogEntryUpdatedEventArgs
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin.Streaming
{
  public sealed class LogEntryUpdatedEventArgs : EventArgs
  {
    private LogEntryUpdatedEventArgs()
    {
    }

    public byte EntryType { get; private set; }

    public byte[] Data { get; private set; }

    internal static LogEntryUpdatedEventArgs DeserializeFromBand(
      ICargoReader reader)
    {
      byte num1 = reader.ReadByte();
      byte num2 = reader.ReadByte();
      byte[] numArray = reader.ReadExact((int) num2);
      return new LogEntryUpdatedEventArgs()
      {
        EntryType = num1,
        Data = numArray
      };
    }
  }
}
