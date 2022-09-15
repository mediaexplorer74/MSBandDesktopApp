// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.DynamicSettings
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows;

namespace DesktopSyncApp
{
  public sealed class DynamicSettings : ApplicationSettingsBase
  {
    private static readonly string nameMainWindowPosition = nameof (MainWindowPosition);
    private static readonly string nameStartOnLogin = nameof (StartOnLogin);
    private static readonly string nameAutoSync = nameof (AutoSync);
    private static readonly string nameShowCloseWarning = nameof (ShowCloseWindowWarning);
    private static readonly string nameLastLogSync = nameof (LastLogSync);

    public new event PropertyChangedEventHandler PropertyChanged;

    public event PropertyValueChangedEventHandler StartOnLoginChanged;

    public event PropertyValueChangedEventHandler AutoSyncChanged;

    private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
    }

    [SettingsProvider(typeof (RegistrySettingsProvider))]
    [UserScopedSetting]
    [DefaultSettingValue(null)]
    public Point? MainWindowPosition
    {
      get => (Point?) this[DynamicSettings.nameMainWindowPosition];
      set => this[DynamicSettings.nameMainWindowPosition] = (object) value;
    }

    [SettingsProvider(typeof (RegistrySettingsProvider))]
    [UserScopedSetting]
    [DefaultSettingValue("True")]
    public bool StartOnLogin
    {
      get => (bool) this[DynamicSettings.nameStartOnLogin];
      set
      {
        bool flag = (bool) this[DynamicSettings.nameStartOnLogin];
        if (flag == value)
          return;
        this[DynamicSettings.nameStartOnLogin] = (object) value;
        this.OnPropertyChanged(DynamicSettings.nameStartOnLogin, this.PropertyChanged, this.StartOnLoginChanged, (object) flag, (object) value);
        Telemetry.LogEvent("Settings/App/AutoLaunch", (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Value",
            value.ToString()
          }
        }, (IDictionary<string, double>) null);
      }
    }

    [SettingsProvider(typeof (RegistrySettingsProvider))]
    [UserScopedSetting]
    [DefaultSettingValue("True")]
    public bool AutoSync
    {
      get => (bool) this[DynamicSettings.nameAutoSync];
      set
      {
        bool flag = (bool) this[DynamicSettings.nameAutoSync];
        if (flag == value)
          return;
        this[DynamicSettings.nameAutoSync] = (object) value;
        this.OnPropertyChanged(DynamicSettings.nameAutoSync, this.PropertyChanged, this.AutoSyncChanged, (object) flag, (object) value);
        Telemetry.LogEvent("Settings/App/AutoSync", (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Value",
            value.ToString()
          }
        }, (IDictionary<string, double>) null);
      }
    }

    [SettingsProvider(typeof (RegistrySettingsProvider))]
    [UserScopedSetting]
    [DefaultSettingValue("True")]
    public bool ShowCloseWindowWarning
    {
      get => (bool) this[DynamicSettings.nameShowCloseWarning];
      set
      {
        if ((bool) this[DynamicSettings.nameShowCloseWarning] == value)
          return;
        this[DynamicSettings.nameShowCloseWarning] = (object) value;
        this.OnPropertyChanged(DynamicSettings.nameShowCloseWarning, this.PropertyChanged);
      }
    }

    [SettingsProvider(typeof (RegistrySettingsProvider))]
    [UserScopedSetting]
    [DefaultSettingValue(null)]
    public DateTime? LastLogSync
    {
      get => (DateTime?) this[DynamicSettings.nameLastLogSync];
      set
      {
        DateTime? nullable1 = (DateTime?) this[DynamicSettings.nameLastLogSync];
        DateTime? nullable2 = value;
        if ((nullable1.HasValue == nullable2.HasValue ? (nullable1.HasValue ? (nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this[DynamicSettings.nameLastLogSync] = (object) value;
        this.OnPropertyChanged(DynamicSettings.nameLastLogSync, this.PropertyChanged);
      }
    }
  }
}
