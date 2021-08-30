// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.IUnityContainer
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity
{
  public interface IUnityContainer : IDisposable
  {
    IUnityContainer RegisterType(
      Type from,
      Type to,
      string name,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers);

    IUnityContainer RegisterInstance(
      Type t,
      string name,
      object instance,
      LifetimeManager lifetime);

    object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides);

    IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides);

    object BuildUp(
      Type t,
      object existing,
      string name,
      params ResolverOverride[] resolverOverrides);

    void Teardown(object o);

    IUnityContainer AddExtension(UnityContainerExtension extension);

    object Configure(Type configurationInterface);

    IUnityContainer RemoveAllExtensions();

    IUnityContainer Parent { get; }

    IUnityContainer CreateChildContainer();

    IEnumerable<ContainerRegistration> Registrations { get; }
  }
}
