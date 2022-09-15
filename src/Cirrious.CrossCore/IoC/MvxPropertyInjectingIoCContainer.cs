// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxPropertyInjectingIoCContainer
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Core;
using System;

namespace Cirrious.CrossCore.IoC
{
  [Obsolete("This functionality is now moved into MvxSimpleIoCContainer and can be enabled using MvxIocOptions")]
  public class MvxPropertyInjectingIoCContainer : MvxSimpleIoCContainer
  {
    public new static IMvxIoCProvider Initialize(IMvxIocOptions options)
    {
      if (MvxSingleton<IMvxIoCProvider>.Instance != null)
        return MvxSingleton<IMvxIoCProvider>.Instance;
      MvxPropertyInjectingIoCContainer injectingIoCcontainer = new MvxPropertyInjectingIoCContainer(options);
      return MvxSingleton<IMvxIoCProvider>.Instance;
    }

    protected MvxPropertyInjectingIoCContainer(IMvxIocOptions options)
      : base(options ?? (IMvxIocOptions) MvxPropertyInjectingIoCContainer.CreatePropertyInjectionOptions())
    {
    }

    private static MvxIocOptions CreatePropertyInjectionOptions() => new MvxIocOptions()
    {
      TryToDetectDynamicCircularReferences = true,
      TryToDetectSingletonCircularReferences = true,
      CheckDisposeIfPropertyInjectionFails = true,
      PropertyInjectorType = typeof (MvxPropertyInjector),
      PropertyInjectorOptions = (IMvxPropertyInjectorOptions) new MvxPropertyInjectorOptions()
      {
        ThrowIfPropertyInjectionFails = false,
        InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties
      }
    };
  }
}
