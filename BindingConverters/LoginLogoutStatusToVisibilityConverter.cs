// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.LoginLogoutStatusToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (LoginLogoutStatus), typeof (Visibility))]
  public class LoginLogoutStatusToVisibilityConverter : IValueConverter
  {
    public LoginLogoutStatusToVisibilityConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((LoginLogoutStatus) value, parameter == null || !(parameter is LoginLogoutStatusToVisibilityConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public Visibility Convert(
      LoginLogoutStatus value,
      LoginLogoutStatusToVisibilityConverter.Mapping parameter)
    {
      switch (parameter)
      {
        case LoginLogoutStatusToVisibilityConverter.Mapping.LoggedOutCollapsed:
          return value != LoginLogoutStatus.LoggedIn ? Visibility.Collapsed : Visibility.Visible;
        case LoginLogoutStatusToVisibilityConverter.Mapping.LoggedInHidden:
          return value == LoginLogoutStatus.LoggedIn ? Visibility.Hidden : Visibility.Visible;
        case LoginLogoutStatusToVisibilityConverter.Mapping.LoggedInCollapsed:
          return value == LoginLogoutStatus.LoggedIn ? Visibility.Collapsed : Visibility.Visible;
        default:
          return value != LoginLogoutStatus.LoggedIn ? Visibility.Hidden : Visibility.Visible;
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
      LoggedOutHidden,
      LoggedOutCollapsed,
      LoggedInHidden,
      LoggedInCollapsed,
    }
  }
}
