// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Tiles.Pages.Margins
// Assembly: Microsoft.Band, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: AFCBFE03-E2CF-481D-86F4-92C60C36D26A
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band.dll

namespace Microsoft.Band.Tiles.Pages
{
  public struct Margins
  {
    private readonly short left;
    private readonly short top;
    private readonly short right;
    private readonly short bottom;

    public Margins(short left, short top, short right, short bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    public short Left => this.left;

    public short Top => this.top;

    public short Right => this.right;

    public short Bottom => this.bottom;

    internal void SerializeToBand(ICargoWriter writer)
    {
      writer.WriteInt16(this.Left);
      writer.WriteInt16(this.Top);
      writer.WriteInt16(this.Right);
      writer.WriteInt16(this.Bottom);
    }

    internal static Margins DeserializeFromBand(ICargoReader reader) => new Margins(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
  }
}
