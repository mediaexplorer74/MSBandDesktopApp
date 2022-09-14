// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Authentication.ConnectionInfoProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Authentication;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Authentication
{
  public class ConnectionInfoProvider : IConnectionInfoProvider, IKatTrackingService
  {
    
    private const string LastOdsUserIdKey = "LastODSUserId";
    public const string ConnectionInfoProviderCategory = "ConnectionInfoProvider";
    public static readonly TimeSpan TokenRefreshBuffer = TimeSpan.FromMinutes(5.0);
    public static readonly ConfigurationValue<bool> IsClientSideTokenValidationEnabledConfigurationValue = ConfigurationValue.CreateBoolean(nameof (ConnectionInfoProvider), nameof (IsClientSideTokenValidationEnabled), true);
    private readonly IConfig config;
    private readonly IConfigurationService configurationService;
    private readonly IConnectionInfoStore connectionInfoStore;
    private readonly IMsaTokenProvider msaTokenSource;
    private readonly IHealthDiscoveryClient healthDiscoveryClient;
    private readonly List<INewKatListener> newKatListeners = new List<INewKatListener>();
    private Mutex connectionInfoMutex;
    private readonly IConfigProvider configProvider;

        public bool IsClientSideTokenValidationEnabled
        {
            get
            {
                return this.configurationService.GetValue<bool>(
                    ConnectionInfoProvider.IsClientSideTokenValidationEnabledConfigurationValue);
            }
        }

    public ConnectionInfoProvider
    (
      IMsaTokenProvider msaTokenSource,
      IConnectionInfoStore connectionInfoStore,
      IConfig config,
      IConfigProvider configProvider,
      IConfigurationService configurationService,
      IHealthDiscoveryClient healthDiscoveryClient,
      IMutexService mutexProvider
    )
    {
      Assert.ParamIsNotNull((object) msaTokenSource, nameof (msaTokenSource));
      Assert.ParamIsNotNull((object) connectionInfoStore, nameof (connectionInfoStore));
      Assert.ParamIsNotNull((object) config, nameof (config));
      Assert.ParamIsNotNull((object) configProvider, nameof (configProvider));
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      Assert.ParamIsNotNull((object) healthDiscoveryClient, nameof (healthDiscoveryClient));
      this.msaTokenSource = msaTokenSource;
      this.connectionInfoStore = connectionInfoStore;
      this.config = config;
      this.configProvider = configProvider;
      this.configurationService = configurationService;
      this.healthDiscoveryClient = healthDiscoveryClient;
      this.connectionInfoMutex = mutexProvider.GetNamedMutex(false, "KApp.ConnectionInfoProvider");
      Telemetry.SetOdsUserId(configProvider.GetGuid("LastODSUserId", Guid.Empty));
    }

    public async Task<HealthCloudConnectionInfo> GetConnectionInfoAsync
    (
      CancellationToken cancellationToken,
      bool allowUI = false
    )
    {
      HealthCloudConnectionInfo info = (HealthCloudConnectionInfo) null;
      Guid userId = Guid.Empty;
      try
      {
        await this.connectionInfoMutex.RunSynchronizedAsync((Func<Task>) (async () =>
        {
          info = await this.connectionInfoStore.TryGetAsync().ConfigureAwait(false);
          if (info != null && (!this.IsClientSideTokenValidationEnabled || !new SimpleWebToken(info.PodSecurityToken).IsExpired(ConnectionInfoProvider.TokenRefreshBuffer)))
            return;

          // ISSUE: variable of a compiler-generated type
          //ConnectionInfoProvider.\u003C\u003Ec__DisplayClass15_0 cDisplayClass150;
          ConnectionInfoProvider cDisplayClass150 = new ConnectionInfoProvider(null,null,null,null,null,null,null);

            // ISSUE: reference to a compiler-generated field
          HealthCloudConnectionInfo info3 = default;//cDisplayClass150.info; // RnD
          HealthCloudConnectionInfo cloudConnectionInfo = await this.GetConnectionInfoFromServiceAsync(allowUI, cancellationToken).ConfigureAwait(false);
          // ISSUE: reference to a compiler-generated field
          //cDisplayClass150.info = cloudConnectionInfo; // RnD
          //cDisplayClass150 = (ConnectionInfoProvider.\u003C\u003Ec__DisplayClass15_0) null;
          ConfiguredTaskAwaitable configuredTaskAwaitable = this.connectionInfoStore.SetAsync(info).ConfigureAwait(false);
          await configuredTaskAwaitable;
          if (info != null && info.UserId != null)
            Guid.TryParse(info.UserId, out userId);
          foreach (INewKatListener newKatListener in this.newKatListeners)
          {
            configuredTaskAwaitable = newKatListener.OnNewKatStoredAsync(userId, cancellationToken).ConfigureAwait(false);
            await configuredTaskAwaitable;
          }
        }), cancellationToken).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        if (info == null)
          throw new MissingCredentialsException("Could not get credentials.", ex);
      }
      if (userId != Guid.Empty)
      {
        Telemetry.SetOdsUserId(userId);
        this.configProvider.SetGuid("LastODSUserId", userId);
      }
      return info;
    }

    public void RegisterNewKatStored(INewKatListener newKatListener) => this.newKatListeners.Add(newKatListener);

    public void UnregisterNewKatStored(INewKatListener newKatListener) => this.newKatListeners.Remove(newKatListener);

    private async Task<HealthCloudConnectionInfo> GetConnectionInfoFromServiceAsync(
      bool allowUI,
      CancellationToken cancellationToken)
    {
      string msaToken = await this.msaTokenSource.GetAsync(allowUI).ConfigureAwait(false);
      Uri baseUri = new Uri(this.config.AuthBaseUrl);
      KdsResponse kdsResponse = await this.healthDiscoveryClient.AuthenticateAsync(cancellationToken).ConfigureAwait(false);
      UserInfo userInfo = kdsResponse.UserInfo;
      string podAccessToken = kdsResponse.PodAccessToken;
      return new HealthCloudConnectionInfo()
      {
        BaseUri = baseUri,
        FusEndpoint = new Uri(userInfo.FusEndPoint),
        PodEndpoint = new Uri(userInfo.EndPoint),
        HnFEndpoint = new Uri(userInfo.AuthedHnFEndPoint),
        HnFQueryParameters = userInfo.AuthedHnFQueryParameters,
        PodSecurityToken = podAccessToken,
        SecurityToken = msaToken,
        SocialServiceEndPoint = new Uri(userInfo.SocialServiceEndPoint),
        UserId = userInfo.OdsUserID
      };
    }
  }
}
