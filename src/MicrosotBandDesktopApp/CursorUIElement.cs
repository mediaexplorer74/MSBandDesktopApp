// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.CursorUIElement
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Win32.SafeHandles;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopSyncApp
{
  public class CursorUIElement
  {
    private static Cursor InternalCreateCursor(Bitmap bmp, int xHotspot, int yHotspot)
    {
      CursorUIElement.NativeMethods.TileIcon tileIcon = new CursorUIElement.NativeMethods.TileIcon();
      CursorUIElement.NativeMethods.GetIconInfo(bmp.GetHicon(), ref tileIcon);
      tileIcon.xHotspot = xHotspot;
      tileIcon.yHotspot = yHotspot;
      tileIcon.fIcon = false;
      return CursorInteropHelper.Create((SafeHandle) CursorUIElement.NativeMethods.CreateIconIndirect(ref tileIcon));
    }

    public static Cursor CreateCursor(UIElement element, int xHotspot = 0, int yHotspot = 0)
    {
      element.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
      element.Arrange(new Rect(new System.Windows.Point(), element.DesiredSize));
      System.Windows.Size desiredSize = element.DesiredSize;
      int width = (int) desiredSize.Width;
      desiredSize = element.DesiredSize;
      int height = (int) desiredSize.Height;
      PixelFormat pbgra32 = PixelFormats.Pbgra32;
      RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96.0, 96.0, pbgra32);
      renderTargetBitmap.Render((Visual) element);
      PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
      pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) renderTargetBitmap));
      using (MemoryStream memoryStream = new MemoryStream())
      {
        pngBitmapEncoder.Save((Stream) memoryStream);
        using (Bitmap bmp = new Bitmap((Stream) memoryStream))
          return CursorUIElement.InternalCreateCursor(bmp, xHotspot, yHotspot);
      }
    }

    private static class NativeMethods
    {
      [DllImport("user32.dll")]
      public static extern CursorUIElement.SafeIconHandle CreateIconIndirect(
        ref CursorUIElement.NativeMethods.TileIcon icon);

      [DllImport("user32.dll")]
      public static extern bool DestroyIcon(IntPtr hIcon);

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool GetIconInfo(
        IntPtr hIcon,
        ref CursorUIElement.NativeMethods.TileIcon pIconInfo);

      public struct TileIcon
      {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
      }
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    private class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
      public SafeIconHandle()
        : base(true)
      {
      }

      protected override bool ReleaseHandle() => CursorUIElement.NativeMethods.DestroyIcon(this.handle);
    }
  }
}
