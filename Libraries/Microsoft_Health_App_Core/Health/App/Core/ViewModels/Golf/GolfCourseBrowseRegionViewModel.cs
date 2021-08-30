// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseBrowseRegionViewModel
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Browse"})]
  public sealed class GolfCourseBrowseRegionViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfCourseBrowseRegionViewModel.cs");
    private readonly IGolfCourseProvider golfCourseProvider;
    private readonly IPagePicker pagePicker;
    private readonly ISmoothNavService smoothNavService;
    private ICommand backCommand;
    private ICommand nextCommand;
    private bool isLoading;
    private string regionName;
    private int regionId;
    private string searchTitle;
    private IList<GolfCourseLocalityViewModel> golfCourses;

    public string SearchTitle
    {
      get => this.searchTitle;
      set => this.SetProperty<string>(ref this.searchTitle, value, nameof (SearchTitle));
    }

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

    public IList<GolfCourseLocalityViewModel> GolfCourses
    {
      get => this.golfCourses ?? (this.golfCourses = (IList<GolfCourseLocalityViewModel>) new ObservableCollection<GolfCourseLocalityViewModel>());
      private set => this.SetProperty<IList<GolfCourseLocalityViewModel>>(ref this.golfCourses, value, nameof (GolfCourses));
    }

    public bool IsLoading
    {
      get => this.isLoading;
      private set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public ICommand BackCommand => this.backCommand ?? (this.backCommand = (ICommand) new HealthCommand(new Action(this.smoothNavService.GoBack)));

    public ICommand NextCommand => this.nextCommand ?? (this.nextCommand = (ICommand) new HealthCommand<GolfCourseLocalityViewModel>((Action<GolfCourseLocalityViewModel>) (golfCourse =>
    {
      GolfCourseLocality rawLocality = golfCourse.GetRawLocality();
      if (rawLocality.NumberOfSubregions == 0)
        this.smoothNavService.Navigate(this.pagePicker.GolfCourseRegionResults, (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "RequiresFilterInitialization",
            bool.TrueString
          },
          {
            "RegionId",
            rawLocality.Id
          },
          {
            "RegionName",
            rawLocality.Name
          },
          {
            "RegionCourseCount",
            rawLocality.NumberOfCourses.ToString((IFormatProvider) CultureInfo.InvariantCulture)
          }
        });
      else
        this.smoothNavService.Navigate(typeof (GolfCourseBrowseRegionViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "RegionId",
            rawLocality.Id
          }
        });
    })));

    public GolfCourseBrowseRegionViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IGolfCourseProvider golfCourseProvider,
      IPagePicker pagePicker)
      : base(networkService)
    {
      this.SupportsChanges = false;
      this.smoothNavService = smoothNavService;
      this.golfCourseProvider = golfCourseProvider;
      this.pagePicker = pagePicker;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      bool flag = false;
      int result;
      if (parameters != null && parameters.ContainsKey("RegionId") && int.TryParse(parameters["RegionId"], out result))
      {
        this.RegionId = result;
        flag = true;
      }
      this.SearchTitle = flag ? AppResources.GolfCourseBrowsePageHeaderStates : AppResources.GolfCourseBrowsePageHeaderRegions;
      this.GolfCourses = (IList<GolfCourseLocalityViewModel>) await this.GetGolfCoursesAsync();
      if (this.GolfCourses.Count == 0)
        throw new CustomErrorException(AppResources.NoGolfCourseDataFoundTryLaterMessage);
    }

    private async Task<ObservableCollection<GolfCourseLocalityViewModel>> GetGolfCoursesAsync()
    {
      ObservableCollection<GolfCourseLocalityViewModel> golfCourseViewModel;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        IReadOnlyList<GolfCourseLocality> result;
        if (this.regionId > 0)
          result = await this.golfCourseProvider.GetStatesAsync(this.regionId.ToString(), cancellationTokenSource.Token);
        else
          result = await this.golfCourseProvider.GetRegionsAsync(cancellationTokenSource.Token);
        golfCourseViewModel = this.CreateGolfCourseViewModel(result);
      }
      return golfCourseViewModel;
    }

    private ObservableCollection<GolfCourseLocalityViewModel> CreateGolfCourseViewModel(
      IReadOnlyList<GolfCourseLocality> result)
    {
      return new ObservableCollection<GolfCourseLocalityViewModel>(result.Select<GolfCourseLocality, GolfCourseLocalityViewModel>((Func<GolfCourseLocality, GolfCourseLocalityViewModel>) (p => new GolfCourseLocalityViewModel(int.Parse(p.Id), p.Name, p.NumberOfSubregions, p.NumberOfCourses, p))));
    }
  }
}
