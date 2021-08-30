// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.StringToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (string), typeof (Visibility))]
  public class StringToVisibilityConverter : IValueConverter
  {
    public StringToVisibilityConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((string) value, parameter == null || !(parameter is StringToVisibilityConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public Visibility Convert(string value, StringToVisibilityConverter.Mapping parameter)
    {
      switch (parameter)
      {
        case StringToVisibilityConverter.Mapping.BlankCollapsed:
          return value == null || value.Length <= 0 ? Visibility.Collapsed : Visibility.Visible;
        case StringToVisibilityConverter.Mapping.NotBlankHidden:
          switch (value)
          {
            case "":
            case null:
              return Visibility.Visible;
            default:
              return Visibility.Hidden;
          }
        case StringToVisibilityConverter.Mapping.NotBlankCollapsed:
          switch (value)
          {
            case "":
            case null:
              return Visibility.Visible;
            default:
              return Visibility.Collapsed;
          }
        default:
          return value == null || value.Length <= 0 ? Visibility.Hidden : Visibility.Visible;
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
      BlankHidden,
      BlankCollapsed,
      NotBlankHidden,
      NotBlankCollapsed,
    }
  }
}
