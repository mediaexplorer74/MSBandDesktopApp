// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.FirmwareStatusToStatusStringConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (FirmwareStatus), typeof (string))]
  public class FirmwareStatusToStatusStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((FirmwareStatus) value);

    public string Convert(FirmwareStatus value)
    {
      switch (value)
      {
        case FirmwareStatus.Unknown:
          return LStrings.Status_FW_Unknown;
        case FirmwareStatus.UpToDate:
          return LStrings.Status_FW_UpToDate;
        case FirmwareStatus.Checking:
        case FirmwareStatus.ReChecking:
          return LStrings.Status_FW_Checking;
        case FirmwareStatus.Available:
          return LStrings.Status_FW_Available;
        case FirmwareStatus.Downloading:
          return LStrings.Status_FW_Downloading;
        case FirmwareStatus.ReadyToUpdate:
          return LStrings.Status_FW_ReadyToUpdate;
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
