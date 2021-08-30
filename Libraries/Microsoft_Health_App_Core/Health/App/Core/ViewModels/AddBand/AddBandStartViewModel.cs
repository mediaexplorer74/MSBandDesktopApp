// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.AddBand.AddBandStartViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Bluetooth;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Pedometer;
using Microsoft.Health.App.Core.Services.Sync;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.AddBand
{
  [PageTaxonomy(new string[] {"App", "Add Band", "Choose Band"})]
  public class AddBandStartViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\AddBand\\AddBandStartViewModel.cs");
    private readonly IAddBandService addBandService;
    private readonly IBluetoothService bluetoothService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService smoothNavService;
    private readonly ISyncEntryService syncEntryService;
    private readonly IPedometerManager pedometerManager;
    private readonly ILauncherService launcherService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageSender messageSender;
    private readonly IPagePicker pagePicker;
    private readonly IEnvironmentService environmentService;
    private HealthCommand<BandClass> addBandCommand;
    private HealthCommand learnMoreCommand;
    private HealthCommand noBandCommand;
    private bool isAddBandCommandEnabled = true;
    private bool islearnMoreCommandEnabled = true;
    private bool isNoBandComandEnabled = true;
    private bool isOobe;

    public AddBandStartViewModel(
      INetworkService networkService,
      IAddBandService addBandService,
      IBluetoothService bluetoothService,
      IBandConnectionFactory cargoConnectionFactory,
      IMessageBoxService messageBoxService,
      IPedometerManager pedometerManager,
      ISmoothNavService smoothNavService,
      ISyncEntryService syncEntryService,
      ILauncherService launcherService,
      IErrorHandlingService errorHandlingService,
      IMessageSender messageSender,
      IPagePicker pagePicker,
      IEnvironmentService environmentService)
      : base(networkService)
    {
      this.addBandService = addBandService;
      this.bluetoothService = bluetoothService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.messageBoxService = messageBoxService;
      this.smoothNavService = smoothNavService;
      this.pedometerManager = pedometerManager;
      this.syncEntryService = syncEntryService;
      this.launcherService = launcherService;
      this.errorHandlingService = errorHandlingService;
      this.messageSender = messageSender;
      this.pagePicker = pagePicker;
      this.environmentService = environmentService;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.smoothNavService.DisableNavPanel(typeof (AddBandStartViewModel));
      if (parameters != null)
        this.IsOobe = bool.Parse(parameters["IsOobe"]);
      this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (async message =>
      {
        message.CancelAction();
        await this.CancelAddBandAsync();
      }));
      await base.LoadDataAsync();
    }

    public BandClass Envoy => BandClass.Envoy;

    public BandClass Cargo => BandClass.Cargo;

    public bool IsOobe
    {
      get => this.isOobe;
      set
      {
        this.SetProperty<bool>(ref this.isOobe, value, nameof (IsOobe));
        this.RaisePropertyChanged("NoBandText");
      }
    }

    public string NoBandText => !this.isOobe ? AppResources.AddBandStartCancelButtonText : AppResources.AddBandStartNoBandButtonText;

    public string PageTitle { get; } = AppResources.AddBandStartPageTitle;

    public string PageSubtitle { get; } = AppResources.AddBandStartPageSubtitle;

    public ICommand AddBandCommand => (ICommand) this.addBandCommand ?? (ICommand) (this.addBandCommand = new HealthCommand<BandClass>((Action<BandClass>) (async bandType =>
    {
      this.isAddBandCommandEnabled = false;
      this.addBandCommand.RaiseCanExecuteChanged();
      bool? pairBandAfterPrompt = new bool?();
      if (this.environmentService.IsUwpAppOnDesktop())
        pairBandAfterPrompt = new bool?(await this.messageBoxService.ShowAsync(AppResources.AddBandRecommendPairOnPhoneBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OKCancel, AppResources.Continue, AppResources.Cancel) == PortableMessageBoxResult.OK);
      bool? nullable = pairBandAfterPrompt;
      bool flag = false;
      if ((nullable.GetValueOrDefault() == flag ? (!nullable.HasValue ? 1 : 0) : 1) != 0)
      {
        using (CancellationTokenSource cts = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          if (await this.IsBluetoothEnabledAsync(cts.Token))
          {
            ApplicationTelemetry.LogAddBandChoice(bandType, pairBandAfterPrompt);
            await this.addBandService.StartAsync(cts.Token);
          }
        }
      }
      else
      {
        this.isNoBandComandEnabled = false;
        this.noBandCommand.RaiseCanExecuteChanged();
        await this.CancelAddBandAsync(pairBandAfterPrompt);
        this.isNoBandComandEnabled = true;
        this.noBandCommand.RaiseCanExecuteChanged();
      }
      this.isAddBandCommandEnabled = true;
      this.addBandCommand.RaiseCanExecuteChanged();
    }), (Func<BandClass, bool>) (bandType => this.isAddBandCommandEnabled)));

    public ICommand LearnMoreCommand => (ICommand) this.learnMoreCommand ?? (ICommand) (this.learnMoreCommand = new HealthCommand((Action) (() =>
    {
      this.islearnMoreCommandEnabled = false;
      this.learnMoreCommand.RaiseCanExecuteChanged();
      this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkID=532705"));
      this.islearnMoreCommandEnabled = true;
      this.learnMoreCommand.RaiseCanExecuteChanged();
    }), (Func<bool>) (() => this.islearnMoreCommandEnabled)));

    public ICommand NoBandCommand => (ICommand) this.noBandCommand ?? (ICommand) (this.noBandCommand = new HealthCommand((Action) (async () =>
    {
      this.isNoBandComandEnabled = false;
      this.noBandCommand.RaiseCanExecuteChanged();
      await this.CancelAddBandAsync();
      this.isNoBandComandEnabled = true;
      this.noBandCommand.RaiseCanExecuteChanged();
    }), (Func<bool>) (() => this.isNoBandComandEnabled)));

    private async Task<bool> IsBluetoothEnabledAsync(CancellationToken token)
    {
      if (!await this.bluetoothService.IsBluetoothEnabledAsync(token))
      {
        try
        {
          if (this.bluetoothService.CanEnableBluetoothWithoutSystemPrompt)
            return await this.PromptUserEnableBluetoothAsync(token);
          await this.bluetoothService.EnableBluetoothAsync(token);
        }
        catch (BluetoothEnableRequestDeniedException ex)
        {
          return false;
        }
        catch (Exception ex)
        {
          AddBandStartViewModel.Logger.Error((object) "Attempt to enable bluetooth failed.", ex);
          await this.errorHandlingService.HandleExceptionAsync(ex);
          return false;
        }
      }
      return true;
    }

    private async Task<bool> PromptUserEnableBluetoothAsync(CancellationToken token)
    {
      if (await this.messageBoxService.ShowAsync(AppResources.AddBandEnableBluetoothMessage, AppResources.AddBandEnableBluetoothTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK)
      {
        AddBandStartViewModel.Logger.Info((object) "<BEGIN> Enable bluetooth.");
        await this.bluetoothService.EnableBluetoothAsync(token);
        AddBandStartViewModel.Logger.Info((object) "<END> Enable bluetooth.");
        return true;
      }
      AddBandStartViewModel.Logger.Info((object) "User rejected enabling bluetooth.");
      return false;
    }

    private async Task CancelAddBandAsync(bool? desktopPromptResult = null)
    {
      try
      {
        if (this.IsOobe)
        {
          this.isNoBandComandEnabled = true;
          ApplicationTelemetry.LogAddBandChoice("No Band", desktopPromptResult);
          using (CancellationTokenSource cts = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
          {
            bool flag = await this.pedometerManager.IsAvailableAsync(cts.Token);
            if (flag)
              flag = !await this.pedometerManager.IsEnabledAsync(cts.Token);
            if (flag)
            {
              this.smoothNavService.Navigate(this.pagePicker.OobeMotionTrackingSettings, action: NavigationStackAction.RemovePrevious);
            }
            else
            {
              this.smoothNavService.Navigate(typeof (TilesViewModel), action: NavigationStackAction.RemovePrevious);
              AddBandStartViewModel.Logger.Debug((object) "<FLAG> start update and sync band");
              this.syncEntryService.Sync(SyncType.NoBand, CancellationTokenUtilities.DefaultCancellationTokenTimespan);
            }
          }
        }
        else
        {
          bool? nullable = desktopPromptResult;
          bool flag1 = false;
          bool flag2 = nullable.GetValueOrDefault() == flag1 && nullable.HasValue;
          if (!desktopPromptResult.HasValue)
            flag2 = await this.messageBoxService.ShowAsync(AppResources.AddBandCancelDialogMessage, AppResources.ApplicationTitle, PortableMessageBoxButton.CasualBooleanChoice) == PortableMessageBoxResult.OK;
          if (!flag2)
            return;
          ApplicationTelemetry.LogAddBandChoice("Cancel", desktopPromptResult);
          this.smoothNavService.GoBack();
        }
      }
      finally
      {
        this.smoothNavService.EnableNavPanel(typeof (AddBandStartViewModel));
      }
    }
  }
}
