// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.GenderComboBoxIndexConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (Gender), typeof (int))]
  public class GenderComboBoxIndexConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((Gender) value);

    public int Convert(Gender value)
    {
      if (value == null)
        return 1;
      return 0;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) this.ConvertBack((int) value);
    }

    public Gender ConvertBack(int value) => value == 0 || value != 1 ? (Gender) 1 : (Gender) 0;
  }
}
