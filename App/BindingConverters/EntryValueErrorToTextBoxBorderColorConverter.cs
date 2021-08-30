// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.EntryValueErrorToTextBoxBorderColorConverter
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
  public class EntryValueErrorToTextBoxBorderColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((bool) value);

    public Color Convert(bool entryValueError) => entryValueError ? Colors.Red : Color.FromRgb((byte) 171, (byte) 173, (byte) 179);

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
