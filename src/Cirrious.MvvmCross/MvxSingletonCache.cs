// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.MvxSingletonCache
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross
{
  public class MvxSingletonCache : MvxSingleton<IMvxSingletonCache>, IMvxSingletonCache
  {
    private bool _inpcInterceptorResolveAttempted;
    private IMvxInpcInterceptor _inpcInterceptor;
    private IMvxStringToTypeParser _parser;
    private IMvxSettings _settings;

    public static MvxSingletonCache Initialize()
    {
      if (MvxSingleton<IMvxSingletonCache>.Instance != null)
        throw new MvxException("You should only initialize MvxBindingSingletonCache once");
      return new MvxSingletonCache();
    }

    private MvxSingletonCache()
    {
    }

    public IMvxInpcInterceptor InpcInterceptor
    {
      get
      {
        if (this._inpcInterceptorResolveAttempted)
          return this._inpcInterceptor;
        Mvx.TryResolve<IMvxInpcInterceptor>(out this._inpcInterceptor);
        this._inpcInterceptorResolveAttempted = true;
        return this._inpcInterceptor;
      }
    }

    public IMvxStringToTypeParser Parser
    {
      get
      {
        this._parser = this._parser ?? Mvx.Resolve<IMvxStringToTypeParser>();
        return this._parser;
      }
    }

    public IMvxSettings Settings
    {
      get
      {
        this._settings = this._settings ?? Mvx.Resolve<IMvxSettings>();
        return this._settings;
      }
    }
  }
}
