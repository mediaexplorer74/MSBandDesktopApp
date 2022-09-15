// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxNotifyPropertyChanged
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.Core;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Cirrious.MvvmCross.ViewModels
{
  public abstract class MvxNotifyPropertyChanged : 
    MvxMainThreadDispatchingObject,
    IMvxNotifyPropertyChanged,
    INotifyPropertyChanged
  {
    private bool _shouldAlwaysRaiseInpcOnUserInterfaceThread;

    public event PropertyChangedEventHandler PropertyChanged;

    public bool ShouldAlwaysRaiseInpcOnUserInterfaceThread() => this._shouldAlwaysRaiseInpcOnUserInterfaceThread;

    public void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value) => this._shouldAlwaysRaiseInpcOnUserInterfaceThread = value;

    protected MvxNotifyPropertyChanged() => this.ShouldAlwaysRaiseInpcOnUserInterfaceThread(MvxSingleton<IMvxSingletonCache>.Instance == null || MvxSingleton<IMvxSingletonCache>.Instance.Settings.AlwaysRaiseInpcOnUserInterfaceThread);

    public void RaisePropertyChanged<T>(Expression<Func<T>> property) => this.RaisePropertyChanged(this.GetPropertyNameFromExpression<T>(property));

    public void RaisePropertyChanged([CallerMemberName] string whichProperty = "") => this.RaisePropertyChanged(new PropertyChangedEventArgs(whichProperty));

    public virtual void RaiseAllPropertiesChanged() => this.RaisePropertyChanged(new PropertyChangedEventArgs(string.Empty));

    public virtual void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
    {
      if (this.InterceptRaisePropertyChanged(changedArgs) == MvxInpcInterceptionResult.DoNotRaisePropertyChanged)
        return;
      Action action = (Action) (() =>
      {
        PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
        if (propertyChanged == null)
          return;
        propertyChanged((object) this, changedArgs);
      });
      if (this.ShouldAlwaysRaiseInpcOnUserInterfaceThread())
      {
        if (this.PropertyChanged == null)
          return;
        this.InvokeOnMainThread(action);
      }
      else
        action();
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
      if (object.Equals((object) storage, (object) value))
        return false;
      storage = value;
      this.RaisePropertyChanged(propertyName);
      return true;
    }

    protected virtual MvxInpcInterceptionResult InterceptRaisePropertyChanged(
      PropertyChangedEventArgs changedArgs)
    {
      if (MvxSingleton<IMvxSingletonCache>.Instance != null)
      {
        IMvxInpcInterceptor inpcInterceptor = MvxSingleton<IMvxSingletonCache>.Instance.InpcInterceptor;
        if (inpcInterceptor != null)
          return inpcInterceptor.Intercept((IMvxNotifyPropertyChanged) this, changedArgs);
      }
      return MvxInpcInterceptionResult.NotIntercepted;
    }
  }
}
