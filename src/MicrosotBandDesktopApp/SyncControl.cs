// Decompiled with JetBrains decompiler
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
  public partial class SyncControl : SyncAppPageControl, IComponentConnector
  {
    private DispatcherTimer chargeBlinkTimer;
    internal TextBlock lblDeviceName;
    internal TextBlock lblProfileFullName;
    internal Image imgBatteryEmpty;
    internal Image imgBatteryCharging;
    private bool _contentLoaded;

    public SyncControl(AppMainWindow parent)
      : base(parent, true)
    {
      this.InitializeComponent();
      this.chargeBlinkTimer = new DispatcherTimer();
      this.chargeBlinkTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
      this.chargeBlinkTimer.Tick += new EventHandler(this.chargeBlinkTimer_Tick);
      this.model.DeviceManager.CurrentDeviceChanged += new PropertyChangedEventHandler(this.DeviceManager_CurrentDeviceChanged);
    }

    public override void OnShow() => base.OnShow();

    private void DeviceManager_CurrentDeviceChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.model.DeviceManager.CurrentDevice != null)
      {
        this.model.DeviceManager.CurrentDevice.BatteryPercentChargeChanged += new PropertyValueChangedEventHandler(this.CurrentDevice_BatteryPercentChargeChanged);
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

    private void chargeBlinkTimer_Tick(object sender, EventArgs e) => this.model.BatteryPlugVisible = !this.model.BatteryPlugVisible;

    private async void btSyncSensorLogs_Click(object sender, RoutedEventArgs e) => await this.parent.SyncDeviceToCloud();

    private void Button_Click(object sender, RoutedEventArgs e) => this.parent.CancelSensorLogSync();

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/synccontrol.xaml", UriKind.Relative));
    }
        */

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.lblDeviceName = (TextBlock) target;
          break;
        case 2:
          this.lblProfileFullName = (TextBlock) target;
          break;
        case 3:
          this.imgBatteryEmpty = (Image) target;
          break;
        case 4:
          this.imgBatteryCharging = (Image) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
        */
  }
}
