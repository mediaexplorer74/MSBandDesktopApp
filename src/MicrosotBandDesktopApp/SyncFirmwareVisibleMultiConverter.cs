// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.SyncFirmwareVisibleMultiConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  public class SyncFirmwareVisibleMultiConverter : IMultiValueConverter
  {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return !(values[0] is bool) || !(values[1] is FirmwareStatus) 
                ? (object)null 
                : (object)this.Convert((bool)values[0], (FirmwareStatus)values[1]);
        }

        private Visibility Convert(bool syncing, FirmwareStatus firmwareStatus)
        {
            return syncing || firmwareStatus == FirmwareStatus.Checking 
                           || firmwareStatus == FirmwareStatus.ReChecking 
                           || firmwareStatus == FirmwareStatus.Downloading 
                           || firmwareStatus == FirmwareStatus.Updating 
                ? Visibility.Hidden 
                : Visibility.Visible;
        }

        public object[] ConvertBack
        (
          object value,
          Type[] targetTypes,
          object parameter,
          CultureInfo culture
        )
        {
          throw new InvalidOperationException();
        }
  }
}
