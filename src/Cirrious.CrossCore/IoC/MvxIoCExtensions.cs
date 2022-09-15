// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxIoCExtensions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.IoC
{
  [Obsolete("We prefer to use IoC directly using Mvx.Resolve<T>() now")]
  public static class MvxIoCExtensions
  {
    public static bool IsServiceAvailable<TService>(this IMvxConsumer consumer) where TService : class => Mvx.CanResolve<TService>();

    public static TService GetService<TService>(this IMvxConsumer consumer) where TService : class => Mvx.Resolve<TService>();

    public static bool TryGetService<TService>(this IMvxConsumer consumer, out TService service) where TService : class => Mvx.TryResolve<TService>(out service);

    public static void RegisterServiceInstance<TInterface>(
      this IMvxProducer producer,
      Func<TInterface> serviceConstructor)
      where TInterface : class
    {
      Mvx.RegisterSingleton<TInterface>(serviceConstructor);
    }

    public static void RegisterServiceInstance<TInterface>(
      this IMvxProducer producer,
      TInterface service)
      where TInterface : class
    {
      Mvx.RegisterSingleton<TInterface>(service);
    }

    public static void RegisterServiceType<TInterface, TType>(this IMvxProducer producer)
      where TInterface : class
      where TType : class, TInterface
    {
      Mvx.RegisterType<TInterface, TType>();
    }
  }
}
