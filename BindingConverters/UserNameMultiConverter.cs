// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.UserNameMultiConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  public class UserNameMultiConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!(values[0] is string str1))
        str1 = "";
      string str2 = str1.Trim();
      if (!(values[1] is string str3))
        str3 = "";
      str3.Trim();
      stringBuilder.Append(str2);
      return (object) stringBuilder.ToString();
    }

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
