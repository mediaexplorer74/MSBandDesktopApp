// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.DateTimeAndFormatMultiConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  public class DateTimeAndFormatMultiConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(values[0] is DateTime?))
        return (object) null;
      DateTime? nullable = (DateTime?) values[0];
      if (!nullable.HasValue)
        return (object) "";
      string format = "h:mm";
      if (values[1] is DisplayTimeFormat)
      {
        switch ((DisplayTimeFormat) values[1] - 1)
        {
          case 0:
            format = "HH:mm";
            break;
          case 1:
            format = "H:mm";
            break;
          case 2:
            format = "hh:mm";
            break;
          case 3:
            format = "h:mm";
            break;
        }
      }
      return (object) nullable.Value.ToString(format);
    }

    public object[] ConvertBack(
      object value,
      Type[] targetTypes,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException();
    }
  }
}
