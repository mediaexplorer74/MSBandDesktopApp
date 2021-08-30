// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugSettingsSyncHistoryPageViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  internal class DebugSettingsSyncHistoryPageViewModel : DebugSettingsPageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Debug\\DebugSettingsSyncHistoryPageViewModel.cs");
    private readonly ICalendarService calendarService;
    private readonly IDispatchService dispatchService;
    private readonly IDebugReporterService debugReporterService;
    private readonly IConfig config;
    private string calendarEvents;
    private IList<DebugReporterEntry> syncHistory;
    private HealthCommand refreshCommand;
    private string timestamp;

    public bool IsForegroundSyncOnAppStartupEnabled
    {
      get => this.config.IsForegroundSyncOnAppStartupEnabled;
      set => this.config.IsForegroundSyncOnAppStartupEnabled = value;
    }

    public DebugSettingsSyncHistoryPageViewModel(
      IConfig config,
      INetworkService networkService,
      IDispatchService dispatchService,
      ICalendarService calendarService,
      IDebugReporterService debugReporterService)
      : base(networkService)
    {
      this.config = config;
      this.dispatchService = dispatchService;
      this.calendarService = calendarService;
      this.debugReporterService = debugReporterService;
      this.Header = "Sync History";
      this.SubHeader = "Sync history";
      this.GlyphIcon = "\uE161";
    }

    public string LastSentCalendarEvents
    {
      get => this.calendarEvents;
      set => this.SetProperty<string>(ref this.calendarEvents, value, nameof (LastSentCalendarEvents));
    }

    public string LastSentCalendarEventsTimestamp
    {
      get => this.timestamp;
      set => this.SetProperty<string>(ref this.timestamp, value, nameof (LastSentCalendarEventsTimestamp));
    }

    public IList<DebugReporterEntry> SyncHistory
    {
      get
      {
        this.syncHistory = this.syncHistory ?? (IList<DebugReporterEntry>) new ObservableCollection<DebugReporterEntry>();
        return this.syncHistory;
      }
      set => this.SetProperty<IList<DebugReporterEntry>>(ref this.syncHistory, value, nameof (SyncHistory));
    }

    public new ICommand RefreshCommand => (ICommand) this.refreshCommand ?? (ICommand) (this.refreshCommand = new HealthCommand(new Action(this.RefreshPanel)));

    private async void RefreshPanel() => await this.RefreshPanelAsync();

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null) => await this.RefreshPanelAsync();

    private async Task RefreshPanelAsync()
    {
      if (ServiceLocator.Current.GetInstance<IEnvironmentService>().IsPublicRelease)
        return;
      LastSentCalendarEventsState calendarEventsAsync = await this.calendarService.GetLastCalendarEventsAsync();
      if (calendarEventsAsync != null)
      {
        string timestamp = calendarEventsAsync.Timestamp;
        string calendarEvents = calendarEventsAsync.SerializedCalendarEvents;
        await this.dispatchService.RunOnUIThreadAsync((Action) (() =>
        {
          this.LastSentCalendarEventsTimestamp = timestamp;
          this.LastSentCalendarEvents = calendarEvents;
        }));
      }
      this.SyncHistory.Clear();
      foreach (DebugReporterEntry debugReporterEntry in (IEnumerable<DebugReporterEntry>) this.debugReporterService.GetReport())
        this.SyncHistory.Add(debugReporterEntry);
    }
  }
}
