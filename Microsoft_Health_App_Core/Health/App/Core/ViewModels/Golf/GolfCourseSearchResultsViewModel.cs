// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseSearchResultsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Golf.Courses;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Browse results"})]
  public sealed class GolfCourseSearchResultsViewModel : GolfCourseSearchResultsViewModelBase
  {
    private const int CountPerPage = 10;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfCourseSearchResultsViewModel.cs");
    private readonly IGolfCourseProvider golfCourseProvider;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly ISmoothNavService smoothNavService;
    private readonly ILauncherService launcherService;
    private ICommand backCommand;
    private ICommand loadMoreCommand;
    private ICommand searchCommand;
    private ICommand suggestCourseCommand;
    private bool isLoading;
    private bool hasCourses;
    private bool endReached;
    private int nextPageNumber;
    private string query;
    private string searchTitle;

    public new string SearchTitle
    {
      get => this.searchTitle;
      set => this.SetProperty<string>(ref this.searchTitle, value, nameof (SearchTitle));
    }

    public bool IsLoading
    {
      get => this.isLoading;
      private set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public bool HasCourses
    {
      get => this.hasCourses;
      private set => this.SetProperty<bool>(ref this.hasCourses, value, nameof (HasCourses));
    }

    public ICommand BackCommand => this.backCommand ?? (this.backCommand = (ICommand) new HealthCommand(new Action(this.smoothNavService.GoBack)));

    public ICommand LoadMoreCommand => this.loadMoreCommand ?? (this.loadMoreCommand = (ICommand) new HealthCommand(new Action(this.Paginate)));

    public ICommand SearchCommand => this.searchCommand ?? (this.searchCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (GolfCourseSearchViewModel), action: NavigationStackAction.RemovePrevious))));

    public ICommand SuggestCourseCommand => this.suggestCourseCommand ?? (this.suggestCourseCommand = (ICommand) new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkId=625095")))));

    public override string TelemetryParameters => "Search";

    public override bool ShowSearch => true;

    public GolfCourseSearchResultsViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      ILauncherService launcherService,
      IFormattingService formattingService,
      IGolfCourseProvider golfCourseProvider,
      IErrorHandlingService errorHandlingService,
      IGolfCourseFilterService golfCourseFilterService)
      : base(networkService, smoothNavService, formattingService, golfCourseFilterService)
    {
      this.smoothNavService = smoothNavService;
      this.golfCourseProvider = golfCourseProvider;
      this.errorHandlingService = errorHandlingService;
      this.launcherService = launcherService;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await base.LoadDataAsync(parameters);
      this.query = this.GetStringParameter("query");
      this.endReached = false;
      this.nextPageNumber = 1;
      this.SearchTitle = string.Format(AppResources.SearchHeaderFormat, new object[1]
      {
        (object) this.query
      });
      this.GolfCourses = (IList<GolfCourseSummaryViewModel>) await this.GetGolfCoursesAsync();
      this.ResultsTitle = string.Format(AppResources.GolfCourseSearchResultsLabel, new object[1]
      {
        (object) this.GolfCourses.Count
      });
      this.IsFilteringEnabled = true;
      if (this.GolfCourses.Count != 0)
        return;
      this.HasCourses = false;
    }

    private async void Paginate()
    {
      if (this.IsLoading || this.endReached)
        return;
      this.IsLoading = true;
      try
      {
        foreach (GolfCourseSummaryViewModel summaryViewModel in (Collection<GolfCourseSummaryViewModel>) await this.GetGolfCoursesAsync())
          this.GolfCourses.Add(summaryViewModel);
        this.ResultsTitle = string.Format(AppResources.GolfCourseSearchResultsLabel, new object[1]
        {
          (object) this.GolfCourses.Count
        });
      }
      catch (Exception ex)
      {
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.IsLoading = false;
    }

    protected override async Task<ObservableCollection<GolfCourseSummaryViewModel>> GetGolfCoursesAsync()
    {
      this.HasCourses = true;
      ObservableCollection<GolfCourseSummaryViewModel> golfCourseViewModel;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        IReadOnlyList<GolfCourseSummary> coursesByNameAsync = await this.golfCourseProvider.GetCoursesByNameAsync(this.query, this.nextPageNumber, 10, cancellationTokenSource.Token, this.GetGolfCourseTypeFilter(), this.GetGolfCourseHoleFilter());
        if (coursesByNameAsync.Count == 0)
          this.HasCourses = false;
        if (coursesByNameAsync.Count < 10)
          this.endReached = true;
        ++this.nextPageNumber;
        golfCourseViewModel = this.CreateGolfCourseViewModel(coursesByNameAsync);
      }
      return golfCourseViewModel;
    }
  }
}
