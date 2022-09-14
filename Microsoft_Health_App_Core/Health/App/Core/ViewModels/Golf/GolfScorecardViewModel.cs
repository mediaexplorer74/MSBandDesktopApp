// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfScorecardViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Providers.Golf.Rounds;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.Utilities.Logging;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Scorecard"})]
  public sealed class GolfScorecardViewModel : PanelViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfScorecardViewModel.cs");
    private static readonly IActivityManager ActivityManager = GolfScorecardViewModel.Logger.CreateActivityManager();
    private readonly IGolfRoundProvider golfRoundProvider;
    private readonly IConnectedAppsProvider connectedAppsProvider;
    private readonly ISmoothNavService smoothNavService;
    private readonly IFormattingService formattingService;
    private readonly IUserProfileService userProfileService;
    private bool showTopConnectMessage;
    private bool refreshOnBack;
    private IEnumerable<GolfScorecardElementViewModel> elements;
    private string teesPlayed;
    private string yardUnitType;
    private HealthCommand connectCommand;
    private readonly IEnvironmentService environmentService;

    public GolfScorecardViewModel(
      IGolfRoundProvider golfRoundProvider,
      INetworkService networkService,
      IConnectedAppsProvider connectedAppsProvider,
      ISmoothNavService smoothNavService,
      IFormattingService formattingService,
      IEnvironmentService environmentService,
      IUserProfileService userProfileService)
      : base(networkService)
    {
      Assert.ParamIsNotNull((object) golfRoundProvider, nameof (golfRoundProvider));
      Assert.ParamIsNotNull((object) connectedAppsProvider, nameof (connectedAppsProvider));
      Assert.ParamIsNotNull((object) smoothNavService, nameof (smoothNavService));
      Assert.ParamIsNotNull((object) formattingService, nameof (formattingService));
      Assert.ParamIsNotNull((object) environmentService, nameof (environmentService));
      this.golfRoundProvider = golfRoundProvider;
      this.connectedAppsProvider = connectedAppsProvider;
      this.smoothNavService = smoothNavService;
      this.formattingService = formattingService;
      this.environmentService = environmentService;
      this.userProfileService = userProfileService;
    }

    public bool ShowTopConnectMessage
    {
      get => this.showTopConnectMessage;
      private set => this.SetProperty<bool>(ref this.showTopConnectMessage, value, nameof (ShowTopConnectMessage));
    }

    public IEnumerable<GolfScorecardElementViewModel> Elements
    {
      get => this.elements;
      private set => this.SetProperty<IEnumerable<GolfScorecardElementViewModel>>(ref this.elements, value, nameof (Elements));
    }

    public string YardageUnitType
    {
      get => this.yardUnitType;
      private set => this.SetProperty<string>(ref this.yardUnitType, value, nameof (YardageUnitType));
    }

    public string TeesPlayed
    {
      get => this.teesPlayed;
      private set => this.SetProperty<string>(ref this.teesPlayed, value, nameof (TeesPlayed));
    }

    public ICommand ConnectCommand => (ICommand) this.connectCommand ?? (ICommand) (this.connectCommand = new HealthCommand((Action) (() =>
    {
      this.refreshOnBack = true;
      this.smoothNavService.Navigate(typeof (PartnerConnectViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "PartnerName",
          "tmag"
        }
      });
    })));

    public void OnBackNavigation()
    {
      if (!this.refreshOnBack)
        return;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        this.StartCheckingPartnerConnection(cancellationTokenSource.Token);
      this.refreshOnBack = false;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null) => GolfScorecardViewModel.ActivityManager.RunAsActivityAsync(Level.Debug, (Func<string>) (() => "Load Golf Scorecard Panel"), (Func<Task>) (async () =>
    {
      using (CancellationTokenSource tokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        this.StartCheckingPartnerConnection(tokenSource.Token);
        this.YardageUnitType = this.userProfileService.DistanceUnitType == DistanceUnitType.Metric ? AppResources.GolfScorecardMetersColumnLabel : AppResources.GolfScorecardYardsColumnLabel;
        string eventId;
        GolfRound round;
        if (parameters != null && parameters.TryGetValue("ID", out eventId))
          round = await this.golfRoundProvider.GetRoundAsync(eventId, tokenSource.Token);
        else
          round = await this.golfRoundProvider.GetLastRoundAsync(tokenSource.Token);
        if (round != null)
        {
          this.Elements = this.CreateElementsFromRound(round);
          this.TeesPlayed = string.Format((IFormatProvider) CultureInfo.CurrentCulture, AppResources.GolfScorecardTeesPlayedFormatString, new object[1]
          {
            (object) round.TeesPlayed
          });
        }
        else
        {
          this.Elements = Enumerable.Empty<GolfScorecardElementViewModel>();
          this.TeesPlayed = string.Format((IFormatProvider) CultureInfo.CurrentCulture, AppResources.GolfScorecardTeesPlayedFormatString, new object[1]
          {
            (object) AppResources.NotAvailable
          });
          throw new NoDataException();
        }
      }
    }));

    private async void StartCheckingPartnerConnection(CancellationToken token)
    {
      try
      {
        this.ShowTopConnectMessage = !(await this.connectedAppsProvider.GetConnectedAppsAsync(token)).Contains<string>("TMag");
      }
      catch (Exception ex)
      {
        GolfScorecardViewModel.Logger.Error(ex, "Checking golf partner connection status failed.");
      }
    }

    private IEnumerable<GolfScorecardElementViewModel> CreateElementsFromRound(
      GolfRound round)
    {
      List<GolfScorecardElementViewModel> elementViewModelList = new List<GolfScorecardElementViewModel>();
      List<GolfRoundHole> list1 = round.Holes.Where<GolfRoundHole>((Func<GolfRoundHole, bool>) (hole => hole.Number >= 1 && hole.Number <= 9)).OrderBy<GolfRoundHole, int>((Func<GolfRoundHole, int>) (hole => hole.Number)).ToList<GolfRoundHole>();
      elementViewModelList.AddRange((IEnumerable<GolfScorecardElementViewModel>) list1.Select<GolfRoundHole, GolfScorecardHoleViewModel>((Func<GolfRoundHole, GolfScorecardHoleViewModel>) (hole => this.CreateElementFromHole(hole))));
      elementViewModelList.Add((GolfScorecardElementViewModel) this.CreateHeaderFromHoles(GolfScorecardHeaderType.Outward, (IEnumerable<GolfRoundHole>) list1));
      List<GolfRoundHole> list2 = round.Holes.Where<GolfRoundHole>((Func<GolfRoundHole, bool>) (hole => hole.Number >= 10 && hole.Number <= 18)).OrderBy<GolfRoundHole, int>((Func<GolfRoundHole, int>) (hole => hole.Number)).ToList<GolfRoundHole>();
      if (list2.Any<GolfRoundHole>())
      {
        elementViewModelList.AddRange((IEnumerable<GolfScorecardElementViewModel>) list2.Select<GolfRoundHole, GolfScorecardHoleViewModel>((Func<GolfRoundHole, GolfScorecardHoleViewModel>) (hole => this.CreateElementFromHole(hole))));
        elementViewModelList.Add((GolfScorecardElementViewModel) this.CreateHeaderFromHoles(GolfScorecardHeaderType.Inward, (IEnumerable<GolfRoundHole>) list2));
      }
      elementViewModelList.Add((GolfScorecardElementViewModel) this.CreateHeaderFromHoles(GolfScorecardHeaderType.Total, list1.Concat<GolfRoundHole>((IEnumerable<GolfRoundHole>) list2)));
      return (IEnumerable<GolfScorecardElementViewModel>) elementViewModelList;
    }

    private GolfScorecardHoleViewModel CreateElementFromHole(
      GolfRoundHole hole)
    {
      return new GolfScorecardHoleViewModel(hole.Number, hole.Par, hole.DistanceToPin, (string) this.formattingService.FormatDistanceMetersOrYards(new Length?(hole.DistanceToPin)), hole.HoleDifficultyIndex.HasValue ? hole.HoleDifficultyIndex.Value.ToString((IFormatProvider) CultureInfo.CurrentCulture) : AppResources.NotAvailable, hole.Score, hole.DifferenceFromPar, this.formattingService.FormatStat((object) hole.Duration, StatValueType.DurationWithText), this.formattingService.FormatStat((object) hole.DistanceWalked, StatValueType.DistanceShort), this.formattingService.FormatStat((object) hole.StepsTaken, StatValueType.Integer), this.formattingService.FormatStat((object) hole.CaloriesBurned, StatValueType.Calories), this.formattingService.FormatStat((object) hole.LowestHeartRate, StatValueType.HeartRate), this.formattingService.FormatStat((object) hole.PeakHeartRate, StatValueType.HeartRate), this.formattingService.FormatStat((object) hole.AverageHeartRate, StatValueType.HeartRate), this.GetConvertedImageUrl(hole.ShotOverlayImageUrl));
    }

    private Uri GetConvertedImageUrl(Uri shotOverlayImageUrl)
    {
      if (shotOverlayImageUrl == (Uri) null || string.IsNullOrEmpty(shotOverlayImageUrl.AbsoluteUri))
      {
        GolfScorecardViewModel.Logger.Error((object) ("Golf hole shot overlay image base uri error: " + (object) shotOverlayImageUrl));
        return (Uri) null;
      }
      int width = this.environmentService.PixelScreenSize.Width;
      Uri golfHoleImage = ImageUriUtilities.GetGolfHoleImage(shotOverlayImageUrl, width);
      if (!(golfHoleImage == (Uri) null))
        return golfHoleImage;
      GolfScorecardViewModel.Logger.Error((object) ("Golf hole shot overlay image converted uri error: " + (object) shotOverlayImageUrl));
      return (Uri) null;
    }

    private GolfScorecardHeaderViewModel CreateHeaderFromHoles(
      GolfScorecardHeaderType headerType,
      IEnumerable<GolfRoundHole> holes)
    {
      List<GolfRoundHole> golfRoundHoleList = new List<GolfRoundHole>(holes);
      List<int> list = golfRoundHoleList.Where<GolfRoundHole>((Func<GolfRoundHole, bool>) (hole => hole.Score.HasValue)).Select<GolfRoundHole, int>((Func<GolfRoundHole, int>) (hole => hole.Score.Value)).ToList<int>();
      Length distance = golfRoundHoleList.Sum<GolfRoundHole>((Func<GolfRoundHole, Length>) (hole => hole.DistanceToPin));
      return new GolfScorecardHeaderViewModel(headerType, golfRoundHoleList.Sum<GolfRoundHole>((Func<GolfRoundHole, int>) (hole => hole.Par)), distance, (string) this.formattingService.FormatDistanceMetersOrYards(new Length?(distance)), list.Any<int>() ? new int?(list.Sum()) : new int?());
    }
  }
}
