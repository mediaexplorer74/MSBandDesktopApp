// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ServiceLocation.Properties.Resources
// Assembly: Microsoft.Practices.ServiceLocation, Version=1.3.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 7D3316BA-C928-4A64-AD5F-824E0C3D6D36
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_ServiceLocation.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Practices.ServiceLocation.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Microsoft.Practices.ServiceLocation.Properties.Resources.resourceMan, (object) null))
          Microsoft.Practices.ServiceLocation.Properties.Resources.resourceMan = new ResourceManager("Microsoft.Practices.ServiceLocation.Properties.Resources", typeof (Microsoft.Practices.ServiceLocation.Properties.Resources).Assembly);
        return Microsoft.Practices.ServiceLocation.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Microsoft.Practices.ServiceLocation.Properties.Resources.resourceCulture;
      set => Microsoft.Practices.ServiceLocation.Properties.Resources.resourceCulture = value;
    }

    internal static string ActivateAllExceptionMessage => Microsoft.Practices.ServiceLocation.Properties.Resources.ResourceManager.GetString(nameof (ActivateAllExceptionMessage), Microsoft.Practices.ServiceLocation.Properties.Resources.resourceCulture);

    internal static string ActivationExceptionMessage => Microsoft.Practices.ServiceLocation.Properties.Resources.ResourceManager.GetString(nameof (ActivationExceptionMessage), Microsoft.Practices.ServiceLocation.Properties.Resources.resourceCulture);

    internal static string ServiceLocationProviderNotSetMessage => Microsoft.Practices.ServiceLocation.Properties.Resources.ResourceManager.GetString(nameof (ServiceLocationProviderNotSetMessage), Microsoft.Practices.ServiceLocation.Properties.Resources.resourceCulture);
  }
}
