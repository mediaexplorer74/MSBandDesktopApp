// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.UserService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class UserService : IUserService, INewKatListener
  {
    private const string LastUserIdKey = "UserService.LastUserId";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\UserService.cs");
    private readonly IEnvironmentService environmentService;
    private readonly IHttpCacheService httpCacheService;
    private readonly IConfigProvider configProvider;
    private readonly IConfig config;
    private readonly IFileSystemService fileSystemService;
    private readonly IPagePicker pagePicker;
    private readonly IOAuthMsaTokenStore msaTokenStore;
    private readonly IConnectionInfoStore connectionInfoStore;
    private readonly IKatTrackingService katTrackingService;
    private readonly IUserProfileService userProfileService;
    private readonly ISmoothNavService smoothNavService;

    public UserService(
      IEnvironmentService environmentService,
      IHttpCacheService httpCacheService,
      IConfigProvider configProvider,
      IConfig config,
      IFileSystemService fileSystemService,
      IPagePicker pagePicker,
      ISmoothNavService smoothNavService,
      IOAuthMsaTokenStore msaTokenStore,
      IConnectionInfoStore connectionInfoStore,
      IKatTrackingService katTrackingService,
      IUserProfileService userProfileService)
    {
      this.environmentService = environmentService;
      this.httpCacheService = httpCacheService;
      this.configProvider = configProvider;
      this.config = config;
      this.fileSystemService = fileSystemService;
      this.pagePicker = pagePicker;
      this.smoothNavService = smoothNavService;
      this.msaTokenStore = msaTokenStore;
      this.connectionInfoStore = connectionInfoStore;
      this.katTrackingService = katTrackingService;
      this.userProfileService = userProfileService;
    }

    public bool SupportsSignOut => this.environmentService.SupportsSignOut;

    public void Initialize() => this.katTrackingService.RegisterNewKatStored((INewKatListener) this);

    public virtual async Task SignOutAsync()
    {
      UserService.Logger.Info((object) "Signing out");
      this.configProvider.SetGuid("UserService.LastUserId", this.userProfileService.CurrentUserProfile.ProfileId);
      await this.userProfileService.SetUserProfileAsync((BandUserProfile) null, CancellationToken.None);
      ConfiguredTaskAwaitable configuredTaskAwaitable = this.msaTokenStore.ClearAsync().ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.connectionInfoStore.ClearAsync().ConfigureAwait(false);
      await configuredTaskAwaitable;
      this.config.OobeStatus = OobeStatus.NotShown;
      this.smoothNavService.Navigate(this.pagePicker.MarketingSignIn);
      this.smoothNavService.ClearBackStack();
    }

    protected virtual async Task ClearUserDataAsync()
    {
      UserService.Logger.Info((object) "Clearing user data");
      ConfiguredTaskAwaitable configuredTaskAwaitable = this.httpCacheService.RemoveAllAsync().ConfigureAwait(false);
      await configuredTaskAwaitable;
      this.configProvider.Clear(ConfigDomain.User);
      configuredTaskAwaitable = this.fileSystemService.ClearUserFilesAsync().ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    async Task INewKatListener.OnNewKatStoredAsync(
      Guid userId,
      CancellationToken token)
    {
      Guid guid = this.configProvider.GetGuid("UserService.LastUserId", Guid.Empty);
      if (!(guid != Guid.Empty) || !(userId != Guid.Empty))
        return;
      if (!(guid != userId))
        return;
      try
      {
        await this.ClearUserDataAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        UserService.Logger.Warn((object) "Unable to remove all user data.", ex);
      }
    }
  }
}
