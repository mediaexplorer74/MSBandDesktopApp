// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.WriteableBitmapExtensions
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using Microsoft.Band.Personalization;
using Microsoft.Band.Tiles;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Band
{
  public static class WriteableBitmapExtensions
  {
    public static BandImage ToBandImage(this WriteableBitmap bitmap)
    {
      if (bitmap == null)
        throw new ArgumentException(nameof (bitmap));
      try
      {
        byte[] pixelArrayBgr565 = bitmap.GetPixelArrayBgr565();
        return new BandImage(bitmap.PixelWidth, bitmap.PixelHeight, pixelArrayBgr565);
      }
      catch (Exception ex)
      {
        throw new BandException("Failed to decode bitmap", ex);
      }
    }

    public static BandIcon ToBandIcon(this WriteableBitmap bitmap)
    {
      if (bitmap == null)
        throw new ArgumentException();
      byte[] pixelArrayAlpha4;
      try
      {
        pixelArrayAlpha4 = bitmap.GetPixelArrayAlpha4();
      }
      catch (Exception ex)
      {
        throw new BandException("Failed to decode bitmap", ex);
      }
      return new BandIcon(bitmap.PixelWidth, bitmap.PixelHeight, pixelArrayAlpha4);
    }

    private static byte[] GetPixelArray(this WriteableBitmap bitmap)
    {
      if (bitmap.Format != PixelFormats.Pbgra32)
        throw new ArgumentException("Image must be BGRA32 format");
      byte[] numArray = new byte[bitmap.PixelWidth * bitmap.PixelHeight * 4];
      bitmap.CopyPixels((Array) numArray, bitmap.PixelWidth * 4, 0);
      return numArray;
    }

    internal static byte[] GetPixelArrayBgr565(this WriteableBitmap bitmap)
    {
      byte[] pixelArray = bitmap.GetPixelArray();
      using (Bgr565Pbgra32ConversionStream conversionStream = new Bgr565Pbgra32ConversionStream(bitmap.PixelWidth * bitmap.PixelHeight * 4))
      {
        conversionStream.Write(pixelArray, 0, pixelArray.Length);
        return conversionStream.Bgr565Array;
      }
    }

    internal static byte[] GetPixelArrayAlpha4(this WriteableBitmap bitmap)
    {
      byte[] pixelArray = bitmap.GetPixelArray();
      using (Alpha4Pbgra32ConversionStream conversionStream = new Alpha4Pbgra32ConversionStream(bitmap.PixelWidth * bitmap.PixelHeight * 4))
      {
        conversionStream.Write(pixelArray, 0, pixelArray.Length);
        return conversionStream.Alpha4Array;
      }
    }

    internal static void SetPixelArrayBgr565(this WriteableBitmap bitmap, byte[] bgr565Array)
    {
      byte[] buffer = new byte[bgr565Array.Length * 2];
      using (MemoryStream memoryStream = new MemoryStream(buffer))
      {
        using (Bgr565Pbgra32ConversionStream conversionStream = new Bgr565Pbgra32ConversionStream(bgr565Array))
          conversionStream.CopyTo((Stream) memoryStream);
      }
      bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), (Array) buffer, bitmap.PixelWidth * 4, 0);
    }

    internal static void SetPixelArrayAlpha4(
      this WriteableBitmap bitmap,
      byte[] alpha4Array,
      int argb32ByteCount)
    {
      byte[] buffer = new byte[argb32ByteCount];
      using (MemoryStream memoryStream = new MemoryStream(buffer))
      {
        using (Alpha4Pbgra32ConversionStream conversionStream = new Alpha4Pbgra32ConversionStream(alpha4Array, argb32ByteCount))
          conversionStream.CopyTo((Stream) memoryStream);
      }
      bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), (Array) buffer, bitmap.PixelWidth * 4, 0);
    }
  }
}
