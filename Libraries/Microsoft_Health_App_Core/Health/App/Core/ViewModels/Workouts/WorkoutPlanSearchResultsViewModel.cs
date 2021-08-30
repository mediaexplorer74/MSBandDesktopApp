// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutPlanSearchResultsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  [PageTaxonomy(new string[] {"Fitness", "Guided Workouts", "Browse", "Results"})]
  public class WorkoutPlanSearchResultsViewModel : SearchResultsViewModelBase
  {
    public const string WorkoutTypeKey = "WorkoutType";
    public const string WorkoutTypeCustomWorkout = "Custom";
    public const string WorkoutTypeFavorite = "Favorite";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Workouts\\WorkoutPlanSearchResultsViewModel.cs");
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService navigation;
    private readonly IWorkoutsProvider provider;
    private IList<Histogram> availableFilters;
    private IList<WorkoutSearchFilter> baseSelectedFilters;
    private HealthCommand cancelCommand;
    private HealthCommand backCommand;
    private string categoryDisplayName;
    private string categoryImageUrl;
    private HealthCommand clearSelectedFilters;
    private HealthCommand confirmCommand;
    private IList<FilterGrouping> filterGrouping;
    private string filterSubTitle;
    private string filterTitle;
    private bool filtersOpen;
    private int lastWorkoutPlanOpenedIndex = -1;
    private WorkoutPlanSearchResultsViewModel.WorkoutType workoutType;
    private HealthCommand openFiltersCommand;
    private HealthCommand<WorkoutSearchResultViewModel> openWorkoutCommand;
    private string query;
    private string savedfilterGrouping;
    private HealthCommand searchCommand;
    private HealthCommand<FilterGrouping> selectFilterTypeCommand;
    private FilterGrouping selectedFilterGroup;
    private IList<WorkoutSearchFilter> selectedFilters;
    private bool showPublishDate;
    private bool showSearch;
    private bool showFilterClear;
    private bool showFilterSelect;
    private HealthCommand<SearchFilter> toggleFilterSelect;
    private string typeDisplayName;
    private IList<WorkoutSearchResultViewModel> workouts;

    public bool FiltersOpen
    {
      get => this.filtersOpen;
      set => this.SetProperty<bool>(ref this.filtersOpen, value, nameof (FiltersOpen));
    }

    public string CategoryName
    {
      get => this.categoryDisplayName;
      set => this.SetProperty<string>(ref this.categoryDisplayName, value, nameof (CategoryName));
    }

    public string TypeName
    {
      get => this.typeDisplayName;
      set => this.SetProperty<string>(ref this.typeDisplayName, value, nameof (TypeName));
    }

    public string CategoryImageUrl
    {
      get => this.categoryImageUrl;
      set => this.SetProperty<string>(ref this.categoryImageUrl, value, nameof (CategoryImageUrl));
    }

    public new bool ShowSearch
    {
      get => this.showSearch;
      set => this.SetProperty<bool>(ref this.showSearch, value, nameof (ShowSearch));
    }

    public bool ShowPublishDate
    {
      get => this.showPublishDate;
      set => this.SetProperty<bool>(ref this.showPublishDate, value, nameof (ShowPublishDate));
    }

    public IList<WorkoutSearchResultViewModel> Workouts
    {
      get => this.workouts;
      set => this.SetProperty<IList<WorkoutSearchResultViewModel>>(ref this.workouts, value, nameof (Workouts));
    }

    public IList<FilterGrouping> Filters
    {
      get => this.filterGrouping;
      set => this.SetProperty<IList<FilterGrouping>>(ref this.filterGrouping, value, nameof (Filters));
    }

    public bool ShowFilterSelect
    {
      get => this.showFilterSelect;
      set => this.SetProperty<bool>(ref this.showFilterSelect, value, nameof (ShowFilterSelect));
    }

    public bool ShowFilterClear
    {
      get => this.showFilterClear;
      set => this.SetProperty<bool>(ref this.showFilterClear, value, nameof (ShowFilterClear));
    }

    public string FilterTitle
    {
      get => this.filterTitle;
      set => this.SetProperty<string>(ref this.filterTitle, value, nameof (FilterTitle));
    }

    public string FilterSubTitle
    {
      get => this.filterSubTitle;
      set => this.SetProperty<string>(ref this.filterSubTitle, value, nameof (FilterSubTitle));
    }

    public FilterGrouping SelectedFilterGroup
    {
      get => this.selectedFilterGroup;
      set => this.SetProperty<FilterGrouping>(ref this.selectedFilterGroup, value, nameof (SelectedFilterGroup));
    }

    public override ICommand OpenFiltersCommand => (ICommand) this.openFiltersCommand ?? (ICommand) (this.openFiltersCommand = new HealthCommand(new Action(this.OpenFilters)));

    public ICommand OpenWorkoutCommand => (ICommand) this.openWorkoutCommand ?? (ICommand) (this.openWorkoutCommand = new HealthCommand<WorkoutSearchResultViewModel>((Action<WorkoutSearchResultViewModel>) (workout => this.OpenWorkout(workout))));

    public ICommand CancelCommand
    {
      get
      {
        this.cancelCommand = this.cancelCommand ?? new HealthCommand(new Action(this.CancelButtonPress));
        return (ICommand) this.cancelCommand;
      }
    }

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.navigation.GoBack())));

    public ICommand ConfirmCommand
    {
      get
      {
        this.confirmCommand = this.confirmCommand ?? new HealthCommand(new Action(this.ConfirmButtonPress));
        return (ICommand) this.confirmCommand;
      }
    }

    public ICommand SelectFilterTypeCommand => (ICommand) this.selectFilterTypeCommand ?? (ICommand) (this.selectFilterTypeCommand = new HealthCommand<FilterGrouping>((Action<FilterGrouping>) (filter => this.SelectFilterType(filter))));

    public ICommand ToggleFilterSelectCommand => (ICommand) this.toggleFilterSelect ?? (ICommand) (this.toggleFilterSelect = new HealthCommand<SearchFilter>((Action<SearchFilter>) (filterItem => this.ToggleFilterSelection(filterItem))));

    public ICommand ClearSelectedFiltersCommand => (ICommand) this.clearSelectedFilters ?? (ICommand) (this.clearSelectedFilters = new HealthCommand(new Action(this.ClearFilters)));

    public ICommand SearchCommand => (ICommand) this.searchCommand ?? (ICommand) (this.searchCommand = new HealthCommand(new Action(this.OpenSearch)));

    public WorkoutPlanSearchResultsViewModel(INetworkService networkService)
      : base(networkService)
    {
    }

    public WorkoutPlanSearchResultsViewModel(
      IWorkoutsProvider provider,
      ISmoothNavService navigation,
      INetworkService networkService,
      IMessageSender messageSender)
      : base(networkService)
    {
      if (provider == null)
        throw new ArgumentNullException(nameof (provider), "You must provide a valid exercise data provider to instantiate this class");
      if (navigation == null)
        throw new ArgumentNullException(nameof (navigation), "You must provide a navigation provider");
      this.provider = provider;
      this.navigation = navigation;
      this.messageSender = messageSender;
      this.selectedFilters = (IList<WorkoutSearchFilter>) new List<WorkoutSearchFilter>();
      this.availableFilters = (IList<Histogram>) new List<Histogram>();
    }

    private void OnBackKeyPressed(BackButtonPressedMessage message)
    {
      if (!this.FiltersOpen)
        return;
      WorkoutPlanSearchResultsViewModel.Logger.Info((object) "Back key pressed, closing filters.");
      if (this.ShowFilterSelect)
      {
        this.ShowFilterSelect = false;
        this.SetFiltersTitle();
      }
      else
      {
        this.FiltersOpen = false;
        this.ShowFilterSelect = false;
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      }
      message.CancelAction();
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
    }

    protected override async void OnBackNavigation()
    {
      if (this.lastWorkoutPlanOpenedIndex < 0 || this.lastWorkoutPlanOpenedIndex >= this.Workouts.Count)
        return;
      bool isLastSelectedFavorite = false;
      string lastSelectedPlanId = this.Workouts[this.lastWorkoutPlanOpenedIndex].Id;
      try
      {
        foreach (FavoriteWorkout favoriteWorkout in (IEnumerable<FavoriteWorkout>) await this.provider.GetFavoriteWorkoutsAsync(CancellationToken.None))
        {
          if (favoriteWorkout.WorkoutPlanId == lastSelectedPlanId)
            isLastSelectedFavorite = true;
        }
        this.Workouts[this.lastWorkoutPlanOpenedIndex].IsFavorite = isLastSelectedFavorite;
      }
      catch (Exception ex)
      {
        WorkoutPlanSearchResultsViewModel.Logger.Error(ex, "Could not refresh favorite state of last selected plan.");
      }
      lastSelectedPlanId = (string) null;
    }

    private void OpenWorkout(WorkoutSearchResultViewModel workoutPlan)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary["WorkoutPlanId"] = workoutPlan.Id;
      dictionary["Type"] = "WorkoutPlanDetail";
      this.lastWorkoutPlanOpenedIndex = this.Workouts.IndexOf(workoutPlan);
      this.navigation.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) dictionary);
      string howFound = this.query == null ? (this.workoutType != WorkoutPlanSearchResultsViewModel.WorkoutType.Favorite ? (this.workoutType != WorkoutPlanSearchResultsViewModel.WorkoutType.Custom ? "Browse" : "MyWorkouts") : "Favorites") : "Search";
      ApplicationTelemetry.LogWorkoutChosen(this.Workouts.Count, workoutPlan.Name, workoutPlan.PartnerName, howFound);
    }

    private async Task LoadWorkoutsAsync(string query, IList<WorkoutSearchFilter> filters)
    {
      this.LoadState = LoadState.Loading;
      List<WorkoutSearchResult> searchResults = new List<WorkoutSearchResult>();
      List<Histogram> histograms = new List<Histogram>();
      IList<FavoriteWorkout> favorites;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        favorites = await this.provider.GetFavoriteWorkoutsAsync(cancellationTokenSource.Token);
        if (this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.Custom)
        {
          WorkoutSearch workoutsAsync1 = await this.provider.GetWorkoutsAsync(cancellationTokenSource.Token, string.Empty, WorkoutPublisher.Me, filters);
          searchResults.AddRange((IEnumerable<WorkoutSearchResult>) workoutsAsync1.WorkoutResults.Results.OrderByDescending<WorkoutSearchResult, DateTimeOffset>((Func<WorkoutSearchResult, DateTimeOffset>) (p => p.PublishDate)));
          WorkoutSearch workoutsAsync2 = await this.provider.GetWorkoutsAsync(cancellationTokenSource.Token, string.Empty, WorkoutPublisher.Provider, (IList<WorkoutSearchFilter>) new List<WorkoutSearchFilter>());
          histograms.AddRange((IEnumerable<Histogram>) workoutsAsync2.Histograms);
        }
        else if (this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.None)
        {
          WorkoutSearch workoutsAsync = await this.provider.GetWorkoutsAsync(cancellationTokenSource.Token, query, WorkoutPublisher.Provider, filters);
          searchResults.AddRange((IEnumerable<WorkoutSearchResult>) workoutsAsync.WorkoutResults.Results);
          if (!string.IsNullOrEmpty(query) && filters.Count > 0)
            workoutsAsync = await this.provider.GetWorkoutsAsync(cancellationTokenSource.Token, string.Empty, WorkoutPublisher.Provider, (IList<WorkoutSearchFilter>) new List<WorkoutSearchFilter>());
          histograms.AddRange((IEnumerable<Histogram>) workoutsAsync.Histograms);
        }
      }
      if (searchResults.Count == 0 && this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.None)
        throw new NoDataException();
      this.ShowPublishDate = this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.Custom;
      if (this.availableFilters != null && this.availableFilters.Count == 0)
        this.availableFilters = (IList<Histogram>) histograms;
      this.Workouts = (IList<WorkoutSearchResultViewModel>) new ObservableCollection<WorkoutSearchResultViewModel>((IEnumerable<WorkoutSearchResultViewModel>) this.AddWorkoutSearchResult((IList<WorkoutSearchResult>) searchResults, favorites));
      this.LogFilterResults();
      this.SetResultsTitle();
      if (this.Workouts.Any<WorkoutSearchResultViewModel>())
      {
        this.LoadState = LoadState.Loaded;
      }
      else
      {
        string errorMessage;
        switch (this.workoutType)
        {
          case WorkoutPlanSearchResultsViewModel.WorkoutType.Custom:
            errorMessage = AppResources.WorkoutNoCustomWorkouts;
            break;
          case WorkoutPlanSearchResultsViewModel.WorkoutType.Favorite:
            errorMessage = AppResources.NoFavoritesMessage;
            break;
          default:
            errorMessage = AppResources.NoWorkoutDataFoundMessage;
            break;
        }
        throw new CustomErrorException(errorMessage);
      }
    }

    private List<WorkoutSearchResultViewModel> AddWorkoutSearchResult(
      IList<WorkoutSearchResult> searchResults,
      IList<FavoriteWorkout> favoriteResults)
    {
      List<WorkoutSearchResultViewModel> searchResultViewModelList = new List<WorkoutSearchResultViewModel>();
      if (this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.Favorite)
      {
        foreach (FavoriteWorkout favoriteResult in (IEnumerable<FavoriteWorkout>) favoriteResults)
          searchResultViewModelList.Add(new WorkoutSearchResultViewModel(favoriteResult));
      }
      else
      {
        foreach (WorkoutSearchResult searchResult in (IEnumerable<WorkoutSearchResult>) searchResults)
        {
          WorkoutSearchResult result = searchResult;
          if ((result.PartnerName == this.CategoryName || string.IsNullOrWhiteSpace(this.CategoryName)) && (result.DisplaySubType.ToFriendlyString() == this.TypeName || string.IsNullOrWhiteSpace(this.TypeName)))
            searchResultViewModelList.Add(new WorkoutSearchResultViewModel(result, favoriteResults.Any<FavoriteWorkout>((Func<FavoriteWorkout, bool>) (s => s.WorkoutPlanId == result.Id))));
        }
      }
      return searchResultViewModelList;
    }

    private bool IsFilterSelected(
      string filterName,
      string filterId,
      IList<WorkoutSearchFilter> selectedFilters)
    {
      bool flag = false;
      foreach (WorkoutSearchFilter selectedFilter in (IEnumerable<WorkoutSearchFilter>) selectedFilters)
      {
        if (selectedFilter.Id == filterId && selectedFilter.FilterName == filterName)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void OpenSearch() => this.navigation.Navigate(typeof (WorkoutPlanSearchViewModel), action: NavigationStackAction.RemovePrevious);

    private IList<FilterGrouping> GetFilters()
    {
      List<string> stringList = new List<string>();
      if (this.baseSelectedFilters != null)
      {
        foreach (WorkoutSearchFilter baseSelectedFilter in (IEnumerable<WorkoutSearchFilter>) this.baseSelectedFilters)
          stringList.Add(baseSelectedFilter.FilterName);
      }
      List<SearchFilter> source = new List<SearchFilter>();
      foreach (Histogram availableFilter in (IEnumerable<Histogram>) this.availableFilters)
      {
        if (availableFilter.Values.Count != 1 && !stringList.Contains(availableFilter.FilterName) && ((IEnumerable<string>) WorkoutConstants.BrowseWorkoutsValidFilters).Contains<string>(availableFilter.FilterName))
        {
          foreach (WorkoutSearchFilter workoutSearchFilter in (IEnumerable<WorkoutSearchFilter>) availableFilter.Values)
          {
            workoutSearchFilter.FilterName = availableFilter.FilterName;
            SearchFilter searchFilter = new SearchFilter()
            {
              FilterName = availableFilter.FilterName,
              FilterValueName = workoutSearchFilter.Name,
              Id = workoutSearchFilter.Id,
              DisplayName = availableFilter.DisplayName,
              WorkoutFilter = workoutSearchFilter,
              IsSelected = this.IsFilterSelected(availableFilter.FilterName, workoutSearchFilter.Id, this.selectedFilters)
            };
            source.Add(searchFilter);
          }
        }
      }
      return (IList<FilterGrouping>) source.GroupBy<SearchFilter, string>((Func<SearchFilter, string>) (item => item.DisplayName)).Select<IGrouping<string, SearchFilter>, FilterGrouping>((Func<IGrouping<string, SearchFilter>, FilterGrouping>) (group => new FilterGrouping((IEnumerable<SearchFilter>) group.ToArray<SearchFilter>())
      {
        FilterName = group.Key
      })).ToList<FilterGrouping>();
    }

    private void OpenFilters()
    {
      this.SetFiltersTitle();
      this.ShowAppBar();
      this.FiltersOpen = true;
      this.ShowFilterClear = this.IsFiltered;
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
      {
        "Fitness",
        "Guided Workouts",
        "Filters Summary"
      });
      this.Filters = (IList<FilterGrouping>) new ObservableCollection<FilterGrouping>((IEnumerable<FilterGrouping>) this.GetFilters());
    }

    private void CancelButtonPress()
    {
      if (this.ShowFilterSelect)
      {
        List<FilterGrouping> filterGroupingList = JsonConvert.DeserializeObject<List<FilterGrouping>>(this.savedfilterGrouping);
        for (int index1 = 0; index1 < filterGroupingList.Count; ++index1)
        {
          FilterGrouping filterGrouping = filterGroupingList[index1];
          for (int index2 = 0; index2 < filterGrouping.Count; ++index2)
            this.Filters[index1][index2].IsSelected = filterGrouping[index2].IsSelected;
        }
        this.ShowFilterSelect = false;
        this.SetFiltersTitle();
      }
      else
      {
        this.FiltersOpen = false;
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      }
    }

    private async void ConfirmButtonPress()
    {
      if (this.ShowFilterSelect)
      {
        this.ShowFilterSelect = false;
        this.SetFiltersTitle();
      }
      else
      {
        this.FiltersOpen = false;
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
        this.SetSelectedFilters(this.baseSelectedFilters);
        foreach (FilterGrouping filter in (IEnumerable<FilterGrouping>) this.Filters)
        {
          foreach (WorkoutSearchFilter workoutSearchFilter in (IEnumerable<WorkoutSearchFilter>) filter.Selected)
            this.selectedFilters.Add(workoutSearchFilter);
        }
        if (this.selectedFilters.Count == 0)
          this.SetSelectedFilters(this.baseSelectedFilters);
        if (this.baseSelectedFilters != null && this.baseSelectedFilters.Count > 0)
          this.IsFiltered = this.selectedFilters.Count > this.baseSelectedFilters.Count;
        else
          this.IsFiltered = this.selectedFilters.Count > 0;
        try
        {
          await this.LoadWorkoutsAsync(this.query, this.selectedFilters);
        }
        catch (Exception ex)
        {
          WorkoutPlanSearchResultsViewModel.Logger.Error(ex, "Search failed to return a result");
          this.ResultsTitle = string.Format("{0} ({1})", new object[2]
          {
            (object) AppResources.FindWorkoutsResultsTitle,
            (object) 0
          });
          this.SetErrorMessage();
          this.LoadState = LoadState.CustomError;
        }
      }
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirmLocked, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancelLocked, AppBarIcon.Cancel, this.CancelCommand)
    });

    private void ClearFilters()
    {
      foreach (FilterGrouping filter in (IEnumerable<FilterGrouping>) this.Filters)
        filter.SetAllSelection(false);
      this.ShowFilterClear = false;
    }

    private void SetResultsTitle() => this.ResultsTitle = string.Format("{0} ({1})", new object[2]
    {
      (object) AppResources.FindWorkoutsResultsTitle,
      (object) this.Workouts.Count
    });

    private void SetSearchTitle()
    {
      if (!string.IsNullOrEmpty(this.query))
        this.SearchTitle = string.Format("\"{0}\"", new object[1]
        {
          (object) this.query
        });
      else if (this.selectedFilters.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        if (!string.IsNullOrWhiteSpace(this.CategoryName))
        {
          stringBuilder.Append(this.CategoryName);
        }
        else
        {
          num = 1;
          if (this.selectedFilters.Count > 0)
            stringBuilder.Append(this.selectedFilters[0].Name);
        }
        for (int index = num; index < this.selectedFilters.Count; ++index)
        {
          WorkoutSearchFilter selectedFilter = this.selectedFilters[index];
          stringBuilder.AppendFormat(" / {0}", new object[1]
          {
            (object) selectedFilter.Name
          });
        }
        this.SearchTitle = stringBuilder.ToString();
      }
      else if (!string.IsNullOrWhiteSpace(this.CategoryName))
        this.SearchTitle = this.CategoryName;
      else if (!string.IsNullOrWhiteSpace(this.TypeName))
        this.SearchTitle = this.TypeName;
      else if (this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.Favorite)
        this.SearchTitle = AppResources.WorkoutBrowseFavorites.ToLower();
      else if (this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.Custom)
        this.SearchTitle = AppResources.WorkoutBrowseCustom;
      else
        this.SearchTitle = AppResources.NavigationBrowseAll.ToLower();
    }

    private void SetFiltersTitle()
    {
      if (this.ShowFilterSelect)
      {
        this.FilterTitle = AppResources.TitleFilterBy;
        this.FilterSubTitle = this.SelectedFilterGroup.FilterName;
        this.ShowFilterClear = false;
      }
      else
      {
        this.FilterTitle = AppResources.EventTypeNameWorkoutLocked;
        this.FilterSubTitle = AppResources.TitleFiltersLocked;
        this.ShowFilterClear = this.IsAnyFilterSelected();
      }
    }

    private bool IsAnyFilterSelected()
    {
      if (this.Filters != null)
      {
        foreach (FilterGrouping filter in (IEnumerable<FilterGrouping>) this.Filters)
        {
          if (filter.IsAnyFilterSelected)
            return true;
        }
      }
      return false;
    }

    private void SelectFilterType(FilterGrouping filter)
    {
      this.savedfilterGrouping = JsonConvert.SerializeObject((object) this.filterGrouping);
      this.SelectedFilterGroup = filter;
      this.ShowFilterSelect = true;
      this.SetFiltersTitle();
    }

    private void ToggleFilterSelection(SearchFilter filterItem) => filterItem.IsSelected = !filterItem.IsSelected;

    private void SetSelectedFilters(IList<WorkoutSearchFilter> filters)
    {
      this.selectedFilters = (IList<WorkoutSearchFilter>) new List<WorkoutSearchFilter>();
      if (filters == null)
        return;
      foreach (WorkoutSearchFilter filter in (IEnumerable<WorkoutSearchFilter>) filters)
        this.selectedFilters.Add(filter);
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters != null && parameters.ContainsKey("query"))
        this.query = parameters["query"];
      string str1 = "Results";
      string str2;
      if (parameters != null && parameters.TryGetValue("WorkoutType", out str2))
      {
        if (!(str2 == "Custom"))
        {
          if (str2 == "Favorite")
          {
            this.workoutType = WorkoutPlanSearchResultsViewModel.WorkoutType.Favorite;
            str1 = "Favorites";
          }
          else
          {
            this.workoutType = WorkoutPlanSearchResultsViewModel.WorkoutType.None;
            str1 = "Browse All";
          }
        }
        else
        {
          this.workoutType = WorkoutPlanSearchResultsViewModel.WorkoutType.Custom;
          str1 = "MyWorkouts";
        }
      }
      if (parameters != null && parameters.ContainsKey("SelectedFilters"))
      {
        this.baseSelectedFilters = (IList<WorkoutSearchFilter>) JsonConvert.DeserializeObject<List<WorkoutSearchFilter>>(parameters["SelectedFilters"]);
        this.SetSelectedFilters(this.baseSelectedFilters);
        int index1 = -1;
        int index2 = -1;
        for (int index3 = 0; index3 < this.selectedFilters.Count; ++index3)
        {
          WorkoutSearchFilter selectedFilter = this.selectedFilters[index3];
          if (selectedFilter.FilterName.Equals("brandcategory", StringComparison.CurrentCultureIgnoreCase))
          {
            this.CategoryName = selectedFilter.Name;
            this.CategoryImageUrl = selectedFilter.Image;
            index1 = index3;
            break;
          }
          if (selectedFilter.FilterName.Equals("type", StringComparison.CurrentCultureIgnoreCase))
          {
            this.TypeName = selectedFilter.Name;
            index2 = index3;
            break;
          }
        }
        if (index1 >= 0)
        {
          this.selectedFilters.RemoveAt(index1);
          this.baseSelectedFilters.RemoveAt(index1);
        }
        if (index2 >= 0)
        {
          this.selectedFilters.RemoveAt(index2);
          this.baseSelectedFilters.RemoveAt(index2);
        }
      }
      this.SetSearchTitle();
      this.ShowSearch = this.workoutType == WorkoutPlanSearchResultsViewModel.WorkoutType.None && !string.IsNullOrEmpty(this.query);
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
      {
        "Fitness",
        "Guided Workouts",
        "Browse",
        str1
      });
      await this.LoadWorkoutsAsync(this.query, this.selectedFilters);
      this.IsFilteringEnabled = this.Workouts.Count > 1 && this.GetFilters().Count > 0;
    }

    private void LogFilterResults()
    {
      Dictionary<string, string> dictionary = this.selectedFilters.GroupBy<WorkoutSearchFilter, string>((Func<WorkoutSearchFilter, string>) (filter => filter.FilterName)).ToDictionary<IGrouping<string, WorkoutSearchFilter>, string, string>((Func<IGrouping<string, WorkoutSearchFilter>, string>) (filterGroup => filterGroup.Key), (Func<IGrouping<string, WorkoutSearchFilter>, string>) (filterGroup => string.Join(",", (IEnumerable<string>) filterGroup.Select<WorkoutSearchFilter, string>((Func<WorkoutSearchFilter, string>) (filter => filter.Name)).OrderBy<string, string>((Func<string, string>) (filterName => filterName)))));
      if (!string.IsNullOrWhiteSpace(this.CategoryName))
        dictionary.Add("brandcategory", this.CategoryName);
      if (!string.IsNullOrWhiteSpace(this.TypeName))
        dictionary.Add("type", this.CategoryName);
      ApplicationTelemetry.LogWorkoutPlanFilterResults(this.Workouts.Count, (IReadOnlyDictionary<string, string>) dictionary);
    }

    private enum WorkoutType
    {
      None,
      Custom,
      Favorite,
    }
  }
}
