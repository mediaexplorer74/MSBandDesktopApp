// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.BandColor
// Assembly: Microsoft.Band, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: AFCBFE03-E2CF-481D-86F4-92C60C36D26A
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band.dll

namespace Microsoft.Band
{
  public struct BandColor
  {
    private byte r;
    private byte g;
    private byte b;

    public BandColor(byte red, byte green, byte blue)
    {
      this.r = red;
      this.g = green;
      this.b = blue;
    }

    internal BandColor(uint rgb)
    {
      this.r = (byte) (rgb >> 16 & (uint) byte.MaxValue);
      this.g = (byte) (rgb >> 8 & (uint) byte.MaxValue);
      this.b = (byte) (rgb & (uint) byte.MaxValue);
    }

    public byte R => this.r;

    public byte G => this.g;

    public byte B => this.b;

    internal uint ToRgb(byte alpha = 255) => (uint) ((int) alpha << 24 | (int) this.R << 16 | (int) this.G << 8) | (uint) this.B;
  }
}
