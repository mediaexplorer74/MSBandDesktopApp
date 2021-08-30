// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.UnityContainerExtensions
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.Unity
{
  public static class UnityContainerExtensions
  {
    public static IUnityContainer RegisterType<T>(
      this IUnityContainer container,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, typeof (T), (string) null, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType<TFrom, TTo>(
      this IUnityContainer container,
      params InjectionMember[] injectionMembers)
      where TTo : TFrom
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType(typeof (TFrom), typeof (TTo), (string) null, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType<TFrom, TTo>(
      this IUnityContainer container,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers)
      where TTo : TFrom
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType(typeof (TFrom), typeof (TTo), (string) null, lifetimeManager, injectionMembers);
    }

    public static IUnityContainer RegisterType<TFrom, TTo>(
      this IUnityContainer container,
      string name,
      params InjectionMember[] injectionMembers)
      where TTo : TFrom
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType(typeof (TFrom), typeof (TTo), name, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType<TFrom, TTo>(
      this IUnityContainer container,
      string name,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers)
      where TTo : TFrom
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType(typeof (TFrom), typeof (TTo), name, lifetimeManager, injectionMembers);
    }

    public static IUnityContainer RegisterType<T>(
      this IUnityContainer container,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, typeof (T), (string) null, lifetimeManager, injectionMembers);
    }

    public static IUnityContainer RegisterType<T>(
      this IUnityContainer container,
      string name,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, typeof (T), name, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType<T>(
      this IUnityContainer container,
      string name,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, typeof (T), name, lifetimeManager, injectionMembers);
    }

    public static IUnityContainer RegisterType(
      this IUnityContainer container,
      Type t,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, t, (string) null, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType(
      this IUnityContainer container,
      Type from,
      Type to,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType(from, to, (string) null, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType(
      this IUnityContainer container,
      Type from,
      Type to,
      string name,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType(from, to, name, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType(
      this IUnityContainer container,
      Type from,
      Type to,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType(from, to, (string) null, lifetimeManager, injectionMembers);
    }

    public static IUnityContainer RegisterType(
      this IUnityContainer container,
      Type t,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, t, (string) null, lifetimeManager, injectionMembers);
    }

    public static IUnityContainer RegisterType(
      this IUnityContainer container,
      Type t,
      string name,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, t, name, (LifetimeManager) null, injectionMembers);
    }

    public static IUnityContainer RegisterType(
      this IUnityContainer container,
      Type t,
      string name,
      LifetimeManager lifetimeManager,
      params InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterType((Type) null, t, name, lifetimeManager, injectionMembers);
    }

    public static IUnityContainer RegisterInstance<TInterface>(
      this IUnityContainer container,
      TInterface instance)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterInstance(typeof (TInterface), (string) null, (object) instance, UnityContainerExtensions.CreateDefaultInstanceLifetimeManager());
    }

    public static IUnityContainer RegisterInstance<TInterface>(
      this IUnityContainer container,
      TInterface instance,
      LifetimeManager lifetimeManager)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterInstance(typeof (TInterface), (string) null, (object) instance, lifetimeManager);
    }

    public static IUnityContainer RegisterInstance<TInterface>(
      this IUnityContainer container,
      string name,
      TInterface instance)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterInstance(typeof (TInterface), name, (object) instance, UnityContainerExtensions.CreateDefaultInstanceLifetimeManager());
    }

    public static IUnityContainer RegisterInstance<TInterface>(
      this IUnityContainer container,
      string name,
      TInterface instance,
      LifetimeManager lifetimeManager)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterInstance(typeof (TInterface), name, (object) instance, lifetimeManager);
    }

    public static IUnityContainer RegisterInstance(
      this IUnityContainer container,
      Type t,
      object instance)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterInstance(t, (string) null, instance, UnityContainerExtensions.CreateDefaultInstanceLifetimeManager());
    }

    public static IUnityContainer RegisterInstance(
      this IUnityContainer container,
      Type t,
      object instance,
      LifetimeManager lifetimeManager)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterInstance(t, (string) null, instance, lifetimeManager);
    }

    public static IUnityContainer RegisterInstance(
      this IUnityContainer container,
      Type t,
      string name,
      object instance)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.RegisterInstance(t, name, instance, UnityContainerExtensions.CreateDefaultInstanceLifetimeManager());
    }

    public static T Resolve<T>(this IUnityContainer container, params ResolverOverride[] overrides)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return (T) container.Resolve(typeof (T), (string) null, overrides);
    }

    public static T Resolve<T>(
      this IUnityContainer container,
      string name,
      params ResolverOverride[] overrides)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return (T) container.Resolve(typeof (T), name, overrides);
    }

    public static object Resolve(
      this IUnityContainer container,
      Type t,
      params ResolverOverride[] overrides)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.Resolve(t, (string) null, overrides);
    }

    public static IEnumerable<T> ResolveAll<T>(
      this IUnityContainer container,
      params ResolverOverride[] resolverOverrides)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.ResolveAll(typeof (T), resolverOverrides).Cast<T>();
    }

    public static T BuildUp<T>(
      this IUnityContainer container,
      T existing,
      params ResolverOverride[] resolverOverrides)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return (T) container.BuildUp(typeof (T), (object) existing, (string) null, resolverOverrides);
    }

    public static T BuildUp<T>(
      this IUnityContainer container,
      T existing,
      string name,
      params ResolverOverride[] resolverOverrides)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return (T) container.BuildUp(typeof (T), (object) existing, name, resolverOverrides);
    }

    public static object BuildUp(
      this IUnityContainer container,
      Type t,
      object existing,
      params ResolverOverride[] resolverOverrides)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.BuildUp(t, existing, (string) null, resolverOverrides);
    }

    public static IUnityContainer AddNewExtension<TExtension>(
      this IUnityContainer container)
      where TExtension : UnityContainerExtension
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      TExtension extension = container.Resolve<TExtension>();
      return container.AddExtension((UnityContainerExtension) extension);
    }

    public static TConfigurator Configure<TConfigurator>(this IUnityContainer container) where TConfigurator : IUnityContainerExtensionConfigurator
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return (TConfigurator) container.Configure(typeof (TConfigurator));
    }

    public static bool IsRegistered(this IUnityContainer container, Type typeToCheck)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      Guard.ArgumentNotNull((object) typeToCheck, nameof (typeToCheck));
      return container.IsRegistered(typeToCheck, (string) null);
    }

    public static bool IsRegistered(
      this IUnityContainer container,
      Type typeToCheck,
      string nameToCheck)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      Guard.ArgumentNotNull((object) typeToCheck, nameof (typeToCheck));
      return container.Registrations.Where<ContainerRegistration>((Func<ContainerRegistration, bool>) (r => (object) r.RegisteredType == (object) typeToCheck && r.Name == nameToCheck)).FirstOrDefault<ContainerRegistration>() != null;
    }

    public static bool IsRegistered<T>(this IUnityContainer container)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.IsRegistered(typeof (T));
    }

    public static bool IsRegistered<T>(this IUnityContainer container, string nameToCheck)
    {
      Guard.ArgumentNotNull((object) container, nameof (container));
      return container.IsRegistered(typeof (T), nameToCheck);
    }

    private static LifetimeManager CreateDefaultInstanceLifetimeManager() => (LifetimeManager) new ContainerControlledLifetimeManager();
  }
}
