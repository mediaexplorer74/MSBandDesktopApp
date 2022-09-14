// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.AppUpgradeService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class AppUpgradeService : IAppUpgradeService
  {
    internal const string LastKnownAppVersionKey = "AppUpgradeService.LastKnownAppVersion";
    internal const string PromptNotificationAccessIfRequiredKey = "PromptNotificationAccessIfRequired";
    private readonly IAppUpgradeListener[] appUpgradeListeners;
    private readonly IEnvironmentService environmentService;
    private readonly IConfigProvider configProvider;

    public AppUpgradeService(
      IAppUpgradeListener[] appUpgradeListeners,
      IEnvironmentService environmentService,
      IConfigProvider configProvider)
    {
      this.appUpgradeListeners = appUpgradeListeners;
      this.environmentService = environmentService;
      this.configProvider = configProvider;
    }

    protected IConfigProvider ConfigProvider => this.configProvider;

    public virtual async Task UpgradeIfNeededAsync(CancellationToken token)
    {
      Version currentVersion = this.environmentService.ApplicationVersion;
      Version previousVersion = this.configProvider.GetVersion("AppUpgradeService.LastKnownAppVersion", (Version) null);
      if (!(previousVersion == (Version) null) && !(currentVersion > previousVersion))
        return;
      if (this.appUpgradeListeners != null)
      {
        IAppUpgradeListener[] appUpgradeListenerArray = this.appUpgradeListeners;
        for (int index = 0; index < appUpgradeListenerArray.Length; ++index)
          await appUpgradeListenerArray[index].OnAppUpgradeAsync(currentVersion, previousVersion, token).ConfigureAwait(false);
        appUpgradeListenerArray = (IAppUpgradeListener[]) null;
      }
      this.configProvider.SetVersion("AppUpgradeService.LastKnownAppVersion", currentVersion);
      this.configProvider.Set<bool>("PromptNotificationAccessIfRequired", true);
    }
  }
}
