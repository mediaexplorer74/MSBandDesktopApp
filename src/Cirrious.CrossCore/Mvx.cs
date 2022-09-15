// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Mvx
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using System;

namespace Cirrious.CrossCore
{
  public static class Mvx
  {
    public static bool CanResolve<TService>() where TService : class => MvxSingleton<IMvxIoCProvider>.Instance.CanResolve<TService>();

    public static bool CanResolve(Type serviceType) => MvxSingleton<IMvxIoCProvider>.Instance.CanResolve(serviceType);

    public static TService Resolve<TService>() where TService : class => MvxSingleton<IMvxIoCProvider>.Instance.Resolve<TService>();

    public static object Resolve(Type serviceType) => MvxSingleton<IMvxIoCProvider>.Instance.Resolve(serviceType);

    public static bool TryResolve<TService>(out TService service) where TService : class => MvxSingleton<IMvxIoCProvider>.Instance.TryResolve<TService>(out service);

    public static bool TryResolve(Type serviceType, out object service) => MvxSingleton<IMvxIoCProvider>.Instance.TryResolve(serviceType, out service);

    public static T Create<T>() where T : class => MvxSingleton<IMvxIoCProvider>.Instance.Create<T>();

    public static T GetSingleton<T>() where T : class => MvxSingleton<IMvxIoCProvider>.Instance.GetSingleton<T>();

    public static void RegisterSingleton<TInterface>(Func<TInterface> serviceConstructor) where TInterface : class => MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton<TInterface>(serviceConstructor);

    public static void RegisterSingleton(Type tInterface, Func<object> serviceConstructor) => MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton(tInterface, serviceConstructor);

    public static void RegisterSingleton<TInterface>(TInterface service) where TInterface : class => MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton<TInterface>(service);

    public static void RegisterSingleton(Type tInterface, object service) => MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton(tInterface, service);

    public static void ConstructAndRegisterSingleton<TInterface, TType>()
      where TInterface : class
      where TType : TInterface
    {
      MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton<TInterface>((TInterface) Mvx.IocConstruct<TType>());
    }

    public static void LazyConstructAndRegisterSingleton<TInterface, TType>()
      where TInterface : class
      where TType : TInterface
    {
      MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton<TInterface>((Func<TInterface>) (() => (TInterface) Mvx.IocConstruct<TType>()));
    }

    public static void LazyConstructAndRegisterSingleton<TInterface>(Func<TInterface> constructor) where TInterface : class => MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton<TInterface>(constructor);

    public static void LazyConstructAndRegisterSingleton(Type type, Func<object> constructor) => MvxSingleton<IMvxIoCProvider>.Instance.RegisterSingleton(type, constructor);

    public static void RegisterType<TInterface, TType>()
      where TInterface : class
      where TType : class, TInterface
    {
      MvxSingleton<IMvxIoCProvider>.Instance.RegisterType<TInterface, TType>();
    }

    public static void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class => MvxSingleton<IMvxIoCProvider>.Instance.RegisterType<TInterface>(constructor);

    public static void RegisterType(Type type, Func<object> constructor) => MvxSingleton<IMvxIoCProvider>.Instance.RegisterType(type, constructor);

    public static void RegisterType(Type tInterface, Type tType) => MvxSingleton<IMvxIoCProvider>.Instance.RegisterType(tInterface, tType);

    public static T IocConstruct<T>() => (T) MvxSingleton<IMvxIoCProvider>.Instance.IoCConstruct(typeof (T));

    public static object IocConstruct(Type t) => MvxSingleton<IMvxIoCProvider>.Instance.IoCConstruct(t);

    public static void CallbackWhenRegistered<T>(Action<T> action) where T : class => Mvx.CallbackWhenRegistered<T>((Action) (() => action(Mvx.Resolve<T>())));

    public static void CallbackWhenRegistered<T>(Action action) => MvxSingleton<IMvxIoCProvider>.Instance.CallbackWhenRegistered<T>(action);

    public static void CallbackWhenRegistered(Type type, Action action) => MvxSingleton<IMvxIoCProvider>.Instance.CallbackWhenRegistered(type, action);

    public static void TaggedTrace(
      MvxTraceLevel level,
      string tag,
      string message,
      params object[] args)
    {
      MvxTrace.TaggedTrace(level, tag, message, args);
    }

    public static void Trace(MvxTraceLevel level, string message, params object[] args) => MvxTrace.Trace(level, message, args);

    public static void TaggedTrace(string tag, string message, params object[] args) => Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, tag, message, args);

    public static void TaggedWarning(string tag, string message, params object[] args) => Mvx.TaggedTrace(MvxTraceLevel.Warning, tag, message, args);

    public static void TaggedError(string tag, string message, params object[] args) => Mvx.TaggedTrace(MvxTraceLevel.Error, tag, message, args);

    public static void Trace(string message, params object[] args) => Mvx.Trace(MvxTraceLevel.Diagnostic, message, args);

    public static void Warning(string message, params object[] args) => Mvx.Trace(MvxTraceLevel.Warning, message, args);

    public static void Error(string message, params object[] args) => Mvx.Trace(MvxTraceLevel.Error, message, args);

    public static MvxException Exception(string message) => new MvxException(message);

    public static MvxException Exception(string message, params object[] args) => new MvxException(message, args);

    public static MvxException Exception(
      System.Exception innerException,
      string message,
      params object[] args)
    {
      return new MvxException(innerException, message, args);
    }
  }
}
