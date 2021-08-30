// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ArgbColor32
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core
{
  public class ArgbColor32
  {
    public ArgbColor32(uint color) => this.Color = color;

    public ArgbColor32(byte a, byte r, byte g, byte b) => this.Color = (uint) ((ulong) ((int) a << 24 | (int) r << 16 | (int) g << 8 | (int) b) & (ulong) uint.MaxValue);

    public uint Color { get; private set; }

    public byte A => (byte) ((this.Color & 4278190080U) >> 24);

    public byte R => (byte) ((this.Color & 16711680U) >> 16);

    public byte G => (byte) ((this.Color & 65280U) >> 8);

    public byte B => (byte) (this.Color & (uint) byte.MaxValue);

    public override string ToString() => string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (object) this.A, (object) this.R, (object) this.G, (object) this.B);

    public string ToRgbString() => string.Format("#{0:X2}{1:X2}{2:X2}", new object[3]
    {
      (object) this.R,
      (object) this.G,
      (object) this.B
    });
  }
}
