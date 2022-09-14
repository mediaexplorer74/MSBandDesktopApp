// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Views.MvxViewsContainer
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Views
{
  public abstract class MvxViewsContainer : IMvxViewsContainer, IMvxViewFinder
  {
    private readonly Dictionary<Type, Type> _bindingMap = new Dictionary<Type, Type>();
    private readonly List<IMvxViewFinder> _secondaryViewFinders;
    private IMvxViewFinder _lastResortViewFinder;

    protected MvxViewsContainer() => this._secondaryViewFinders = new List<IMvxViewFinder>();

    public void AddAll(IDictionary<Type, Type> lookup)
    {
      foreach (KeyValuePair<Type, Type> keyValuePair in (IEnumerable<KeyValuePair<Type, Type>>) lookup)
        this.Add(keyValuePair.Key, keyValuePair.Value);
    }

    public void Add(Type viewModelType, Type viewType) => this._bindingMap[viewModelType] = viewType;

    public void Add<TViewModel, TView>()
      where TViewModel : IMvxViewModel
      where TView : IMvxView
    {
      this.Add(typeof (TViewModel), typeof (TView));
    }

    public Type GetViewType(Type viewModelType)
    {
      Type type1;

      if (this._bindingMap.TryGetValue(viewModelType, out type1))
        return type1;
      
      foreach (IMvxViewFinder secondaryViewFinder in this._secondaryViewFinders)
      {
        Type viewType = secondaryViewFinder.GetViewType(viewModelType);
        if ((object) viewType != null)
          return viewType;
      }
    
      Type type2 = this._lastResortViewFinder != null
                ? this._lastResortViewFinder.GetViewType(viewModelType)
                : null;//throw new KeyNotFoundException("Could not find view for " + (object) viewModelType);
      if ((object) type2 != null)
        return type2;
            
      return null; // * ME *
    }

    public void AddSecondary(IMvxViewFinder finder) => this._secondaryViewFinders.Add(finder);

    public void SetLastResort(IMvxViewFinder finder) => this._lastResortViewFinder = finder;
  }
}
