// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.RelativeFWCheckTimeConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  public class RelativeFWCheckTimeConverter : IValueConverter
  {
    public static readonly RelativeSyncTimeConverter Default = new RelativeSyncTimeConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((TimeSpan?) value, parameter == null || !(parameter is RelativeTimeFormat relativeTimeFormat) ? RelativeTimeFormat.Long : relativeTimeFormat, true);

    public string Convert(TimeSpan? value, RelativeTimeFormat format, bool showLabel)
    {
      string str = !value.HasValue ? RelativeTimeStrings.Never : RelativeTime.FormatRelativeTime(value.Value, format);
      if (showLabel)
        str = string.Format("{0} {1}", (object) Strings.Title_LastFWCheck, (object) str);
      return str;
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
