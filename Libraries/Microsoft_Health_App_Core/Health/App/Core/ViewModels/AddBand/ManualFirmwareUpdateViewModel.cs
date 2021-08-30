// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.AddBand.ManualFirmwareUpdateViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.AddBand
{
  public class ManualFirmwareUpdateViewModel : PanelViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\AddBand\\ManualFirmwareUpdateViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IFirmwareUpdateService firmwareUpdateService;
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService smoothNavService;

    public ManualFirmwareUpdateViewModel(
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IMessageBoxService messageBoxService,
      IFirmwareUpdateService firmwareUpdateService,
      INetworkService networkService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.messageBoxService = messageBoxService;
      this.firmwareUpdateService = firmwareUpdateService;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> navigationParameters = null) => await this.PerformCheckForFirmwareUpdateAsync();

    private async Task PerformCheckForFirmwareUpdateAsync()
    {
      if (await this.CheckForFirmwareUpdateAsync())
      {
        if (await this.messageBoxService.ShowAsync(AppResources.FirmwareUpdateMessageBoxOptionalUpdateMessage, AppResources.FirmwareUpdateMessageBoxOptionalUpdateHeader, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK)
          this.smoothNavService.Navigate(typeof (FirmwareUpdateViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "Type",
              "Normal"
            }
          });
        else
          this.smoothNavService.Navigate(typeof (TilesViewModel));
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.FirmwareUpdateMessageBoxBandUpToDateMessage, AppResources.FirmwareUpdateMessageBoxBandUpToDateHeader, PortableMessageBoxButton.OK);
        this.smoothNavService.Navigate(typeof (TilesViewModel));
      }
      this.smoothNavService.ClearBackStack();
    }

    private async Task<bool> CheckForFirmwareUpdateAsync()
    {
      try
      {
        return await this.firmwareUpdateService.CheckForFirmwareUpdateAsync(new CancellationTokenSource(TimeSpan.FromMinutes(1.0)).Token);
      }
      catch (Exception ex)
      {
        ManualFirmwareUpdateViewModel.Logger.Error((object) ex);
      }
      await this.errorHandlingService.HandleExceptionAsync(ex);
      return false;
    }
  }
}
