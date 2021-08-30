// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Types.BandIconRleCodecProxy
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using Microsoft.Band.Tiles;

namespace Microsoft.Band.Admin.Types
{
  public static class BandIconRleCodecProxy
  {
    public static byte[] EncodeTileIconRle(BandIcon icon) => BandIconRleCodec.EncodeTileIconRleToArray(icon);

    public static BandIcon DecodeTileIconRle(byte[] rleArray) => BandIconRleCodec.DecodeTileIconRle(rleArray);
  }
}
