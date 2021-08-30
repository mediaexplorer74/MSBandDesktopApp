// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutPlanSelectViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  public class WorkoutPlanSelectViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Workouts\\WorkoutPlanSelectViewModel.cs");
    private static readonly EmbeddedAsset[] AvailableBackgrounds = new EmbeddedAsset[5]
    {
      EmbeddedAsset.GuidedWorkoutBrandBackground1,
      EmbeddedAsset.GuidedWorkoutBrandBackground2,
      EmbeddedAsset.GuidedWorkoutBrandBackground3,
      EmbeddedAsset.GuidedWorkoutBrandBackground4,
      EmbeddedAsset.GuidedWorkoutBrandBackground5
    };
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService navigation;
    private readonly INetworkService networkService;
    private readonly IWorkoutsProvider provider;
    private int filterSelectionIndex;
    private IList<Tuple<BrowseWorkoutsDisplayType, string>> filterSelections;
    private string filterSelectionsArg;
    private IList<WorkoutSearchFilterViewModel> filters;
    private HealthCommand<WorkoutSearchFilterViewModel> selectFilterCommand;
    private HealthCommand backCommand;
    private BrowseWorkoutsDisplayType selectType;
    private IList<WorkoutSearchFilter> selectedFilters;
    private string title;

    public string Title
    {
      get => this.title;
      set => this.SetProperty<string>(ref this.title, value, nameof (Title));
    }

    public BrowseWorkoutsDisplayType SelectType
    {
      get => this.selectType;
      set => this.SetProperty<BrowseWorkoutsDisplayType>(ref this.selectType, value, nameof (SelectType));
    }

    public IList<WorkoutSearchFilterViewModel> Filters
    {
      get => this.filters;
      set => this.SetProperty<IList<WorkoutSearchFilterViewModel>>(ref this.filters, value, nameof (Filters));
    }

    public ICommand SelectFilterCommand => (ICommand) this.selectFilterCommand ?? (ICommand) (this.selectFilterCommand = new HealthCommand<WorkoutSearchFilterViewModel>((Action<WorkoutSearchFilterViewModel>) (model => this.SelectFilter(model.Filter))));

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.navigation.GoBack())));

    public WorkoutPlanSelectViewModel(
      IWorkoutsProvider provider,
      ISmoothNavService navigation,
      INetworkService networkService,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.provider = provider;
      this.navigation = navigation;
      this.networkService = networkService;
      this.messageSender = messageSender;
    }

    private void SelectFilter(WorkoutSearchFilter filter)
    {
      if (this.selectedFilters == null)
        this.selectedFilters = (IList<WorkoutSearchFilter>) new List<WorkoutSearchFilter>();
      for (int index = 0; index < this.selectedFilters.Count; ++index)
      {
        if (this.selectedFilters[index].FilterName == filter.FilterName)
        {
          this.selectedFilters.RemoveAt(index);
          break;
        }
      }
      this.selectedFilters.Add(filter);
      int num = this.filterSelectionIndex + 1;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string str = JsonConvert.SerializeObject((object) this.selectedFilters);
      dictionary.Add("SelectedFilters", str);
      if (num < this.filterSelections.Count)
      {
        dictionary.Add("FilterSelections", this.filterSelectionsArg);
        dictionary.Add("FilterSelectionIndex", num.ToString());
        this.navigation.Navigate(typeof (WorkoutPlanSelectViewModel), (IDictionary<string, string>) dictionary);
      }
      else
        this.navigation.Navigate(typeof (WorkoutPlanSearchResultsViewModel), (IDictionary<string, string>) dictionary);
    }

    private async Task<IList<WorkoutSearchFilterViewModel>> LoadTypeFiltersAsync()
    {
      List<WorkoutSearchFilterViewModel> filters = new List<WorkoutSearchFilterViewModel>();
      IList<DisplaySubType> workoutTypesAsync = await this.provider.GetWorkoutTypesAsync();
      foreach (DisplaySubType type in (IEnumerable<DisplaySubType>) workoutTypesAsync)
      {
        if (type != DisplaySubType.Unknown)
          filters.Add(new WorkoutSearchFilterViewModel(new WorkoutSearchFilter()
          {
            Id = workoutTypesAsync.IndexOf(type).ToString((IFormatProvider) CultureInfo.InvariantCulture),
            Name = type.ToFriendlyString(),
            FilterName = "type"
          }));
      }
      return (IList<WorkoutSearchFilterViewModel>) filters;
    }

    private async Task<IList<WorkoutSearchFilterViewModel>> LoadBrandCategoryFiltersAsync()
    {
      IList<WorkoutSearchFilterViewModel> filters = (IList<WorkoutSearchFilterViewModel>) new List<WorkoutSearchFilterViewModel>();
      IList<BrowseCategory> workoutBrandsAsync = await this.provider.GetWorkoutBrandsAsync();
      int index = 0;
      foreach (BrowseCategory browseCategory in (IEnumerable<BrowseCategory>) workoutBrandsAsync)
      {
        filters.Add(new WorkoutSearchFilterViewModel(new WorkoutSearchFilter()
        {
          Id = workoutBrandsAsync.IndexOf(browseCategory).ToString((IFormatProvider) CultureInfo.InvariantCulture),
          Name = browseCategory.Name,
          FilterName = "brandcategory",
          Image = browseCategory.Image
        }, new EmbeddedAsset?(WorkoutPlanSelectViewModel.AvailableBackgrounds[index])));
        ++index;
        if (index >= WorkoutPlanSelectViewModel.AvailableBackgrounds.Length)
          index = 0;
      }
      return filters;
    }

    private async Task<IList<WorkoutSearchFilterViewModel>> LoadFiltersByNameAsync(
      string filterName)
    {
      IList<WorkoutSearchFilter> filters = (IList<WorkoutSearchFilter>) new List<WorkoutSearchFilter>();
      foreach (Histogram histogram in (IEnumerable<Histogram>) (await this.provider.GetWorkoutsAsync(CancellationToken.None, string.Empty, WorkoutPublisher.Provider, this.selectedFilters)).Histograms)
      {
        if (string.Compare(histogram.FilterName, filterName, StringComparison.CurrentCultureIgnoreCase) == 0)
        {
          foreach (WorkoutSearchFilter workoutSearchFilter in (IEnumerable<WorkoutSearchFilter>) histogram.Values)
          {
            if (workoutSearchFilter.Count != 0)
              workoutSearchFilter.FilterName = histogram.FilterName;
          }
          filters = histogram.Values;
          break;
        }
      }
      return (IList<WorkoutSearchFilterViewModel>) filters.Select<WorkoutSearchFilter, WorkoutSearchFilterViewModel>((Func<WorkoutSearchFilter, WorkoutSearchFilterViewModel>) (x => new WorkoutSearchFilterViewModel(x))).ToList<WorkoutSearchFilterViewModel>();
    }

    private async Task LoadSelectionsAsync(BrowseWorkoutsDisplayType type, string filterName)
    {
      this.SelectType = type;
      IList<WorkoutSearchFilterViewModel> searchFilterViewModelList = (IList<WorkoutSearchFilterViewModel>) new List<WorkoutSearchFilterViewModel>();
      switch (type)
      {
        case BrowseWorkoutsDisplayType.Type:
          searchFilterViewModelList = await this.LoadTypeFiltersAsync();
          ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
          {
            "Fitness",
            "Guided Workouts",
            "Browse",
            "Fitness Goals"
          });
          break;
        case BrowseWorkoutsDisplayType.Brand:
          searchFilterViewModelList = await this.LoadBrandCategoryFiltersAsync();
          ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
          {
            "Fitness",
            "Guided Workouts",
            "Browse",
            "Fitness Pros"
          });
          break;
        case BrowseWorkoutsDisplayType.Filter:
          searchFilterViewModelList = await this.LoadFiltersByNameAsync(filterName);
          break;
      }
      this.Filters = searchFilterViewModelList.Count != 0 ? (IList<WorkoutSearchFilterViewModel>) new ObservableCollection<WorkoutSearchFilterViewModel>((IEnumerable<WorkoutSearchFilterViewModel>) searchFilterViewModelList) : throw new NoDataException();
    }

    private IList<Tuple<BrowseWorkoutsDisplayType, string>> ParseSelectionValues(
      string filterSelections)
    {
      List<Tuple<BrowseWorkoutsDisplayType, string>> tupleList = new List<Tuple<BrowseWorkoutsDisplayType, string>>();
      char[] separator = new char[1]{ ';' };
      foreach (string str in filterSelections.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        char[] chArray = new char[1]{ '|' };
        string[] strArray = str.Split(chArray);
        BrowseWorkoutsDisplayType result;
        if (strArray.Length == 2 && Enum.TryParse<BrowseWorkoutsDisplayType>(strArray[0], out result))
        {
          Tuple<BrowseWorkoutsDisplayType, string> tuple = new Tuple<BrowseWorkoutsDisplayType, string>(result, strArray[1]);
          tupleList.Add(tuple);
        }
      }
      return (IList<Tuple<BrowseWorkoutsDisplayType, string>>) tupleList;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters.ContainsKey("FilterSelections") && parameters.ContainsKey("FilterSelectionIndex"))
      {
        this.filterSelectionsArg = parameters["FilterSelections"];
        this.filterSelections = this.ParseSelectionValues(this.filterSelectionsArg);
        if (!int.TryParse(parameters["FilterSelectionIndex"], out this.filterSelectionIndex))
        {
          WorkoutPlanSelectViewModel.Logger.Warn((object) "Missing or invalid required parameter 'FilterSelectionIndex'");
          throw new NoDataException();
        }
        if (this.filterSelections == null || this.filterSelectionIndex >= this.filterSelections.Count)
        {
          WorkoutPlanSelectViewModel.Logger.Warn("Invalid index of {0} 'FilterSelectionIndex' in a list of selections count {0}", (object) this.filterSelectionIndex, (object) this.filterSelections.Count);
          throw new NoDataException();
        }
        Tuple<BrowseWorkoutsDisplayType, string> filterSelection = this.filterSelections[this.filterSelectionIndex];
        this.SelectType = filterSelection.Item1;
        if (this.SelectType == BrowseWorkoutsDisplayType.Filter && string.IsNullOrWhiteSpace(filterSelection.Item2))
        {
          WorkoutPlanSelectViewModel.Logger.Warn((object) "Missing value 'FilterName' required for loading selections of type Filter");
          throw new NoDataException();
        }
        this.selectedFilters = !parameters.ContainsKey("SelectedFilters") ? (IList<WorkoutSearchFilter>) new List<WorkoutSearchFilter>() : (IList<WorkoutSearchFilter>) JsonConvert.DeserializeObject<List<WorkoutSearchFilter>>(parameters["SelectedFilters"]);
        string str = filterSelection.Item2;
        if (string.IsNullOrWhiteSpace(str))
          str = filterSelection.Item1.ToString();
        this.Title = AppResources.ResourceManager.GetString(string.Format("BrowseWorkoutsFilterTitle{0}", new object[1]
        {
          (object) str
        }));
        this.networkService.EnsureInternetAvailable();
        await this.LoadSelectionsAsync(filterSelection.Item1, filterSelection.Item2);
      }
      else
      {
        WorkoutPlanSelectViewModel.Logger.Warn((object) "Missing parameter required parameter 'SelectType'");
        throw new NoDataException();
      }
    }

    public override Task ChangeAsync(IDictionary<string, string> parameters = null) => this.LoadAsync(parameters);
  }
}
