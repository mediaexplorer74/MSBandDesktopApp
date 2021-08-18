// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.TrayIcon
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using DesktopSyncApp.BindingConverters;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DesktopSyncApp
{
  internal class TrayIcon : IDisposable
  {
    private static readonly Icon[] SyncAnimationFrames = new Icon[3]
    {
      Icons.syncing_frame_01,
      Icons.syncing_frame_02,
      Icons.syncing_frame_03
    };
    private AppMainWindow owner;
    private ViewModel model;
    private NotifyIcon icon;
    private int syncAnimationFrame;
    private DispatcherTimer syncAnimationTimer;
    private DispatcherTimer doubleClickTimer;
    private DispatcherTimer balloonClickTimer;
    private bool closeWindowWarningShowing;
    private string previousBalloonTip = "";
    private ToolStripMenuItem miClose;

    public TrayIcon(AppMainWindow owner)
    {
      this.owner = owner;
      this.model = owner.Model;
      this.icon = new NotifyIcon();
      this.syncAnimationTimer = new DispatcherTimer();
      this.doubleClickTimer = new DispatcherTimer();
      this.balloonClickTimer = new DispatcherTimer()
      {
        Interval = TimeSpan.FromMilliseconds(50.0)
      };
      this.icon.Icon = Icons.app_icon;
      this.syncAnimationTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
      this.syncAnimationTimer.Tick += new EventHandler(this.syncAnimationTimer_Tick);
      this.doubleClickTimer.Tick += new EventHandler(this.doubleClickTimer_Tick);
      this.balloonClickTimer.Tick += new EventHandler(this.balloonClickTimer_Tick);
      this.icon.Text = Strings.Title_TrayIconToolTip_LoginToStart;
      this.icon.BalloonTipTitle = Strings.Title_MainWindow;
      this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_LoginToStart;
      this.icon.Visible = true;
      this.icon.ContextMenuStrip = new ContextMenuStrip();
      this.icon.Click += new EventHandler(this.Icon_Click);
      this.icon.DoubleClick += new EventHandler(this.Icon_DoubleClick);
      this.icon.BalloonTipClicked += new EventHandler(this.icon_BalloonTipClicked);
      this.icon.BalloonTipClosed += new EventHandler(this.icon_BalloonTipClosed);
      ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(Strings.Title_Command_Open, (Image) null, new EventHandler(this.ContextMenuItem_Open_Click));
      toolStripMenuItem.Font = new Font(toolStripMenuItem.Font, toolStripMenuItem.Font.Style | System.Drawing.FontStyle.Bold);
      this.icon.ContextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem);
      this.icon.ContextMenuStrip.Items.Add("-");
      this.miClose = new ToolStripMenuItem(Strings.Title_Command_Close, (Image) null, new EventHandler(this.ContextMenuItem_Close_Click));
      this.icon.ContextMenuStrip.Items.Add((ToolStripItem) this.miClose);
      this.model.DeviceManager.CurrentDeviceChanged += new PropertyChangedEventHandler(this.DeviceManager_CurrentDeviceChanged);
      this.model.UserDeviceStatusChanged += new PropertyValueChangedEventHandler(this.model_UserDeviceStatusChanged);
      this.model.LoginLogoutStatusChanged += new PropertyValueChangedEventHandler(this.model_LoginLogoutStatusChanged);
      this.model.AppVisibilityChanged += new PropertyValueChangedEventHandler(this.model_AppVisibilityChanged);
    }

    private void DeviceManager_CurrentDeviceChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.model.DeviceManager.CurrentDevice == null)
        return;
      this.model.DeviceManager.CurrentDevice.SyncingChanged += new PropertyChangedEventHandler(this.CurrentDevice_SyncingChanged);
    }

    private void CurrentDevice_SyncingChanged(object sender, PropertyChangedEventArgs e) => this.model_UserDeviceStatusChanged(sender, new PropertyValueChangedEventArgs((string) null, (object) this.model.UserDeviceStatus, (object) this.model.UserDeviceStatus));

    private void icon_BalloonTipClicked(object sender, EventArgs e)
    {
      this.balloonClickTimer.Stop();
      this.balloonClickTimer.Start();
    }

    private void icon_BalloonTipClosed(object sender, EventArgs e)
    {
      if (!this.closeWindowWarningShowing)
        return;
      this.icon.BalloonTipText = this.previousBalloonTip;
      this.closeWindowWarningShowing = false;
    }

    private void model_LoginLogoutStatusChanged(object sender, PropertyValueChangedEventArgs args)
    {
      switch ((LoginLogoutStatus) args.NewValue)
      {
        case LoginLogoutStatus.LoggedOut:
          this.syncAnimationTimer.Stop();
          this.icon.Icon = Icons.app_icon;
          this.icon.Text = Strings.Title_TrayIconToolTip_LoginToStart;
          this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_LoginToStart;
          this.closeWindowWarningShowing = false;
          break;
        case LoginLogoutStatus.LoggedIn:
          this.model_UserDeviceStatusChanged(sender, new PropertyValueChangedEventArgs((string) null, (object) UserDeviceStatus.None, (object) this.model.UserDeviceStatus));
          break;
      }
    }

    private void model_UserDeviceStatusChanged(object sender, PropertyValueChangedEventArgs args)
    {
      bool flag = true;
      if (this.model.LoginLogoutStatus != LoginLogoutStatus.LoggedIn)
        return;
      if (this.model.DeviceManager.CurrentDevice != null && this.model.DeviceManager.CurrentDevice.LastSyncError != null)
      {
        this.syncAnimationTimer.Stop();
        this.icon.Icon = Icons.error_red;
        this.icon.Text = Strings.Title_TrayIconToolTip_Error;
        this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_Error;
        this.closeWindowWarningShowing = false;
        if (!this.closeWindowWarningShowing && !this.model.IsAppRestored)
          this.icon.ShowBalloonTip(2500);
      }
      else
      {
        switch ((UserDeviceStatus) args.NewValue)
        {
          case UserDeviceStatus.None:
          case UserDeviceStatus.CantRegisterReset:
          case UserDeviceStatus.CantRegisterUnregister:
          case UserDeviceStatus.CantRegisterUnregisterReset:
          case UserDeviceStatus.CanRegister:
            this.syncAnimationTimer.Stop();
            this.icon.Icon = Icons.app_icon;
            this.icon.Text = Strings.Title_TrayIconToolTip_DeviceAbsent;
            this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_DeviceAbsent;
            this.closeWindowWarningShowing = false;
            if (!this.closeWindowWarningShowing && !this.model.IsAppRestored)
            {
              this.icon.ShowBalloonTip(2500);
              break;
            }
            break;
          case UserDeviceStatus.Multiple:
            this.syncAnimationTimer.Stop();
            this.icon.Icon = Icons.app_icon;
            this.icon.Text = Strings.Title_TrayIconToolTip_MultipleDevices;
            this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_MultipleDevices;
            this.closeWindowWarningShowing = false;
            if (!this.closeWindowWarningShowing && !this.model.IsAppRestored)
            {
              this.icon.ShowBalloonTip(2500);
              break;
            }
            break;
          case UserDeviceStatus.Registered:
            if (this.model.DeviceManager.CurrentDevice.Syncing)
            {
              this.syncAnimationTimer.Start();
              this.syncAnimationFrame = 0;
              this.icon.Icon = TrayIcon.SyncAnimationFrames[this.syncAnimationFrame];
              this.icon.Text = Strings.Title_TrayIconToolTip_Syncing;
              this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_Syncing;
              this.closeWindowWarningShowing = false;
              if (!this.closeWindowWarningShowing && !this.model.IsAppRestored)
              {
                this.icon.ShowBalloonTip(2500);
                break;
              }
              break;
            }
            this.syncAnimationTimer.Stop();
            this.icon.Icon = Icons.app_icon;
            this.icon.Text = Strings.Title_TrayIconToolTip_DevicePresent;
            this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_DevicePresent;
            this.closeWindowWarningShowing = false;
            if (!this.closeWindowWarningShowing && !this.model.IsAppRestored && ((UserDeviceStatus) args.OldValue == UserDeviceStatus.None || (UserDeviceStatus) args.OldValue == UserDeviceStatus.Multiple))
            {
              this.icon.ShowBalloonTip(2500);
              break;
            }
            break;
          case UserDeviceStatus.RegisteredRequiresFW:
            this.icon.Icon = Icons.app_icon;
            this.icon.Text = Strings.Title_TrayIconToolTip_FirmwareUpdateRequired;
            this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_FirmwateUpdateRequired;
            this.closeWindowWarningShowing = false;
            if (!this.closeWindowWarningShowing && !this.model.IsAppRestored)
            {
              this.icon.ShowBalloonTip(2500);
              break;
            }
            break;
          case UserDeviceStatus.RegisteredFWUpdating:
            this.syncAnimationTimer.Start();
            this.syncAnimationFrame = 0;
            this.icon.Icon = TrayIcon.SyncAnimationFrames[this.syncAnimationFrame];
            this.icon.Text = Strings.Title_TrayIconToolTip_UpdatingFirmware;
            this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_UpdatingFirmware;
            this.closeWindowWarningShowing = false;
            if (!this.closeWindowWarningShowing && !this.model.IsAppRestored)
              this.icon.ShowBalloonTip(2500);
            flag = false;
            break;
        }
      }
      this.miClose.Enabled = flag;
    }

    private void model_AppVisibilityChanged(object sender, PropertyValueChangedEventArgs args)
    {
      if (!this.model.DynamicSettings.ShowCloseWindowWarning || (Visibility) args.NewValue != Visibility.Hidden)
        return;
      if (!this.closeWindowWarningShowing)
      {
        this.previousBalloonTip = this.icon.BalloonTipText;
        this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_CloseWindowWarning;
        this.closeWindowWarningShowing = true;
      }
      this.icon.ShowBalloonTip(5000);
    }

    private void model_LastLogSyncRelativeTimeChanged(object sender, PropertyChangedEventArgs e)
    {
      TimeSpan? nullable = new TimeSpan?();
      if (this.model.DeviceManager.CurrentDevice != null)
        nullable = this.model.DeviceManager.CurrentDevice.LastLogSyncRelativeTime;
      if (nullable.HasValue)
      {
        this.icon.Text = RelativeSyncTimeConverter.Default.Convert(nullable, RelativeTimeFormat.Long, false);
        this.icon.BalloonTipText = RelativeSyncTimeConverter.Default.Convert(nullable, RelativeTimeFormat.Long, false);
      }
      else
      {
        this.icon.Text = Strings.Title_TrayIconToolTip_DevicePresent;
        this.icon.BalloonTipText = Strings.Text_TrayIconBalloonTip_DevicePresent;
      }
    }

    private void syncAnimationTimer_Tick(object sender, EventArgs e)
    {
      ++this.syncAnimationFrame;
      if (this.syncAnimationFrame == TrayIcon.SyncAnimationFrames.Length)
        this.syncAnimationFrame = 0;
      this.icon.Icon = TrayIcon.SyncAnimationFrames[this.syncAnimationFrame];
    }

    private void doubleClickTimer_Tick(object sender, EventArgs e)
    {
      this.doubleClickTimer.Stop();
      this.icon.ShowBalloonTip(5000);
    }

    private void balloonClickTimer_Tick(object sender, EventArgs e)
    {
      if (!this.closeWindowWarningShowing)
        return;
      this.icon.BalloonTipText = this.previousBalloonTip;
      this.closeWindowWarningShowing = false;
      this.model.DynamicSettings.ShowCloseWindowWarning = false;
    }

    private void Icon_Click(object Target, EventArgs oe)
    {
      this.balloonClickTimer.Stop();
      if ((oe as MouseEventArgs).Button != MouseButtons.Left)
        return;
      this.doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, SystemInformation.DoubleClickTime + 50);
      this.doubleClickTimer.Start();
    }

    private void Icon_DoubleClick(object Target, EventArgs oe)
    {
      MouseEventArgs mouseEventArgs = oe as MouseEventArgs;
      this.doubleClickTimer.Stop();
      if (mouseEventArgs.Button != MouseButtons.Left)
        return;
      this.model.ShowAppCommand.Execute((object) null);
    }

    private void ContextMenuItem_Open_Click(object Target, EventArgs e) => this.model.ShowAppCommand.Execute((object) null);

    private void ContextMenuItem_Close_Click(object Target, EventArgs e) => this.model.CloseAppCommand.Execute((object) false);

    public void Dispose()
    {
      if (this.icon == null)
        return;
      this.icon.Dispose();
      this.icon = (NotifyIcon) null;
    }
  }
}
