// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Golf.Rounds;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.Utilities.Logging;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Events.Golf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Golf"})]
  public sealed class GolfTileViewModel : EventTileViewModelBase<GolfEvent>
  {
    private static readonly IActivityManager ActivityManager = EventTileViewModelBase<GolfEvent>.Logger.CreateActivityManager();
    private static readonly TimeSpan AgeCurrentThreshold = TimeSpan.FromDays(14.0);
    private readonly IGolfRoundProvider golfRoundProvider;
    private readonly IFormattingService formatter;
    private readonly ISmoothNavService navigationService;
    private bool hasNavigatedTo;
    private GolfRound round;

    public GolfTileViewModel(
      IGolfRoundProvider golfRoundProvider,
      IEventTrackingService eventViewTracker,
      IFormattingService formatter,
      INetworkService networkService,
      IMessageSender messageSender,
      GolfScorecardViewModel golfScorecardViewModel,
      GolfSummaryViewModel golfSummaryViewModel,
      ISmoothNavService navigationService,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(eventViewTracker, EventType.Golf, networkService, messageSender, navigationService, (EventSummaryViewModelBase<GolfEvent>) golfSummaryViewModel, firstTimeUse)
    {
      Assert.ParamIsNotNull((object) golfRoundProvider, nameof (golfRoundProvider));
      Assert.ParamIsNotNull((object) formatter, nameof (formatter));
      Assert.ParamIsNotNull((object) navigationService, nameof (navigationService));
      this.golfRoundProvider = golfRoundProvider;
      this.formatter = formatter;
      this.navigationService = navigationService;
      this.TileIcon = "\uE151";
      this.Pivots.Add(new PivotDefinition(AppResources.GolfScorecardHeader, (object) golfScorecardViewModel));
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSummary, (object) golfSummaryViewModel));
      this.FirstTimeUse.IsSupported = true;
      this.FirstTimeUse.AlwaysShowLearnMoreCommand = true;
      this.FirstTimeUse.LearnMoreCommandAction = new Action(this.OnLearnMore);
      this.FirstTimeUse.LearnMoreCommandText = AppResources.TileFirstTimeUseGolfLearnMore;
      this.FirstTimeUse.Message = AppResources.TileFirstTimeUseGolfMessage;
      this.FirstTimeUse.Type = TileFirstTimeUseViewModel.FirstTimeUseType.Golf;
    }

    private GolfRound Round
    {
      get => this.round;
      set
      {
        this.round = value;
        this.Event = this.round != null ? this.round.GetRawEvent() : (GolfEvent) null;
      }
    }

    protected override Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null) => GolfTileViewModel.ActivityManager.RunAsActivityAsync<bool>(Level.Debug, (Func<string>) (() => "Load Golf Tile"), (Func<Task<bool>>) (async () =>
    {
      GolfTileViewModel golfTileViewModel;
      using (CancellationTokenSource tokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        string eventId;
        if (parameters != null && parameters.TryGetValue("ID", out eventId))
        {
          GolfRound roundAsync = await this.golfRoundProvider.GetRoundAsync(eventId, tokenSource.Token);
          golfTileViewModel.Round = roundAsync;
          golfTileViewModel = (GolfTileViewModel) null;
        }
        else
        {
          golfTileViewModel = this;
          GolfRound lastRoundAsync = await this.golfRoundProvider.GetLastRoundAsync(tokenSource.Token);
          golfTileViewModel.Round = lastRoundAsync;
          golfTileViewModel = (GolfTileViewModel) null;
        }
      }
      this.RefreshColorLevel();
      return this.Round != null;
    }), true);

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      if (this.hasNavigatedTo)
        this.Pivots.Select<PivotDefinition, object>((Func<PivotDefinition, object>) (pivot => pivot.Content)).OfType<GolfScorecardViewModel>().FirstOrDefault<GolfScorecardViewModel>()?.OnBackNavigation();
      else
        this.hasNavigatedTo = true;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.CanOpen = true;
      this.Header = this.formatter.FormatGolfScore(this.Round.TotalScore, this.Round.TotalDifferenceFromPar);
      this.Subheader = this.formatter.FormatTileTime(this.Round.StartTime) + Environment.NewLine;
      if (!string.IsNullOrEmpty(this.Round.Name))
        this.Subheader += this.Round.Name;
      else
        this.Subheader += this.Round.CourseName;
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.GolfTileNoContent;
    }

    protected override void RefreshColorLevel()
    {
      base.RefreshColorLevel();
      this.UpdateColorLevel(GolfTileViewModel.AgeCurrentThreshold);
    }

    private void OnLearnMore() => this.navigationService.Navigate(typeof (GolfLandingViewModel));
  }
}
