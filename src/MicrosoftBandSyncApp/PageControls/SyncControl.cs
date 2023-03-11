// SyncControl.cs
// Type: DesktopSyncApp.SyncControl
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
using System.Windows.Threading;

namespace DesktopSyncApp
{
  public partial class SyncControl : SyncAppPageControl
  {
    private DispatcherTimer chargeBlinkTimer;


    public SyncControl(AppMainWindow parent)
      : base(parent, true)
    {
      this.InitializeComponent();
      this.chargeBlinkTimer = new DispatcherTimer();
      this.chargeBlinkTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
      this.chargeBlinkTimer.Tick += new EventHandler(this.chargeBlinkTimer_Tick);

      this.model.DeviceManager.CurrentDeviceChanged += 
                new PropertyChangedEventHandler(this.DeviceManager_CurrentDeviceChanged);
    }

    public override void OnShow() => base.OnShow();

    private void DeviceManager_CurrentDeviceChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.model.DeviceManager.CurrentDevice != null)
      {
        this.model.DeviceManager.CurrentDevice.BatteryPercentChargeChanged += 
                    new PropertyValueChangedEventHandler(this.CurrentDevice_BatteryPercentChargeChanged);

        if (this.model.DeviceManager.CurrentDevice.BatteryPercentCharge.HasValue)
          return;
        this.chargeBlinkTimer.Start();
      }
      else
        this.chargeBlinkTimer.Stop();
    }

    private void CurrentDevice_BatteryPercentChargeChanged(
      object sender,
      PropertyValueChangedEventArgs e)
    {
      if (e.NewValue == null)
      {
        this.chargeBlinkTimer.Start();
      }
      else
      {
        if (e.OldValue != null)
          return;
        this.chargeBlinkTimer.Stop();
        this.model.BatteryPlugVisible = true;
      }
    }

    private void chargeBlinkTimer_Tick(object sender, EventArgs e) 
            => this.model.BatteryPlugVisible = !this.model.BatteryPlugVisible;

    private async void btSyncSensorLogs_Click(object sender, RoutedEventArgs e) 
            => await this.parent.SyncDeviceToCloud();

    private void Button_Click(object sender, RoutedEventArgs e) 
            => this.parent.CancelSensorLogSync();

     
  }
}
