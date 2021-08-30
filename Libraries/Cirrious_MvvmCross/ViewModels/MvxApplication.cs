// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxApplication
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public abstract class MvxApplication : IMvxApplication, IMvxViewModelLocatorCollection
  {
    private IMvxViewModelLocator _defaultLocator;

    private IMvxViewModelLocator DefaultLocator
    {
      get
      {
        this._defaultLocator = this._defaultLocator ?? this.CreateDefaultViewModelLocator();
        return this._defaultLocator;
      }
    }

    protected virtual IMvxViewModelLocator CreateDefaultViewModelLocator() => (IMvxViewModelLocator) new MvxDefaultViewModelLocator();

    public virtual void LoadPlugins(IMvxPluginManager pluginManager)
    {
    }

    public virtual void Initialize()
    {
    }

    public IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request) => this.DefaultLocator;

    protected void RegisterAppStart<TViewModel>() where TViewModel : IMvxViewModel => Mvx.RegisterSingleton<IMvxAppStart>((IMvxAppStart) new MvxAppStart<TViewModel>());

    protected void RegisterAppStart(IMvxAppStart appStart) => Mvx.RegisterSingleton<IMvxAppStart>(appStart);

    protected IEnumerable<Type> CreatableTypes() => this.CreatableTypes(this.GetType().GetTypeInfo().Assembly);

    protected IEnumerable<Type> CreatableTypes(Assembly assembly) => assembly.CreatableTypes();
  }
}
