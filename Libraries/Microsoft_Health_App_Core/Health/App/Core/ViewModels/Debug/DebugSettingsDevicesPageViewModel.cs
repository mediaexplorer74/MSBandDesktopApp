// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugSettingsDevicesPageViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public sealed class DebugSettingsDevicesPageViewModel : DebugSettingsPageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Debug\\DebugSettingsDevicesPageViewModel.cs");
    private readonly IEnvironmentService applicationEnvironmentService;
    private readonly IFirmwareUpdateService firmwareUpdateService;
    private readonly IOobeService oobeService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IDebuggableHttpCacheService cacheService;
    private readonly IMessageSender messageSender;
    private readonly IConfig config;
    private bool isResetButtonEnabled;

    public DebugSettingsDevicesPageViewModel(
      IConfig config,
      ISmoothNavService smoothNavService,
      IFirmwareUpdateService firmwareUpdateService,
      INetworkService networkService,
      IOobeService oobeService,
      IEnvironmentService applicationEnvironmentService,
      IDebuggableHttpCacheService cacheService,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.config = config;
      this.smoothNavService = smoothNavService;
      this.IsResetButtonEnabled = true;
      this.firmwareUpdateService = firmwareUpdateService;
      this.oobeService = oobeService;
      this.applicationEnvironmentService = applicationEnvironmentService;
      this.cacheService = cacheService;
      this.messageSender = messageSender;
      this.ResetFreCommand = (ICommand) new HealthCommand(new Action(this.ResetFre));
      this.UpdateFirmwareCommand = (ICommand) new HealthCommand(new Action(this.UpdateFirmware));
      this.ClearCachedFirmwareUpdateCommand = (ICommand) new HealthCommand(new Action(this.ClearCachedFirmwareUpdateInfo));
      this.Header = "Devices";
      this.SubHeader = "Band, firmware";
      this.GlyphIcon = "\uE175";
    }

    private async void ClearCachedFirmwareUpdateInfo() => await this.firmwareUpdateService.ClearCachedFirmwareUpdateInfoAsync(CancellationToken.None);

    public bool IsResetButtonEnabled
    {
      get => this.isResetButtonEnabled;
      set => this.SetProperty<bool>(ref this.isResetButtonEnabled, value, nameof (IsResetButtonEnabled));
    }

    public bool IsFirmwareUpdateCheckingEnabled
    {
      get => this.config.IsFirmwareUpdateCheckingEnabled;
      set => this.config.IsFirmwareUpdateCheckingEnabled = value;
    }

    public ICommand ResetFreCommand { get; private set; }

    public ICommand UpdateFirmwareCommand { get; private set; }

    public ICommand ClearCachedFirmwareUpdateCommand { get; private set; }

    private async void ResetFre()
    {
      this.IsResetButtonEnabled = false;
      int num = await this.oobeService.ResetOobeStatusAsync() ? 1 : 0;
      this.IsResetButtonEnabled = true;
    }

    private void UpdateFirmware()
    {
      if (this.applicationEnvironmentService.IsEmulated)
        this.smoothNavService.Navigate(typeof (FirmwareUpdateViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Type",
            "Normal"
          }
        });
      else
        this.smoothNavService.Navigate(typeof (ManualFirmwareUpdateViewModel));
    }
  }
}
