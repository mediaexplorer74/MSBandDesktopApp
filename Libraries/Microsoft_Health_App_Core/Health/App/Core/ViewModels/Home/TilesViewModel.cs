// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.TilesViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Bluetooth;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Social;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.Utilities.EqualityComparer;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using Microsoft.Health.App.Core.ViewModels.Coaching;
using Microsoft.Health.App.Core.ViewModels.Golf;
using Microsoft.Health.App.Core.ViewModels.Social;
using Microsoft.Health.App.Core.ViewModels.WeightTracking;
using Microsoft.Health.App.Core.ViewModels.Workouts;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  [PageTaxonomy(new string[] {"App", "Home"})]
  [PageMetadata(PageContainerType.HomeShell)]
  public class TilesViewModel : RefreshablePageViewModelBase, IPageTaxonomyTypeProvider
  {
    internal const string TileKey = "Tile";
    internal const string PivotKey = "Pivot";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\TilesViewModel.cs");
    private readonly object persistentMessageListener = new object();
    private readonly IEqualityComparer<AppBandTile> appBandTileInstanceEqualityComparer = (IEqualityComparer<AppBandTile>) EqualsOverride.Create<AppBandTile>(new Func<AppBandTile, AppBandTile, bool>(object.ReferenceEquals));
    private readonly IDispatchService dispatchService;
    private readonly IUserProfileService userProfileService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly ITileManagementService tileManager;
    private readonly ISmoothNavService smoothNavService;
    private readonly IServiceLocator serviceLocator;
    private readonly IMessageSender messageSender;
    private readonly ISocialEngagementService socialEngagementService;
    private readonly IBluetoothService bluetoothService;
    private readonly IMessageBoxService messageBoxService;
    private readonly ITileNotificationService tileNotificationService;
    private readonly IConfigProvider configProvider;
    private readonly Lazy<AppBandTile[]> fixedRegisteredBandTiles;
    private readonly Lazy<AppBandTile[]> fixedNoBandTiles;
    private readonly Lazy<AppBandTile[]> addToFixedRegisteredBandTilesIfBandTileDisabled;
    private readonly ObservableCollection<TileViewModel> tiles = new ObservableCollection<TileViewModel>();
    private readonly object promptLock = new object();
    private bool hasBeenLoaded;
    private bool showFirstTimeUseControl;
    private bool showPivot;
    private bool showPivotHeader;
    private MetricTileViewModel selectedTile;
    private HealthCommand manageCommand;
    private ICoachingService coachingService;

    public TilesViewModel(
      IRefreshService refreshService,
      IDispatchService dispatchService,
      INetworkService networkService,
      IUserProfileService userProfileService,
      IErrorHandlingService errorHandlingService,
      ITileManagementService tileManager,
      ITileNotificationService tileNotificationService,
      ISmoothNavService smoothNavService,
      IServiceLocator serviceLocator,
      IMessageSender messageSender,
      ICoachingService coachingService,
      ISocialEngagementService socialEngagementService,
      IBluetoothService bluetoothService,
      IMessageBoxService messageBoxService,
      IConfigProvider configProvider)
      : base(refreshService, dispatchService, networkService)
    {
      this.dispatchService = dispatchService;
      this.userProfileService = userProfileService;
      this.errorHandlingService = errorHandlingService;
      this.tileManager = tileManager;
      this.tileNotificationService = tileNotificationService;
      this.smoothNavService = smoothNavService;
      this.serviceLocator = serviceLocator;
      this.messageSender = messageSender;
      this.coachingService = coachingService;
      this.socialEngagementService = socialEngagementService;
      this.bluetoothService = bluetoothService;
      this.messageBoxService = messageBoxService;
      this.configProvider = configProvider;
      this.UseTransitions = true;
      this.messageSender.Register<EventChangedMessage>(this.persistentMessageListener, (Action<EventChangedMessage>) (message =>
      {
        if (this.IsActive || message.IsRefreshCanceled)
          return;
        this.RefreshPending = true;
      }));
      this.fixedRegisteredBandTiles = new Lazy<AppBandTile[]>((Func<AppBandTile[]>) (() => new AppBandTile[2]
      {
        this.tileManager.KnownTiles.StepsTile,
        this.tileManager.KnownTiles.CaloriesTile
      }));
      this.fixedNoBandTiles = new Lazy<AppBandTile[]>((Func<AppBandTile[]>) (() => new AppBandTile[8]
      {
        this.tileManager.KnownTiles.StepsTile,
        this.tileManager.KnownTiles.CaloriesTile,
        this.tileManager.KnownTiles.SleepTile,
        this.tileManager.KnownTiles.RunTile,
        this.tileManager.KnownTiles.BikeTile,
        this.tileManager.KnownTiles.ExerciseTile,
        this.tileManager.KnownTiles.GuidedWorkoutResultTile,
        this.tileManager.KnownTiles.GolfTile
      }));
      this.addToFixedRegisteredBandTilesIfBandTileDisabled = new Lazy<AppBandTile[]>((Func<AppBandTile[]>) (() => new AppBandTile[1]
      {
        this.tileManager.KnownTiles.SleepTile
      }));
    }

    public string StartupTile { get; set; }

    public bool DelayTilesLoad { get; set; }

    public bool UseTransitions { get; private set; }

    public IList<TileViewModel> Tiles => (IList<TileViewModel>) this.tiles;

    public MetricTileViewModel SelectedTile
    {
      get => this.selectedTile;
      set
      {
        if (this.SelectedTile == value)
          return;
        if (this.selectedTile != null)
        {
          ApplicationTelemetry.LogTileTap((TileViewModel) this.selectedTile, false, this.ShowFirstTimeUseControl);
          if (this.selectedTile.Pivots is INotifyCollectionChanged pivots2)
            pivots2.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSelectedTilePivotsCollectionChanged);
          this.selectedTile.PropertyChanged -= new PropertyChangedEventHandler(this.OnSelectedTilePropertyChanged);
          this.selectedTile.FirstTimeUse.PropertyChanged -= new PropertyChangedEventHandler(this.OnSelectedTilePropertyChanged);
        }
        this.selectedTile = value;
        this.UpdateSelectedTileRelatedProperties(true);
        if (value != null)
        {
          this.selectedTile.FirstTimeUse.PropertyChanged += new PropertyChangedEventHandler(this.OnSelectedTilePropertyChanged);
          this.selectedTile.PropertyChanged += new PropertyChangedEventHandler(this.OnSelectedTilePropertyChanged);
          if (this.selectedTile.Pivots is INotifyCollectionChanged pivots5)
            pivots5.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSelectedTilePivotsCollectionChanged);
          foreach (PanelViewModelBase panelViewModelBase in this.selectedTile.Pivots.Select<PivotDefinition, object>((Func<PivotDefinition, object>) (p => p.Content)).OfType<PanelViewModelBase>())
            panelViewModelBase.LoadState = LoadState.Loading;
          ApplicationTelemetry.LogTileTap((TileViewModel) this.selectedTile, true, this.ShowFirstTimeUseControl);
        }
        this.RaisePropertyChanged(nameof (SelectedTile));
      }
    }

    public bool ManageButtonVisible => this.userProfileService.IsRegisteredBandPaired;

    public ICommand ManageCommand => (ICommand) this.manageCommand ?? (ICommand) (this.manageCommand = new HealthCommand(new Action(this.Manage)));

    public bool ShowFirstTimeUseControl
    {
      get => this.showFirstTimeUseControl;
      private set => this.SetProperty<bool>(ref this.showFirstTimeUseControl, value, nameof (ShowFirstTimeUseControl));
    }

    public bool ShowPivot
    {
      get => this.showPivot;
      private set => this.SetProperty<bool>(ref this.showPivot, value, nameof (ShowPivot));
    }

    public bool ShowPivotHeader
    {
      get => this.showPivotHeader;
      private set => this.SetProperty<bool>(ref this.showPivotHeader, value, nameof (ShowPivotHeader));
    }

    private async void Manage()
    {
      ApplicationTelemetry.LogTileTap("Manage Tiles", true, false);
      if (this.userProfileService.IsBandRegistered)
        this.smoothNavService.Navigate(typeof (ManageTilesViewModel));
      else
        await this.errorHandlingService.ShowBandErrorMessageAsync();
    }

    public void ExecuteTileCommand(TileViewModel tile)
    {
      Assert.ParamIsNotNull((object) tile, nameof (tile));
      if (!this.OpenInProgress && tile.TileCommand.CanExecute((object) null))
      {
        if (this.SelectedTile != tile)
        {
          TilesViewModel.Logger.Debug((object) string.Format("<TilesViewModel> Resetting {0}, and {1} for metric tiles in {2} before opening the new selected tile.", new object[3]
          {
            (object) "SelectedTile",
            (object) "IsOpen",
            (object) "Tiles"
          }));
          this.SelectedTile = (MetricTileViewModel) null;
          foreach (MetricTileViewModel metricTileViewModel in this.Tiles.OfType<MetricTileViewModel>().Where<MetricTileViewModel>((Func<MetricTileViewModel, bool>) (t => t.IsOpen)))
            metricTileViewModel.IsOpen = false;
          if (tile.WillOpenOnTileCommand())
            this.OpenInProgress = true;
          tile.TileCommand.Execute((object) null);
        }
        else
          TilesViewModel.Logger.Debug((object) string.Format("<TilesViewModel> {0} skipped because the tile is already selected.", new object[1]
          {
            (object) nameof (ExecuteTileCommand)
          }));
      }
      else
        TilesViewModel.Logger.Debug((object) string.Format("<TilesViewModel> {0} skipped because the tile cannot execute or because there's already an Open in progress.", new object[1]
        {
          (object) nameof (ExecuteTileCommand)
        }));
    }

    public void StartLoadPivots()
    {
      foreach (PanelViewModelBase panel in this.selectedTile.Pivots.Select<PivotDefinition, object>((Func<PivotDefinition, object>) (p => p.Content)).OfType<PanelViewModelBase>())
        TilesViewModel.StartLoadPivot(panel, this.Parameters);
    }

    protected static async void StartLoadPivot(
      PanelViewModelBase panel,
      IDictionary<string, string> parameters)
    {
      await panel.LoadAsync(parameters);
    }

    protected override async void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.smoothNavService.EnableNavPanel((System.Type) null);
      await this.LoadDataForTilesAsync();
      foreach (TileViewModel tile in (IEnumerable<TileViewModel>) this.Tiles)
        tile.NavigateTo();
    }

    protected override void OnNavigatedFrom()
    {
      try
      {
        base.OnNavigatedFrom();
        foreach (TileViewModel tile in (IEnumerable<TileViewModel>) this.Tiles)
          tile.NavigateFrom();
      }
      finally
      {
        this.OpenInProgress = false;
      }
    }

    public async Task LoadTilesAsync()
    {
      List<Task> taskList = new List<Task>();
      foreach (TileViewModel tile in (IEnumerable<TileViewModel>) this.Tiles)
      {
        tile.NavigateTo();
        taskList.Add(tile.LoadAsync((IDictionary<string, string>) null));
      }
      if (!this.hasBeenLoaded)
      {
        if (this.Parameters != null && this.Parameters.ContainsKey("Tile"))
          taskList.Add(this.dispatchService.RunOnUIThreadAsync((Action) (() =>
          {
            this.UseTransitions = false;
            foreach (TileViewModel tile in (IEnumerable<TileViewModel>) this.Tiles)
              tile.UseTransitions = false;
            this.SelectedTile = this.Tiles.OfType<MetricTileViewModel>().Where<MetricTileViewModel>((Func<MetricTileViewModel, bool>) (tp => tp.GetType().Name == this.Parameters["Tile"])).SingleOrDefault<MetricTileViewModel>();
            this.UseTransitions = true;
            foreach (TileViewModel tile in (IEnumerable<TileViewModel>) this.Tiles)
              tile.UseTransitions = true;
          }), false));
        this.hasBeenLoaded = true;
      }
      await Task.WhenAll((IEnumerable<Task>) taskList);
    }

    public override async Task ChangeAsync(IDictionary<string, string> parameters = null)
    {
      await base.ChangeAsync(parameters);
      if (parameters != null && parameters.ContainsKey("Tile"))
      {
        this.SelectedTile = this.Tiles.OfType<MetricTileViewModel>().Where<MetricTileViewModel>((Func<MetricTileViewModel, bool>) (tp => tp.GetType().Name == parameters["Tile"])).SingleOrDefault<MetricTileViewModel>();
        if (this.SelectedTile == null || !parameters.ContainsKey("Pivot"))
          return;
        foreach (PivotDefinition pivot in (IEnumerable<PivotDefinition>) this.SelectedTile.Pivots)
          pivot.IsSelected = pivot.Header.Equals(parameters["Pivot"]);
      }
      else
      {
        this.SelectedTile = (MetricTileViewModel) null;
        await this.LoadDataForTilesAsync();
      }
    }

    private async Task LoadDataForTilesAsync()
    {
      if (!this.tileManager.HaveTilesBeenUpdated)
        return;
      try
      {
        await this.LoadDataAsync((IDictionary<string, string>) null);
      }
      catch (Exception ex)
      {
      }
      this.tileManager.HaveTilesBeenUpdated = false;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters != null)
      {
        string str;
        parameters.TryGetValue("DelayTilesLoad", out str);
        bool result;
        bool.TryParse(str, out result);
        this.DelayTilesLoad = result;
      }
      try
      {
        ITimedTelemetryEvent timedEvent = ApplicationTelemetry.TimeHomePageLoad();
        await timedEvent.TimeWithStatusAsync((Func<Task>) (async () =>
        {
          foreach (TileViewModel tile in (IEnumerable<TileViewModel>) this.Tiles)
            tile.NavigateFrom();
          this.Tiles.Clear();
          foreach (TileViewModel shownTileViewModel in this.GetShownTileViewModels())
            this.Tiles.Add(shownTileViewModel);
          timedEvent.AddMetric("Number of Tiles", (double) this.Tiles.Count);
          if (this.DelayTilesLoad)
          {
            foreach (TileViewModel tileViewModel in this.Tiles.OfType<MetricTileViewModel>())
              await tileViewModel.ShowLoadingMessageAsync();
          }
          else
            await this.LoadTilesAsync();
          if (!this.configProvider.Get<bool>("PromptNotificationAccessIfRequired", false))
            return;
          int num = await this.tileNotificationService.VerifyNotificationRequirementsAsync((IEnumerable<AppBandTile>) this.tileManager.EnabledTiles) ? 1 : 0;
          this.configProvider.Set<bool>("PromptNotificationAccessIfRequired", false);
        }));
      }
      catch (Exception ex)
      {
        await this.errorHandlingService.HandleExceptionAsync(ex);
        throw;
      }
    }

    private IEnumerable<TileViewModel> GetShownTileViewModels()
    {
      List<TileViewModel> tileViewModelList = new List<TileViewModel>();
      bool? isPlanActive = this.coachingService.IsPlanActive;
      if (isPlanActive.HasValue)
      {
        if (isPlanActive.Value)
          tileViewModelList.Add((TileViewModel) this.serviceLocator.GetInstance<CoachingTileViewModel>());
      }
      else
        this.StartCoachingTileTask();
      if (this.socialEngagementService.IsSocialEnabled)
        tileViewModelList.Add((TileViewModel) this.serviceLocator.GetInstance<SocialTileViewModel>());
      foreach (AppBandTile shownTile in this.GetShownTiles(this.userProfileService.IsRegisteredBandPaired))
      {
        tileViewModelList.Add((TileViewModel) this.serviceLocator.GetInstance(shownTile.TileViewModelType));
        if (shownTile == this.tileManager.KnownTiles.GolfTile)
          tileViewModelList.Add((TileViewModel) this.serviceLocator.GetInstance<GolfSplitTileViewModel>());
        if (shownTile == this.tileManager.KnownTiles.GuidedWorkoutResultTile)
          tileViewModelList.Add((TileViewModel) this.serviceLocator.GetInstance<GuidedWorkoutSplitTileViewModel>());
      }
      tileViewModelList.Add((TileViewModel) this.serviceLocator.GetInstance<WeightTileViewModel>());
      return (IEnumerable<TileViewModel>) tileViewModelList;
    }

    private async void StartCoachingTileTask()
    {
      try
      {
        TilesViewModel.Logger.Debug((object) "Starting coaching tile background task");
        if (await this.coachingService.RefreshIsPlanActiveAsync(CancellationToken.None).ConfigureAwait(false) && this.SelectedTile == null)
        {
          TilesViewModel.Logger.Debug((object) "Adding coaching tile in background.");
          await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () =>
          {
            CoachingTileViewModel instance = this.serviceLocator.GetInstance<CoachingTileViewModel>();
            this.Tiles.Insert(0, (TileViewModel) instance);
            instance.NavigateTo();
            await instance.LoadAsync((IDictionary<string, string>) null);
          })).ConfigureAwait(false);
          TilesViewModel.Logger.Debug((object) "Coaching tile added.");
        }
        else if (this.SelectedTile != null)
          TilesViewModel.Logger.Warn((object) "Another tile is open, aborting add of coaching tile.");
        else
          TilesViewModel.Logger.Debug((object) "Coaching plan is not active, tile not added.");
      }
      catch (Exception ex)
      {
        TilesViewModel.Logger.Error((object) "Could not determine status of coaching plan. Leaving off tile for now.", ex);
      }
    }

    protected IEnumerable<AppBandTile> GetShownTiles(
      bool isRegisteredBandPaired)
    {
      if (!isRegisteredBandPaired)
        return (IEnumerable<AppBandTile>) this.fixedNoBandTiles.Value;
      List<AppBandTile> list = this.tileManager.EnabledTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileViewModelType != null)).Except<AppBandTile>((IEnumerable<AppBandTile>) this.fixedRegisteredBandTiles.Value, this.appBandTileInstanceEqualityComparer).ToList<AppBandTile>();
      return (IEnumerable<AppBandTile>) ((IEnumerable<AppBandTile>) this.fixedRegisteredBandTiles.Value).Concat<AppBandTile>(((IEnumerable<AppBandTile>) this.addToFixedRegisteredBandTilesIfBandTileDisabled.Value).Except<AppBandTile>((IEnumerable<AppBandTile>) list)).Concat<AppBandTile>((IEnumerable<AppBandTile>) list).ToArray<AppBandTile>();
    }

    public override async Task RefreshAsync()
    {
      List<Task> taskList = new List<Task>();
      if (this.SelectedTile != null)
      {
        foreach (PivotDefinition pivot in (IEnumerable<PivotDefinition>) this.SelectedTile.Pivots)
        {
          if (pivot.Content is PanelViewModelBase content2)
            taskList.Add(content2.RefreshAsync());
        }
      }
      taskList.AddRange(this.Tiles.Select<TileViewModel, Task>((Func<TileViewModel, Task>) (tile => tile.LoadAsync((IDictionary<string, string>) null))));
      taskList.Add(this.PromptAddPairedBandAsync());
      await Task.WhenAll((IEnumerable<Task>) taskList);
    }

    public override bool HeaderOpen => this.Parameters == null || !this.Parameters.ContainsKey("Tile");

    public bool OpenInProgress { get; set; }

    public System.Type GetPageTaxonomyType(bool isBackNavigation)
    {
      if (this.SelectedTile != null)
      {
        if (!isBackNavigation)
          return (System.Type) null;
        PivotDefinition pivotDefinition = this.SelectedTile.Pivots.FirstOrDefault<PivotDefinition>((Func<PivotDefinition, bool>) (pivot => pivot.IsSelected));
        if (pivotDefinition != null && pivotDefinition.Content != null)
          return pivotDefinition.Content.GetType();
        DebugUtilities.Fail("Unable to retrieve a proper taxonomy for the selected tile on the Home page.");
      }
      return this.GetType();
    }

    private void OnSelectedTilePropertyChanged(object sender, PropertyChangedEventArgs e) => this.UpdateSelectedTileRelatedProperties(false);

    private void OnSelectedTilePivotsCollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs e)
    {
      this.UpdateSelectedTileRelatedProperties(false);
    }

    private void UpdateSelectedTileRelatedProperties(bool selectedTileChanged)
    {
      if (this.SelectedTile != null)
      {
        if (selectedTileChanged || this.SelectedTile.LoadState == TileViewModel.TileLoadState.Loaded || this.SelectedTile.LoadState == TileViewModel.TileLoadState.NoData)
          this.ShowFirstTimeUseControl = this.SelectedTile.FirstTimeUse.IsSupported && this.SelectedTile.LoadState == TileViewModel.TileLoadState.NoData;
        this.ShowPivot = !this.ShowFirstTimeUseControl && this.SelectedTile.Pivots.Count > 0;
        this.ShowPivotHeader = this.ShowPivot && this.SelectedTile.ShowPivotHeader;
      }
      else
      {
        this.ShowFirstTimeUseControl = false;
        this.ShowPivot = false;
        this.ShowPivotHeader = false;
      }
    }

    private async Task PromptAddPairedBandAsync()
    {
      if (this.userProfileService.IsBandRegistered)
        return;
      bool lockTaken = false;
      try
      {
        Monitor.TryEnter(this.promptLock, TimeSpan.FromSeconds(0.0), ref lockTaken);
        if (!lockTaken)
          return;
        using (CancellationTokenSource tcs = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          try
          {
            if ((await this.bluetoothService.GetPairedBandsAsync(tcs.Token)).Length != 0)
            {
              if (this.NavigationState == PageNavigationState.Current)
              {
                int num = (int) await this.messageBoxService.ShowAsync(AppResources.AddBandMessageBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OKCancel);
                if (num == 0)
                  this.smoothNavService.Navigate(typeof (AddBandStartViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
                  {
                    {
                      "IsOobe",
                      bool.FalseString
                    }
                  });
                ApplicationTelemetry.LogNewBandDetected(num == 0);
              }
            }
          }
          catch (Exception ex)
          {
            TilesViewModel.Logger.Error((object) "Failed to get paired bands during refresh.", ex);
          }
        }
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(this.promptLock);
      }
    }
  }
}
