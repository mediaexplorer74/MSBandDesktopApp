// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.NotificationCenterSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.TileSettings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class NotificationCenterSettingsViewModel : 
    SettingsViewModelBase<NotificationCenterPendingTileSettings>
  {
    private readonly IAppListService appListService;
    private readonly ObservableCollection<AppNotificationViewModel> apps;
    private IList<AppNotificationViewModel> allApps;
    private CancellationTokenSource delaySearchTokenSource;
    private string search;
    private bool suppressSavingSettings;
    private HealthCommand selectAllCommand;
    private HealthCommand deselectAllCommand;

    public NotificationCenterSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService errorHandlingService,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory,
      IAppListService appListService)
      : base(networkService, tileManagementService, errorHandlingService, navService, cargoConnectionFactory)
    {
      this.appListService = appListService;
      this.apps = new ObservableCollection<AppNotificationViewModel>();
    }

    public IList<AppNotificationViewModel> Apps => (IList<AppNotificationViewModel>) this.apps;

    public string Search
    {
      get => this.search;
      set
      {
        this.SetProperty<string>(ref this.search, value, nameof (Search));
        this.delaySearchTokenSource?.Cancel();
        this.delaySearchTokenSource = new CancellationTokenSource();
        this.DelaySearchApps(this.delaySearchTokenSource.Token);
      }
    }

    public bool ShowSelectButtons => string.IsNullOrEmpty(this.Search);

    public ICommand SelectAllCommand => (ICommand) this.selectAllCommand ?? (ICommand) (this.selectAllCommand = new HealthCommand((Action) (() => this.SetAll(true))));

    public ICommand DeselectAllCommand => (ICommand) this.deselectAllCommand ?? (ICommand) (this.deselectAllCommand = new HealthCommand((Action) (() => this.SetAll(false))));

    private void SetAll(bool enabled)
    {
      this.suppressSavingSettings = true;
      foreach (AppNotificationViewModel app in (IEnumerable<AppNotificationViewModel>) this.Apps)
        app.Enabled = enabled;
      this.suppressSavingSettings = false;
      this.SaveToPendingSettings();
    }

    public void SaveToPendingSettings()
    {
      if (this.suppressSavingSettings)
        return;
      this.PendingTileSettings.EnabledAppIds = (IList<string>) this.allApps.Where<AppNotificationViewModel>((Func<AppNotificationViewModel, bool>) (app => app.Enabled)).Select<AppNotificationViewModel, string>((Func<AppNotificationViewModel, string>) (app => app.Id)).ToList<string>();
      this.PendingTileSettings.DisabledAppIds = (IList<string>) this.allApps.Where<AppNotificationViewModel>((Func<AppNotificationViewModel, bool>) (app => !app.Enabled)).Select<AppNotificationViewModel, string>((Func<AppNotificationViewModel, string>) (app => app.Id)).ToList<string>();
    }

    public override string TileGuid => "4076b009-0455-4af7-a705-6d4acd45a556";

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      this.allApps = (IList<AppNotificationViewModel>) (await this.appListService.GetAppsAsync()).Select<AppInfo, AppNotificationViewModel>((Func<AppInfo, AppNotificationViewModel>) (app => new AppNotificationViewModel(this, app.Id, app.DisplayName, !this.PendingTileSettings.DisabledAppIds.Contains(app.Id)))).OrderBy<AppNotificationViewModel, string>((Func<AppNotificationViewModel, string>) (app => app.DisplayName)).ToList<AppNotificationViewModel>();
      this.PopulateAppsListFromMaster();
    }

    private async void DelaySearchApps(CancellationToken token)
    {
      try
      {
        await Task.Delay(250, token);
        if (this.delaySearchTokenSource == null)
          return;
        this.PopulateAppsListFromMaster();
        this.RaisePropertyChanged("ShowSelectButtons");
        this.delaySearchTokenSource.Dispose();
        this.delaySearchTokenSource = (CancellationTokenSource) null;
      }
      catch (OperationCanceledException ex)
      {
      }
    }

    private void PopulateAppsListFromMaster()
    {
      this.Apps.Clear();
      foreach (AppNotificationViewModel allApp in (IEnumerable<AppNotificationViewModel>) this.allApps)
      {
        if (string.IsNullOrEmpty(this.Search) || allApp.DisplayName.IndexOf(this.Search, StringComparison.CurrentCultureIgnoreCase) >= 0)
          this.Apps.Add(allApp);
      }
    }
  }
}
