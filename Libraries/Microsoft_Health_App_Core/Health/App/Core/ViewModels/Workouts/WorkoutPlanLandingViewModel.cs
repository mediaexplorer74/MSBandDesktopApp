// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutPlanLandingViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  [PageTaxonomy(new string[] {"Fitness", "Guided Workouts", "Find a Workout"})]
  [PageMetadata(PageContainerType.HomeShell)]
  public sealed class WorkoutPlanLandingViewModel : PageViewModelBase, IHomeShellViewModel
  {
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService navigation;
    private readonly IWorkoutsProvider provider;
    private readonly AsyncHealthCommand openMyWorkoutsCommand;
    private IList<BrowseCategory> categories;
    private IList<WorkoutCarouselItemViewModel> featuredWorkouts;
    private HealthCommand openFavoritesCommand;
    private HealthCommand<string> browseCommand;
    private HealthCommand<CarouselSelectedEventArgs<WorkoutCarouselItemViewModel>> openFeaturedCommand;
    private HealthCommand searchCommand;
    private bool isFiltered;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Workouts\\WorkoutPlanLandingViewModel.cs");

    public bool IsFiltered
    {
      get => this.isFiltered;
      set => this.SetProperty<bool>(ref this.isFiltered, value, nameof (IsFiltered));
    }

    public IList<BrowseCategory> Categories
    {
      get => this.categories;
      set => this.SetProperty<IList<BrowseCategory>>(ref this.categories, value, nameof (Categories));
    }

    public IList<WorkoutCarouselItemViewModel> FeaturedWorkouts
    {
      get => this.featuredWorkouts;
      set => this.SetProperty<IList<WorkoutCarouselItemViewModel>>(ref this.featuredWorkouts, value, nameof (FeaturedWorkouts));
    }

    public ICommand OpenMyWorkoutsCommand => (ICommand) this.openMyWorkoutsCommand;

    public ICommand OpenFavoritesCommand => (ICommand) this.openFavoritesCommand ?? (ICommand) (this.openFavoritesCommand = new HealthCommand(new Action(this.OpenFavorites)));

    public ICommand BrowseCommand => (ICommand) this.browseCommand ?? (ICommand) (this.browseCommand = new HealthCommand<string>(new Action<string>(this.Browse)));

    public ICommand OpenFeaturedCommand => (ICommand) this.openFeaturedCommand ?? (ICommand) (this.openFeaturedCommand = new HealthCommand<CarouselSelectedEventArgs<WorkoutCarouselItemViewModel>>(new Action<CarouselSelectedEventArgs<WorkoutCarouselItemViewModel>>(this.OpenFeatured)));

    public ICommand SearchCommand => (ICommand) this.searchCommand ?? (ICommand) (this.searchCommand = new HealthCommand(new Action(this.OpenSearch)));

    public WorkoutPlanLandingViewModel(INetworkService networkService, IMessageSender messageSender)
      : base(networkService)
    {
      this.messageSender = messageSender;
    }

    public WorkoutPlanLandingViewModel(
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
      this.openMyWorkoutsCommand = AsyncHealthCommand.Create(new Func<Task>(this.OnOpenMyWorkoutsAsync));
    }

    private Task OnOpenMyWorkoutsAsync()
    {
      this.navigation.Navigate(typeof (WorkoutPlanSearchResultsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "WorkoutType",
          "Custom"
        }
      });
      return (Task) Task.FromResult<bool>(true);
    }

    private void OpenFavorites() => this.navigation.Navigate(typeof (WorkoutPlanSearchResultsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "WorkoutType",
        "Favorite"
      }
    });

    private void Browse(string type)
    {
      string str = string.Empty;
      if (!(type == "0"))
      {
        if (type == "1")
          str = "Brand|;";
      }
      else
        str = "Type|;";
      if (!string.IsNullOrWhiteSpace(str))
        this.navigation.Navigate(typeof (WorkoutPlanSelectViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "FilterSelections",
            str
          },
          {
            "FilterSelectionIndex",
            "0"
          }
        });
      else
        this.navigation.Navigate(typeof (WorkoutPlanSearchResultsViewModel));
    }

    private void OpenFeatured(
      CarouselSelectedEventArgs<WorkoutCarouselItemViewModel> args)
    {
      if (args == null || args.SelectedItem == null || string.IsNullOrEmpty(args.SelectedItem.Id))
        return;
      this.navigation.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        ["WorkoutPlanId"] = args.SelectedItem.Id,
        ["Type"] = "WorkoutPlanDetail"
      });
      ApplicationTelemetry.LogWorkoutChosen(1, args.SelectedItem.Title, (string) null, "Featured");
    }

    private void OpenSearch() => this.navigation.Navigate(typeof (WorkoutPlanSearchViewModel));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.FeaturedWorkouts = (IList<WorkoutCarouselItemViewModel>) new ObservableCollection<WorkoutCarouselItemViewModel>();
      this.FeaturedWorkouts.Add(new WorkoutCarouselItemViewModel()
      {
        Title = AppResources.FeaturedWorkoutsDefaultTitle,
        SubTitle = AppResources.FeaturedWorkoutsDefaultPhrase,
        Image = new EmbeddedOrRemoteImageSource(EmbeddedAsset.FeaturedWorkoutDefault)
      });
      try
      {
        IList<FeaturedWorkout> featuredWorkoutsAsync = await this.provider.GetFeaturedWorkoutsAsync();
        if (featuredWorkoutsAsync == null)
          return;
        foreach (FeaturedWorkout featuredWorkout in (IEnumerable<FeaturedWorkout>) featuredWorkoutsAsync)
          this.FeaturedWorkouts.Add(new WorkoutCarouselItemViewModel()
          {
            Id = featuredWorkout.WorkoutPlanId,
            Image = new EmbeddedOrRemoteImageSource(featuredWorkout.WorkoutPlanBrowseDetails.Image),
            Title = AppResources.BrowseWorkoutsFeaturedTitle,
            SubTitle = featuredWorkout.WorkoutPlanBrowseDetails.Name,
            IsSelectable = true
          });
      }
      catch (Exception ex)
      {
        WorkoutPlanLandingViewModel.Logger.Error(ex, "<FAILED> Get Featured Workout.");
      }
    }

    public NavigationHeaderBackground NavigationHeaderBackground => NavigationHeaderBackground.Default;
  }
}
