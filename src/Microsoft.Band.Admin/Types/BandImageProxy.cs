// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Types.BandImageProxy
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using Microsoft.Band.Personalization;

namespace Microsoft.Band.Admin.Types
{
  public class BandImageProxy : BandImage
  {
    public BandImageProxy(int width, int height, byte[] pixelData)
      : base(width, height, pixelData)
    {
    }

    public BandImageProxy(BandImage bandImage)
      : base(bandImage.Width, bandImage.Height, bandImage.PixelData)
    {
    }

    public new byte[] PixelData => base.PixelData;
  }
}
