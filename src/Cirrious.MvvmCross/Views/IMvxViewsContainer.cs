// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Views.IMvxViewsContainer
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Views
{
  public interface IMvxViewsContainer : IMvxViewFinder
  {
    void AddAll(IDictionary<Type, Type> viewModelViewLookup);

    void Add(Type viewModelType, Type viewType);

    void Add<TViewModel, TView>()
      where TViewModel : IMvxViewModel
      where TView : IMvxView;

    void AddSecondary(IMvxViewFinder finder);

    void SetLastResort(IMvxViewFinder finder);
  }
}
