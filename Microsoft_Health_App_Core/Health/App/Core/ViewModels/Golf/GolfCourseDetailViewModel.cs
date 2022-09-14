// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseDetailViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Golf.Courses;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class GolfCourseDetailViewModel : PageViewModelBase
  {
    internal const string CourseIdKey = "CourseId";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfCourseDetailViewModel.cs");
    private static readonly IActivityManager ActivityManager = GolfCourseDetailViewModel.Logger.CreateActivityManager();
    private readonly ISmoothNavService smoothNavService;
    private readonly ILauncherService launcherService;
    private readonly IGolfCourseProvider golfCourseProvider;
    private readonly IGolfSyncService syncService;
    private readonly IUserProfileService userProfileService;
    private readonly ITileManagementService tileManagementService;
    private readonly IMessageBoxService messageBoxService;
    private GolfCourseDetails courseDetails;
    private bool isBandPaired;
    private bool isCourseOnBand;
    private bool showEditTeesButton;
    private string editTeesMessage;
    private string pageTitle;
    private int holeCount;
    private string courseType;
    private string courseRating;
    private string slopeRating;
    private bool showWebsite;
    private bool showPhoneNumber;
    private string phoneNumber;
    private bool showAddress;
    private bool hasTees;
    private string displayAddress;
    private HealthCommand backCommand;
    private HealthCommand pickTeesCommand;
    private HealthCommand openWebsiteCommand;
    private HealthCommand openPhoneNumberCommand;
    private HealthCommand openAddressCommand;
    private HealthCommand findOtherCoursesCommand;

    public GolfCourseDetailViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      ILauncherService launcherService,
      IGolfCourseProvider golfCourseProvider,
      IGolfSyncService syncService,
      IUserProfileService userProfileService,
      ITileManagementService tileManagementService,
      IMessageBoxService messageBoxService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.launcherService = launcherService;
      this.golfCourseProvider = golfCourseProvider;
      this.syncService = syncService;
      this.userProfileService = userProfileService;
      this.tileManagementService = tileManagementService;
      this.messageBoxService = messageBoxService;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null) => GolfCourseDetailViewModel.ActivityManager.RunAsActivityAsync(Level.Debug, (Func<string>) (() => "Load golf course detail page"), (Func<Task>) (async () =>
    {
      string stringParameter = this.GetStringParameter("CourseId");
      GolfCourseDetailViewModel courseDetailViewModel = this;
      GolfCourseDetails courseDetails = courseDetailViewModel.courseDetails;
      GolfCourseDetails courseDetailsAsync = await this.golfCourseProvider.GetCourseDetailsAsync(stringParameter, CancellationToken.None);
      courseDetailViewModel.courseDetails = courseDetailsAsync;
      courseDetailViewModel = (GolfCourseDetailViewModel) null;
      this.RefreshBandStatus();
      this.PageTitle = this.courseDetails.Name;
      this.HoleCount = this.courseDetails.NumberOfHoles;
      switch (this.courseDetails.CourseType)
      {
        case GolfCourseType.Public:
          this.CourseType = EnumResources.GolfCourseType_Public;
          break;
        case GolfCourseType.Private:
          this.CourseType = EnumResources.GolfCourseType_Private;
          break;
        default:
          this.CourseType = (string) null;
          break;
      }
      if (this.courseDetails.Tees != null && this.courseDetails.Tees.Count > 0)
      {
        this.HasTees = true;
        GolfCourseTee golfCourseTee;
        try
        {
          golfCourseTee = this.courseDetails.Tees.Single<GolfCourseTee>((Func<GolfCourseTee, bool>) (t => t.IsDefault));
        }
        catch (Exception ex)
        {
          GolfCourseDetailViewModel.Logger.Warn((object) ("Error finding a valid default tee for course " + this.courseDetails.Name), ex);
          golfCourseTee = this.courseDetails.Tees.First<GolfCourseTee>();
        }
        if (golfCourseTee != null)
        {
          this.CourseRating = golfCourseTee.Rating == 0.0 ? AppResources.NotAvailable : Formatter.FormatDouble(golfCourseTee.Rating, 1);
          int slope = (int) golfCourseTee.Slope;
          this.SlopeRating = slope == 0 ? AppResources.NotAvailable : slope.ToString();
        }
      }
      else
      {
        this.HasTees = false;
        GolfCourseDetailViewModel.Logger.Warn((object) ("Error finding  a valid default tee for course " + this.courseDetails.Name));
      }
      this.ShowWebsite = this.courseDetails.WebsiteUrl != (Uri) null;
      this.ShowPhoneNumber = !string.IsNullOrWhiteSpace(this.courseDetails.PhoneNumber);
      this.PhoneNumber = this.courseDetails.PhoneNumber;
      IList<string> addressLines = this.courseDetails.GetAddressLines();
      this.ShowAddress = addressLines.Count > 0;
      this.DisplayAddress = string.Join(Environment.NewLine, (IEnumerable<string>) addressLines);
    }));

    protected override void OnBackNavigation() => this.RefreshBandStatus();

    private void RefreshBandStatus()
    {
      this.IsBandPaired = this.userProfileService.IsRegisteredBandPaired;
      if (this.IsBandPaired)
      {
        SyncedGolfCourse lastSyncedCourse = this.syncService.LastSyncedCourse;
        this.IsCourseOnBand = lastSyncedCourse != null && lastSyncedCourse.CourseId == this.courseDetails.CourseId;
        this.ShowEditTeesButton = this.IsCourseOnBand && this.courseDetails.Tees.Count > 1;
        if (this.ShowEditTeesButton)
        {
          GolfCourseTee golfCourseTee = this.courseDetails.Tees.FirstOrDefault<GolfCourseTee>((Func<GolfCourseTee, bool>) (t => lastSyncedCourse != null && t.Id == lastSyncedCourse.TeeId));
          this.EditTeesMessage = string.Format(AppResources.CourseDetailSetToPlayTeesMessage, new object[1]
          {
            (object) (golfCourseTee == null ? this.courseDetails.Tees[0].Name : golfCourseTee.Name)
          });
        }
      }
      else
        this.ShowEditTeesButton = false;
      GolfCourseDetailViewModel.Logger.Debug((object) string.Format("Band status refreshed. IsBandPaired: {0}, IsCourseOnBand: {1}", new object[2]
      {
        (object) this.IsBandPaired,
        (object) this.IsCourseOnBand
      }));
    }

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    public ICommand PickTeesCommand => (ICommand) this.pickTeesCommand ?? (ICommand) (this.pickTeesCommand = new HealthCommand((Action) (async () =>
    {
      Guid golfTileGuid = Guid.Parse("fb9d005a-c3da-49d4-8e7b-c6f674fc4710");
      if (!this.tileManagementService.EnabledTiles.Any<AppBandTile>((Func<AppBandTile, bool>) (t => t.TileId == golfTileGuid)))
      {
        if (await this.messageBoxService.ShowAsync(AppResources.CourseDetailManageTilesDialogMessage, AppResources.CourseDetailManageTilesDialogTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
          return;
        this.smoothNavService.Navigate(typeof (ManageTilesViewModel));
      }
      else
      {
        string courseId = this.courseDetails.CourseId;
        if (this.courseDetails.Tees.Count > 1)
        {
          this.smoothNavService.Navigate(typeof (TeePickViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "CourseId",
              courseId
            }
          });
        }
        else
        {
          string str = this.courseDetails.Tees.Count == 0 ? (string) null : this.courseDetails.Tees[0].Id;
          this.smoothNavService.Navigate(typeof (GolfSyncViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "CourseId",
              courseId
            },
            {
              "TeeId",
              str
            }
          });
        }
      }
    })));

    public ICommand OpenWebsiteCommand => (ICommand) this.openWebsiteCommand ?? (ICommand) (this.openWebsiteCommand = new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser(this.courseDetails.WebsiteUrl))));

    public ICommand OpenPhoneNumberCommand => (ICommand) this.openPhoneNumberCommand ?? (ICommand) (this.openPhoneNumberCommand = new HealthCommand((Action) (() => this.launcherService.CallPhoneNumber(this.PhoneNumber))));

    public ICommand OpenAddressCommand => (ICommand) this.openAddressCommand ?? (ICommand) (this.openAddressCommand = new HealthCommand((Action) (() => this.launcherService.MapAddress((IEnumerable<string>) this.courseDetails.GetAddressLines()))));

    public ICommand FindOtherCoursesCommand => (ICommand) this.findOtherCoursesCommand ?? (ICommand) (this.findOtherCoursesCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (GolfLandingViewModel)))));

    public bool IsBandPaired
    {
      get => this.isBandPaired;
      set
      {
        this.SetProperty<bool>(ref this.isBandPaired, value, nameof (IsBandPaired));
        this.RaisePropertyChanged("ShowSyncToBand");
      }
    }

    public bool IsCourseOnBand
    {
      get => this.isCourseOnBand;
      set => this.SetProperty<bool>(ref this.isCourseOnBand, value, nameof (IsCourseOnBand));
    }

    public bool ShowEditTeesButton
    {
      get => this.showEditTeesButton;
      set => this.SetProperty<bool>(ref this.showEditTeesButton, value, nameof (ShowEditTeesButton));
    }

    public string EditTeesMessage
    {
      get => this.editTeesMessage;
      set => this.SetProperty<string>(ref this.editTeesMessage, value, nameof (EditTeesMessage));
    }

    public string PageTitle
    {
      get => this.pageTitle;
      set => this.SetProperty<string>(ref this.pageTitle, value, nameof (PageTitle));
    }

    public int HoleCount
    {
      get => this.holeCount;
      set => this.SetProperty<int>(ref this.holeCount, value, nameof (HoleCount));
    }

    public string CourseType
    {
      get => this.courseType;
      set => this.SetProperty<string>(ref this.courseType, value, nameof (CourseType));
    }

    public string CourseRating
    {
      get => this.courseRating;
      set => this.SetProperty<string>(ref this.courseRating, value, nameof (CourseRating));
    }

    public string SlopeRating
    {
      get => this.slopeRating;
      set => this.SetProperty<string>(ref this.slopeRating, value, nameof (SlopeRating));
    }

    public bool ShowWebsite
    {
      get => this.showWebsite;
      set => this.SetProperty<bool>(ref this.showWebsite, value, nameof (ShowWebsite));
    }

    public bool ShowPhoneNumber
    {
      get => this.showPhoneNumber;
      set => this.SetProperty<bool>(ref this.showPhoneNumber, value, nameof (ShowPhoneNumber));
    }

    public string PhoneNumber
    {
      get => this.phoneNumber;
      set => this.SetProperty<string>(ref this.phoneNumber, value, nameof (PhoneNumber));
    }

    public bool ShowAddress
    {
      get => this.showAddress;
      set => this.SetProperty<bool>(ref this.showAddress, value, nameof (ShowAddress));
    }

    public bool ShowSyncToBand => this.hasTees && this.isBandPaired;

    private bool HasTees
    {
      get => this.hasTees;
      set
      {
        this.SetProperty<bool>(ref this.hasTees, value, nameof (HasTees));
        this.RaisePropertyChanged("ShowSyncToBand");
      }
    }

    public string DisplayAddress
    {
      get => this.displayAddress;
      set => this.SetProperty<string>(ref this.displayAddress, value, nameof (DisplayAddress));
    }
  }
}
