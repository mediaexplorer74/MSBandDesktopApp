// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.EditReminderViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Coaching;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  public class EditReminderViewModel : PageViewModelBase
  {
    public const string HabitIdParameter = "EditHabit.HabitId";
    public const string HabitIdDuplicateIndex = "EditHabit.HabitIdDuplicateIndex";
    public const string DayParameter = "EditHabit.Day";
    public const string DayRangeStartParameter = "EditHabit.RangeStart";
    public const string DayRangeEndParameter = "EditHabit.RangeEnd";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Coaching\\EditReminderViewModel.cs");
    private readonly ISmoothNavService smoothNavService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly ICoachingProvider coachingProvider;
    private readonly ICoachingService coachingService;
    private readonly ITileUpdateService tileUpdateService;
    private readonly IMessageBoxService messageBoxService;
    private string habitId;
    private int habitIdDuplicateIndex;
    private DateTimeOffset day;
    private Range<DateTimeOffset> timelineDayRange;
    private string name;
    private DateTimeOffset startTime;
    private DateTimeOffset allowableStart;
    private DateTimeOffset allowableEnd;
    private bool isReminderEnabled;
    private bool displayDetails;
    private bool canEditTargetTime;
    private bool canEditReminder;
    private bool canSkip;
    private TimeSpan selectedTime;
    private string error;
    private bool displaySaveChanges;
    private ICommand saveCommand;
    private ICommand cancelCommand;
    private ICommand skipCommand;

    public EditReminderViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IBandConnectionFactory cargoConnectionFactory,
      IErrorHandlingService errorHandlingService,
      ICoachingProvider coachingProvider,
      ICoachingService coachingService,
      ITileUpdateService tileUpdateService,
      IMessageBoxService messageBoxService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.errorHandlingService = errorHandlingService;
      this.coachingProvider = coachingProvider;
      this.coachingService = coachingService;
      this.tileUpdateService = tileUpdateService;
      this.messageBoxService = messageBoxService;
    }

    public string NameDisplay
    {
      get
      {
        if (this.CanEditTargetTime)
          return this.name;
        return string.Format(AppResources.CoachingEditHabitNameLabel, new object[1]
        {
          (object) this.name
        });
      }
    }

    public string Details
    {
      get
      {
        if (!this.DisplayDetails)
          return string.Empty;
        return string.Format(AppResources.CoachingEditHabitDetailsLabel, new object[2]
        {
          (object) Formatter.FormatTimeWithAMOrPM(this.AllowableStart),
          (object) Formatter.FormatTimeWithAMOrPM(this.AllowableEnd)
        });
      }
    }

    public TimeSpan SelectedTime
    {
      get => this.selectedTime;
      set
      {
        this.SetProperty<TimeSpan>(ref this.selectedTime, value, nameof (SelectedTime));
        if (this.LoadState != LoadState.Loaded)
          return;
        this.ValidateTime();
        this.DisplaySaveChanges = string.IsNullOrEmpty(this.Error);
      }
    }

    public string TargetTimeDisplay => !this.CanEditTargetTime ? Formatter.FormatTimeWithAMOrPM(this.startTime) : string.Empty;

    public DateTimeOffset AllowableStart
    {
      get => this.allowableStart;
      private set => this.SetProperty<DateTimeOffset>(ref this.allowableStart, value, nameof (AllowableStart));
    }

    public DateTimeOffset AllowableEnd
    {
      get => this.allowableEnd;
      private set => this.SetProperty<DateTimeOffset>(ref this.allowableEnd, value, nameof (AllowableEnd));
    }

    public bool IsReminderEnabled
    {
      get => this.isReminderEnabled;
      set
      {
        this.SetProperty<bool>(ref this.isReminderEnabled, value, nameof (IsReminderEnabled));
        if (this.LoadState != LoadState.Loaded)
          return;
        this.DisplaySaveChanges = true;
      }
    }

    public bool DisplayDetails
    {
      get => this.displayDetails;
      private set
      {
        this.SetProperty<bool>(ref this.displayDetails, value, nameof (DisplayDetails));
        this.RaisePropertyChanged("Details");
      }
    }

    public bool CanEditTargetTime
    {
      get => this.canEditTargetTime;
      private set
      {
        this.SetProperty<bool>(ref this.canEditTargetTime, value, nameof (CanEditTargetTime));
        this.RaisePropertyChanged("NameDisplay");
        this.RaisePropertyChanged("TargetTimeDisplay");
      }
    }

    public bool CanEditReminder
    {
      get => this.canEditReminder;
      private set => this.SetProperty<bool>(ref this.canEditReminder, value, nameof (CanEditReminder));
    }

    public bool CanSkip
    {
      get => this.canSkip;
      private set => this.SetProperty<bool>(ref this.canSkip, value, nameof (CanSkip));
    }

    public string Error
    {
      get => this.error;
      private set => this.SetProperty<string>(ref this.error, value, nameof (Error));
    }

    public bool DisplaySaveChanges
    {
      get => this.displaySaveChanges;
      private set
      {
        this.SetProperty<bool>(ref this.displaySaveChanges, value, nameof (DisplaySaveChanges));
        this.UpdateAppBar();
      }
    }

    public ICommand CancelCommand => this.cancelCommand ?? (this.cancelCommand = (ICommand) new HealthCommand(new Action(this.GoBack)));

    public ICommand SaveCommand => this.saveCommand ?? (this.saveCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      if (!string.IsNullOrEmpty(this.Error) || this.LoadState == LoadState.Loading)
        return;
      this.LoadState = LoadState.Loading;
      this.DisplaySaveChanges = false;
      try
      {
        await this.SaveChangesAsync((Func<CancellationToken, Task>) (token => this.coachingProvider.SaveReminderAsync(this.habitId, this.habitIdDuplicateIndex, this.day, this.startTime, this.IsReminderEnabled, this.timelineDayRange, token)));
        this.coachingService.ComingUpPendingRefresh = true;
        this.coachingService.TilePendingRefresh = true;
        this.GoBack();
      }
      catch (Exception ex)
      {
        this.LoadState = LoadState.Loaded;
        EditReminderViewModel.Logger.Error((object) "Failed to save changes to reminder.", ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.DisplaySaveChanges = true;
      }
    })));

    public ICommand SkipCommand => this.skipCommand ?? (this.skipCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      if (await this.messageBoxService.ShowAsync(AppResources.CoachingEditHabitSkipBody, (string) null, PortableMessageBoxButton.CasualBooleanChoice) != PortableMessageBoxResult.OK || this.LoadState == LoadState.Loading)
        return;
      this.LoadState = LoadState.Loading;
      try
      {
        await this.SaveChangesAsync((Func<CancellationToken, Task>) (token => this.coachingProvider.SkipReminderAsync(this.habitId, this.habitIdDuplicateIndex, this.day, this.timelineDayRange, token)));
        this.coachingService.ComingUpPendingRefresh = true;
        this.coachingService.TilePendingRefresh = true;
        this.GoBack();
      }
      catch (Exception ex)
      {
        this.LoadState = LoadState.Loaded;
        EditReminderViewModel.Logger.Error((object) "Failed to skip reminder.", ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    })));

    private async Task SaveChangesAsync(Func<CancellationToken, Task> cloudFunc)
    {
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
          await cargoConnection.CheckConnectionWorkingAsync(cancellationTokenSource.Token);
        await cloudFunc(cancellationTokenSource.Token);
        try
        {
          await this.tileUpdateService.UpdateCalendarAsync(cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
          EditReminderViewModel.Logger.Error((object) "Failed to save changes to reminder to band.", ex);
        }
      }
    }

    private void GoBack() => this.smoothNavService.GoBack();

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.habitId = this.GetStringParameter("EditHabit.HabitId");
      this.habitIdDuplicateIndex = this.GetIntParameter("EditHabit.HabitIdDuplicateIndex");
      this.day = DateTimeOffset.Parse(this.GetStringParameter("EditHabit.Day"));
      string stringParameter1 = this.GetStringParameter("EditHabit.RangeStart");
      string stringParameter2 = this.GetStringParameter("EditHabit.RangeEnd");
      this.timelineDayRange = new Range<DateTimeOffset>()
      {
        Low = DateTimeOffset.Parse(stringParameter1),
        High = DateTimeOffset.Parse(stringParameter2)
      };
      Reminder reminderAsync = await this.coachingProvider.GetReminderAsync(this.habitId, this.habitIdDuplicateIndex, this.day, this.timelineDayRange, CancellationToken.None);
      this.name = reminderAsync != null ? reminderAsync.HabitName : throw new NoDataException();
      this.startTime = reminderAsync.TargetTime;
      this.SelectedTime = this.startTime.TimeOfDay;
      if (reminderAsync.HasRestrictedSchedule && reminderAsync.AllowableStart.HasValue && reminderAsync.AllowableEnd.HasValue)
      {
        if (reminderAsync.AllowableStart.Value == reminderAsync.TargetTime && reminderAsync.AllowableEnd.Value == reminderAsync.TargetTime)
        {
          this.DisplayDetails = false;
          this.CanEditTargetTime = false;
        }
        else
        {
          this.AllowableStart = reminderAsync.AllowableStart.Value;
          this.AllowableEnd = reminderAsync.AllowableEnd.Value;
          this.DisplayDetails = true;
          this.CanEditTargetTime = true;
        }
      }
      else
      {
        DateTimeOffset dateTimeOffset = this.startTime.RoundDown(TimeSpan.FromDays(1.0));
        this.AllowableStart = dateTimeOffset;
        this.AllowableEnd = dateTimeOffset.AddDays(1.0).AddSeconds(-1.0);
        this.DisplayDetails = false;
        this.CanEditTargetTime = true;
      }
      this.IsReminderEnabled = reminderAsync.Enabled;
      this.CanEditReminder = reminderAsync.CanEditReminder;
      this.CanSkip = reminderAsync.CanSkip;
      if (this.CanEditTargetTime)
        this.ValidateTime();
      this.DisplaySaveChanges = false;
    }

    private void ValidateTime()
    {
      try
      {
        DateTimeOffset dateTimeOffset = default;
        ref DateTimeOffset local = ref dateTimeOffset;
        int year = this.startTime.Year;
        int month = this.startTime.Month;
        int day = this.startTime.Day;
        TimeSpan selectedTime = this.SelectedTime;
        int hours = selectedTime.Hours;
        selectedTime = this.SelectedTime;
        int minutes = selectedTime.Minutes;
        selectedTime = this.SelectedTime;
        int seconds = selectedTime.Seconds;
        selectedTime = this.SelectedTime;
        int milliseconds = selectedTime.Milliseconds;
        TimeSpan offset = this.startTime.Offset;
        local = new DateTimeOffset(year, month, day, hours, minutes, seconds, milliseconds, offset);
        if (dateTimeOffset < this.AllowableStart || dateTimeOffset > this.AllowableEnd)
        {
          this.SetError();
        }
        else
        {
          this.Error = string.Empty;
          this.startTime = dateTimeOffset;
        }
      }
      catch (Exception ex)
      {
        EditReminderViewModel.Logger.Error(ex, "Failed to create a valid DateTimeOffset. {0}/{1}/{2} {3}:{4}:{5}.{6} {7}", (object) this.startTime.Year, (object) this.startTime.Month, (object) this.startTime.Day, (object) this.SelectedTime.Hours, (object) this.SelectedTime.Minutes, (object) this.SelectedTime.Seconds, (object) this.SelectedTime.Milliseconds, (object) this.startTime.Offset);
        this.errorHandlingService.HandleExceptionAsync(ex);
        this.SetError();
      }
    }

    private void SetError() => this.Error = string.Format(AppResources.CoachingEditHabitTimePickerErrorLabel, new object[2]
    {
      (object) Formatter.FormatTimeWithAMOrPM(this.AllowableStart),
      (object) Formatter.FormatTimeWithAMOrPM(this.AllowableEnd)
    });

    private void UpdateAppBar()
    {
      if (this.DisplaySaveChanges)
        this.ShowAppBar(true);
      else
        this.ShowAppBar(false);
    }

    private void ShowAppBar(bool includeSave)
    {
      List<AppBarButton> appBarButtonList = new List<AppBarButton>();
      if (includeSave)
        appBarButtonList.Add(new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.SaveCommand));
      appBarButtonList.Add(new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand));
      this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar((IEnumerable<AppBarButton>) appBarButtonList);
    }
  }
}
