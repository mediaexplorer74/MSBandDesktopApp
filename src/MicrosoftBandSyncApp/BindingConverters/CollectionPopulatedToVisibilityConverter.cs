// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.CollectionPopulatedToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (ICollection), typeof (Visibility))]
  public class CollectionPopulatedToVisibilityConverter : IValueConverter
  {
    public CollectionPopulatedToVisibilityConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((ICollection) value, parameter == null || !(parameter is CollectionPopulatedToVisibilityConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public Visibility Convert(
      ICollection value,
      CollectionPopulatedToVisibilityConverter.Mapping parameter)
    {
      switch (parameter)
      {
        case CollectionPopulatedToVisibilityConverter.Mapping.EmptyCollapsed:
          return value == null || value.Count <= 0 ? Visibility.Collapsed : Visibility.Visible;
        case CollectionPopulatedToVisibilityConverter.Mapping.PopulatedHidden:
          return value != null && value.Count != 0 ? Visibility.Hidden : Visibility.Visible;
        case CollectionPopulatedToVisibilityConverter.Mapping.PopulatedCollapsed:
          return value != null && value.Count != 0 ? Visibility.Collapsed : Visibility.Visible;
        default:
          return value == null || value.Count <= 0 ? Visibility.Hidden : Visibility.Visible;
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
      EmptyHidden,
      EmptyCollapsed,
      PopulatedHidden,
      PopulatedCollapsed,
    }
  }
}
