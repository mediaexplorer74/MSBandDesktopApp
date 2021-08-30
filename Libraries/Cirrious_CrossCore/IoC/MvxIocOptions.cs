// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxIocOptions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.IoC
{
  public class MvxIocOptions : IMvxIocOptions
  {
    private IMvxPropertyInjector _injector;

    public MvxIocOptions()
    {
      this.TryToDetectSingletonCircularReferences = true;
      this.TryToDetectDynamicCircularReferences = true;
      this.CheckDisposeIfPropertyInjectionFails = true;
      this.PropertyInjectorType = typeof (MvxPropertyInjector);
      this.PropertyInjectorOptions = (IMvxPropertyInjectorOptions) new MvxPropertyInjectorOptions();
    }

    public bool TryToDetectSingletonCircularReferences { get; set; }

    public bool TryToDetectDynamicCircularReferences { get; set; }

    public bool CheckDisposeIfPropertyInjectionFails { get; set; }

    public Type PropertyInjectorType { get; set; }

    public IMvxPropertyInjectorOptions PropertyInjectorOptions { get; set; }
  }
}
