// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.AddBand.FirmwareUpdateViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Band.Admin.WebTiles;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.ToastNotification;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.App.Core.ViewModels.Oobe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.AddBand
{
  [PageTaxonomy(new string[] {"App", "Firmware", "Update in progress"})]
  public class FirmwareUpdateViewModel : PageViewModelBase, IOobeViewModel
  {
    private const string CustomTileUpdateCategory = "CustomTileUpdate";
    private const string CustomTileLayoutUpdateFlag = "1";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\AddBand\\FirmwareUpdateViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IFirmwareUpdateService firmwareUpdateService;
    private readonly IAddBandService addBandService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private readonly ITileUpdateService syncManager;
    private readonly IMessageBoxService messageBoxService;
    private readonly IToastNotificationService toastNotificationService;
    private readonly ITileManagementService tileManagementService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IConfigProvider configProvider;
    private readonly IWebTileService webTileService;
    public static readonly string CustomTileLayoutUpdateKey = ConfigurationValue.CreateKey("CustomTileUpdate", "AreCustomTileLayoutsUpdated");
    private string type = "New";
    private string pageTitle;
    private string pageSubtitle;
    private string stepNumberText;
    private string stepText;
    private double percentageCompletion;
    private FirmwareUpdateViewModel.FirmwareUpdateStatus updateStatus;
    private AsyncHealthCommand cancelCommand;
    private AsyncHealthCommand retryCommand;
    private AsyncHealthCommand finishCommand;

    public FirmwareUpdateViewModel(
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IFirmwareUpdateService firmwareUpdateService,
      ITileUpdateService syncManager,
      INetworkService networkService,
      IAddBandService addBandService,
      IMessageSender messageSender,
      IMessageBoxService messageBoxService,
      IToastNotificationService toastNotificationService,
      IBandConnectionFactory cargoConnectionFactory,
      ITileManagementService tileManagementService,
      IConfigProvider configProvider,
      IWebTileService webTileService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.firmwareUpdateService = firmwareUpdateService;
      this.syncManager = syncManager;
      this.addBandService = addBandService;
      this.messageSender = messageSender;
      this.messageBoxService = messageBoxService;
      this.toastNotificationService = toastNotificationService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.tileManagementService = tileManagementService;
      this.configProvider = configProvider;
      this.webTileService = webTileService;
      this.cancelCommand = AsyncHealthCommand.Create(new Func<Task>(this.CancelAsync));
      this.retryCommand = AsyncHealthCommand.Create(new Func<Task>(this.RetryAsync));
      this.finishCommand = AsyncHealthCommand.Create(new Func<Task>(this.FinishAsync));
    }

    public string PageTitle
    {
      get => this.pageTitle;
      set => this.SetProperty<string>(ref this.pageTitle, value, nameof (PageTitle));
    }

    public string PageSubtitle
    {
      get => this.pageSubtitle;
      set => this.SetProperty<string>(ref this.pageSubtitle, value, nameof (PageSubtitle));
    }

    public string StepNumberText
    {
      get => this.stepNumberText;
      set => this.SetProperty<string>(ref this.stepNumberText, value, nameof (StepNumberText));
    }

    public string StepText
    {
      get => this.stepText;
      set => this.SetProperty<string>(ref this.stepText, value, nameof (StepText));
    }

    public double PercentageCompletion
    {
      get => this.percentageCompletion;
      set => this.SetProperty<double>(ref this.percentageCompletion, value, nameof (PercentageCompletion));
    }

    public FirmwareUpdateViewModel.FirmwareUpdateStatus UpdateStatus
    {
      get => this.updateStatus;
      set
      {
        this.SetProperty<FirmwareUpdateViewModel.FirmwareUpdateStatus>(ref this.updateStatus, value, nameof (UpdateStatus));
        this.RefreshTitle();
      }
    }

    public ICommand CancelCommand => (ICommand) this.cancelCommand;

    private async Task CancelAsync()
    {
      ApplicationTelemetry.LogFirmwareUpdateError("Cancel");
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        if (await this.messageBoxService.ShowAsync(AppResources.AddBandCancelDialogMessage, AppResources.ApplicationTitle, PortableMessageBoxButton.CasualBooleanChoice) == PortableMessageBoxResult.OK)
          await this.addBandService.ExitAsync(cancellationTokenSource.Token);
      }
    }

    public ICommand RetryCommand => (ICommand) this.retryCommand;

    private async Task RetryAsync()
    {
      ApplicationTelemetry.LogFirmwareUpdateError("Try again");
      try
      {
        if (await this.CheckForFirmwareUpdateAsync())
          await this.TryOobeUpdateAsync();
        else
          this.CompleteFirmwareUpdate(true);
      }
      catch (Exception ex)
      {
        FirmwareUpdateViewModel.Logger.Error((object) "Update retry failed.", ex);
        this.UpdateStatus = FirmwareUpdateViewModel.FirmwareUpdateStatus.Error;
      }
    }

    public ICommand FinishCommand => (ICommand) this.finishCommand;

    private async Task FinishAsync()
    {
      string type = this.type;
      if (!(type == "Normal"))
      {
        if (!(type == "New"))
          return;
        await this.AddBandRedirectAsync();
      }
      else
        this.NavigateHome();
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> navigationParameters = null)
    {
      this.type = navigationParameters != null && navigationParameters.ContainsKey("Type") ? navigationParameters["Type"] : throw new NavigationParameterException(string.Format("firmware update view model requires a navigation parameter \"{0}\" with possible values: \"{1}\", \"{2}\"", new object[3]
      {
        (object) "Type",
        (object) "New",
        (object) "Normal"
      }));
      this.UpdateStatus = FirmwareUpdateViewModel.FirmwareUpdateStatus.Updating;
      if (this.type == "New")
        await this.TryOobeUpdateAsync();
      else
        await this.TryUpdateAsync();
    }

    private async Task TryUpdateAsync()
    {
      while (true)
      {
        try
        {
          if (await this.CheckForFirmwareUpdateAsync())
            await this.FirmwareUpdateAsync(this.type == "New");
          this.CompleteFirmwareUpdate(this.type == "New");
          break;
        }
        catch (Exception ex)
        {
          FirmwareUpdateViewModel.Logger.Error((object) ex);
          int num = await this.errorHandlingService.HandleExceptionWithRetryAsync(ex, false) ? 1 : 0;
        }
      }
    }

    private async Task TryOobeUpdateAsync()
    {
      try
      {
        await this.FirmwareUpdateAsync(this.type == "New");
        this.CompleteFirmwareUpdate(this.type == "New");
      }
      catch (Exception ex)
      {
        FirmwareUpdateViewModel.Logger.Error((object) ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.UpdateStatus = FirmwareUpdateViewModel.FirmwareUpdateStatus.Error;
        ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
        {
          "App",
          "Firmware",
          "Something went wrong"
        });
      }
    }

    private async Task<bool> CheckForFirmwareUpdateAsync()
    {
      this.UpdateStatus = FirmwareUpdateViewModel.FirmwareUpdateStatus.Updating;
      this.PercentageCompletion = 0.0;
      this.RefreshStepText(FirmwareUpdateState.NotStarted);
      bool flag;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5.0)))
        flag = await this.firmwareUpdateService.CheckForFirmwareUpdateAsync(cancellationTokenSource.Token, true);
      return flag;
    }

    private void CompleteFirmwareUpdate(bool isOobeUpdate)
    {
      this.UpdateStatus = FirmwareUpdateViewModel.FirmwareUpdateStatus.Success;
      this.PercentageCompletion = 100.0;
      this.toastNotificationService.ClearToast(HealthAppConstants.ToastNotificationIds.FirmwareUpdateRequired);
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
      {
        "App",
        "Firmware",
        "Update Complete"
      });
      if (isOobeUpdate)
        return;
      this.UpdateTilesInBackgroundToFixPotentialIssuesWithLayouts();
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (message => message.CancelAction()));
    }

    private async Task AddBandRedirectAsync()
    {
      try
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5.0)))
        {
          await this.addBandService.SetBandScreenAsync(BandScreen.AlmostDone, cancellationTokenSource.Token);
          await this.addBandService.NextAsync(cancellationTokenSource.Token);
        }
      }
      catch (Exception ex)
      {
        FirmwareUpdateViewModel.Logger.Error(ex, "Failed to complete adding a band.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    }

    private void NavigateHome()
    {
      this.smoothNavService.Navigate(typeof (TilesViewModel));
      this.smoothNavService.ClearBackStack();
    }

    private async Task FirmwareUpdateAsync(bool isOobeUpdate)
    {
      FirmwareUpdateViewModel.Logger.Debug((object) "<START> updating the firmware on the band");
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
        await this.firmwareUpdateService.UpdateFirmwareAsync(isOobeUpdate, cancellationTokenSource.Token, (IProgress<FirmwareUpdateProgressReport>) new Progress<FirmwareUpdateProgressReport>(new Action<FirmwareUpdateProgressReport>(this.ProgressReportHandler)));
      FirmwareUpdateViewModel.Logger.Debug((object) "<END> updating the firmware on the band");
    }

    private void ProgressReportHandler(FirmwareUpdateProgressReport report)
    {
      this.PercentageCompletion = report.PercentageCompletion;
      this.RefreshStepText(report.FirmwareUpdateState);
    }

    private void RefreshTitle()
    {
      switch (this.UpdateStatus)
      {
        case FirmwareUpdateViewModel.FirmwareUpdateStatus.Updating:
          this.PageTitle = AppResources.FirmwareUpdatePageTitle;
          this.PageSubtitle = AppResources.FirmwareUpdateSubtitle;
          break;
        case FirmwareUpdateViewModel.FirmwareUpdateStatus.Error:
          this.PageTitle = AppResources.ErrorPageTitle;
          this.PageSubtitle = AppResources.FirmwareUpdateErrorSubtitle;
          break;
        case FirmwareUpdateViewModel.FirmwareUpdateStatus.Success:
          this.PageTitle = AppResources.FirmwareUpdateSucceededHeader;
          this.PageSubtitle = AppResources.FirmwareUpdateSucceededMessage;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void RefreshStepText(FirmwareUpdateState state)
    {
      int num = 0;
      switch (state)
      {
        case FirmwareUpdateState.NotStarted:
          num = 1;
          this.StepText = AppResources.FirmwareUpdateState1NotStarted;
          break;
        case FirmwareUpdateState.DownloadingUpdate:
          num = 2;
          this.StepText = AppResources.FirmwareUpdateState2DownloadingUpdate;
          break;
        case FirmwareUpdateState.SyncingLog:
          num = 3;
          this.StepText = AppResources.FirmwareUpdateState3SyncingLog;
          break;
        case FirmwareUpdateState.BootingToUpdateMode:
          num = 4;
          this.StepText = AppResources.FirmwareUpdateState4BootingToUpdateMode;
          break;
        case FirmwareUpdateState.SendingUpdateToDevice:
          num = 5;
          this.StepText = AppResources.FirmwareUpdateState5SendingUpdateToBand;
          break;
        case FirmwareUpdateState.WaitingtoConnectAfterUpdate:
          num = 6;
          this.StepText = AppResources.FirmwareUpdateState4BootingToUpdateMode;
          break;
        case FirmwareUpdateState.Done:
          num = 7;
          this.StepText = AppResources.FirmwareUpdateState7Done;
          break;
        default:
          this.StepText = string.Empty;
          break;
      }
      this.StepNumberText = string.Format(AppResources.FirmwareUpdateStepText, new object[2]
      {
        (object) num,
        (object) 7
      });
    }

    private async void UpdateTilesInBackgroundToFixPotentialIssuesWithLayouts()
    {
      try
      {
        FirmwareUpdateViewModel.Logger.Debug((object) "<START> updating tiles in background to fix potential issues with layouts");
        if (!(this.configProvider.Get<string>(FirmwareUpdateViewModel.CustomTileLayoutUpdateKey, (string) null) != "1") || !(this.tileManagementService.EnabledTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (p => p.TileId == Guid.Parse("69a39b4e-084b-4b53-9a1b-581826df9e36"))) != (AppBandTile) null) && !(this.tileManagementService.EnabledTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (p => p.TileId == Guid.Parse("64a29f65-70bb-4f32-99a2-0f250a05d427"))) != (AppBandTile) null))
          return;
        StartStrip startStrip;
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
            startStrip = await cargoConnection.GetStartStripAsync(cancellationTokenSource.Token);
        }
        if (startStrip != null)
        {
          IList<AppBandTile> updatedTiles = (IList<AppBandTile>) new List<AppBandTile>();
          foreach (AdminBandTile adminBandTile in startStrip)
          {
            AdminBandTile tileFromBand = adminBandTile;
            AppBandTile appBandTile1 = this.tileManagementService.KnownTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tileFromApp => tileFromApp.TileId == tileFromBand.TileId));
            if (appBandTile1 != (AppBandTile) null)
            {
              AppBandTile appBandTile2 = appBandTile1.Copy();
              appBandTile2.ShowTile = true;
              appBandTile2.Settings = tileFromBand.SettingsMask;
              updatedTiles.Add(appBandTile2);
            }
            else if (this.tileManagementService.KnownTiles.DisabledTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tileFromApp => tileFromApp.TileId == tileFromBand.TileId)) == (AppBandTile) null)
            {
              bool flag1 = false;
              bool flag2 = false;
              if (tileFromBand.IsWebTile)
              {
                IWebTile webTile = this.webTileService.GetWebTileManager.GetWebTile(tileFromBand.TileId);
                flag1 = webTile.HasNotifications;
                flag2 = webTile.NotificationEnabled;
              }
              AppBandTile appBandTile3 = new AppBandTile()
              {
                TileId = tileFromBand.TileId,
                Title = tileFromBand.Name,
                ThirdPartyIcon = tileFromBand.Image,
                ThirdPartyOwnerId = tileFromBand.OwnerId,
                ShowTile = true,
                Settings = tileFromBand.SettingsMask,
                HasSettings = flag1,
                IsThirdParty = true,
                IsWebTile = tileFromBand.IsWebTile,
                IsWebTileNotificationEnabled = flag2
              };
              if (appBandTile3.HasSettings)
              {
                appBandTile3.DefaultOffSettings = tileFromBand.SettingsMask - (ushort) 1;
                appBandTile3.DefaultOnSettings = tileFromBand.SettingsMask;
              }
              updatedTiles.Add(appBandTile3);
            }
          }
          int bandClassAsync = (int) await this.tileManagementService.GetBandClassAsync(CancellationToken.None);
          IEnumerable<AppBandTile> appBandTiles = await this.tileManagementService.SetTilesAsync((ICollection<AppBandTile>) updatedTiles, (ICollection<AdminBandTile>) startStrip, (BandClass) bandClassAsync);
          this.configProvider.Set<string>(FirmwareUpdateViewModel.CustomTileLayoutUpdateKey, "1");
          updatedTiles = (IList<AppBandTile>) null;
        }
        else
          this.configProvider.Set<string>(FirmwareUpdateViewModel.CustomTileLayoutUpdateKey, "1");
        await this.syncManager.UpdateTilesAsync(CancellationToken.None);
        FirmwareUpdateViewModel.Logger.Debug((object) "<END> updating tiles in background to fix potential issues with layouts");
        startStrip = (StartStrip) null;
      }
      catch (Exception ex)
      {
        FirmwareUpdateViewModel.Logger.Error(ex, "<FAILED> updating tiles in background to fix potential issues with layouts");
      }
    }

    public static class NavigationParameter
    {
      public const string Type = "Type";
    }

    public static class Type
    {
      public const string New = "New";
      public const string Normal = "Normal";
    }

    public enum FirmwareUpdateStatus
    {
      Updating,
      Error,
      Success,
    }
  }
}
