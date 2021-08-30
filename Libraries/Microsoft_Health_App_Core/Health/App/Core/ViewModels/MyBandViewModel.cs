// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.MyBandViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.HomeShell)]
  [PageTaxonomy(new string[] {"Settings", "Personalize", "My Band"})]
  public class MyBandViewModel : PageViewModelBase, INotifyDataErrorInfo, IHomeShellViewModel
  {
    private const int MinNameLength = 1;
    private const int MaxNameLength = 15;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\MyBandViewModel.cs");
    private readonly IEnvironmentService environmentService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IConfig config;
    private readonly IBandThemeManager bandThemeManager;
    private readonly Dictionary<string, IList<string>> errors = new Dictionary<string, IList<string>>();
    private readonly IMessageBoxService messageBoxService;
    private readonly IGolfSyncService golfSyncService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly ISmoothNavService smoothNavService;
    private readonly HealthCommand unregisterBandCommand;
    private readonly IUserProfileService userProfileService;
    private readonly IPackageResourceService packageResourceService;
    private int batteryCharge;
    private HealthCommand cancelCommand;
    private HealthCommand confirmCommand;
    private string bandName;
    private string originalBandName;
    private bool displayBandData;
    private bool displaySaveChanges;
    private string firmwareVersion;
    private bool hasErrors;
    private bool isNameLoaded;
    private bool pageLoading;
    private string serialNumber;
    private bool showError;
    private bool showProgress;

    public MyBandViewModel(
      IBandConnectionFactory cargoConnectionFactory,
      ISmoothNavService smoothNavService,
      IConfig config,
      IErrorHandlingService errorHandlingService,
      IUserProfileService userProfileService,
      IMessageBoxService messageBoxService,
      IEnvironmentService environmentService,
      IBandThemeManager bandThemeManager,
      INetworkService networkService,
      IMessageSender messageSender,
      IGolfSyncService golfSyncService,
      IPackageResourceService packageResourceService)
      : base(networkService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.smoothNavService = smoothNavService;
      this.config = config;
      this.errorHandlingService = errorHandlingService;
      this.userProfileService = userProfileService;
      this.messageBoxService = messageBoxService;
      this.environmentService = environmentService;
      this.bandThemeManager = bandThemeManager;
      this.golfSyncService = golfSyncService;
      this.packageResourceService = packageResourceService;
      this.unregisterBandCommand = new HealthCommand(new Action(this.OnUnregisterBand));
    }

    public NavigationHeaderBackground NavigationHeaderBackground => NavigationHeaderBackground.Dark;

    private bool CheckFieldsFilled
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.BandName) || this.BandName.Length < 1 || this.BandName.Length > 15)
          this.AddError("BandName", string.Format(AppResources.CharacterRangeErrorMessage, new object[2]
          {
            (object) 1,
            (object) 15
          }));
        this.RefreshBandStatus();
        return !this.HasErrors;
      }
    }

    public IEnumerable GetErrors(string propertyName)
    {
      IList<string> stringList;
      this.errors.TryGetValue(propertyName, out stringList);
      return (IEnumerable) stringList;
    }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors
    {
      get => this.hasErrors;
      set => this.SetProperty<bool>(ref this.hasErrors, value, nameof (HasErrors));
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      MyBandViewModel.Logger.Debug((object) "<START> loading my band page");
      try
      {
        this.ShowProgress = false;
        this.DisplaySaveChanges = false;
        this.ShowError = false;
        this.PageLoading = true;
        this.BandName = this.userProfileService.CurrentUserProfile.BandName;
        this.originalBandName = this.BandName;
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          MyBandViewModel.Logger.Debug((object) "<START> fetching current theme information from band");
          AppBandTheme themeFromBandAsync = await this.bandThemeManager.GetCurrentThemeFromBandAsync(cancellationTokenSource.Token);
          MyBandViewModel.Logger.Debug((object) "<END> fetching current theme information from band");
          FirmwareVersions firmwareVersionAsync = await this.GetBandFirmwareVersionAsync(cancellationTokenSource.Token);
          this.FirmwareVersion = string.Format(AppResources.FirmwareVersionFormat, new object[3]
          {
            (object) firmwareVersionAsync.ApplicationVersion.ToString(),
            (object) firmwareVersionAsync.PcbId,
            firmwareVersionAsync.ApplicationVersion.Debug ? (object) AppResources.FirmwareVersionDebugAbbr : (object) AppResources.FirmwareVersionReleaseAbbr
          });
          this.SerialNumber = await this.GetProductSerialNumberAsync(cancellationTokenSource.Token);
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
            this.BatteryCharge = await cargoConnection.GetBatteryChargeAsync(cancellationTokenSource.Token);
        }
        this.DisplayBandData = true;
        MyBandViewModel.Logger.Debug((object) "<END> loading my band page");
      }
      catch (Exception ex)
      {
        MyBandViewModel.Logger.Error(ex, "<FAILED> loading my band page");
        this.FirmwareVersion = AppResources.NotAvailable;
        this.SerialNumber = AppResources.NotAvailable;
        this.DisplayBandData = false;
        this.ShowError = true;
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.IsNameLoaded = true;
      this.PageLoading = false;
    }

    private async void SaveBandName()
    {
      MyBandViewModel.Logger.Debug((object) "<START> save band name");
      try
      {
        if (!this.CheckFieldsFilled)
          throw new InvalidOperationException("Cannot set band name if RequiredFieldsFilled is false.");
        this.DisplaySaveChanges = false;
        this.ShowError = false;
        this.ShowProgress = true;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan);
        this.IsNameLoaded = false;
        BandUserProfile userProfileAsync = await this.GetUserProfileAsync(cancellationTokenSource.Token);
        userProfileAsync.BandName = this.BandName;
        await this.SaveUserProfileAsync(userProfileAsync, cancellationTokenSource.Token);
        ApplicationTelemetry.LogBandRename(false);
        this.ShowProgress = false;
        this.IsNameLoaded = true;
        MyBandViewModel.Logger.Debug((object) "<END> save band name");
        cancellationTokenSource = (CancellationTokenSource) null;
      }
      catch (Exception ex)
      {
        MyBandViewModel.Logger.Error(ex, "<FAILED> save band name");
        this.ShowProgress = false;
        this.IsNameLoaded = true;
        this.DisplaySaveChanges = true;
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    }

    protected override void OnBackNavigation() => this.UpdateAppBar();

    private void UpdateAppBar()
    {
      if (this.DisplaySaveChanges)
        this.ShowAppBar();
      else
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private async void OnUnregisterBand()
    {
      if (await this.messageBoxService.ShowAsync(AppResources.UnregisterThisBandConfirmationBody, AppResources.UnregisterThisBandConfirmationTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
        return;
      this.UnregisterBand();
    }

    private async void UnregisterBand()
    {
      MyBandViewModel.Logger.Debug((object) "<START> Unregistering band");
      this.ShowProgress = true;
      try
      {
        await this.userProfileService.UnlinkBandFromProfileAsync(new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan).Token);
        this.bandThemeManager.SetCurrentTheme(this.bandThemeManager.DefaultTheme);
        this.golfSyncService.ClearLastSyncedCourse();
        this.smoothNavService.Navigate(typeof (TilesViewModel));
        this.smoothNavService.ClearBackStack();
        MyBandViewModel.Logger.Debug((object) "<END> Unregistering band");
      }
      catch (Exception ex)
      {
        MyBandViewModel.Logger.Error(ex, "<FAILED> Unregistering band");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      finally
      {
        this.ShowProgress = false;
      }
    }

    internal void AddError(string propertyName, string errorMessage)
    {
      this.errors[propertyName] = (IList<string>) new List<string>()
      {
        errorMessage
      };
      this.OnErrorsChanged(propertyName);
    }

    internal void ClearError(string propertyName)
    {
      if (this.errors.ContainsKey(propertyName))
        this.errors.Remove(propertyName);
      this.OnErrorsChanged(propertyName);
    }

    private void OnErrorsChanged(string propertyName)
    {
      EventHandler<DataErrorsChangedEventArgs> errorsChanged = this.ErrorsChanged;
      if (errorsChanged != null)
        errorsChanged((object) this, new DataErrorsChangedEventArgs(propertyName));
      this.RefreshBandStatus();
    }

    private void RefreshBandStatus() => this.HasErrors = this.errors.Count > 0;

    public AppBandTheme CurrentBandTheme => this.bandThemeManager.CurrentTheme;

    public ResourceIdentifier CurrentWallpaperResource => this.packageResourceService.GetWallpaperResourceId(this.CurrentBandTheme);

    public int BatteryCharge
    {
      get => this.batteryCharge;
      set => this.SetProperty<int>(ref this.batteryCharge, value, nameof (BatteryCharge));
    }

    public string BandName
    {
      get => this.bandName;
      set
      {
        if (this.IsNameLoaded)
        {
          if (string.IsNullOrWhiteSpace(value) || value.Length < 1 || value.Length > 15)
          {
            this.AddError(nameof (BandName), string.Format(AppResources.CharacterRangeErrorMessage, new object[2]
            {
              (object) 1,
              (object) 15
            }));
            if (this.DisplaySaveChanges)
              this.DisplaySaveChanges = false;
          }
          else
          {
            this.ClearError(nameof (BandName));
            if (!this.DisplaySaveChanges)
              this.DisplaySaveChanges = true;
          }
        }
        this.SetProperty<string>(ref this.bandName, value, nameof (BandName));
      }
    }

    public bool DisplayBandData
    {
      get => this.displayBandData;
      set => this.SetProperty<bool>(ref this.displayBandData, value, nameof (DisplayBandData));
    }

    public bool ShowProgress
    {
      get => this.showProgress;
      set => this.SetProperty<bool>(ref this.showProgress, value, nameof (ShowProgress));
    }

    public bool PageLoading
    {
      get => this.pageLoading;
      set => this.SetProperty<bool>(ref this.pageLoading, value, nameof (PageLoading));
    }

    public bool IsNameLoaded
    {
      get => this.isNameLoaded;
      set => this.SetProperty<bool>(ref this.isNameLoaded, value, nameof (IsNameLoaded));
    }

    public bool ShowError
    {
      get => this.showError;
      set => this.SetProperty<bool>(ref this.showError, value, nameof (ShowError));
    }

    public bool DisplaySaveChanges
    {
      get => this.displaySaveChanges;
      set
      {
        this.SetProperty<bool>(ref this.displaySaveChanges, value, nameof (DisplaySaveChanges));
        this.UpdateAppBar();
      }
    }

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand(new Action(this.SaveBandName)));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.Cancel)));

    private void Cancel()
    {
      this.bandName = this.originalBandName;
      this.RaisePropertyChanged("BandName");
      this.DisplaySaveChanges = false;
    }

    public ICommand UnregisterBandCommand => (ICommand) this.unregisterBandCommand;

    public string ApiVersion => this.config.ApiVersion;

    public string AppVersion => this.environmentService.ApplicationVersion.ToString();

    public string FirmwareVersion
    {
      get => this.firmwareVersion;
      set => this.SetProperty<string>(ref this.firmwareVersion, value, nameof (FirmwareVersion));
    }

    public string SerialNumber
    {
      get => this.serialNumber;
      set => this.SetProperty<string>(ref this.serialNumber, value, nameof (SerialNumber));
    }

    public DateTimeOffset CurrentTime => DateTimeOffset.Now.ToLocalTime();

    private async Task<BandUserProfile> GetUserProfileAsync(
      CancellationToken token)
    {
      MyBandViewModel.Logger.Debug((object) "<START> get user profile");
      BandUserProfile userProfileAsync = await this.userProfileService.GetUserProfileAsync(token);
      MyBandViewModel.Logger.Debug((object) "<END> get user profile");
      return userProfileAsync;
    }

    private async Task SaveUserProfileAsync(
      BandUserProfile userProfileWrapper,
      CancellationToken token)
    {
      MyBandViewModel.Logger.Debug((object) "<START> save user profile");
      await this.userProfileService.SaveCloudAndBandUserProfileAsync(userProfileWrapper, token);
      MyBandViewModel.Logger.Debug((object) "<END> save user profile");
    }

    private async Task<FirmwareVersions> GetBandFirmwareVersionAsync(
      CancellationToken token)
    {
      MyBandViewModel.Logger.Debug((object) "<START> get band firmware version");
      FirmwareVersions firmwareVersions;
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
      {
        FirmwareVersions firmwareVersionAsync = await cargoConnection.GetBandFirmwareVersionAsync(token);
        MyBandViewModel.Logger.Debug("<END> get band firmware version (version={0})", (object) firmwareVersionAsync.ToString());
        firmwareVersions = firmwareVersionAsync;
      }
      return firmwareVersions;
    }

    private async Task<string> GetProductSerialNumberAsync(CancellationToken token)
    {
      MyBandViewModel.Logger.Debug((object) "<START> get product serial number");
      string str;
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
      {
        string serialNumberAsync = await cargoConnection.GetProductSerialNumberAsync(token);
        MyBandViewModel.Logger.Debug("<END> get product serial number (serialNumber={0})", (object) serialNumberAsync);
        str = serialNumberAsync;
      }
      return str;
    }
  }
}
