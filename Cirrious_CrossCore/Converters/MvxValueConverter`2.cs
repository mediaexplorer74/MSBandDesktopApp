// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Converters.MvxValueConverter`2
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Globalization;

namespace Cirrious.CrossCore.Converters
{
  public abstract class MvxValueConverter<TFrom, TTo> : IMvxValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        return (object) this.Convert((TFrom) value, targetType, parameter, culture);
      }
      catch (Exception ex)
      {
        return (object) MvxBindingConstant.UnsetValue;
      }
    }

    protected virtual TTo Convert(
      TFrom value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      try
      {
        return (object) this.ConvertBack((TTo) value, targetType, parameter, culture);
      }
      catch (Exception ex)
      {
        return (object) MvxBindingConstant.UnsetValue;
      }
    }

    protected virtual TFrom ConvertBack(
      TTo value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
