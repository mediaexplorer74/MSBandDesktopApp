// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.WebBrowserJavascriptErrors
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.Reflection;
using System.Windows.Controls;

namespace DesktopSyncApp
{
  public static class WebBrowserJavascriptErrors
  {
    public static void HandleJavascriptErrors(WebBrowser wb, bool hide)
    {
      FieldInfo field = typeof (WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
      if (field == (FieldInfo) null)
        return;
      object target = field.GetValue((object) wb);
      target?.GetType().InvokeMember("Silent", BindingFlags.SetProperty, (Binder) null, target, new object[1]
      {
        (object) hide
      });
    }
  }
}
