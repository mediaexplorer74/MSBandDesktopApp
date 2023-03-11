// ThemeColorPickerControl.cs
// Type: DesktopSyncApp.ThemeColorPickerControl
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace DesktopSyncApp
{
  public partial class ThemeColorPickerControl : SyncAppPageControl
  {

    public ThemeColorPickerControl(AppMainWindow parent)
      : base(parent, false)
    {
      this.InitializeComponent();

      if ((int) this.model.ThemeManager.CurrentBandClass == 2)
      {
        this.PatternButton.Width = 137.0;
        this.PatternButton.Height = 58.0;
        this.DeviceEnvoy.Visibility = Visibility.Visible;
        this.DeviceCargo.Visibility = Visibility.Hidden;
      }
      else
      {
        this.DeviceEnvoy.Visibility = Visibility.Hidden;
        this.DeviceCargo.Visibility = Visibility.Visible;
      }
    }
              

    private async void SaveCustomization_Click(object sender, RoutedEventArgs e)
    {
      using (new DisposableAction((Action) (() => this.model.ThemeManager.UpdatingDeviceTheme = true), 
          (Action) (() => this.model.ThemeManager.UpdatingDeviceTheme = false)))
      {
        await this.model.DeviceManager.SaveThemeToBand(
            this.model.DeviceManager.CurrentDevice.CargoClient, 
            this.model.DeviceManager.CurrentDevice, false);
        this.model.ShowSyncCommand.Execute((object) null);
      }
    }

   
  }
}
