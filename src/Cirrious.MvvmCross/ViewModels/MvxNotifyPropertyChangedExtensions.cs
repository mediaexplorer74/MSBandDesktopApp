// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxNotifyPropertyChangedExtensions
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.ViewModels
{
  public static class MvxNotifyPropertyChangedExtensions
  {
    private static TReturn RaiseAndSetIfChanged<T, TReturn, TActionParameter>(
      T source,
      ref TReturn backingField,
      TReturn newValue,
      Action<TActionParameter> raiseAction,
      TActionParameter raiseActionParameter)
      where T : IMvxNotifyPropertyChanged
    {
      if (!EqualityComparer<TReturn>.Default.Equals(backingField, newValue))
      {
        backingField = newValue;
        raiseAction(raiseActionParameter);
      }
      return newValue;
    }

    public static TReturn RaiseAndSetIfChanged<T, TReturn>(
      this T source,
      ref TReturn backingField,
      TReturn newValue,
      Expression<Func<TReturn>> propertySelector)
      where T : IMvxNotifyPropertyChanged
    {
      return MvxNotifyPropertyChangedExtensions.RaiseAndSetIfChanged<T, TReturn, Expression<Func<TReturn>>>(source, ref backingField, newValue, new Action<Expression<Func<TReturn>>>(((IMvxNotifyPropertyChanged) source).RaisePropertyChanged<TReturn>), propertySelector);
    }

    public static TReturn RaiseAndSetIfChanged<T, TReturn>(
      this T source,
      ref TReturn backingField,
      TReturn newValue,
      string propertyName)
      where T : IMvxNotifyPropertyChanged
    {
      return MvxNotifyPropertyChangedExtensions.RaiseAndSetIfChanged<T, TReturn, string>(source, ref backingField, newValue, new Action<string>(((IMvxNotifyPropertyChanged) source).RaisePropertyChanged), propertyName);
    }

    public static TReturn RaiseAndSetIfChanged<T, TReturn>(
      this T source,
      ref TReturn backingField,
      TReturn newValue,
      PropertyChangedEventArgs args)
      where T : IMvxNotifyPropertyChanged
    {
      return MvxNotifyPropertyChangedExtensions.RaiseAndSetIfChanged<T, TReturn, PropertyChangedEventArgs>(source, ref backingField, newValue, new Action<PropertyChangedEventArgs>(((IMvxNotifyPropertyChanged) source).RaisePropertyChanged), args);
    }
  }
}
