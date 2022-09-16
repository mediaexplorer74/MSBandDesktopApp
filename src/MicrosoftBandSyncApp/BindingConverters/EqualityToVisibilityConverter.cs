// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.EqualityToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (object), typeof (Visibility))]
  public class EqualityToVisibilityConverter : IValueConverter
  {
    public EqualityToVisibilityConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool flag = value != null && parameter != null && value.GetType() == parameter.GetType() && value.Equals(parameter);
      switch (this.DefaultMapping)
      {
        case EqualityToVisibilityConverter.Mapping.UnequalCollapsed:
          return (object) (Visibility) (flag ? 0 : 2);
        case EqualityToVisibilityConverter.Mapping.EqualHidden:
          return (object) (Visibility) (flag ? 1 : 0);
        case EqualityToVisibilityConverter.Mapping.EqualCollapsed:
          return (object) (Visibility) (flag ? 2 : 0);
        default:
          return (object) (Visibility) (flag ? 0 : 1);
      }
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException();
    }

    public enum Mapping
    {
      UnequalHidden,
      UnequalCollapsed,
      EqualHidden,
      EqualCollapsed,
    }
  }
}
