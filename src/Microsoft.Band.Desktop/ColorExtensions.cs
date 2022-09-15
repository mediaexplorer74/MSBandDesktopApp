// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.ColorExtensions
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System.Windows.Media;

namespace Microsoft.Band
{
  public static class ColorExtensions
  {
    public static BandColor ToBandColor(this Color color) => new BandColor(color.R, color.G, color.B);

    public static Color ToColor(this BandColor color) => Color.FromArgb(byte.MaxValue, color.R, color.G, color.B);
  }
}
