// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.StringTrimmedToVisibilityConverter
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
  public class StringTrimmedToVisibilityConverter : IValueConverter
  {
    public StringTrimmedToVisibilityConverter.Mapping DefaultMapping { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (object)this.Convert
                (
                    (string)value, parameter == null 
                    || !(parameter is StringTrimmedToVisibilityConverter.Mapping mapping) 
                        ? this.DefaultMapping 
                        : mapping
                );
        }

        public Visibility Convert
        (
            string value,
            StringTrimmedToVisibilityConverter.Mapping parameter
        )
        {
          switch (parameter)
          {
            case StringTrimmedToVisibilityConverter.Mapping.BlankCollapsed:
              return value == null || value.Trim().Length <= 0 ? Visibility.Collapsed : Visibility.Visible;
            case StringTrimmedToVisibilityConverter.Mapping.NotBlankHidden:
              return value != null && value.Trim().Length != 0 ? Visibility.Hidden : Visibility.Visible;
            case StringTrimmedToVisibilityConverter.Mapping.NotBlankCollapsed:
              return value != null && value.Trim().Length != 0 ? Visibility.Collapsed : Visibility.Visible;
            default:
              return value == null || value.Trim().Length <= 0 ? Visibility.Hidden : Visibility.Visible;
          }
        }

        public object ConvertBack
        (
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture
        )
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
