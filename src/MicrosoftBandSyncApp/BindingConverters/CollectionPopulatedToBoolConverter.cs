// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.CollectionPopulatedToBoolConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (ICollection), typeof (bool))]
  public class CollectionPopulatedToBoolConverter : IValueConverter
  {
    public CollectionPopulatedToBoolConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((ICollection) value, parameter == null || !(parameter is CollectionPopulatedToBoolConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public bool Convert(
      ICollection value,
      CollectionPopulatedToBoolConverter.Mapping parameter)
    {
      return parameter == CollectionPopulatedToBoolConverter.Mapping.EmptyFalse || parameter != CollectionPopulatedToBoolConverter.Mapping.EmptyTrue ? value != null && value.Count > 0 : value == null || value.Count == 0;
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
      EmptyFalse,
      EmptyTrue,
    }
  }
}
