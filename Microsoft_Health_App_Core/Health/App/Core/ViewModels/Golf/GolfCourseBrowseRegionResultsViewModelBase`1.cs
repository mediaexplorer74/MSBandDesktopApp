// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseBrowseRegionResultsViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Golf.Courses;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Search results"})]
  public abstract class GolfCourseBrowseRegionResultsViewModelBase<T> : 
    GolfCourseSearchResultsViewModelBase
  {
    private const int PageNumber = 1;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfCourseBrowseRegionResultsViewModelBase.cs");
    private readonly IGolfCourseProvider golfCourseProvider;
    private readonly ISmoothNavService smoothNavService;
    private readonly IFormattingService formattingService;
    private ICommand backCommand;
    private bool isLoading;
    private string regionName;
    private int regionId;
    private int resultsPerPage;
    private IList<T> golfCoursesZoom;

    public string RegionName
    {
      get => this.regionName;
      set => this.SetProperty<string>(ref this.regionName, value, nameof (RegionName));
    }

    public int RegionId
    {
      get => this.regionId;
      set => this.SetProperty<int>(ref this.regionId, value, nameof (RegionId));
    }

    public IList<T> GolfCoursesZoom
    {
      get => this.golfCoursesZoom;
      set => this.SetProperty<IList<T>>(ref this.golfCoursesZoom, value, nameof (GolfCoursesZoom));
    }

    public abstract int GolfCoursesCount { get; }

    public bool IsLoading
    {
      get => this.isLoading;
      private set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public ICommand BackCommand => this.backCommand ?? (this.backCommand = (ICommand) new HealthCommand(new Action(this.smoothNavService.GoBack)));

    public override string TelemetryParameters => "Browse By Country";

    protected GolfCourseBrowseRegionResultsViewModelBase(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IFormattingService formattingService,
      IGolfCourseProvider golfCourseProvider,
      IGolfCourseFilterService golfCourseFilterService)
      : base(networkService, smoothNavService, formattingService, golfCourseFilterService)
    {
      this.smoothNavService = smoothNavService;
      this.golfCourseProvider = golfCourseProvider;
      this.formattingService = formattingService;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await base.LoadDataAsync(parameters);
      this.RegionId = this.GetIntParameter("RegionId");
      this.RegionName = this.GetStringParameter("RegionName");
      this.resultsPerPage = this.GetIntParameter("RegionCourseCount");
      this.SearchTitle = this.RegionName.ToLower();
      this.GolfCoursesZoom = (IList<T>) await this.GetGolfCoursesAsync();
      this.ResultsTitle = string.Format(AppResources.GolfCourseSearchResultsLabel, new object[1]
      {
        (object) this.GolfCoursesCount
      });
      this.IsFilteringEnabled = true;
      if (this.GolfCoursesCount == 0)
        throw new CustomErrorException(AppResources.NoGolfCourseDataFoundTryLaterMessage);
    }

    private async Task<ObservableCollection<T>> GetGolfCoursesAsync()
    {
      ObservableCollection<T> golfCourseViewModel;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        golfCourseViewModel = this.CreateGolfCourseViewModel((IEnumerable<GolfCourseSummary>) await this.golfCourseProvider.GetCoursesByStateAsync(this.regionId.ToString(), cancellationTokenSource.Token, this.GetGolfCourseTypeFilter(), this.GetGolfCourseHoleFilter(), new int?(1), new int?(this.resultsPerPage)));
      return golfCourseViewModel;
    }

    protected abstract ObservableCollection<T> CreateGolfCourseViewModel(
      IEnumerable<GolfCourseSummary> result);

    protected IEnumerable<IGrouping<char, GolfCourseSummaryViewModel>> CreateGolfCourseQuery(
      IEnumerable<GolfCourseSummary> result)
    {
      return (IEnumerable<IGrouping<char, GolfCourseSummaryViewModel>>) result.OrderBy<GolfCourseSummary, string>((Func<GolfCourseSummary, string>) (p => p.Name)).Select<GolfCourseSummary, GolfCourseSummaryViewModel>((Func<GolfCourseSummary, GolfCourseSummaryViewModel>) (p => new GolfCourseSummaryViewModel(p.Name, p.Distance.HasValue ? (string) this.formattingService.FormatDistanceDynamic(new Length?(p.Distance.Value)) : string.Empty, p.CourseType, p.NumberOfHoles, p.City, p))).GroupBy<GolfCourseSummaryViewModel, char>((Func<GolfCourseSummaryViewModel, char>) (p => p.Name.ToLower()[0])).ToList<IGrouping<char, GolfCourseSummaryViewModel>>();
    }

    protected IList<char> Characters => (IList<char>) "#abcdefghijklmnopqrstuvwxyz".ToCharArray();
  }
}
