// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.TileUpdateInfo
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Band
{
  public class TileUpdateInfo
  {
    public TileUpdateInfo()
    {
    }

    public TileUpdateInfo(Guid tileId) => this.TileId = tileId;

    public Guid TileId { get; set; }

    public ICollection<TilePageUpdateInfo> PageUdpates { get; } = (ICollection<TilePageUpdateInfo>) new List<TilePageUpdateInfo>();
  }
}
