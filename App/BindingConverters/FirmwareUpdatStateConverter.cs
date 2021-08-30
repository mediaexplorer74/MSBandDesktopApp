// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.FirmwareUpdatStateConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (double?), typeof (string))]
  public class FirmwareUpdatStateConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((FirmwareUpdateState?) value);

    public string Convert(FirmwareUpdateState? value)
    {
      if (!value.HasValue)
        return "";
      switch ((int) value.Value)
      {
        case 0:
          return "";
        case 1:
          return Strings.Status_FirmwareUpdate_Downloading;
        case 2:
          return Strings.Status_FirmwareUpdate_SyncingLog;
        case 3:
          return Strings.Status_FirmwareUpdate_BootingToUpdate;
        case 4:
          return Strings.Status_FirmwareUpdate_Uploading;
        case 5:
          return Strings.Status_FirmwareUpdate_WaitingForReconnect;
        case 6:
          return "";
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
