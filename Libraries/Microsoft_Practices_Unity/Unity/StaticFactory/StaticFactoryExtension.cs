// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.StaticFactory.StaticFactoryExtension
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.Unity.StaticFactory
{
  [Obsolete("Use RegisterType<TInterface, TImpl>(new InjectionFactory(...)) instead of the extension's methods.")]
  public class StaticFactoryExtension : 
    UnityContainerExtension,
    IStaticFactoryConfiguration,
    IUnityContainerExtensionConfigurator
  {
    protected override void Initialize()
    {
    }

    public IStaticFactoryConfiguration RegisterFactory<TTypeToBuild>(
      string name,
      Func<IUnityContainer, object> factoryMethod)
    {
      this.Container.RegisterType<TTypeToBuild>(name, (InjectionMember) new InjectionFactory(factoryMethod));
      return (IStaticFactoryConfiguration) this;
    }

    public IStaticFactoryConfiguration RegisterFactory<TTypeToBuild>(
      Func<IUnityContainer, object> factoryMethod)
    {
      return this.RegisterFactory<TTypeToBuild>((string) null, factoryMethod);
    }
  }
}
