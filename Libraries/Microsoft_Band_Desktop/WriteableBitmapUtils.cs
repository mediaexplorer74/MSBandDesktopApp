// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.WriteableBitmapUtils
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Band
{
  internal static class WriteableBitmapUtils
  {
    public static WriteableBitmap GetEmptyWriteableBitmap(
      int pixelWidth,
      int pixelHeight)
    {
      return new WriteableBitmap(pixelWidth, pixelHeight, 96.0, 96.0, PixelFormats.Pbgra32, (BitmapPalette) null);
    }
  }
}
