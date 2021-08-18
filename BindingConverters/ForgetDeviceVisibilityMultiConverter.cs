// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.ForgetDeviceVisibilityMultiConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  public class ForgetDeviceVisibilityMultiConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] is Guid && values[1] is bool ? (object) this.Convert((Guid) values[0], (bool) values[1]) : (object) Visibility.Hidden;

    public Visibility Convert(Guid deviceID, bool editing) => editing || !(deviceID != Guid.Empty) ? Visibility.Hidden : Visibility.Visible;

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
