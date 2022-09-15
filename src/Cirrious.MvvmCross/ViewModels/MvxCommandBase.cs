// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxCommandBase
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using System;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxCommandBase
  {
    private readonly IMvxCommandHelper _commandHelper;

    public MvxCommandBase()
    {
      if (Mvx.TryResolve<IMvxCommandHelper>(out this._commandHelper))
        return;
      this._commandHelper = (IMvxCommandHelper) new MvxWeakCommandHelper();
    }

    public event EventHandler CanExecuteChanged
    {
      add => this._commandHelper.CanExecuteChanged += value;
      remove => this._commandHelper.CanExecuteChanged -= value;
    }

    public void RaiseCanExecuteChanged() => this._commandHelper.RaiseCanExecuteChanged((object) this);
  }
}
