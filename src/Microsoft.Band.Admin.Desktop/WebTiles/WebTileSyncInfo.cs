// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileSyncInfo
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin.WebTiles
{
  [DataContract]
  internal class WebTileSyncInfo
  {
    private DateTimeOffset lastSyncTime;

    public DateTimeOffset LastSyncTime
    {
      get => this.lastSyncTime;
      set => this.lastSyncTime = value;
    }

    [DataMember]
    private string LastSyncTimeSerialized
    {
      get => this.lastSyncTime.ToString("o");
      set
      {
        DateTimeOffset result;
        if (DateTimeOffset.TryParse(value, out result))
          this.lastSyncTime = result;
        else
          this.lastSyncTime = DateTimeOffset.MinValue;
      }
    }
  }
}
