// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.BandIconExtensions
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using Microsoft.Band.Tiles;
using System;
using System.Windows.Media.Imaging;

namespace Microsoft.Band
{
  public static class BandIconExtensions
  {
    public static WriteableBitmap ToWriteableBitmap(this BandIcon icon)
    {
      try
      {
        WriteableBitmap emptyWriteableBitmap = WriteableBitmapUtils.GetEmptyWriteableBitmap(icon.Width, icon.Height);
        emptyWriteableBitmap.SetPixelArrayAlpha4(icon.IconData, icon.Width * icon.Height * 4);
        return emptyWriteableBitmap;
      }
      catch (Exception ex)
      {
        throw new BandException("Failed to decode bitmap", ex);
      }
    }
  }
}
