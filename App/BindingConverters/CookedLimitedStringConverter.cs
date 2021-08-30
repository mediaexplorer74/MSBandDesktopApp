// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.CookedLimitedStringConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (string), typeof (string))]
  public class CookedLimitedStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value;

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) this.ConvertBack((string) value, (int) parameter);
    }

    public string ConvertBack(string raw, int maxLength)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (char ch in raw.Trim())
      {
        switch (ch)
        {
          case '\t':
          case ' ':
            if (!flag)
            {
              flag = true;
              stringBuilder.Append(' ');
              break;
            }
            break;
          default:
            flag = false;
            stringBuilder.Append(ch);
            break;
        }
      }
      return stringBuilder.Length <= maxLength ? stringBuilder.ToString() : stringBuilder.ToString().Substring(0, maxLength).TrimEnd();
    }
  }
}
