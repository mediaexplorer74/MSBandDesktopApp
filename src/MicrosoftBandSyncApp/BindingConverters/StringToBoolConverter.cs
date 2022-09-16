// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.StringToBoolConverter
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
  public class StringToBoolConverter : IValueConverter
  {
    public StringToBoolConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((string) value, parameter == null || !(parameter is StringToBoolConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public bool Convert(string value, StringToBoolConverter.Mapping parameter) => parameter == StringToBoolConverter.Mapping.BlankFalse || parameter != StringToBoolConverter.Mapping.NotBlankFalse ? value != null && value.Length > 0 : value == null || value.Length == 0;

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
      BlankFalse,
      NotBlankFalse,
    }
  }
}
