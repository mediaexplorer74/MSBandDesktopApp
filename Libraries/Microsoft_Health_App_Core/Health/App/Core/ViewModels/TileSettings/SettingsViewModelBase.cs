// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.SettingsViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Band.Admin.WebTiles;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public abstract class SettingsViewModelBase : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\TileSettings\\SettingsViewModelBase.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly ISmoothNavService navService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private HealthCommand editCommand;
    private HealthCommand backCommand;
    private string header;
    private string subheader;
    private bool enableNotifications;
    private bool hasNotifications;
    private bool showProgress;

    protected SettingsViewModelBase(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService errorHandlingService,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService)
    {
      this.TileManagementService = tileManagementService;
      this.errorHandlingService = errorHandlingService;
      this.navService = navService;
      this.cargoConnectionFactory = cargoConnectionFactory;
    }

    protected bool SuppressNotificationChange { get; set; }

    public abstract string TileGuid { get; }

    protected ITileManagementService TileManagementService { get; }

    public virtual bool CanEdit => false;

    public bool ShowProgress
    {
      get => this.showProgress;
      set => this.SetProperty<bool>(ref this.showProgress, value, nameof (ShowProgress));
    }

    public string Header
    {
      get => this.header;
      set => this.SetProperty<string>(ref this.header, value, nameof (Header));
    }

    public string Subheader
    {
      get => this.subheader;
      set => this.SetProperty<string>(ref this.subheader, value, nameof (Subheader));
    }

    public bool HasNotifications
    {
      get => this.hasNotifications;
      set => this.SetProperty<bool>(ref this.hasNotifications, value, nameof (HasNotifications));
    }

    public bool EnableNotifications
    {
      get => this.enableNotifications;
      set
      {
        this.SetProperty<bool>(ref this.enableNotifications, value, nameof (EnableNotifications));
        this.RaisePropertyChanged("SwitchText");
        if (this.SuppressNotificationChange)
          return;
        this.ToggleTile();
      }
    }

    public string SwitchText => !this.EnableNotifications ? AppResources.Off : AppResources.On;

    public ICommand EditCommand => (ICommand) this.editCommand ?? (ICommand) (this.editCommand = new HealthCommand(new Action(this.OnEdit)));

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.navService.GoBack())));

    protected virtual Task LoadPendingSettingsAsync() => (Task) Task.FromResult<bool>(true);

    protected abstract Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null);

    protected override sealed async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.SuppressNotificationChange = true;
      this.ShowProgress = true;
      try
      {
        await this.LoadPendingSettingsAsync();
        await this.LoadSettingsDataAsync(parameters);
        Guid tileGuidStruct = Guid.Parse(this.TileGuid);
        AppBandTile appBandTile = this.TileManagementService.KnownTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (s => s.TileId == tileGuidStruct));
        if (appBandTile != (AppBandTile) null && appBandTile.SettingsPageTaxonomy != null)
          ApplicationTelemetry.LogPageView(appBandTile.SettingsPageTaxonomy);
      }
      finally
      {
        this.SuppressNotificationChange = false;
        this.ShowProgress = false;
      }
    }

    protected virtual void OnEdit()
    {
    }

    private async void ToggleTile()
    {
      this.SuppressNotificationChange = true;
      await this.SetNotificationsSettingsAsync();
      this.SuppressNotificationChange = false;
    }

    private async Task SetNotificationsSettingsAsync()
    {
      try
      {
        SettingsViewModelBase.Logger.Debug("Setting notification settings (EnableNotifications = {0})", (object) this.EnableNotifications);
        string tileGuid1 = this.TileGuid;
        Guid tileGuid = Guid.Parse(this.TileGuid);
        AppBandTile tile = this.TileManagementService.PendingTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (item => item.TileId == tileGuid)).First<AppBandTile>();
        if (tile.TileId.ToString().Equals("4076b009-0455-4af7-a705-6d4acd45a556"))
          ApplicationTelemetry.LogNotificationsToggle();
        if (tile.IsWebTile)
        {
          this.SaveWebTileNotificationSetting(tile, this.EnableNotifications);
          tile.IsWebTileNotificationEnabled = this.enableNotifications;
        }
        else
        {
          tile.Settings = !this.EnableNotifications ? tile.DefaultOffSettings : tile.DefaultOnSettings;
          if (!this.TileManagementService.PendingNotifications.Contains(tileGuid1))
            this.TileManagementService.PendingNotifications.Add(tileGuid1);
        }
      }
      catch (Exception ex)
      {
        SettingsViewModelBase.Logger.Error(ex, "Error when setting notification settings");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.EnableNotifications = !this.EnableNotifications;
      }
    }

    private async void SaveWebTileNotificationSetting(AppBandTile tile, bool enabled)
    {
      IWebTile webTileById = this.TileManagementService.GetWebTileById(tile.TileId);
      if (webTileById == null || !tile.IsThirdParty || !webTileById.HasNotifications)
        return;
      webTileById.NotificationEnabled = this.EnableNotifications;
      await webTileById.SetNotificationEnabledAsync(enabled);
    }

    protected async Task RefreshNotificationToggleAsync()
    {
      if (!this.HasNotifications)
        return;
      AdminTileSettings settings = AdminTileSettings.None;
      string tileGuid1 = this.TileGuid;
      Guid tileGuid = Guid.Parse(this.TileGuid);
      AppBandTile tile = this.TileManagementService.PendingTiles.First<AppBandTile>((Func<AppBandTile, bool>) (item => item.TileId == tileGuid));
      try
      {
        if (this.TileManagementService.PendingNotifications.Contains(tileGuid1))
        {
          settings = tile.Settings;
        }
        else
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
            settings = await cargoConnection.GetTileSettingsAsync(tileGuid);
        }
      }
      catch (Exception ex)
      {
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.navService.GoBack();
      }
      this.EnableNotifications = !tile.IsWebTile ? settings == tile.DefaultOnSettings : tile.IsWebTileNotificationEnabled;
      tile = (AppBandTile) null;
    }
  }
}
