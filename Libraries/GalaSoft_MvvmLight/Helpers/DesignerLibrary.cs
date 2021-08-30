// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.DesignerLibrary
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;

namespace GalaSoft.MvvmLight.Helpers
{
  internal static class DesignerLibrary
  {
    private static DesignerPlatformLibrary? _detectedDesignerPlatformLibrary;

    internal static DesignerPlatformLibrary DetectedDesignerLibrary
    {
      get
      {
        if (!DesignerLibrary._detectedDesignerPlatformLibrary.HasValue)
          DesignerLibrary._detectedDesignerPlatformLibrary = new DesignerPlatformLibrary?(DesignerLibrary.GetCurrentPlatform());
        return DesignerLibrary._detectedDesignerPlatformLibrary.Value;
      }
    }

    private static DesignerPlatformLibrary GetCurrentPlatform()
    {
      if ((object) Type.GetType("System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e") != null)
        return DesignerPlatformLibrary.Silverlight;
      if ((object) Type.GetType("System.ComponentModel.DesignerProperties, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35") != null)
        return DesignerPlatformLibrary.Net;
      return (object) Type.GetType("Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime") != null ? DesignerPlatformLibrary.WinRt : DesignerPlatformLibrary.Unknown;
    }
  }
}
