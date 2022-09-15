// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.DesignEditReminderViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  public class DesignEditReminderViewModel
  {
    private readonly DateTimeOffset currentTime = DateTimeOffset.Now.RoundDown(TimeSpan.FromMinutes(15.0));

    public string NameDisplay => "Bed time";

    public string Details => string.Format(AppResources.CoachingEditHabitDetailsLabel, new object[2]
    {
      (object) Formatter.FormatTimeWithAMOrPM(this.AllowableStart),
      (object) Formatter.FormatTimeWithAMOrPM(this.AllowableEnd)
    });

    public TimeSpan SelectedTime => this.currentTime.TimeOfDay + TimeSpan.FromHours(1.5);

    public string TargetTimeDisplay => Formatter.FormatTimeWithAMOrPM(this.currentTime);

    public DateTimeOffset AllowableStart => this.currentTime;

    public DateTimeOffset AllowableEnd => this.currentTime + TimeSpan.FromHours(1.0);

    public bool IsReminderEnabled => true;

    public bool DisplayDetails => true;

    public bool CanEditTargetTime => true;

    public bool CanEditReminder => true;

    public bool CanSkip => true;

    public string Error => string.Format(AppResources.CoachingEditHabitTimePickerErrorLabel, new object[2]
    {
      (object) Formatter.FormatTimeWithAMOrPM(this.AllowableStart),
      (object) Formatter.FormatTimeWithAMOrPM(this.AllowableEnd)
    });

    public ICommand SkipCommand => (ICommand) null;
  }
}
