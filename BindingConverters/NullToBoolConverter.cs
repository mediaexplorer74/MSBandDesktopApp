// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.NullToBoolConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (object), typeof (bool))]
  public class NullToBoolConverter : IValueConverter
  {
    public NullToBoolConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert(value, parameter == null || !(parameter is NullToBoolConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public bool Convert(object value, NullToBoolConverter.Mapping parameter) => parameter == NullToBoolConverter.Mapping.NullFalse || parameter != NullToBoolConverter.Mapping.NullTrue ? value != null : value == null;

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
      NullFalse,
      NullTrue,
    }
  }
}
