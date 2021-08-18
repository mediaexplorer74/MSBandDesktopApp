// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.LoginLogoutStatusToBoolConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (LoginLogoutStatus), typeof (bool))]
  public class LoginLogoutStatusToBoolConverter : IValueConverter
  {
    public LoginLogoutStatusToBoolConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((LoginLogoutStatus) value, parameter == null || !(parameter is LoginLogoutStatusToBoolConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public bool Convert(LoginLogoutStatus value, LoginLogoutStatusToBoolConverter.Mapping parameter) => parameter == LoginLogoutStatusToBoolConverter.Mapping.LoggedOutFalse || parameter != LoginLogoutStatusToBoolConverter.Mapping.LoggedOutTrue ? value == LoginLogoutStatus.LoggedIn : value != LoginLogoutStatus.LoggedIn;

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
      LoggedOutFalse,
      LoggedOutTrue,
    }
  }
}
