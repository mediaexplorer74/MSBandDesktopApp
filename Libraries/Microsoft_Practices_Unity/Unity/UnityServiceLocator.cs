// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.UnityServiceLocator
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity
{
  public sealed class UnityServiceLocator : ServiceLocatorImplBase, IDisposable
  {
    private IUnityContainer container;

    public UnityServiceLocator(IUnityContainer container)
    {
      this.container = container;
      container.RegisterInstance<IServiceLocator>((IServiceLocator) this, (LifetimeManager) new ExternallyControlledLifetimeManager());
    }

    public void Dispose()
    {
      if (this.container == null)
        return;
      this.container.Dispose();
      this.container = (IUnityContainer) null;
    }

    protected override object DoGetInstance(Type serviceType, string key)
    {
      if (this.container == null)
        throw new ObjectDisposedException("container");
      return this.container.Resolve(serviceType, key);
    }

    protected override IEnumerable<object> DoGetAllInstances(Type serviceType) => this.container != null ? this.container.ResolveAll(serviceType) : throw new ObjectDisposedException("container");
  }
}
