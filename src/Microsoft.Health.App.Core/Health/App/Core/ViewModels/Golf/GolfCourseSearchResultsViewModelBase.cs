// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseSearchResultsViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public abstract class GolfCourseSearchResultsViewModelBase : SearchResultsViewModelBase
  {
    public const string RegionIdParameter = "RegionId";
    public const string RegionNameParameter = "RegionName";
    public const string RegionCourseCountParameter = "RegionCourseCount";
    internal const string FilterInitializationRequiredKey = "RequiresFilterInitialization";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfCourseSearchResultsViewModelBase.cs");
    private readonly ISmoothNavService smoothNavService;
    private readonly IFormattingService formattingService;
    private readonly IGolfCourseFilterService golfCourseFilterService;
    private IList<GolfCourseSummaryViewModel> golfCourses;
    private ICommand showGolfCourseDetailsCommand;
    private ICommand openFiltersCommand;
    private bool refreshPending;

    public IList<GolfCourseSummaryViewModel> GolfCourses
    {
      get => this.golfCourses ?? (this.golfCourses = (IList<GolfCourseSummaryViewModel>) new ObservableCollection<GolfCourseSummaryViewModel>());
      protected set => this.SetProperty<IList<GolfCourseSummaryViewModel>>(ref this.golfCourses, value, nameof (GolfCourses));
    }

    public abstract string TelemetryParameters { get; }

    public ICommand ShowGolfCourseDetailsCommand => this.showGolfCourseDetailsCommand ?? (this.showGolfCourseDetailsCommand = (ICommand) new HealthCommand<GolfCourseItemViewModel>((Action<GolfCourseItemViewModel>) (p =>
    {
      if (!(p is GolfCourseSummaryViewModel summaryViewModel2))
        return;
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "CourseId",
          summaryViewModel2.GetRawSummary().CourseId
        }
      };
      ApplicationTelemetry.LogGolfBrowseCourse(this.TelemetryParameters);
      this.smoothNavService.Navigate(typeof (GolfCourseDetailViewModel), (IDictionary<string, string>) dictionary);
    })));

    public override ICommand OpenFiltersCommand => this.openFiltersCommand ?? (this.openFiltersCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.refreshPending = true;
      this.smoothNavService.Navigate(typeof (GolfCourseFilterCategoryViewModel));
    })));

    protected GolfCourseSearchResultsViewModelBase(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IFormattingService formattingService,
      IGolfCourseFilterService golfCourseFilterService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.formattingService = formattingService;
      this.golfCourseFilterService = golfCourseFilterService;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.refreshPending = false;
      this.ResultsTitle = string.Empty;
      bool result;
      if (!bool.TryParse(this.GetStringParameter("RequiresFilterInitialization"), out result))
        throw new ArgumentException("RequiresFilterInitialization must be a bool.");
      if (result)
        this.golfCourseFilterService.Initialize();
      this.IsFiltered = this.golfCourseFilterService.EnabledFilters != null && this.golfCourseFilterService.EnabledFilters.Any<GolfCourseFilter>((Func<GolfCourseFilter, bool>) (p => p.Items.Any<GolfCourseFilterItem>((Func<GolfCourseFilterItem, bool>) (s => s.IsSelected))));
      return (Task) Task.FromResult<bool>(true);
    }

    protected override void OnBackNavigation()
    {
      base.OnBackNavigation();
      if (!this.refreshPending || !this.IsFilteringEnabled)
        return;
      GolfCourseSearchResultsViewModelBase.Logger.Info((object) "Refreshing golf course results with selected filters.");
      this.Parameters["RequiresFilterInitialization"] = bool.FalseString;
      this.Refresh();
    }

    protected virtual Task<ObservableCollection<GolfCourseSummaryViewModel>> GetGolfCoursesAsync() => Task.FromResult<ObservableCollection<GolfCourseSummaryViewModel>>((ObservableCollection<GolfCourseSummaryViewModel>) null);

    protected Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType? GetGolfCourseTypeFilter()
    {
      if (this.IsFilteringEnabled && this.IsFiltered)
      {
        GolfCourseSearchResultsViewModelBase.Logger.Info((object) "Getting selected golf course type filters.");
        IList<GolfCourseFilterItem> selectedFilters = this.GetSelectedFilters(GolfCourseFilterCategory.Type);
        if (selectedFilters != null && selectedFilters.Count == 1)
        {
          switch (selectedFilters.First<GolfCourseFilterItem>().Subcategory)
          {
            case GolfCourseFilterSubcategory.Public:
              return new Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType?(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType.Public);
            case GolfCourseFilterSubcategory.Private:
              return new Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType?(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType.Private);
          }
        }
      }
      return new Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType?();
    }

    protected int? GetGolfCourseHoleFilter()
    {
      if (this.IsFilteringEnabled && this.IsFiltered)
      {
        GolfCourseSearchResultsViewModelBase.Logger.Info((object) "Getting selected golf course hole filters.");
        IList<GolfCourseFilterItem> selectedFilters = this.GetSelectedFilters(GolfCourseFilterCategory.Hole);
        if (selectedFilters != null && selectedFilters.Count == 1)
        {
          switch (selectedFilters.First<GolfCourseFilterItem>().Subcategory)
          {
            case GolfCourseFilterSubcategory.Nine:
              return new int?(9);
            case GolfCourseFilterSubcategory.Eighteen:
              return new int?(18);
          }
        }
      }
      return new int?();
    }

    private IList<GolfCourseFilterItem> GetSelectedFilters(
      GolfCourseFilterCategory category)
    {
      return (IList<GolfCourseFilterItem>) this.golfCourseFilterService.EnabledFilters.First<GolfCourseFilter>((Func<GolfCourseFilter, bool>) (p => p.Category == category)).Items.Where<GolfCourseFilterItem>((Func<GolfCourseFilterItem, bool>) (s => s.IsSelected)).ToList<GolfCourseFilterItem>();
    }

    protected ObservableCollection<GolfCourseSummaryViewModel> CreateGolfCourseViewModel(
      IReadOnlyList<Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseSummary> result)
    {
      return new ObservableCollection<GolfCourseSummaryViewModel>(result.Select<Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseSummary, GolfCourseSummaryViewModel>((Func<Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseSummary, GolfCourseSummaryViewModel>) (p => new GolfCourseSummaryViewModel(p.Name, p.Distance.HasValue ? (string) this.formattingService.FormatDistanceDynamic(new Length?(p.Distance.Value), true, true) : string.Empty, p.CourseType, p.NumberOfHoles, p.City, p))));
    }
  }
}
