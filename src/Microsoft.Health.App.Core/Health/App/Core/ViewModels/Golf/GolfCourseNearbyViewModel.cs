// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseNearbyViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Geolocator.Plugin.Abstractions;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
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
  public sealed class GolfCourseNearbyViewModel : GolfCourseSearchResultsViewModelBase
  {
    internal const uint LocationDisabled = 2147500036;
    private const int CountPerPage = 10;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfCourseNearbyViewModel.cs");
    private static readonly TimeSpan GolfCourseLocationDelay = TimeSpan.FromMinutes(2.0);
    private readonly IGolfCourseProvider golfCourseProvider;
    private readonly IGeolocationService geolocationService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly ISmoothNavService smoothNavService;
    private ICommand backCommand;
    private ICommand loadMoreCommand;
    private bool isLoading;
    private bool endReached;
    private int nextPageNumber;
    private PortableGeoposition position;

    public bool IsLoading
    {
      get => this.isLoading;
      private set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public ICommand BackCommand => this.backCommand ?? (this.backCommand = (ICommand) new HealthCommand(new Action(this.smoothNavService.GoBack)));

    public ICommand LoadMoreCommand => this.loadMoreCommand ?? (this.loadMoreCommand = (ICommand) new HealthCommand(new Action(this.Paginate)));

    public override string TelemetryParameters => "Nearby";

    public GolfCourseNearbyViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IFormattingService formattingService,
      IGolfCourseProvider golfCourseProvider,
      IGeolocationService geolocationService,
      IMessageBoxService messageBoxService,
      IErrorHandlingService errorHandlingService,
      IGolfCourseFilterService golfCourseFilterService)
      : base(networkService, smoothNavService, formattingService, golfCourseFilterService)
    {
      this.smoothNavService = smoothNavService;
      this.golfCourseProvider = golfCourseProvider;
      this.geolocationService = geolocationService;
      this.messageBoxService = messageBoxService;
      this.errorHandlingService = errorHandlingService;
      this.SearchTitle = AppResources.GolfCourseNearbyPageHeader;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await base.LoadDataAsync(parameters);
      this.endReached = false;
      this.nextPageNumber = 1;
      using (CancellationTokenSource tokenSource = new CancellationTokenSource(GolfCourseNearbyViewModel.GolfCourseLocationDelay))
      {
        PortableGeoposition? locationAsync = await this.GetLocationAsync(tokenSource.Token);
        if (locationAsync.HasValue)
        {
          this.position = locationAsync.Value;
          this.GolfCourses = (IList<GolfCourseSummaryViewModel>) await this.GetGolfCoursesAsync();
          this.ResultsTitle = string.Format(AppResources.GolfCourseSearchResultsLabel, new object[1]
          {
            (object) this.GolfCourses.Count
          });
          if (this.GolfCourses.Count == 0)
            throw new CustomErrorException(AppResources.NoGolfCourseDataFoundTryLaterMessage);
        }
        else
        {
          this.endReached = true;
          throw new CustomErrorException(AppResources.DataErrorMessageLocked);
        }
      }
      this.IsFilteringEnabled = true;
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
      ObservableCollection<GolfCourseSummaryViewModel> golfCourseViewModel;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        IReadOnlyList<GolfCourseSummary> coursesByLocationAsync = await this.golfCourseProvider.GetCoursesByLocationAsync(this.position.Latitude, this.position.Longitude, this.nextPageNumber, 10, cancellationTokenSource.Token, this.GetGolfCourseTypeFilter(), this.GetGolfCourseHoleFilter());
        if (coursesByLocationAsync.Count < 10)
          this.endReached = true;
        ++this.nextPageNumber;
        golfCourseViewModel = this.CreateGolfCourseViewModel(coursesByLocationAsync);
      }
      return golfCourseViewModel;
    }

    private async Task<PortableGeoposition?> GetLocationAsync(
      CancellationToken token)
    {
      PortableGeoposition? geoPosition = new PortableGeoposition?();
      try
      {
        geoPosition = new PortableGeoposition?(await this.geolocationService.GetGeopositionAsync(token));
      }
      catch (TaskCanceledException ex)
      {
      }
      catch (GeolocationException ex)
      {
        GolfCourseNearbyViewModel.Logger.Error((Exception) ex, "Could not get location for nearby golf courses, is disabled in phone settings.");
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.LocationErrorBody, AppResources.LocationErrorTitle, PortableMessageBoxButton.OK);
      }
      catch (Exception ex)
      {
        if (ex.InnerException is UnauthorizedAccessException)
          GolfCourseNearbyViewModel.Logger.Error(ex, "Could not get location for nearby golf courses, is disabled in phone settings.");
        else if (ex.HResult == -2147467260)
        {
          GolfCourseNearbyViewModel.Logger.Error(ex, "Could not get location for nearby golf courses, is disabled in phone settings.");
          int num = (int) await this.messageBoxService.ShowAsync(AppResources.LocationErrorBody, AppResources.LocationErrorTitle, PortableMessageBoxButton.OK);
        }
        else
        {
          GolfCourseNearbyViewModel.Logger.Error(ex, "Unknown problem trying to get location for nearby golf courses");
          await this.errorHandlingService.HandleExceptionAsync(ex);
        }
      }
      return geoPosition;
    }
  }
}
