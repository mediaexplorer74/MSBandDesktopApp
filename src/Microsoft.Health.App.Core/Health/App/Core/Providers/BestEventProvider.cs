// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.BestEventProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.App.Core.ViewModels.History;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class BestEventProvider : IBestEventProvider
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Providers\\BestEventProvider.cs");
    private readonly IFormattingService formatter;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IUserProfileService userProfileService;

    public BestEventProvider(
      IFormattingService formatter,
      IHealthCloudClient healthCloudClient,
      IUserProfileService userProfileService)
    {
      if (formatter == null)
        throw new ArgumentNullException(nameof (formatter));
      if (healthCloudClient == null)
        throw new ArgumentNullException(nameof (healthCloudClient));
      if (userProfileService == null)
        throw new ArgumentNullException(nameof (userProfileService));
      this.formatter = formatter;
      this.healthCloudClient = healthCloudClient;
      this.userProfileService = userProfileService;
    }

    public async Task<IList<BestEvent>> GetBestEventsAsync(GoalType type)
    {
      try
      {
        return (IList<BestEvent>) (await this.healthCloudClient.GetBestGoalsAsync(CancellationToken.None, type)).Select<UsersGoal, BestEvent>((Func<UsersGoal, BestEvent>) (userGoal => new BestEvent(this.formatter, this.userProfileService, userGoal))).Where<BestEvent>((Func<BestEvent, bool>) (goal => goal.IsGoodGoal)).GroupBy<BestEvent, string>((Func<BestEvent, string>) (goal => goal.Label)).Select<IGrouping<string, BestEvent>, BestEvent>((Func<IGrouping<string, BestEvent>, BestEvent>) (group => group.OrderByDescending<BestEvent, DateTimeOffset>((Func<BestEvent, DateTimeOffset>) (g => g.StartTime)).First<BestEvent>())).ToList<BestEvent>();
      }
      catch (Exception ex)
      {
        BestEventProvider.Logger.Error((object) ex);
      }
      return (IList<BestEvent>) null;
    }

    public async Task<IList<HistoryEventViewModel<T>>> GetBestEventsDetailsAsync<T>(
      ICollection<string> eventIds)
      where T : UserEvent
    {
      try
      {
        if (eventIds == null || !eventIds.Any<string>())
          throw new Exception("eventIds can't be an empty/null collection");
        UserEvent[] userEventArray = await Task.WhenAll<UserEvent>(eventIds.Distinct<string>().Where<string>((Func<string, bool>) (id => !string.IsNullOrWhiteSpace(id))).Select<string, Task<UserEvent>>((Func<string, Task<UserEvent>>) (id => this.healthCloudClient.GetEventAsync<UserEvent>(id, CancellationToken.None))).ToArray<Task<UserEvent>>());
        if (userEventArray != null)
        {
          if (((IEnumerable<UserEvent>) userEventArray).Any<UserEvent>())
            return (IList<HistoryEventViewModel<T>>) ((IEnumerable<UserEvent>) userEventArray).Where<UserEvent>((Func<UserEvent, bool>) (evt => evt != null)).Select<UserEvent, HistoryEventViewModel<T>>((Func<UserEvent, HistoryEventViewModel<T>>) (evt =>
            {
              return new HistoryEventViewModel<T>(evt as T, this.formatter)
              {
                IsBest = true
              };
            })).OrderByDescending<HistoryEventViewModel<T>, DateTimeOffset>((Func<HistoryEventViewModel<T>, DateTimeOffset>) (evt => evt.Event.StartTime)).ToList<HistoryEventViewModel<T>>();
        }
      }
      catch (Exception ex)
      {
        BestEventProvider.Logger.Error((object) ex);
      }
      return (IList<HistoryEventViewModel<T>>) null;
    }

    public async Task<KeyValuePair<bool, string>> GetLabelForEventAsync(
      string eventId,
      EventType type)
    {
      try
      {
        IList<BestEvent> bestEventsAsync = await this.GetBestEventsAsync(BestEventProvider.GetGoalType(type));
        if (bestEventsAsync == null)
          return new KeyValuePair<bool, string>();
        object[] array = (object[]) bestEventsAsync.Where<BestEvent>((Func<BestEvent, bool>) (t => eventId == t.EventId)).Select<BestEvent, string>((Func<BestEvent, string>) (t => t.ReasonBannerText.ToLower())).ToArray<string>();
        return new KeyValuePair<bool, string>((uint) array.Length > 0U, BestEventProvider.BuildEventBestsDisplayString(array));
      }
      catch (Exception ex)
      {
        BestEventProvider.Logger.Error((object) ex);
      }
      return new KeyValuePair<bool, string>();
    }

    private static GoalType GetGoalType(EventType type)
    {
      GoalType goalType = GoalType.Unknown;
      switch (type)
      {
        case EventType.Best:
        case EventType.None:
        case EventType.Driving:
        case EventType.Sleeping:
        case EventType.Walking:
        case EventType.Golf:
          return goalType;
        case EventType.Running:
          goalType = GoalType.RunGoal;
          goto case EventType.Best;
        case EventType.Workout:
        case EventType.GuidedWorkout:
          goalType = GoalType.WorkoutGoal;
          goto case EventType.Best;
        case EventType.Biking:
          goalType = GoalType.BikeGoal;
          goto case EventType.Best;
        case EventType.Hike:
          goalType = GoalType.HikeGoal;
          goto case EventType.Best;
        default:
          Debugger.Break();
          goto case EventType.Best;
      }
    }

    private static string BuildEventBestsDisplayString(object[] bestEventTitles)
    {
      string format = string.Empty;
      switch (bestEventTitles.Length)
      {
        case 1:
          format = AppResources.BestEventString1;
          break;
        case 2:
          format = AppResources.BestEventString2;
          break;
        case 3:
          format = AppResources.BestEventString3;
          break;
        case 4:
          format = AppResources.BestEventString4;
          break;
        case 5:
          format = AppResources.BestEventString5;
          break;
        case 6:
          format = AppResources.BestEventString6;
          break;
      }
      return string.Format(format, bestEventTitles);
    }

    public void PopulateAllEvents<T>(
      ICollection<HistoryEventViewModel<T>> items,
      ICollection<BestEvent> bests,
      ICollection<HistoryEventViewModel<T>> allEvents)
      where T : UserEvent
    {
      foreach (HistoryEventViewModel<T> historyEventViewModel in (IEnumerable<HistoryEventViewModel<T>>) items)
      {
        if (historyEventViewModel != null)
        {
          this.CheckForBest((HistoryEventViewModelBase) historyEventViewModel, bests);
          allEvents.Add(historyEventViewModel);
        }
      }
    }

    public void CheckForBest(HistoryEventViewModelBase historyEvent, ICollection<BestEvent> bests)
    {
      if (bests != null)
      {
        foreach (BestEvent best in (IEnumerable<BestEvent>) bests)
        {
          switch (historyEvent.EventType)
          {
            case EventType.Unknown:
            case EventType.Best:
            case EventType.None:
            case EventType.Driving:
            case EventType.Golf:
              continue;
            case EventType.Running:
              if (best.Type.Equals((object) GoalType.RunGoal) && best.EventId == historyEvent.EventId)
              {
                historyEvent.IsBest = true;
                continue;
              }
              continue;
            case EventType.Sleeping:
              if (best.Type.Equals((object) GoalType.SleepGoal) && best.EventId == historyEvent.EventId)
              {
                historyEvent.IsBest = true;
                continue;
              }
              continue;
            case EventType.Workout:
              if (best.Type.Equals((object) GoalType.WorkoutGoal) && best.EventId == historyEvent.EventId)
              {
                historyEvent.IsBest = true;
                continue;
              }
              continue;
            case EventType.Walking:
              if (best.Type.Equals((object) GoalType.StepGoal) && best.EventId == historyEvent.EventId)
              {
                historyEvent.IsBest = true;
                continue;
              }
              continue;
            case EventType.GuidedWorkout:
              if (best.Type.Equals((object) GoalType.WorkoutGoal) && best.EventId == historyEvent.EventId)
              {
                historyEvent.IsBest = true;
                continue;
              }
              continue;
            case EventType.Biking:
              if (best.Type.Equals((object) GoalType.BikeGoal) && best.EventId == historyEvent.EventId)
              {
                historyEvent.IsBest = true;
                continue;
              }
              continue;
            case EventType.Hike:
              if (best.Type.Equals((object) GoalType.HikeGoal) && best.EventId == historyEvent.EventId)
              {
                historyEvent.IsBest = true;
                continue;
              }
              continue;
            default:
              Debugger.Break();
              continue;
          }
        }
      }
      else
        historyEvent.IsBest = false;
    }
  }
}
