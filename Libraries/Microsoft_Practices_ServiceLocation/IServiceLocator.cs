// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ServiceLocation.IServiceLocator
// Assembly: Microsoft.Practices.ServiceLocation, Version=1.3.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 7D3316BA-C928-4A64-AD5F-824E0C3D6D36
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_ServiceLocation.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Practices.ServiceLocation
{
  public interface IServiceLocator : IServiceProvider
  {
    object GetInstance(Type serviceType);

    object GetInstance(Type serviceType, string key);

    IEnumerable<object> GetAllInstances(Type serviceType);

    TService GetInstance<TService>();

    TService GetInstance<TService>(string key);

    IEnumerable<TService> GetAllInstances<TService>();
  }
}
