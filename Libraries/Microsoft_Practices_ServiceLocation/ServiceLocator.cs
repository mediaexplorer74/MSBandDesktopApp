// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ServiceLocation.ServiceLocator
// Assembly: Microsoft.Practices.ServiceLocation, Version=1.3.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 7D3316BA-C928-4A64-AD5F-824E0C3D6D36
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_ServiceLocation.dll

using Microsoft.Practices.ServiceLocation.Properties;
using System;

namespace Microsoft.Practices.ServiceLocation
{
  public static class ServiceLocator
  {
    private static ServiceLocatorProvider currentProvider;

    public static IServiceLocator Current
    {
      get
      {
        if (!ServiceLocator.IsLocationProviderSet)
          throw new InvalidOperationException(Resources.ServiceLocationProviderNotSetMessage);
        return ServiceLocator.currentProvider();
      }
    }

    public static void SetLocatorProvider(ServiceLocatorProvider newProvider) => ServiceLocator.currentProvider = newProvider;

    public static bool IsLocationProviderSet => ServiceLocator.currentProvider != null;
  }
}
