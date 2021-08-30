// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.InjectedMembers
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.Unity
{
  [Obsolete("Use the IUnityContainer.RegisterType method instead of this interface")]
  public class InjectedMembers : UnityContainerExtension
  {
    protected override void Initialize()
    {
    }

    public InjectedMembers ConfigureInjectionFor<TTypeToInject>(
      params InjectionMember[] injectionMembers)
    {
      return this.ConfigureInjectionFor(typeof (TTypeToInject), (string) null, injectionMembers);
    }

    public InjectedMembers ConfigureInjectionFor<TTypeToInject>(
      string name,
      params InjectionMember[] injectionMembers)
    {
      return this.ConfigureInjectionFor(typeof (TTypeToInject), name, injectionMembers);
    }

    public InjectedMembers ConfigureInjectionFor(
      Type typeToInject,
      params InjectionMember[] injectionMembers)
    {
      return this.ConfigureInjectionFor((Type) null, typeToInject, (string) null, injectionMembers);
    }

    public InjectedMembers ConfigureInjectionFor(
      Type typeToInject,
      string name,
      params InjectionMember[] injectionMembers)
    {
      return this.ConfigureInjectionFor((Type) null, typeToInject, name, injectionMembers);
    }

    public InjectedMembers ConfigureInjectionFor(
      Type serviceType,
      Type implementationType,
      string name,
      params InjectionMember[] injectionMembers)
    {
      this.Container.RegisterType(serviceType, implementationType, name, injectionMembers);
      return this;
    }
  }
}
