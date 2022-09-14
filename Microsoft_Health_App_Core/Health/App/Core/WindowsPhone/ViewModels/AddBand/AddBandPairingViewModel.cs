// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.WindowsPhone.ViewModels.AddBand.AddBandPairingViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Bluetooth;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.WindowsPhone.ViewModels.AddBand
{
  [PageTaxonomy(new string[] {"App", "Add Band", "Pair your Band"})]
  public class AddBandPairingViewModel : PageViewModelBase
  {
    private const int BluetoothInquiryTimeout = 20000;
    private const int RadioShutdownTimeout = 3000;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\AddBand\\AddBandPairingViewModel.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly IAddBandService addBandService;
    private readonly IDispatchService dispatchService;
    private readonly IFirmwareUpdateService firmwareUpdateService;
    private readonly ILauncherService launcherService;
    private readonly IBluetoothService bluetoothService;
    private readonly IEnvironmentService environmentService;
    private readonly IPairingService pairingService;
    private readonly IBandHardwareService bandHardwareService;
    private readonly IPermissionService permissionService;
    private readonly ObservableCollection<DiscoveredBandDevice> discoveredBluetoothDevices;
    private BandPairingSubstate connectState;
    private HealthCommand<DiscoveredBandDevice> pairCommand;
    private HealthCommand finishCommand;
    private HealthCommand cancelCommand;
    private HealthCommand tryAgainCommand;
    private HealthCommand havingTroubleCommand;
    private bool skipSuccessful;
    private bool isLoading;
    private bool isBandListVisible;
    private BandClass pairedBandClass;
    private bool isCheckingForUpdates = true;
    private bool isPairing;
    private bool isDiscovering;
    private CancellationTokenSource cancellationTokenSource;
    private CancellationTokenSource timeoutCancellationTokenSource;
    private bool pairCommandEnabled = true;
    private bool tryAgainCommandEnabled = true;

    public AddBandPairingViewModel(
      IBandConnectionFactory cargoConnectionFactory,
      IErrorHandlingService errorHandlingService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      IMessageSender messageSender,
      IAddBandService addBandService,
      IDispatchService dispatchService,
      IFirmwareUpdateService firmwareUpdateService,
      ILauncherService launcherService,
      IBluetoothService bluetoothService,
      IEnvironmentService environmentService,
      IPairingService pairingService,
      IBandHardwareService bandHardwareService,
      IPermissionService permissionService)
      : base(networkService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.errorHandlingService = errorHandlingService;
      this.messageBoxService = messageBoxService;
      this.messageSender = messageSender;
      this.addBandService = addBandService;
      this.dispatchService = dispatchService;
      this.firmwareUpdateService = firmwareUpdateService;
      this.launcherService = launcherService;
      this.bluetoothService = bluetoothService;
      this.environmentService = environmentService;
      this.pairingService = pairingService;
      this.bandHardwareService = bandHardwareService;
      this.permissionService = permissionService;
      this.discoveredBluetoothDevices = new ObservableCollection<DiscoveredBandDevice>();
      this.IsDiscovering = this.bluetoothService.SupportsContinuousDiscovery;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.EnableTimeout();
      this.cancellationTokenSource = new CancellationTokenSource();
      this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (message => message.CancelAction()));
      this.PairingState = BandPairingSubstate.Inquiry;
      this.IsBandListVisible = false;
      this.IsLoading = true;
      this.DiscoveredBluetoothDevices.Clear();
      this.bluetoothService.BluetoothDeviceStateChanged += new EventHandler<BandDeviceStateChangedEventArgs>(this.BluetoothServiceBluetoothDeviceStateChanged);
      try
      {
        await this.CheckPermissionsAsync();
      }
      catch (PermissionDeniedException ex)
      {
        await this.errorHandlingService.HandleExceptionAsync((Exception) ex);
      }
      using (CancellationTokenSource tcs = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        await this.bluetoothService.EnableDiscoveryAsync(tcs.Token);
      await base.LoadDataAsync(parameters);
    }

    private async void EnableTimeout()
    {
      this.timeoutCancellationTokenSource = new CancellationTokenSource();
      try
      {
        await Task.Delay(20000, this.timeoutCancellationTokenSource.Token);
      }
      catch (TaskCanceledException ex)
      {
        return;
      }
      if (this.DiscoveredBluetoothDevices.Count != 0)
        return;
      this.PairingState = BandPairingSubstate.Timeout;
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
      {
        "App",
        "Add Band",
        "No Bands Found"
      });
    }

    private void DisableTimeout()
    {
      if (this.timeoutCancellationTokenSource == null)
        return;
      this.timeoutCancellationTokenSource.Cancel();
    }

    public IList<DiscoveredBandDevice> DiscoveredBluetoothDevices => (IList<DiscoveredBandDevice>) this.discoveredBluetoothDevices;

    public bool InquiryState => this.PairingState == BandPairingSubstate.Inquiry;

    public bool ShouldShowProgressIndicator
    {
      get
      {
        if (this.PairingState == BandPairingSubstate.Success)
          return false;
        return this.PairingState == BandPairingSubstate.Inquiry && this.IsDiscovering || this.IsPairing;
      }
    }

    public bool TimeoutState => this.PairingState == BandPairingSubstate.Timeout;

    public bool SuccessState => this.PairingState == BandPairingSubstate.Success;

    public bool SuccessStateV1 => this.SuccessState && this.pairedBandClass == BandClass.Cargo;

    public bool SuccessStateV2 => this.SuccessState && this.pairedBandClass == BandClass.Envoy;

    public string PageTitle
    {
      get
      {
        switch (this.PairingState)
        {
          case BandPairingSubstate.Inquiry:
          case BandPairingSubstate.Timeout:
            return AppResources.AddBandPairingPageTitle;
          case BandPairingSubstate.Success:
            return AppResources.AddBandPairingSuccessPageTitle;
          default:
            return string.Empty;
        }
      }
    }

    public string PageSubtitle
    {
      get
      {
        switch (this.PairingState)
        {
          case BandPairingSubstate.Inquiry:
            return AppResources.AddBandPairingInquiryPageSubtitle;
          case BandPairingSubstate.Timeout:
            return AppResources.AddBandPairingTimeoutPageSubtitle;
          case BandPairingSubstate.Success:
            switch (this.pairedBandClass)
            {
              case BandClass.Cargo:
                return string.Format(AppResources.AddBandPairingSuccessSubtitleText, new object[1]
                {
                  (object) AppResources.AddBandStartCargoLabel
                });
              case BandClass.Envoy:
                return string.Format(AppResources.AddBandPairingSuccessSubtitleText, new object[1]
                {
                  (object) AppResources.AddBandStartNeonLabel
                });
              default:
                return string.Empty;
            }
          default:
            return string.Empty;
        }
      }
    }

    public bool IsNeonAndNotPairing => this.pairedBandClass == BandClass.Envoy && !this.IsPairing;

    public bool IsBandListVisible
    {
      get => this.isBandListVisible;
      private set
      {
        this.SetProperty<bool>(ref this.isBandListVisible, value, nameof (IsBandListVisible));
        this.RaisePropertyChanged("IsBandListVisibleOrIsPairing");
        this.RaisePropertyChanged("IsBandListVisibleAndIsNotPairing");
      }
    }

    public bool IsCheckingForUpdates
    {
      get => this.isCheckingForUpdates;
      set => this.SetProperty<bool>(ref this.isCheckingForUpdates, value, nameof (IsCheckingForUpdates));
    }

    public bool IsPairing
    {
      get => this.isPairing;
      set
      {
        this.SetProperty<bool>(ref this.isPairing, value, nameof (IsPairing));
        this.IsBandListVisible = !value;
        this.IsLoading = value;
        this.RaisePropertyChanged("ShouldShowProgressIndicator");
        this.RaisePropertyChanged("IsNeonAndNotPairing");
        this.RaisePropertyChanged("IsBandListVisibleOrIsPairing");
        this.RaisePropertyChanged("IsBandListVisibleAndIsNotPairing");
      }
    }

    public bool IsBandListVisibleOrIsPairing => this.IsBandListVisible || this.IsPairing;

    public bool IsBandListVisibleAndIsNotPairing => this.IsBandListVisible && !this.IsPairing;

    public bool ShowCancelButton => this.PairingState != BandPairingSubstate.Success;

    public BandPairingSubstate PairingState
    {
      get => this.connectState;
      private set
      {
        this.SetProperty<BandPairingSubstate>(ref this.connectState, value, nameof (PairingState));
        this.RaisePropertyChanged("InquiryState");
        this.RaisePropertyChanged("ShouldShowProgressIndicator");
        this.RaisePropertyChanged("TimeoutState");
        this.RaisePropertyChanged("SuccessState");
        this.RaisePropertyChanged("SuccessStateV1");
        this.RaisePropertyChanged("SuccessStateV2");
        this.RaisePropertyChanged("ShowCancelButton");
        this.RaisePropertyChanged("PageTitle");
        this.RaisePropertyChanged("PageSubtitle");
      }
    }

    public bool IsLoading
    {
      get => this.isLoading;
      private set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    private bool IsPairCommandEnabled(DiscoveredBandDevice d) => d != null && this.pairCommandEnabled;

    public ICommand PairCommand => (ICommand) this.pairCommand ?? (ICommand) (this.pairCommand = new HealthCommand<DiscoveredBandDevice>((Action<DiscoveredBandDevice>) (async discoveredWearableWatch =>
    {
      this.pairCommandEnabled = false;
      this.pairCommand.RaiseCanExecuteChanged();
      Exception pairingException = (Exception) null;
      try
      {
        this.IsPairing = true;
        await this.CheckPermissionsAsync();
        if (await this.pairingService.PairAsync(discoveredWearableWatch.Device, discoveredWearableWatch.State, this.cancellationTokenSource.Token).ConfigureAwait(false))
          await this.CompletePairingAsync(this.cancellationTokenSource.Token).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        AddBandPairingViewModel.Logger.Error((object) "Error occured when attempting to pair.", ex);
        pairingException = ex;
      }
      finally
      {
        this.IsPairing = false;
        this.pairCommandEnabled = true;
        this.pairCommand.RaiseCanExecuteChanged();
      }
      switch (pairingException)
      {
        case null:
          break;
        case BandNotInOobeException _:
          int num1 = (int) await this.messageBoxService.ShowAsync(AppResources.AddBandPairedBandNotInOOBEMessage, AppResources.AddBandPairedBandNotInOOBETitle, PortableMessageBoxButton.OK);
          break;
        case PermissionDeniedException _:
          await this.errorHandlingService.HandleExceptionAsync(pairingException);
          break;
        default:
          int num2 = (int) await this.messageBoxService.ShowAsync(AppResources.AddBandStatusPairingFailureMessage, AppResources.AddBandStatusPairingFailureTitle, PortableMessageBoxButton.OK);
          break;
      }
    }), new Func<DiscoveredBandDevice, bool>(this.IsPairCommandEnabled)));

    private async Task CheckPermissionsAsync() => await this.permissionService.RequestPermissionsAsync(FeaturePermissions.BluetoothSearchAndConnect);

    public ICommand HavingTroubleCommand => (ICommand) this.havingTroubleCommand ?? (ICommand) (this.havingTroubleCommand = new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogNoBandsFoundChoice("Trouble connecting");
      this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkID=624677"));
    })));

    private bool IsTryAgainCommandEnabled() => this.tryAgainCommandEnabled;

    public ICommand TryAgainCommand => (ICommand) this.tryAgainCommand ?? (ICommand) (this.tryAgainCommand = new HealthCommand((Action) (async () =>
    {
      try
      {
        await this.CheckPermissionsAsync();
      }
      catch (PermissionDeniedException ex)
      {
        await this.errorHandlingService.HandleExceptionAsync((Exception) ex);
      }
      this.tryAgainCommandEnabled = false;
      this.tryAgainCommand.RaiseCanExecuteChanged();
      if (!this.bluetoothService.SupportsContinuousDiscovery)
      {
        CancellationToken token = this.cancellationTokenSource.Token;
        await this.bluetoothService.DisableDiscoveryAsync(token);
        await this.bluetoothService.EnableDiscoveryAsync(token);
        token = new CancellationToken();
      }
      if (this.PairingState == BandPairingSubstate.Inquiry)
        ApplicationTelemetry.LogPairBandPageChoice("Refresh");
      else if (this.PairingState == BandPairingSubstate.Timeout)
        ApplicationTelemetry.LogNoBandsFoundChoice("Try again");
      this.PairingState = BandPairingSubstate.Inquiry;
      this.EnableTimeout();
      this.tryAgainCommandEnabled = true;
      this.tryAgainCommand.RaiseCanExecuteChanged();
    }), new Func<bool>(this.IsTryAgainCommandEnabled)));

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.bluetoothService.BluetoothDiscoveryStateChanged += new EventHandler<BluetoothDiscoveryStateChangedEventArgs>(this.BluetoothServiceBluetoothDiscoveryStateChanged);
    }

    protected override void OnNavigatedFrom()
    {
      base.OnNavigatedFrom();
      this.bluetoothService.BluetoothDiscoveryStateChanged -= new EventHandler<BluetoothDiscoveryStateChangedEventArgs>(this.BluetoothServiceBluetoothDiscoveryStateChanged);
    }

    private bool IsDiscovering
    {
      get => this.isDiscovering;
      set
      {
        this.SetProperty<bool>(ref this.isDiscovering, value, nameof (IsDiscovering));
        this.RaisePropertyChanged("ShouldShowProgressIndicator");
      }
    }

    private async Task CompletePairingAsync(CancellationToken token)
    {
      await this.ShutdownAsync(token);
      AddBandPairingViewModel pairingViewModel = this;
      int pairedBandClass = (int) pairingViewModel.pairedBandClass;
      int deviceTypeAsync = (int) await this.bandHardwareService.GetDeviceTypeAsync(token);
      pairingViewModel.pairedBandClass = (BandClass) deviceTypeAsync;
      pairingViewModel = (AddBandPairingViewModel) null;
      this.PairingState = BandPairingSubstate.Success;
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
      {
        "App",
        "Add Band",
        "Bluetooth success"
      });
      await this.OobeFirmwareUpdateAsync();
    }

    private async Task ShutdownAsync(CancellationToken token)
    {
      this.bluetoothService.BluetoothDeviceStateChanged -= new EventHandler<BandDeviceStateChangedEventArgs>(this.BluetoothServiceBluetoothDeviceStateChanged);
      AddBandPairingViewModel.Logger.Info((object) "<BEGIN> Shutdown Bluetooth service.");
      try
      {
        await this.bluetoothService.DisableDiscoveryAsync(token);
        await this.bluetoothService.ShutdownAsync(token);
      }
      catch (Exception ex)
      {
        AddBandPairingViewModel.Logger.Warn((object) "Exception occurred when attempting to shut down the Bluetooth service.", ex);
      }
      AddBandPairingViewModel.Logger.Info((object) "<END> Shutdown Bluetooth service.");
    }

    private async void BluetoothServiceBluetoothDeviceStateChanged(
      object sender,
      BandDeviceStateChangedEventArgs e)
    {
      AddBandPairingViewModel.Logger.Info("BluetoothService.BluetoothDeviceStateChanged Name: {0}, Address: {1}, State: {2}", (object) e.Device.Name, e.Device.Address, (object) e.ConnectionState);
      await this.dispatchService.RunOnUIThreadAsync((Action) (() =>
      {
        DiscoveredBandDevice discoveredBandDevice = this.DiscoveredBluetoothDevices.FirstOrDefault<DiscoveredBandDevice>((Func<DiscoveredBandDevice, bool>) (w => w.Device.Equals((object) e.Device)));
        if (discoveredBandDevice == null && (e.ConnectionState == BluetoothConnectionState.Visible || e.ConnectionState == BluetoothConnectionState.Paired || e.ConnectionState == BluetoothConnectionState.Connected) && !string.IsNullOrEmpty(e.Device.Name))
        {
          this.DiscoveredBluetoothDevices.Add(new DiscoveredBandDevice(e.Device, e.ConnectionState));
          if (this.DiscoveredBluetoothDevices.Count != 1)
            return;
          this.DisableTimeout();
          this.IsBandListVisible = true;
          this.IsLoading = false;
        }
        else
        {
          if (discoveredBandDevice == null)
            return;
          if (e.ConnectionState == BluetoothConnectionState.NotVisible)
          {
            this.DiscoveredBluetoothDevices.Remove(discoveredBandDevice);
            if (this.DiscoveredBluetoothDevices.Count != 0)
              return;
            this.EnableTimeout();
            this.IsBandListVisible = false;
            this.IsLoading = true;
          }
          else
          {
            if (!string.IsNullOrEmpty(discoveredBandDevice.Name))
              discoveredBandDevice.Name = e.Device.Name;
            discoveredBandDevice.State = e.ConnectionState;
          }
        }
      }));
    }

    private void BluetoothServiceBluetoothDiscoveryStateChanged(
      object sender,
      BluetoothDiscoveryStateChangedEventArgs e)
    {
      this.IsDiscovering = e.State == BluetoothDiscoveryState.Started;
    }

    public ICommand FinishCommand => (ICommand) this.finishCommand ?? (ICommand) (this.finishCommand = new HealthCommand((Action) (async () =>
    {
      try
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          if (!this.skipSuccessful)
          {
            AddBandPairingViewModel pairingViewModel = this;
            int num1 = pairingViewModel.skipSuccessful ? 1 : 0;
            int num2 = await this.addBandService.SkipAsync(cancellationTokenSource.Token) ? 1 : 0;
            pairingViewModel.skipSuccessful = num2 != 0;
            pairingViewModel = (AddBandPairingViewModel) null;
          }
          await this.addBandService.NextAsync(cancellationTokenSource.Token);
        }
      }
      catch (Exception ex)
      {
        AddBandPairingViewModel.Logger.Error((object) ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    })));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.Cancel)));

    private async void Cancel()
    {
      PortableMessageBoxResult messageBoxResult = await this.messageBoxService.ShowAsync(AppResources.AddBandCancelDialogMessage, AppResources.ApplicationTitle, PortableMessageBoxButton.CasualBooleanChoice);
      int num = 0;
      object obj;
      try
      {
        CancellationTokenSource cancellationToken;
        int num1;
        if (num1 != 1 && num1 != 2)
        {
          if (messageBoxResult == PortableMessageBoxResult.OK)
          {
            this.cancellationTokenSource.Cancel();
            if (this.PairingState == BandPairingSubstate.Inquiry)
              ApplicationTelemetry.LogPairBandPageChoice(nameof (Cancel));
            else if (this.PairingState == BandPairingSubstate.Timeout)
              ApplicationTelemetry.LogNoBandsFoundChoice(nameof (Cancel));
            cancellationToken = new CancellationTokenSource(3000);
          }
          else
            goto label_22;
        }
        try
        {
          if (num1 == 1 || num1 != 2)
          {
            try
            {
              await this.ShutdownAsync(cancellationToken.Token);
            }
            catch (Exception ex)
            {
              AddBandPairingViewModel.Logger.Warn((object) "Error occured when attempting to shutdown radio controller.", ex);
            }
          }
          else
          {
            TaskAwaiter taskAwaiter = new TaskAwaiter();
            // ISSUE: explicit reference operation
            // ISSUE: reference to a compiler-generated field
            (^this).\u003C\u003E1__state = num1 = -1;
          }
          await this.addBandService.ExitAsync(cancellationToken.Token);
        }
        finally
        {
          cancellationToken?.Dispose();
        }
        cancellationToken = (CancellationTokenSource) null;
      }
      catch (Exception ex)
      {
        obj = (object) ex;
        num = 1;
      }
label_22:
      if (num == 1)
      {
        Exception exception = (Exception) obj;
        AddBandPairingViewModel.Logger.Warn((object) "Exception occurred on bluetooth connect cancel.", exception);
        await this.errorHandlingService.HandleExceptionAsync(exception);
      }
      obj = (object) null;
    }

    private async Task OobeFirmwareUpdateAsync()
    {
      this.IsCheckingForUpdates = true;
      if (await this.RepeatFirmwareUpdateCheckWithFailureMessagesUntilSuccessAsync())
      {
        ApplicationTelemetry.LogFirmwarePrompt();
        int num;
        if (EnvironmentUtilities.IsDebugSettingEnabled)
        {
          if (await this.messageBoxService.ShowAsync(AppResources.FirmwareUpdateMessage, AppResources.FirmwareUpdateDialogTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK)
          {
            try
            {
              CancellationTokenSource cancellationTokenSource;
              if (num != 2 && num != 3)
                cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan);
              try
              {
                await this.addBandService.SetBandScreenAsync(BandScreen.StartUpdate, cancellationTokenSource.Token);
                await this.addBandService.NextAsync(cancellationTokenSource.Token);
              }
              finally
              {
                cancellationTokenSource?.Dispose();
              }
              cancellationTokenSource = (CancellationTokenSource) null;
            }
            catch (Exception ex)
            {
              AddBandPairingViewModel.Logger.Error((object) ex);
              await this.errorHandlingService.HandleExceptionAsync(ex);
            }
          }
          else
            await this.FirmwareUpdateNotNeededAsync();
        }
        else if (await this.messageBoxService.ShowAsync(AppResources.FirmwareUpdateMessageBoxRequiredUpdateMessage, AppResources.FirmwareUpdateMessageBoxRequiredUpdateHeader, PortableMessageBoxButton.OK) == PortableMessageBoxResult.OK)
        {
          try
          {
            CancellationTokenSource cancellationTokenSource;
            if (num != 7 && num != 8)
              cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan);
            try
            {
              await this.addBandService.SetBandScreenAsync(BandScreen.StartUpdate, cancellationTokenSource.Token);
              await this.addBandService.NextAsync(cancellationTokenSource.Token);
            }
            finally
            {
              cancellationTokenSource?.Dispose();
            }
            cancellationTokenSource = (CancellationTokenSource) null;
          }
          catch (Exception ex)
          {
            AddBandPairingViewModel.Logger.Error((object) ex);
            await this.errorHandlingService.HandleExceptionAsync(ex);
          }
        }
        else
          await this.ResetAndReloadPageAsync();
      }
      else
        await this.FirmwareUpdateNotNeededAsync();
    }

    private async Task FirmwareUpdateNotNeededAsync()
    {
      await this.SetBandToAlmostThereScreenAsync();
      this.IsCheckingForUpdates = false;
    }

    private async Task<bool> CheckForFirmwareUpdateAsync()
    {
      bool flag;
      try
      {
        flag = await this.firmwareUpdateService.CheckForFirmwareUpdateAsync(new CancellationTokenSource(TimeSpan.FromMinutes(1.0)).Token, true);
      }
      catch (Exception ex)
      {
        AddBandPairingViewModel.Logger.Error((object) ex);
        throw;
      }
      return flag;
    }

    private async Task SetBandToCheckingForUpdatesScreenAsync()
    {
      AddBandPairingViewModel.Logger.Debug((object) "<START> navigating the band to the checking for updates screen");
      try
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
          await this.addBandService.SetBandScreenAsync(BandScreen.CheckUpdate, cancellationTokenSource.Token);
      }
      catch (Exception ex)
      {
        AddBandPairingViewModel.Logger.Error((object) ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      AddBandPairingViewModel.Logger.Debug((object) "<END> navigating the band to the checking for updates screen");
    }

    private async Task SetBandToAlmostThereScreenAsync()
    {
      AddBandPairingViewModel.Logger.Debug((object) "<START> navigating the band to the almost there screen");
      try
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
          await this.addBandService.SetBandScreenAsync(BandScreen.AlmostDone, cancellationTokenSource.Token);
      }
      catch (Exception ex)
      {
        AddBandPairingViewModel.Logger.Error((object) ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      AddBandPairingViewModel.Logger.Debug((object) "<END> navigating the band to the almost there screen");
    }

    private async Task<bool> RepeatFirmwareUpdateCheckWithFailureMessagesUntilSuccessAsync()
    {
      bool flag;
      while (true)
      {
        try
        {
          await this.SetBandToCheckingForUpdatesScreenAsync();
          flag = await this.CheckForFirmwareUpdateAsync();
          break;
        }
        catch (Exception ex)
        {
          await this.errorHandlingService.HandleExceptionAsync(ex);
        }
      }
      return flag;
    }

    private async Task ResetAndReloadPageAsync()
    {
      AddBandPairingViewModel.Logger.Debug((object) "<START> restarting from the bluetooth connect page");
      await this.LoadAsync((IDictionary<string, string>) null);
      AddBandPairingViewModel.Logger.Debug((object) "<END> restarting from the bluetooth connect page");
    }
  }
}
