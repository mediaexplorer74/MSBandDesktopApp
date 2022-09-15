// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Authentication.OAuthMsaTokenProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using CommonServiceLocator;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Http.Clients.LiveLogin;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.Cloud.Client.Authentication;
//using Microsoft.Practices.ServiceLocation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Authentication
{
  public class OAuthMsaTokenProvider : IMsaTokenProvider
  {
    public const string OAuthClientId = "000000004811DB42";
    private static readonly TimeSpan TokenRefreshBuffer = TimeSpan.FromMinutes(5.0);
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Authentication\\OAuthMsaTokenProvider.cs");
    private readonly IOAuthMsaTokenStore oAuthMsaTokenStore;
    private readonly ILiveLoginClient liveLoginClient;
    private readonly IConfig config;
    private readonly IMessageSender messageSender;
    private TaskCompletionSource<object> navigationTaskCompletionSource;

    public OAuthMsaTokenProvider(
      IOAuthMsaTokenStore oAuthMsaTokenStore,
      ILiveLoginClient liveLoginClient,
      IConfig config,
      IMessageSender messageSender)
    {
      this.oAuthMsaTokenStore = oAuthMsaTokenStore;
      this.liveLoginClient = liveLoginClient;
      this.config = config;
      this.messageSender = messageSender;
    }

    public async Task<string> GetAsync(bool allowUI)
    {
      StoredMsaToken storedMsaToken1 = await this.oAuthMsaTokenStore.GetAsync().ConfigureAwait(false);
      if (storedMsaToken1 != null)
      {
        if (storedMsaToken1.Expiry - OAuthMsaTokenProvider.TokenRefreshBuffer >= DateTimeOffset.Now)
          return storedMsaToken1.MsaToken;
        try
        {
          TokenRefreshResponse refreshResponse = await this.liveLoginClient.RefreshTokenAsync(new Uri(CloudEnvironment.GetUrl(this.config.Environment)).Host, storedMsaToken1.RefreshToken, CancellationToken.None).ConfigureAwait(false);
          await this.oAuthMsaTokenStore.SetAsync(new StoredMsaToken()
          {
            MsaToken = refreshResponse.AccessToken,
            RefreshToken = refreshResponse.RefreshToken,
            Expiry = DateTimeOffset.Now.AddSeconds((double) refreshResponse.ExpiresInSeconds)
          }).ConfigureAwait(false);
          return refreshResponse.AccessToken;
        }
        catch (Exception ex)
        {
          OAuthMsaTokenProvider.Logger.Error((object) "Could not refresh access msaToken.", ex);
        }
      }
      if (!allowUI)
        throw new MissingCredentialsException("Cannot get OAuth MSA msaToken in background.");
      this.navigationTaskCompletionSource = new TaskCompletionSource<object>();
     
      ISmoothNavService smoothNavService = ServiceLocator.Current.GetInstance<ISmoothNavService>();
      EventHandler<NavigationEventArguments> navigatingEvent = (EventHandler<NavigationEventArguments>) null;
      navigatingEvent = (EventHandler<NavigationEventArguments>) ((s, e) =>
      {
        if (e.NavigationType != NavigationType.Backward)
          return;
        smoothNavService.Navigating -= navigatingEvent;
        this.navigationTaskCompletionSource.SetResult((object) null);
      });
      smoothNavService.Navigating += navigatingEvent;
      smoothNavService.Navigate(typeof (OAuthSignInViewModel));
      object obj = await this.navigationTaskCompletionSource.Task.ConfigureAwait(false);
      StoredMsaToken storedMsaToken2 = await this.oAuthMsaTokenStore.GetAsync().ConfigureAwait(false);
      if (storedMsaToken2 != null && storedMsaToken2.Expiry > DateTimeOffset.Now)
        return storedMsaToken2.MsaToken;
      throw new MissingCredentialsException("Could not get MSA msaToken.");
    }
  }
}
