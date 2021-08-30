// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.VersionConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (FirmwareVersions), typeof (string))]
  public class VersionConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return (object) null;
      Type type = value.GetType();
      if (type == typeof (FirmwareVersions))
        return (object) this.Convert(value as FirmwareVersions);
      if (!(type == typeof (string)))
        return (object) null;
      string input = (string) value;
      try
      {
        return (object) this.Convert(Version.Parse(input));
      }
      catch
      {
        return (object) input;
      }
    }

    public string Convert(FirmwareVersions value) => string.Format(Strings.Format_FirmwareVersion, (object) value.ApplicationVersion, (object) value.PcbId, value.ApplicationVersion.Debug ? (object) Strings.FirmwareVersionDebugAbbr : (object) Strings.FirmwareVersionReleaseAbbr);

    public string Convert(Version value) => string.Format(Strings.Format_FirmwareVersionShort, (object) value);

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
