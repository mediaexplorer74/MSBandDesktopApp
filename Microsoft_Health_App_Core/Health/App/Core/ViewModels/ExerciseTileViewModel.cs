// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.ExerciseTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Exercise"})]
  public class ExerciseTileViewModel : EventTileViewModelBase<ExerciseEvent>
  {
    private static readonly TimeSpan AgeCurrentThreshold = TimeSpan.FromDays(14.0);
    private readonly IBestEventProvider bestEventProvider;
    private readonly IFormattingService formattingService;
    private readonly IExerciseProvider provider;

    public ExerciseTileViewModel(
      IExerciseProvider provider,
      IBestEventProvider bestEventProvider,
      IEventTrackingService eventViewTracker,
      INetworkService networkService,
      IMessageSender messageSender,
      IFormattingService formattingService,
      ExerciseSummaryViewModel exerciseSummaryViewModel,
      ISmoothNavService smoothNavService,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(eventViewTracker, EventType.Workout, networkService, messageSender, smoothNavService, (EventSummaryViewModelBase<ExerciseEvent>) exerciseSummaryViewModel, firstTimeUse)
    {
      this.TileIcon = "\uE002";
      this.provider = provider;
      this.bestEventProvider = bestEventProvider;
      this.formattingService = formattingService;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSummary, (object) exerciseSummaryViewModel));
      this.FirstTimeUse.IsSupported = true;
      this.FirstTimeUse.Message = AppResources.TileFirstTimeUseExerciseMessage;
      this.FirstTimeUse.Type = TileFirstTimeUseViewModel.FirstTimeUseType.Exercise;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EventTileViewModelBase<ExerciseEvent>.Logger.Debug((object) "<START> loading the exercise tile");
      string eventId;
      if (parameters != null && parameters.TryGetValue("ID", out eventId))
        this.Event = await this.provider.GetExerciseEventAsync(eventId);
      else
        this.Event = await this.provider.GetLastExerciseEventAsync();
      this.RefreshColorLevel();
      EventTileViewModelBase<ExerciseEvent>.Logger.Debug("<END> loading the run tile (ID: {0})", this.Event != null ? (object) this.Event.EventId : (object) "null");
      return this.Event != null;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.CanOpen = true;
      this.IsBest = (await this.bestEventProvider.GetLabelForEventAsync(this.Event.EventId, EventType.Workout)).Key;
      this.Header = this.formattingService.FormatCalories(new int?(this.Event.CaloriesBurned), true);
      this.Subheader = this.formattingService.FormatTileTime(this.Event.StartTime);
      if (!string.IsNullOrEmpty(this.Event.Name) && this.Event.Tags != null && this.Event.Tags.Count > 0 && !string.IsNullOrEmpty(this.Event.Tags[0].Text))
        this.Subheader += string.Format("\n{0} - {1}", new object[2]
        {
          (object) this.Event.Tags[0].Text,
          (object) this.Event.Name
        });
      else if (!string.IsNullOrEmpty(this.Event.Name))
      {
        this.Subheader += string.Format("\n{0}", new object[1]
        {
          (object) this.Event.Name
        });
      }
      else
      {
        if (this.Event.Tags == null || this.Event.Tags.Count <= 0 || string.IsNullOrEmpty(this.Event.Tags[0].Text))
          return;
        this.Subheader += string.Format("\n{0}", new object[1]
        {
          (object) this.Event.Tags[0].Text
        });
      }
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.ExerciseTileNoContent;
    }

    protected override void RefreshColorLevel()
    {
      base.RefreshColorLevel();
      this.UpdateColorLevel(ExerciseTileViewModel.AgeCurrentThreshold);
    }
  }
}
