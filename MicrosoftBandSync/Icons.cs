// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.Icons
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DesktopSyncApp
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Icons
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Icons()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Icons.resourceMan == null)
          Icons.resourceMan = new ResourceManager("DesktopSyncApp.ResourceLinks.Icons", typeof (Icons).Assembly);
        return Icons.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Icons.resourceCulture;
      set => Icons.resourceCulture = value;
    }

    internal static Icon app_icon => (Icon) Icons.ResourceManager.GetObject(nameof (app_icon), Icons.resourceCulture);

    internal static Icon error_red => (Icon) Icons.ResourceManager.GetObject(nameof (error_red), Icons.resourceCulture);

    internal static Icon syncing_frame_01 => (Icon) Icons.ResourceManager.GetObject(nameof (syncing_frame_01), Icons.resourceCulture);

    internal static Icon syncing_frame_02 => (Icon) Icons.ResourceManager.GetObject(nameof (syncing_frame_02), Icons.resourceCulture);

    internal static Icon syncing_frame_03 => (Icon) Icons.ResourceManager.GetObject(nameof (syncing_frame_03), Icons.resourceCulture);
  }
}
