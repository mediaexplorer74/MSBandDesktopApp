// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.History.HistoryEventViewModel`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Events.Golf;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Health.App.Core.ViewModels.History
{
  public class HistoryEventViewModel<TModel> : HistoryEventViewModelBase where TModel : UserEvent
  {
    private readonly IFormattingService formattingService;
    private string name;
    private TModel eventModel;

    public HistoryEventViewModel(TModel model, IFormattingService formattingService)
      : base((UserEvent) model)
    {
      Assert.ParamIsNotNull((object) model, nameof (model));
      Assert.ParamIsNotNull((object) formattingService, nameof (formattingService));
      this.eventModel = model;
      this.formattingService = formattingService;
    }

    public TModel Event => this.eventModel;

    public IList<ExerciseTag> Tags { get; set; }

    public virtual string Title
    {
      get
      {
        switch (this.EventType)
        {
          case EventType.Sleeping:
            if (!(this.eventModel is SleepEvent eventModel4) || !eventModel4.IsAutoDetected)
              return this.Name;
            return string.Format(AppResources.HistoryItem_Name_AutoDetectedSleep_FormatString, new object[1]
            {
              (object) this.Name
            });
          case EventType.Workout:
            IList<ExerciseTag> exerciseTagList = this.eventModel is ExerciseEvent eventModel5 ? eventModel5.Tags : (IList<ExerciseTag>) null;
            return string.IsNullOrEmpty(this.Name) && exerciseTagList != null && exerciseTagList.Count > 0 && !string.IsNullOrEmpty(exerciseTagList[0].Text) ? exerciseTagList[0].Text : this.Name;
          case EventType.Golf:
            return this.eventModel is GolfEvent eventModel6 && string.IsNullOrEmpty(this.Name) ? eventModel6.CourseName : this.Name;
          default:
            return this.Name;
        }
      }
    }

    public string Name
    {
      get
      {
        if (this.name == null)
        {
          if (!string.IsNullOrWhiteSpace(this.eventModel.Name))
          {
            this.name = this.eventModel.Name;
          }
          else
          {
            switch (this.EventType)
            {
              case EventType.Unknown:
              case EventType.None:
              case EventType.Driving:
              case EventType.Walking:
                this.name = string.Empty;
                break;
              case EventType.Best:
                this.name = AppResources.EventTypeNameBest;
                break;
              case EventType.Running:
                this.name = AppResources.EventTypeNameRunning;
                break;
              case EventType.Sleeping:
                this.name = AppResources.EventTypeNameSleeping;
                break;
              case EventType.Workout:
                IList<ExerciseTag> exerciseTagList = this.eventModel is ExerciseEvent eventModel7 ? eventModel7.Tags : (IList<ExerciseTag>) null;
                this.name = exerciseTagList == null || exerciseTagList.Count <= 0 || string.IsNullOrEmpty(exerciseTagList[0].Text) ? AppResources.EventTypeNameWorkout2 : exerciseTagList[0].Text;
                break;
              case EventType.GuidedWorkout:
                this.name = AppResources.EventTypeNameGuidedWorkout;
                break;
              case EventType.Biking:
                this.name = AppResources.EventTypeNameBiking;
                break;
              case EventType.Hike:
                this.name = AppResources.EventTypeNameHike;
                break;
              case EventType.Golf:
                string str = this.eventModel is GolfEvent eventModel8 ? eventModel8.CourseName : (string) null;
                this.name = !string.IsNullOrWhiteSpace(str) ? str : AppResources.EventTypeNameGolf;
                break;
              default:
                Debugger.Break();
                break;
            }
          }
        }
        return this.name;
      }
    }

    public string FormattedDate => this.eventModel.EventType == EventType.Sleeping ? Formatter.FormatSleepTime(this.eventModel.StartTime.ToLocalTime()) : Formatter.GetMonthDayString(this.eventModel.StartTime);

    public virtual StyledSpan Value
    {
      get
      {
        switch (this.eventModel.EventType)
        {
          case EventType.Running:
            return this.formattingService.FormatDistance(((object) this.eventModel as RunEvent)?.TotalDistance, appendUnit: true);
          case EventType.Sleeping:
            return this.eventModel is SleepEvent eventModel3 ? Formatter.FormatTimeSpan(eventModel3.SleepTime, Formatter.TimeSpanFormat.OneChar, false) : Formatter.FormatValueStyledSpan(string.Empty);
          case EventType.Workout:
          case EventType.GuidedWorkout:
            return Formatter.FormatCalories(new int?(this.eventModel.CaloriesBurned), true);
          case EventType.Biking:
            if (!(this.eventModel is BikeEvent eventModel4))
              return Formatter.FormatValueStyledSpan(string.Empty);
            Length totalDistance = eventModel4.TotalDistance;
            return !(totalDistance == Length.Zero) ? this.formattingService.FormatDistance(new Length?(totalDistance), appendUnit: true) : Formatter.FormatCalories(new int?(this.eventModel.CaloriesBurned), true);
          case EventType.Hike:
            return this.formattingService.FormatDistance(((object) this.eventModel as HikeEvent)?.TotalDistance, appendUnit: true);
          case EventType.Golf:
            return (object) this.eventModel is GolfEvent ? Formatter.FormatValueStyledSpan(((object) this.eventModel as GolfEvent).TotalScore.ToString()) : Formatter.FormatValueStyledSpan(string.Empty);
          default:
            return Formatter.FormatValueStyledSpan(string.Empty);
        }
      }
    }
  }
}
