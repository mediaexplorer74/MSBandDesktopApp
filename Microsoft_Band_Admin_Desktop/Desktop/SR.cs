// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Desktop.SR
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Band.Admin.Desktop
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class SR
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal SR()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (SR.resourceMan == null)
          SR.resourceMan = new ResourceManager("Microsoft.Band.Admin.Desktop.SR", typeof (SR).Assembly);
        return SR.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => SR.resourceCulture;
      set => SR.resourceCulture = value;
    }

    internal static string AccessTokenIsMissing => SR.ResourceManager.GetString(nameof (AccessTokenIsMissing), SR.resourceCulture);

    internal static string CannotReadFromStream => SR.ResourceManager.GetString(nameof (CannotReadFromStream), SR.resourceCulture);

    internal static string ConnectionClosed => SR.ResourceManager.GetString(nameof (ConnectionClosed), SR.resourceCulture);

    internal static string DiscoveryServiceAddressIsMissing => SR.ResourceManager.GetString(nameof (DiscoveryServiceAddressIsMissing), SR.resourceCulture);

    internal static string DiscoveryServiceTokenIsMissing => SR.ResourceManager.GetString(nameof (DiscoveryServiceTokenIsMissing), SR.resourceCulture);

    internal static string EndTimeIsSmallerThanStartTime => SR.ResourceManager.GetString(nameof (EndTimeIsSmallerThanStartTime), SR.resourceCulture);

    internal static string GetOverlappedResultFailed => SR.ResourceManager.GetString(nameof (GetOverlappedResultFailed), SR.resourceCulture);

    internal static string ServiceAddressIsMissing => SR.ResourceManager.GetString(nameof (ServiceAddressIsMissing), SR.resourceCulture);

    internal static string UsbDisabled => SR.ResourceManager.GetString(nameof (UsbDisabled), SR.resourceCulture);

    internal static string WaitForSingleObjectFailed => SR.ResourceManager.GetString(nameof (WaitForSingleObjectFailed), SR.resourceCulture);

    internal static string WinUsbSetPipePolicyFailed => SR.ResourceManager.GetString(nameof (WinUsbSetPipePolicyFailed), SR.resourceCulture);
  }
}
