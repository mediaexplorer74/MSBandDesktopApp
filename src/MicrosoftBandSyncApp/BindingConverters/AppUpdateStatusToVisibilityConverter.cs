﻿// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.AppUpdateStatusToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (AppUpdateStatus), typeof (Visibility))]
  public class AppUpdateStatusToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((AppUpdateStatus) value);

    public Visibility Convert(AppUpdateStatus value) => value == AppUpdateStatus.Checking ? Visibility.Visible : Visibility.Hidden;

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
