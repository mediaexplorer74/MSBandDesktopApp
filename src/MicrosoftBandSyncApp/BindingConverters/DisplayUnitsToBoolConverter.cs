// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.DisplayUnitsToBoolConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (byte), typeof (bool))]
  public class DisplayUnitsToBoolConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((byte) value);

    public bool Convert(byte value) => value == (byte) 0;

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) this.ConvertBack((bool) value);
    }

    public byte ConvertBack(bool value) => !value ? (byte) 1 : (byte) 0;
  }
}
