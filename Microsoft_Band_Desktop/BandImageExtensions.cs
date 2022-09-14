// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.BandImageExtensions
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using Microsoft.Band.Personalization;
using System;
using System.Windows.Media.Imaging;

namespace Microsoft.Band
{
  public static class BandImageExtensions
  {
    public static WriteableBitmap ToWriteableBitmap(this BandImage image)
    {
      try
      {
        WriteableBitmap emptyWriteableBitmap = WriteableBitmapUtils.GetEmptyWriteableBitmap(image.Width, image.Height);
        emptyWriteableBitmap.SetPixelArrayBgr565(image.PixelData);
        return emptyWriteableBitmap;
      }
      catch (Exception ex)
      {
        throw new BandException("Failed to decode bitmap", ex);
      }
    }
  }
}
