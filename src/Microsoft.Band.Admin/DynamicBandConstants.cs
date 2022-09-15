// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.DynamicBandConstants
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Runtime.CompilerServices;

namespace Microsoft.Band.Admin
{
  internal class DynamicBandConstants : BandTypeConstants, IDynamicBandConstants
  {
    internal static readonly DynamicBandConstants CargoConstants = new DynamicBandConstants(BandClass.Cargo);
    internal static readonly DynamicBandConstants EnvoyConstants = new DynamicBandConstants(BandClass.Envoy);

    private DynamicBandConstants(BandClass bandClass)
      : base(bandClass.ToBandType())
    {
      this.BandClass = bandClass;
    }

    public BandClass BandClass { get; private set; }

        [SpecialName]
        ushort IDynamicBandConstants.MeTileWidth 
        { 
            get => this.MeTileWidth; 
        }

        [SpecialName]
        ushort IDynamicBandConstants.MeTileHeight
        { 
            get => this.MeTileHeight;
        }

        [SpecialName]
        ushort IDynamicBandConstants.TileIconPreferredSize 
        { 
            get => this.TileIconPreferredSize;
        }

        [SpecialName]
        ushort IDynamicBandConstants.BadgeIconPreferredSize 
        { 
            get => this.BadgeIconPreferredSize;
        }

        [SpecialName]
        ushort IDynamicBandConstants.NotificiationIconPreferredSize
        { 
            get => this.NotificiationIconPreferredSize;
        }

        [SpecialName]
        int IDynamicBandConstants.MaxIconsPerTile 
        { 
            get => this.MaxIconsPerTile;
        }

    }
}
