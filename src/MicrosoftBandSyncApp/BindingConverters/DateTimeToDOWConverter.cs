// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.DateTimeToDOWConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (DateTime?), typeof (string))]
  public class DateTimeToDOWConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((DateTime?) value);

    public string Convert(DateTime? value)
    {
      if (!value.HasValue)
        return "";
      switch (value.Value.DayOfWeek)
      {
        case DayOfWeek.Sunday:
          return Strings.DOW_Sunday;
        case DayOfWeek.Tuesday:
          return Strings.DOW_Tuesday;
        case DayOfWeek.Wednesday:
          return Strings.DOW_Wednesday;
        case DayOfWeek.Thursday:
          return Strings.DOW_Thursday;
        case DayOfWeek.Friday:
          return Strings.DOW_Friday;
        case DayOfWeek.Saturday:
          return Strings.DOW_Saturday;
        default:
          return Strings.DOW_Monday;
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
  }
}
