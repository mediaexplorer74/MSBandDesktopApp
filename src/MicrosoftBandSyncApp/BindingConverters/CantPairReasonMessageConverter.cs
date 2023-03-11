// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.CantPairReasonMessageConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (UserDeviceStatus), typeof (string))]
  public class CantPairReasonMessageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((UserDeviceStatus) value);

    public string Convert(UserDeviceStatus value)
    {
      switch (value)
      {
        case UserDeviceStatus.Multiple:
          return LStrings.Message_Page_Multiple;
        case UserDeviceStatus.CantRegisterReset:
          return LStrings.Message_Page_CantRegister_Reset;
        case UserDeviceStatus.CantRegisterUnregister:
          return LStrings.Message_Page_CantRegister_Unregister;
        case UserDeviceStatus.CantRegisterUnregisterReset:
          return LStrings.Message_Page_CantRegister_UnregisterReset;
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
