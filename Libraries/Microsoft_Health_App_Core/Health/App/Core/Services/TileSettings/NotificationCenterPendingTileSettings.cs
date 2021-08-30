// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.NotificationCenterPendingTileSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.TileSettings
{
  public class NotificationCenterPendingTileSettings : PendingTileSettings
  {
    private readonly INotificationCenterConfigurationService notificationCenterConfigurationService;
    private readonly ITileManagementService tileManagementService;
    private IList<string> enabledAppIds;
    private IList<string> disabledAppIds;

    public NotificationCenterPendingTileSettings(
      INotificationCenterConfigurationService notificationCenterConfigurationService,
      ITileManagementService tileManagementService)
    {
      this.notificationCenterConfigurationService = notificationCenterConfigurationService;
      this.tileManagementService = tileManagementService;
    }

    public IList<string> EnabledAppIds
    {
      get => this.enabledAppIds;
      set
      {
        this.enabledAppIds = value;
        this.IsChanged = true;
      }
    }

    public IList<string> DisabledAppIds
    {
      get => this.disabledAppIds;
      set
      {
        this.disabledAppIds = value;
        this.IsChanged = true;
      }
    }

    public override async Task LoadSettingsAsync(CancellationToken token)
    {
      NotificationCenterConfiguration centerConfiguration = await this.notificationCenterConfigurationService.GetConfigurationAsync(token).ConfigureAwait(false);
      this.enabledAppIds = centerConfiguration.EnabledApps;
      this.disabledAppIds = centerConfiguration.DisabledApps;
    }

    public override async Task ApplyChangesAsync()
    {
      await this.notificationCenterConfigurationService.SetAppsAsync(this.EnabledAppIds, this.DisabledAppIds).ConfigureAwait(false);
      await this.tileManagementService.ConfigureTileUpdateAsync(this.tileManagementService.KnownTiles.FeedTile, true).ConfigureAwait(false);
    }
  }
}
