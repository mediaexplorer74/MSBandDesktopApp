// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseRecentViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public sealed class GolfCourseRecentViewModel : GolfCourseSearchResultsViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfCourseRecentViewModel.cs");
    private readonly IGolfCourseProvider golfCourseProvider;
    private readonly ISmoothNavService smoothNavService;
    private ICommand backCommand;
    private bool isLoading;

    public bool IsLoading
    {
      get => this.isLoading;
      private set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public ICommand BackCommand => this.backCommand ?? (this.backCommand = (ICommand) new HealthCommand(new Action(this.smoothNavService.GoBack)));

    public override string TelemetryParameters => "Recent";

    public GolfCourseRecentViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IFormattingService formattingService,
      IGolfCourseProvider golfCourseProvider,
      IGolfCourseFilterService golfCourseFilterService)
      : base(networkService, smoothNavService, formattingService, golfCourseFilterService)
    {
      this.smoothNavService = smoothNavService;
      this.golfCourseProvider = golfCourseProvider;
      this.SearchTitle = AppResources.GolfCourseRecentPageHeader;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.GolfCourses = (IList<GolfCourseSummaryViewModel>) await this.GetGolfCoursesAsync();
      this.ResultsTitle = string.Format(AppResources.GolfCourseSearchResultsLabel, new object[1]
      {
        (object) this.GolfCourses.Count
      });
      this.IsFilteringEnabled = false;
      if (this.GolfCourses.Count == 0)
        throw new CustomErrorException(AppResources.NoGolfCourseDataFoundMessage);
    }

    protected override async Task<ObservableCollection<GolfCourseSummaryViewModel>> GetGolfCoursesAsync()
    {
      ObservableCollection<GolfCourseSummaryViewModel> golfCourseViewModel;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        golfCourseViewModel = this.CreateGolfCourseViewModel(await this.golfCourseProvider.GetRecentCoursesAsync(cancellationTokenSource.Token));
      return golfCourseViewModel;
    }
  }
}
