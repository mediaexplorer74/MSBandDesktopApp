// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.AppUpdateStatusToStatusStringConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (AppUpdateStatus), typeof (string))]
  public class AppUpdateStatusToStatusStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((AppUpdateStatus) value);

    public string Convert(AppUpdateStatus value)
    {
      switch (value)
      {
        case AppUpdateStatus.Unknown:
          return Strings.Status_AppUpdate_Unknown;
        case AppUpdateStatus.UpToDate:
          return Strings.Status_AppUpdate_UpToDate;
        case AppUpdateStatus.Checking:
          return Strings.Status_AppUpdate_Checking;
        case AppUpdateStatus.Available:
          return Strings.Status_AppUpdate_Available;
        default:
          return "";
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
