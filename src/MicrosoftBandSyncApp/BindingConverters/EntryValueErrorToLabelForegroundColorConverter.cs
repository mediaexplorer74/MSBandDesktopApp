// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.EntryValueErrorToLabelForegroundColorConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (bool), typeof (Brush))]
  public class EntryValueErrorToLabelForegroundColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Color parameter1 = parameter != null ? (Color) parameter : Color.FromRgb((byte) 145, (byte) 131, (byte) 173);
      return (object) this.Convert((bool) value, parameter1);
    }

    public Color Convert(bool entryValueError, Color parameter) => entryValueError ? Colors.Red : parameter;

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
