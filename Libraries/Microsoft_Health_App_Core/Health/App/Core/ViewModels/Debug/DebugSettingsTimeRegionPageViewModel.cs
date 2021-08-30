// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugSettingsTimeRegionPageViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class DebugSettingsTimeRegionPageViewModel : DebugSettingsPageViewModelBase
  {
    private readonly IDateTimeService dateTimeService;

    public IReadOnlyCollection<string> AvailableTimeZones => this.dateTimeService.GetTimeZoneIds();

    public DateTimeOffset DebugNow => this.dateTimeService.Now;

    public DateTimeOffset DebugStartDate
    {
      get => this.dateTimeService.DebugStartDate;
      set
      {
        this.dateTimeService.DebugStartDate = value;
        this.RaisePropertyChanged("DebugNow");
        this.RaisePropertyChanged("DebugStartDateString");
      }
    }

    public string DebugStartDateString => Formatter.FormatShortDate(this.DebugStartDate);

    public TimeSpan DebugStartTime
    {
      get => this.dateTimeService.DebugStartTime;
      set
      {
        this.dateTimeService.DebugStartTime = value;
        this.RaisePropertyChanged("DebugNow");
        this.RaisePropertyChanged("DebugStartTimeString");
      }
    }

    public string DebugStartTimeString
    {
      get
      {
        TimeSpan debugStartTime = this.DebugStartTime;
        int hours = debugStartTime.Hours;
        debugStartTime = this.DebugStartTime;
        int minutes = debugStartTime.Minutes;
        debugStartTime = this.DebugStartTime;
        int seconds = debugStartTime.Seconds;
        return Formatter.FormatTimeWithAMOrPM(new DateTimeOffset(new DateTime(1, 1, 1, hours, minutes, seconds)));
      }
    }

    public string DebugTimeZone
    {
      get => this.dateTimeService.DebugTimeZone.Id;
      set
      {
        this.dateTimeService.DebugTimeZone = this.dateTimeService.TimeZoneWithId(value);
        this.RaisePropertyChanged("DebugNow");
      }
    }

    public ICommand RestartLoggerCommand { get; private set; }

    public ICommand ResetDebugTimePassedCounterCommand { get; private set; }

    public bool IsDebugSelectedDateTimeEnabled
    {
      get => this.dateTimeService.IsDebugSelectedDateTimeEnabled;
      set
      {
        this.dateTimeService.IsDebugSelectedDateTimeEnabled = value;
        this.RaisePropertyChanged(nameof (IsDebugSelectedDateTimeEnabled));
      }
    }

    public DebugSettingsTimeRegionPageViewModel(
      INetworkService networkService,
      IDateTimeService dateTimeService)
      : base(networkService)
    {
      this.dateTimeService = dateTimeService;
      this.Header = "Time & Region";
      this.SubHeader = "Override local time, region";
      this.GlyphIcon = "\uE024";
      this.ResetDebugTimePassedCounterCommand = (ICommand) new HealthCommand((Action) (() =>
      {
        this.dateTimeService.RestartDebugTimePassedCounter();
        this.RaisePropertyChanged(nameof (DebugNow));
      }));
    }
  }
}
