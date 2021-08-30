// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.HistoryViewModel
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
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.Services.ForegroundSync;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.ViewModels.History;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.HomeShell)]
  public class HistoryViewModel : PageViewModelBase
  {
    private const int ResultsPageSize = 20;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\History\\HistoryViewModel.cs");
    private readonly IBestEventProvider bestEventProvider;
    private readonly IConfig config;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly IDeviceManager deviceManager;
    private readonly INetworkService networkService;
    private readonly IHistoryProvider provider;
    private readonly ISmoothNavService smoothNavService;
    private readonly IDispatchService dispatchService;
    private IList<HistoryEventViewModel<UserEvent>> allEvents;
    private IList<HistoryEventViewModel<UserEvent>> bestEventDetails = (IList<HistoryEventViewModel<UserEvent>>) new ObservableCollection<HistoryEventViewModel<UserEvent>>();
    private IList<BestEvent> bestEvents;
    private IList<BestEvent> filteredBests;
    private int currentPage;
    private bool endReached;
    private bool exerciseFilterSelected;
    private BestEvent fastestPaceRun;
    private BestEvent fastestSplitRun;
    private BestEvent mostCalorieRun;
    private BestEvent furthestRun;
    private BestEvent largestElevationGainRide;
    private BestEvent furthestRide;
    private BestEvent mostCaloriesRide;
    private BestEvent fastestSpeedRide;
    private BestEvent longestDurationWorkout;
    private BestEvent mostCalorieWorkout;
    private BestEvent furthestHike;
    private BestEvent mostCaloriesHike;
    private BestEvent largestElevationGainHike;
    private BestEvent longestDurationHike;
    private EventType filter;
    private bool filterButtonEnabled;
    private ICommand filterCommand;
    private IList<FilterItem> filterValues;
    private bool filtersOpen;
    private HeaderType headerType;
    private bool hasHistory = true;
    private bool isFiltered;
    private bool isLoading = true;
    private long filterVersion;
    private bool backRefreshValid = true;
    private string resultAreaMessage;
    private bool retryScheduled;
    private bool runFilterSelected;
    private bool bikeFilterSelected;
    private bool hikeFilterSelected;
    private ICommand selectItemCommand;
    private FilterItem selectedFilterValue;
    private int selectedItemIndex = -1;
    private bool showFilteredBest;
    private ICommand toggleFiltersCommand;
    private ICommand viewBestEventCommand;

    public bool HasHistory
    {
      get => this.hasHistory;
      set => this.SetProperty<bool>(ref this.hasHistory, value, nameof (HasHistory));
    }

    public IList<BestEvent> FilteredBests
    {
      get => this.filteredBests;
      private set => this.SetProperty<IList<BestEvent>>(ref this.filteredBests, value, nameof (FilteredBests));
    }

    public bool IsLoading
    {
      get => this.isLoading;
      set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public string ResultAreaMessage
    {
      get => this.resultAreaMessage;
      set => this.SetProperty<string>(ref this.resultAreaMessage, value, nameof (ResultAreaMessage));
    }

    public IList<HistoryEventViewModel<UserEvent>> AllEvents
    {
      get => this.allEvents;
      set => this.SetProperty<IList<HistoryEventViewModel<UserEvent>>>(ref this.allEvents, value, nameof (AllEvents));
    }

    public IList<BestEvent> BestEvents
    {
      get => this.bestEvents;
      set => this.SetProperty<IList<BestEvent>>(ref this.bestEvents, value, nameof (BestEvents));
    }

    public IList<HistoryEventViewModel<UserEvent>> BestEventDetails
    {
      get => this.bestEventDetails;
      set => this.SetProperty<IList<HistoryEventViewModel<UserEvent>>>(ref this.bestEventDetails, value, nameof (BestEventDetails));
    }

    public EventType Filter
    {
      get => this.filter;
      set => this.SetProperty<EventType>(ref this.filter, value, nameof (Filter));
    }

    public FilterItem SelectedFilterValue
    {
      get => this.selectedFilterValue;
      set => this.SetProperty<FilterItem>(ref this.selectedFilterValue, value, nameof (SelectedFilterValue));
    }

    public IList<FilterItem> FilterValues
    {
      get => this.filterValues;
      set => this.SetProperty<IList<FilterItem>>(ref this.filterValues, value, nameof (FilterValues));
    }

    public bool FiltersOpen
    {
      get => this.filtersOpen;
      set
      {
        this.SetProperty<bool>(ref this.filtersOpen, value, nameof (FiltersOpen));
        this.HeaderType = value ? HeaderType.None : HeaderType.Normal;
      }
    }

    public HeaderType HeaderType
    {
      get => this.headerType;
      set => this.SetProperty<HeaderType>(ref this.headerType, value, nameof (HeaderType));
    }

    public bool FilterButtonEnabled
    {
      get => this.filterButtonEnabled;
      set => this.SetProperty<bool>(ref this.filterButtonEnabled, value, nameof (FilterButtonEnabled));
    }

    public bool RunFilterSelected
    {
      get => this.runFilterSelected;
      set => this.SetProperty<bool>(ref this.runFilterSelected, value, nameof (RunFilterSelected));
    }

    public bool BikeFilterSelected
    {
      get => this.bikeFilterSelected;
      set => this.SetProperty<bool>(ref this.bikeFilterSelected, value, nameof (BikeFilterSelected));
    }

    public bool ExerciseFilterSelected
    {
      get => this.exerciseFilterSelected;
      set => this.SetProperty<bool>(ref this.exerciseFilterSelected, value, nameof (ExerciseFilterSelected));
    }

    public bool HikeFilterSelected
    {
      get => this.hikeFilterSelected;
      set => this.SetProperty<bool>(ref this.hikeFilterSelected, value, nameof (HikeFilterSelected));
    }

    public bool IsFiltered
    {
      get => this.isFiltered;
      set => this.SetProperty<bool>(ref this.isFiltered, value, nameof (IsFiltered));
    }

    public bool ShowFilteredBest
    {
      get => this.showFilteredBest;
      set => this.SetProperty<bool>(ref this.showFilteredBest, value, nameof (ShowFilteredBest));
    }

    public ICommand LoadMoreCommand => (ICommand) new HealthCommand(new Action(this.Paginate));

    public ICommand ToggleFiltersCommand
    {
      get
      {
        if (this.toggleFiltersCommand == null)
          this.toggleFiltersCommand = (ICommand) new HealthCommand(new Action(this.ToggleFilters));
        return this.toggleFiltersCommand;
      }
      set => this.toggleFiltersCommand = value;
    }

    public ICommand FilterCommand
    {
      get
      {
        if (this.filterCommand == null)
          this.filterCommand = (ICommand) new HealthCommand<FilterItem>((Action<FilterItem>) (async filter => await this.FilterHistoryResultsAsync(filter)));
        return this.filterCommand;
      }
      set => this.filterCommand = value;
    }

    public ICommand SelectItemCommand
    {
      get
      {
        if (this.selectItemCommand == null)
          this.selectItemCommand = (ICommand) new HealthCommand<HistoryEventViewModel<UserEvent>>((Action<HistoryEventViewModel<UserEvent>>) (eventItem => this.SelectItem(eventItem)));
        return this.selectItemCommand;
      }
      set => this.selectItemCommand = value;
    }

    public ICommand ViewBestEventCommand
    {
      get
      {
        if (this.viewBestEventCommand == null)
          this.viewBestEventCommand = (ICommand) new HealthCommand<BestEvent>((Action<BestEvent>) (evt => this.SelectItem(evt)));
        return this.viewBestEventCommand;
      }
      set => this.viewBestEventCommand = value;
    }

    public HistoryViewModel(
      IHistoryProvider provider,
      IBestEventProvider bestEventProvider,
      IMessageBoxService messageBoxService,
      IConfig config,
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IDeviceManager deviceManager,
      IDispatchService dispatchService)
      : base(networkService)
    {
      if (provider == null)
        throw new ArgumentNullException(nameof (provider), "You must provide a valid exercise data provider to instantiate this class");
      this.config = config;
      this.networkService = networkService;
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.deviceManager = deviceManager;
      this.provider = provider;
      this.bestEventProvider = bestEventProvider;
      this.messageBoxService = messageBoxService;
      this.dispatchService = dispatchService;
      this.headerType = HeaderType.Normal;
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.deviceManager.SyncStateChanged += new EventHandler<SyncStateChangedEventArgs>(this.OnSyncStateChanged);
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
    }

    protected override void OnNavigatedFrom() => this.deviceManager.SyncStateChanged -= new EventHandler<SyncStateChangedEventArgs>(this.OnSyncStateChanged);

    private void OnBackKeyPressed(BackButtonPressedMessage message)
    {
      if (!this.FiltersOpen)
        return;
      HistoryViewModel.Logger.Info((object) "Back key pressed. Closing filters.");
      this.FiltersOpen = false;
      message.CancelAction();
    }

    public override void OnResume()
    {
      base.OnResume();
      if (!this.IsLoading)
        return;
      this.retryScheduled = true;
    }

    private async void OnSyncStateChanged(object sender, SyncStateChangedEventArgs eventArgs)
    {
      if (eventArgs.IsSyncing || !eventArgs.PercentComplete.Equals(100.0))
        return;
      await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (() => this.LoadAsync((IDictionary<string, string>) null)));
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      try
      {
        this.Filter = EventType.None;
        this.currentPage = 0;
        this.SetupFilterValues();
        this.AllEvents = (IList<HistoryEventViewModel<UserEvent>>) new ObservableCollection<HistoryEventViewModel<UserEvent>>();
        this.BestEventDetails = (IList<HistoryEventViewModel<UserEvent>>) new ObservableCollection<HistoryEventViewModel<UserEvent>>();
        this.BestEvents = (IList<BestEvent>) new List<BestEvent>();
        this.fastestPaceRun = (BestEvent) null;
        this.furthestRun = (BestEvent) null;
        this.mostCalorieRun = (BestEvent) null;
        this.fastestSplitRun = (BestEvent) null;
        this.mostCalorieWorkout = (BestEvent) null;
        this.longestDurationWorkout = (BestEvent) null;
        this.furthestHike = (BestEvent) null;
        this.mostCaloriesHike = (BestEvent) null;
        this.longestDurationHike = (BestEvent) null;
        this.largestElevationGainHike = (BestEvent) null;
        await this.SetupBestEventsAsync();
        await this.LoadNewEventsAsync(this.Filter, true);
        bool flag = this.AllEvents != null && this.AllEvents.Any<HistoryEventViewModel<UserEvent>>();
        this.FilterButtonEnabled = flag;
        this.HasHistory = flag;
        this.ShowFilteredBest = false;
        string filterValue;
        if (parameters != null && parameters.TryGetValue("Filter", out filterValue))
        {
          EventType filter = EventType.None;
          Enum.TryParse<EventType>(filterValue, out filter);
          if (!EventType.None.Equals((object) filter) && !EventType.Unknown.Equals((object) filter))
          {
            this.SelectedFilterValue = this.FilterValues.FirstOrDefault<FilterItem>((Func<FilterItem, bool>) (fv => fv.FilteredEventType.Equals((object) filter)));
            await this.FilterHistoryResultsAsync(this.SelectedFilterValue, true);
          }
        }
        if (this.Filter == EventType.None)
          ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new string[3]
          {
            "Fitness",
            "History",
            HistoryViewModel.GetEventTaxonomy(this.Filter)
          });
        this.IsLoading = false;
        filterValue = (string) null;
      }
      catch (Exception ex)
      {
        if (this.networkService.IsInternetAvailable)
        {
          throw;
        }
        else
        {
          this.HasHistory = true;
          this.IsLoading = false;
          this.ResultAreaMessage = AppResources.InternetRequiredMessage;
        }
      }
    }

    protected override void OnBackNavigation()
    {
      base.OnBackNavigation();
      if (this.selectedItemIndex < 0)
        return;
      this.StartBackRefresh();
    }

    private async void StartBackRefresh()
    {
      this.backRefreshValid = true;
      await this.LoadHistoryItemAtIndexAsync(this.selectedItemIndex);
      if (this.AllEvents.Count != 0 || !this.backRefreshValid)
        return;
      this.SetNoResultsMessage();
      if (this.Filter != EventType.None)
        return;
      this.HasHistory = false;
    }

    private void SetupFilterValues()
    {
      this.FilterValues = (IList<FilterItem>) new List<FilterItem>();
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameAll, AppResources.HistoryFilterUnitLabelMetric, EventType.None));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameBest, AppResources.HistoryFilterUnitLabelMetric, EventType.Best));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameRuns, AppResources.HistoryFilterUnitLabelDistance, EventType.Running));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameBikeRides, AppResources.HistoryFilterUnitLabelMetric, EventType.Biking));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameGolf, AppResources.HistoryFilterUnitLabelScore, EventType.Golf));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameExercises, AppResources.HistoryFilterUnitLabelCalories, EventType.Workout));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameSleep, AppResources.HistoryFilterUnitLabelTime, EventType.Sleeping));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameGuidedWorkout, AppResources.HistoryFilterUnitLabelCalories, EventType.GuidedWorkout));
      this.FilterValues.Add(new FilterItem(AppResources.EventTypeFilterNameHike, AppResources.HistoryFilterUnitLabelMetric, EventType.Hike));
      this.SelectedFilterValue = this.FilterValues.FirstOrDefault<FilterItem>((Func<FilterItem, bool>) (item => item.FilteredEventType == this.Filter));
    }

    private async Task LoadNewEventsAsync(EventType type = EventType.None, bool throwErrors = false)
    {
      this.Filter = type;
      long loadFilterVersion = this.filterVersion;
      IList<HistoryEventViewModel<UserEvent>> nextPageOfEvents = (IList<HistoryEventViewModel<UserEvent>>) null;
      bool succeeded = true;
      this.endReached = false;
      try
      {
        if (type == EventType.Best)
        {
          if (this.AllEvents.Any<HistoryEventViewModel<UserEvent>>())
            return;
          this.AllEvents = (IList<HistoryEventViewModel<UserEvent>>) new ObservableCollection<HistoryEventViewModel<UserEvent>>((IEnumerable<HistoryEventViewModel<UserEvent>>) this.BestEventDetails);
        }
        else
        {
          DateTimeOffset? beforeDate = new DateTimeOffset?();
          if (this.AllEvents != null)
          {
            HistoryEventViewModel<UserEvent> historyEventViewModel = this.AllEvents.LastOrDefault<HistoryEventViewModel<UserEvent>>();
            if (historyEventViewModel != null)
              beforeDate = new DateTimeOffset?(historyEventViewModel.Event.StartTime);
          }
          nextPageOfEvents = (IList<HistoryEventViewModel<UserEvent>>) (await this.provider.GetEventHistoryAsync<UserEvent>(type, 20, beforeDate)).Where<HistoryEventViewModel<UserEvent>>((Func<HistoryEventViewModel<UserEvent>, bool>) (evt => evt.Event.EventType != EventType.Unknown)).ToArray<HistoryEventViewModel<UserEvent>>();
        }
      }
      catch (Exception ex)
      {
        if (this.filterVersion != loadFilterVersion)
          return;
        if (!this.retryScheduled)
        {
          HistoryViewModel.Logger.Fatal(ex, "Loading of events failed for type {0}", (object) type);
          this.IsLoading = false;
          if (throwErrors)
          {
            throw;
          }
          else
          {
            if (this.AllEvents == null || this.AllEvents.Count == 0)
            {
              this.SetFilterFailureMessage();
              return;
            }
            this.StartShowingNetworkError();
            return;
          }
        }
        else
          succeeded = false;
      }
      if (this.filterVersion != loadFilterVersion)
        return;
      await this.HandlePageLoadAsync(type, succeeded, nextPageOfEvents);
    }

    private async Task HandlePageLoadAsync(
      EventType type,
      bool succeeded,
      IList<HistoryEventViewModel<UserEvent>> nextPageOfEvents)
    {
      if (!succeeded && this.retryScheduled)
      {
        this.retryScheduled = false;
        await this.LoadNewEventsAsync(type);
      }
      this.retryScheduled = false;
      if (nextPageOfEvents != null && nextPageOfEvents.Any<HistoryEventViewModel<UserEvent>>())
      {
        this.bestEventProvider.PopulateAllEvents<UserEvent>((ICollection<HistoryEventViewModel<UserEvent>>) nextPageOfEvents, (ICollection<BestEvent>) this.BestEvents, (ICollection<HistoryEventViewModel<UserEvent>>) this.AllEvents);
        this.ResultAreaMessage = (string) null;
      }
      else if (this.AllEvents == null || !this.AllEvents.Any<HistoryEventViewModel<UserEvent>>())
      {
        if (this.Filter == EventType.Best && this.BestEvents == null)
          succeeded = false;
        this.SetResultsAreaMessage(succeeded);
        this.endReached = true;
      }
      else if (nextPageOfEvents != null && nextPageOfEvents.Count == 0)
        this.endReached = true;
      this.IsLoading = false;
    }

    private async void StartShowingNetworkError()
    {
      int num = (int) await this.messageBoxService.ShowAsync(AppResources.NetworkErrorBody, AppResources.NetworkErrorTitle, PortableMessageBoxButton.OK);
    }

    private async Task SetupBestEventsAsync()
    {
      IList<BestEvent> bestEventsAsync = await this.bestEventProvider.GetBestEventsAsync(GoalType.Unknown);
      this.BestEvents = bestEventsAsync == null ? (IList<BestEvent>) null : (IList<BestEvent>) bestEventsAsync.ToList<BestEvent>();
      if (this.BestEvents == null || !this.BestEvents.Any<BestEvent>())
        return;
      this.fastestPaceRun = this.GetBestEvent(BestEventReason.FastestPaceRun);
      this.furthestRun = this.GetBestEvent(BestEventReason.FurthestRun);
      this.mostCalorieRun = this.GetBestEvent(BestEventReason.MostCaloriesRun);
      this.fastestSplitRun = this.GetBestEvent(BestEventReason.FastestSplitRun);
      this.largestElevationGainRide = this.GetBestEvent(BestEventReason.LargestElevationGainRide);
      this.furthestRide = this.GetBestEvent(BestEventReason.FurthestRide);
      this.mostCaloriesRide = this.GetBestEvent(BestEventReason.MostCaloriesRide);
      this.fastestSpeedRide = this.GetBestEvent(BestEventReason.FastestSpeedRide);
      this.mostCalorieWorkout = this.GetBestEvent(BestEventReason.MostCaloriesWorkout);
      this.longestDurationWorkout = this.GetBestEvent(BestEventReason.LongestDurationWorkout);
      this.furthestHike = this.GetBestEvent(BestEventReason.FurthestHike);
      this.mostCaloriesHike = this.GetBestEvent(BestEventReason.MostCaloriesHike);
      this.largestElevationGainHike = this.GetBestEvent(BestEventReason.LargestElevationGainHike);
      this.longestDurationHike = this.GetBestEvent(BestEventReason.LongestDurationHike);
      IList<HistoryEventViewModel<UserEvent>> eventsDetailsAsync = await this.bestEventProvider.GetBestEventsDetailsAsync<UserEvent>((ICollection<string>) this.BestEvents.Select<BestEvent, string>((Func<BestEvent, string>) (evt => evt.EventId)).ToList<string>());
      if (eventsDetailsAsync == null || !eventsDetailsAsync.Any<HistoryEventViewModel<UserEvent>>())
        return;
      this.BestEventDetails = (IList<HistoryEventViewModel<UserEvent>>) new ObservableCollection<HistoryEventViewModel<UserEvent>>((IEnumerable<HistoryEventViewModel<UserEvent>>) eventsDetailsAsync);
    }

    private BestEvent GetBestEvent(BestEventReason reason) => this.BestEvents.FirstOrDefault<BestEvent>((Func<BestEvent, bool>) (evt => evt.Reason == reason));

    private async void Paginate()
    {
      if (this.IsLoading || this.endReached)
        return;
      ++this.currentPage;
      this.IsLoading = true;
      await this.LoadNewEventsAsync(this.filter);
      this.IsLoading = false;
    }

    private async Task FilterHistoryResultsAsync(FilterItem filter, bool throwErrors = false)
    {
      if (filter == null)
        return;
      if (this.BestEvents == null)
      {
        try
        {
          await this.SetupBestEventsAsync();
        }
        catch (Exception ex)
        {
          if (throwErrors)
          {
            throw;
          }
          else
          {
            this.StartShowingNetworkError();
            return;
          }
        }
      }
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new string[3]
      {
        "Fitness",
        "History",
        HistoryViewModel.GetEventTaxonomy(filter.FilteredEventType)
      });
      this.SelectedFilterValue = filter;
      this.FiltersOpen = false;
      this.ResultAreaMessage = (string) null;
      EventType result = EventType.None;
      if (Enum.TryParse<EventType>(filter.FilteredEventType.ToString(), out result))
      {
        this.IsFiltered = (uint) result > 0U;
        this.RunFilterSelected = result == EventType.Running;
        this.BikeFilterSelected = result == EventType.Biking;
        this.ExerciseFilterSelected = result == EventType.Workout || result == EventType.GuidedWorkout;
        this.HikeFilterSelected = result == EventType.Hike;
        this.FilterBests(result);
        if (this.AllEvents == null)
          this.AllEvents = (IList<HistoryEventViewModel<UserEvent>>) new ObservableCollection<HistoryEventViewModel<UserEvent>>();
        else
          this.AllEvents.Clear();
        this.backRefreshValid = false;
        this.currentPage = 0;
        this.IsLoading = true;
        ++this.filterVersion;
        await this.LoadNewEventsAsync(result, throwErrors);
      }
      this.ShowFilteredBest = this.AllEvents.Count > 0;
    }

    private void FilterBests(EventType filterType)
    {
      List<BestEvent> bestEventList;
      switch (filterType)
      {
        case EventType.Running:
          bestEventList = new List<BestEvent>()
          {
            this.furthestRun,
            this.fastestPaceRun,
            this.mostCalorieRun,
            this.fastestSplitRun
          };
          break;
        case EventType.Workout:
        case EventType.GuidedWorkout:
          bestEventList = new List<BestEvent>()
          {
            this.mostCalorieWorkout,
            this.longestDurationWorkout
          };
          break;
        case EventType.Biking:
          bestEventList = new List<BestEvent>()
          {
            this.furthestRide,
            this.fastestSpeedRide,
            this.mostCaloriesRide,
            this.largestElevationGainRide
          };
          break;
        case EventType.Hike:
          bestEventList = new List<BestEvent>()
          {
            this.furthestHike,
            this.mostCaloriesHike,
            this.largestElevationGainHike,
            this.longestDurationHike
          };
          break;
        default:
          bestEventList = new List<BestEvent>();
          break;
      }
      bestEventList.RemoveAll((Predicate<BestEvent>) (item => item == null));
      this.FilteredBests = (IList<BestEvent>) bestEventList;
    }

    private static string GetEventTaxonomy(EventType eventType)
    {
      switch (eventType)
      {
        case EventType.Best:
          return "Bests";
        case EventType.None:
          return "All";
        case EventType.Running:
          return "Runs";
        case EventType.Sleeping:
          return "Sleep";
        case EventType.Workout:
          return "Exercises";
        case EventType.GuidedWorkout:
          return "Guided workouts";
        case EventType.Biking:
          return "Bike rides";
        case EventType.Hike:
          return "Hikes";
        case EventType.Golf:
          return "Golf";
        default:
          HistoryViewModel.Logger.WarnAndDebug("Unknown event type in GetEventTaxonomy: " + (object) eventType);
          return eventType.ToString();
      }
    }

    private async Task LoadHistoryItemAtIndexAsync(int index)
    {
      if (index >= this.AllEvents.Count || index < 0)
      {
        HistoryViewModel.Logger.Warn("Invalid index requested to load.  Index: {0}, Items Counts: {1}", (object) index, (object) this.AllEvents.Count);
      }
      else
      {
        HistoryEventViewModel<UserEvent> localItem = this.AllEvents[index];
        HistoryEventViewModel<UserEvent> serverItem = (HistoryEventViewModel<UserEvent>) null;
        try
        {
          await this.SetupBestEventsAsync();
        }
        catch (Exception ex)
        {
          HistoryViewModel.Logger.Error(ex, "Unable to refresh the best events.");
        }
        try
        {
          serverItem = await this.provider.GetHistoryItemAsync<UserEvent>(localItem.Event.EventId);
        }
        catch (Exception ex)
        {
          HistoryViewModel.Logger.Error(ex, "Unable to retrieve item at history index of {0}", (object) index);
          return;
        }
        try
        {
          if (this.backRefreshValid)
          {
            if (serverItem != null)
            {
              this.bestEventProvider.CheckForBest((HistoryEventViewModelBase) serverItem, (ICollection<BestEvent>) this.bestEvents);
              if (index >= this.AllEvents.Count)
              {
                HistoryViewModel.Logger.Warn("Invalid index requested to set. Index: {0}, Items Counts: {1}", (object) index, (object) this.AllEvents.Count);
                return;
              }
              this.AllEvents[index] = serverItem;
            }
            else
            {
              if (index >= this.AllEvents.Count)
              {
                HistoryViewModel.Logger.Warn("Invalid index requested to remove. Index: {0}, Items Counts: {1}", (object) index, (object) this.AllEvents.Count);
                return;
              }
              this.AllEvents.RemoveAt(index);
              for (int index1 = 0; index1 < this.AllEvents.Count; ++index1)
              {
                HistoryEventViewModel<UserEvent> allEvent = this.AllEvents[index1];
                if (!allEvent.IsBest)
                {
                  this.bestEventProvider.CheckForBest((HistoryEventViewModelBase) allEvent, (ICollection<BestEvent>) this.BestEvents);
                  if (allEvent.IsBest)
                  {
                    this.AllEvents.RemoveAt(index1);
                    this.AllEvents.Insert(index1, allEvent);
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          HistoryViewModel.Logger.Error(ex, "Failed to complete back refresh. Index: {0}, Items Counts: {1}", (object) index, (object) this.AllEvents.Count);
        }
        this.selectedItemIndex = -1;
      }
    }

    private void SetResultsAreaMessage(bool succeeded)
    {
      this.ShowFilteredBest = false;
      if (succeeded)
        this.SetNoResultsMessage();
      else if (!this.networkService.IsInternetAvailable)
        this.ResultAreaMessage = AppResources.InternetRequiredMessage;
      else
        this.ResultAreaMessage = AppResources.DataErrorMessage;
    }

    private void SetNoResultsMessage()
    {
      this.ShowFilteredBest = false;
      if (this.Filter == EventType.None)
        this.HasHistory = false;
      else if (this.Filter == EventType.Best)
      {
        this.ResultAreaMessage = AppResources.NoResultsMessage;
      }
      else
      {
        string filterName = HistoryViewModel.GetFilterName(this.Filter);
        if (filterName == null)
          return;
        this.ResultAreaMessage = string.Format(AppResources.HistoryNoResultsForFilterTypeMessage, new object[1]
        {
          (object) filterName.ToLower()
        });
      }
    }

    private void SetFilterFailureMessage()
    {
      this.ShowFilteredBest = false;
      this.HasHistory = true;
      this.IsLoading = false;
      this.endReached = true;
      this.ResultAreaMessage = this.networkService.IsInternetAvailable ? AppResources.DataErrorMessage : AppResources.InternetRequiredMessage;
    }

    private static string GetFilterName(EventType eventType)
    {
      switch (eventType)
      {
        case EventType.Best:
          return AppResources.EventTypeFilterNameBest;
        case EventType.Running:
          return AppResources.EventTypeFilterNameRuns;
        case EventType.Sleeping:
          return AppResources.EventTypeFilterNameSleep;
        case EventType.Workout:
          return AppResources.EventTypeFilterNameExercises;
        case EventType.GuidedWorkout:
          return AppResources.EventTypeFilterNameGuidedWorkout;
        case EventType.Biking:
          return AppResources.EventTypeFilterNameBikeRides;
        case EventType.Hike:
          return AppResources.EventTypeFilterNameHike;
        case EventType.Golf:
          return AppResources.EventTypeFilterNameGolf;
        default:
          return (string) null;
      }
    }

    private void ToggleFilters()
    {
      this.FiltersOpen = !this.FiltersOpen;
      if (!this.FiltersOpen)
        return;
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new string[3]
      {
        "Fitness",
        "History",
        "Filters Summary"
      });
    }

    private void SelectItem(HistoryEventViewModel<UserEvent> item)
    {
      if (item != null)
      {
        HistoryEventViewModel<UserEvent> historyEventViewModel = this.AllEvents.FirstOrDefault<HistoryEventViewModel<UserEvent>>((Func<HistoryEventViewModel<UserEvent>, bool>) (e => e.EventId == item.EventId));
        if (historyEventViewModel != null)
          item = historyEventViewModel;
      }
      this.selectedItemIndex = this.AllEvents.IndexOf(item);
      if (item == null)
        return;
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      dictionary1.Add("ID", item.EventId);
      dictionary1.Add("Type", item.EventType.ToString());
      dictionary1.Add("History", bool.FalseString);
      if (item.EventType == EventType.GuidedWorkout && item.Event is WorkoutEvent workoutEvent)
      {
        dictionary1.Add("WeekId", workoutEvent.WorkoutWeekId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        Dictionary<string, string> dictionary2 = dictionary1;
        int num = workoutEvent.WorkoutDayId;
        string str1 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        dictionary2.Add("DayId", str1);
        dictionary1.Add("WorkoutPlanId", workoutEvent.WorkoutPlanId);
        Dictionary<string, string> dictionary3 = dictionary1;
        num = workoutEvent.WorkoutIndex;
        string str2 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        dictionary3.Add("WorkoutIndex", str2);
      }
      this.smoothNavService.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) dictionary1);
    }

    private void SelectItem(BestEvent evt)
    {
      if (evt.BestValue == null || string.IsNullOrEmpty(evt.StartTimeStr))
        return;
      this.SelectItem(this.BestEventDetails.FirstOrDefault<HistoryEventViewModel<UserEvent>>((Func<HistoryEventViewModel<UserEvent>, bool>) (historyEvent => historyEvent.EventId == evt.EventId)));
    }
  }
}
