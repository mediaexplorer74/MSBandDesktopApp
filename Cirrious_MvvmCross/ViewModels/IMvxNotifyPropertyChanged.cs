// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.IMvxNotifyPropertyChanged
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.ViewModels
{
  public interface IMvxNotifyPropertyChanged : INotifyPropertyChanged
  {
    bool ShouldAlwaysRaiseInpcOnUserInterfaceThread();

    void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value);

    void RaisePropertyChanged<T>(Expression<Func<T>> property);

    void RaisePropertyChanged(string whichProperty);

    void RaisePropertyChanged(PropertyChangedEventArgs changedArgs);
  }
}
