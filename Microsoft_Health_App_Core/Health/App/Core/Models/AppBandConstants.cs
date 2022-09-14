// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.AppBandConstants
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System;

namespace Microsoft.Health.App.Core.Models
{
  public class AppBandConstants : IDynamicBandConstants
  {
    public static readonly IDynamicBandConstants Cargo = (IDynamicBandConstants) new AppBandConstants(BandClass.Cargo);
    public static readonly IDynamicBandConstants Envoy = (IDynamicBandConstants) new AppBandConstants(BandClass.Envoy);

    private AppBandConstants(BandClass bandClass) => this.BandClass = bandClass;

    public BandClass BandClass { get; }

    public ushort MeTileWidth => throw new NotImplementedException();

    public ushort MeTileHeight => throw new NotImplementedException();

    public ushort TileIconPreferredSize => throw new NotImplementedException();

    public ushort BadgeIconPreferredSize => throw new NotImplementedException();

    public ushort NotificiationIconPreferredSize => throw new NotImplementedException();

    public int MaxIconsPerTile => throw new NotImplementedException();
  }
}
