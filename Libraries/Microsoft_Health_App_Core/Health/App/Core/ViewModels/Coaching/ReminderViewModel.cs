// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.ReminderViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Coaching;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  public class ReminderViewModel : HealthViewModelBase
  {
    private HealthCommand detailsCommand;
    private string guidedWorkoutPlanId;

    public static ReminderViewModel FromProviderModel(Reminder reminder) => new ReminderViewModel()
    {
      Name = reminder.HabitName,
      HabitId = reminder.HabitId,
      Time = Formatter.FormatTime(reminder.TargetTime),
      GuidedWorkoutPlanId = reminder.GuidedWorkoutPlanId,
      Day = reminder.TargetTime.RoundDown(TimeSpan.FromDays(1.0))
    };

    public string Name { get; set; }

    public string Time { get; set; }

    public string HabitId { get; set; }

    public int HabitIdDuplicateIndex { get; set; }

    public DateTimeOffset Day { get; set; }

    public bool ShowDetailsButton => !string.IsNullOrEmpty(this.GuidedWorkoutPlanId);

    public string GuidedWorkoutPlanId
    {
      get => this.guidedWorkoutPlanId;
      set
      {
        if (!this.SetProperty<string>(ref this.guidedWorkoutPlanId, value, nameof (GuidedWorkoutPlanId)))
          return;
        this.RaisePropertyChanged<bool>((Expression<Func<bool>>) (() => this.ShowDetailsButton));
      }
    }

    public ICommand DetailsCommand
    {
      get
      {
        ISmoothNavService navService = ServiceLocator.Current.GetInstance<ISmoothNavService>();
        return (ICommand) this.detailsCommand ?? (ICommand) (this.detailsCommand = new HealthCommand((Action) (() => navService.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          ["WorkoutPlanId"] = this.GuidedWorkoutPlanId,
          ["Type"] = "WorkoutPlanDetail"
        }))));
      }
    }
  }
}
