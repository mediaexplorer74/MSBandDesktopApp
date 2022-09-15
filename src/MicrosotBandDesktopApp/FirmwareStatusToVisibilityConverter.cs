// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.FirmwareStatusToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (FirmwareStatus), typeof (Visibility))]
  public class FirmwareStatusToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((FirmwareStatus) value);

    public Visibility Convert(FirmwareStatus value)
    {
      switch (value)
      {
        case FirmwareStatus.Checking:
        case FirmwareStatus.ReChecking:
        case FirmwareStatus.Downloading:
          return Visibility.Visible;
        default:
          return Visibility.Hidden;
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
