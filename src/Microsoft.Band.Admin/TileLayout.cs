// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TileLayout
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

namespace Microsoft.Band.Admin
{
  public sealed class TileLayout
  {
    public TileLayout(byte[] layoutBlob) => this.layoutBlob = layoutBlob;

    public byte[] layoutBlob { get; set; }
  }
}
