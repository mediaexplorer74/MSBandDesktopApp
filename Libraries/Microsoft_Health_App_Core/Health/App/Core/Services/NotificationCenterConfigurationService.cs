// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.NotificationCenterConfigurationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Storage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class NotificationCenterConfigurationService : INotificationCenterConfigurationService
  {
    private readonly IFileObjectStorageService fileObjectStorageService;

    public NotificationCenterConfigurationService(IFileObjectStorageService fileObjectStorageService) => this.fileObjectStorageService = fileObjectStorageService;

    public async Task<NotificationCenterConfiguration> GetConfigurationAsync(
      CancellationToken token)
    {
      return await this.fileObjectStorageService.ReadObjectAsync<NotificationCenterConfiguration>("NotificationCenterConfiguration.json", token) ?? new NotificationCenterConfiguration();
    }

    public async Task<bool> ShouldForwardAsync(string appId, CancellationToken token) => (await this.GetConfigurationAsync(token).ConfigureAwait(false)).IsAppEnabled(appId);

    public Task SetAppsAsync(IList<string> enabledAppIds, IList<string> disabledAppIds)
    {
      NotificationCenterConfiguration centerConfiguration = new NotificationCenterConfiguration();
      foreach (string enabledAppId in (IEnumerable<string>) enabledAppIds)
        centerConfiguration.EnabledApps.Add(enabledAppId);
      foreach (string disabledAppId in (IEnumerable<string>) disabledAppIds)
        centerConfiguration.DisabledApps.Add(disabledAppId);
      return this.fileObjectStorageService.WriteObjectAsync((object) centerConfiguration, "NotificationCenterConfiguration.json", CancellationToken.None);
    }
  }
}
