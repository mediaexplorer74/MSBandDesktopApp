// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfSplitTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Golf.Rounds;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public sealed class GolfSplitTileViewModel : SplitTileViewModel
  {
    private readonly IGolfRoundProvider golfRoundProvider;
    private readonly IGolfSyncService golfSyncService;
    private readonly ISmoothNavService navigationService;
    private bool showCourseOnBand;

    private bool ShowCourseOnBand
    {
      get => this.showCourseOnBand;
      set
      {
        this.showCourseOnBand = value;
        this.UpdateHeader();
      }
    }

    public GolfSplitTileViewModel(
      ISmoothNavService navigationService,
      IGolfSyncService golfSyncService,
      IGolfRoundProvider golfRoundProvider)
    {
      Assert.ParamIsNotNull((object) navigationService, nameof (navigationService));
      Assert.ParamIsNotNull((object) golfSyncService, nameof (golfSyncService));
      Assert.ParamIsNotNull((object) golfRoundProvider, nameof (golfRoundProvider));
      this.navigationService = navigationService;
      this.golfSyncService = golfSyncService;
      this.golfRoundProvider = golfRoundProvider;
      this.Header = AppResources.GolfSplitTileSyncHeader;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await this.UpdateAsync();
      return true;
    }

    protected override async void OnNavigatedBack()
    {
      base.OnNavigatedBack();
      await this.UpdateAsync();
    }

    protected override void OnTileCommand()
    {
      if (this.ShowCourseOnBand)
      {
        SyncedGolfCourse lastSyncedCourse = this.golfSyncService.LastSyncedCourse;
        if (lastSyncedCourse == null)
          return;
        this.navigationService.Navigate(typeof (GolfCourseDetailViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "CourseId",
            lastSyncedCourse.CourseId
          },
          {
            "Page.HeaderType",
            HeaderType.Normal.ToString()
          },
          {
            "Page.TransitionPageType",
            TransitionPageType.L1WithHeader.ToString()
          }
        });
      }
      else
      {
        ApplicationTelemetry.LogGolfFindCourse("Small Tile");
        this.navigationService.Navigate(typeof (GolfLandingViewModel));
      }
    }

    private async Task UpdateAsync()
    {
      try
      {
        SyncedGolfCourse lastSyncedCourse = this.golfSyncService.LastSyncedCourse;
        if (lastSyncedCourse != null)
        {
          using (CancellationTokenSource tokenSource = new CancellationTokenSource())
          {
            GolfRound lastRoundAsync = await this.golfRoundProvider.GetLastRoundAsync(tokenSource.Token);
            this.ShowCourseOnBand = lastRoundAsync == null || !(lastSyncedCourse.Timestamp < lastRoundAsync.StartTime);
          }
        }
        else
          this.ShowCourseOnBand = false;
        lastSyncedCourse = (SyncedGolfCourse) null;
      }
      catch
      {
        this.ShowCourseOnBand = false;
      }
    }

    private void UpdateHeader()
    {
      if (this.ShowCourseOnBand)
        this.Header = AppResources.GolfSplitTileOnBandHeader;
      else
        this.Header = AppResources.GolfSplitTileSyncHeader;
    }
  }
}
