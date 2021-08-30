// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.LifetimeManagerFactory
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;

namespace Microsoft.Practices.Unity
{
  public class LifetimeManagerFactory : ILifetimeFactoryPolicy, IBuilderPolicy
  {
    private readonly ExtensionContext containerContext;

    public LifetimeManagerFactory(ExtensionContext containerContext, Type lifetimeType)
    {
      this.containerContext = containerContext;
      this.LifetimeType = lifetimeType;
    }

    public ILifetimePolicy CreateLifetimePolicy()
    {
      LifetimeManager lifetimeManager = (LifetimeManager) this.containerContext.Container.Resolve(this.LifetimeType);
      if (lifetimeManager is IDisposable)
        this.containerContext.Lifetime.Add((object) lifetimeManager);
      lifetimeManager.InUse = true;
      return (ILifetimePolicy) lifetimeManager;
    }

    public Type LifetimeType { get; private set; }
  }
}
