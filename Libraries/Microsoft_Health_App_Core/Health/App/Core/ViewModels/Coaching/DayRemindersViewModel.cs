// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.DayRemindersViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Cirrious.MvvmCross.ViewModels;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  public class DayRemindersViewModel : MvxNotifyPropertyChanged
  {
    private readonly CoachingComingUpViewModel parent;
    private IList<ReminderViewModel> reminders;
    private HealthCommand<ReminderViewModel> openReminderCommand;

    public DayRemindersViewModel(
      CoachingComingUpViewModel parent,
      string headerText,
      string noReminderText)
    {
      Assert.ParamIsNotNull((object) parent, nameof (parent));
      Assert.ParamIsNotNullOrEmpty(headerText, nameof (headerText));
      Assert.ParamIsNotNullOrEmpty(noReminderText, nameof (noReminderText));
      this.parent = parent;
      this.HeaderText = headerText;
      this.NoReminderText = noReminderText;
      ObservableCollection<ReminderViewModel> observableCollection = new ObservableCollection<ReminderViewModel>();
      observableCollection.CollectionChanged += (NotifyCollectionChangedEventHandler) ((sender, e) => this.RaisePropertyChanged(nameof (ShowReminders)));
      this.reminders = (IList<ReminderViewModel>) observableCollection;
    }

    public string HeaderText { get; }

    public string NoReminderText { get; }

    public IList<ReminderViewModel> Reminders => this.reminders;

    public bool ShowReminders => (uint) this.Reminders.Count > 0U;

    public ICommand OpenReminderCommand => (ICommand) this.openReminderCommand ?? (ICommand) (this.openReminderCommand = new HealthCommand<ReminderViewModel>((Action<ReminderViewModel>) (reminderViewModel => this.parent.OpenReminder(reminderViewModel))));

    public void AddReminder(ReminderViewModel reminder)
    {
      Assert.ParamIsNotNull((object) reminder, nameof (reminder));
      this.Reminders.Add(reminder);
    }
  }
}
