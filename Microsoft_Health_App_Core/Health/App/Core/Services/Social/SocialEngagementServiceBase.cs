// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Social.SocialEngagementServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models.Social;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Social
{
  public abstract class SocialEngagementServiceBase : ISocialEngagementService
  {
    private const string SocialCategory = "SocialCategory";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Social\\SocialEngagementServiceBase.cs");
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IDateTimeService dateTimeService;
    private readonly IConfigurationService configurationService;
    private readonly ITileManagementService tileManagementService;
    private readonly IFacebookService facebookService;
    private readonly IMessageBoxService messageBoxService;
    public static readonly ConfigurationValue<bool> IsSocialTileEnabled = ConfigurationValue.CreateBoolean(nameof (SocialCategory), nameof (IsSocialTileEnabled), (Func<bool>) (() => true));

    protected SocialEngagementServiceBase(
      IHealthCloudClient healthCloudClient,
      IDateTimeService dateTimeService,
      IConfigurationService configurationService,
      ITileManagementService tileManagementService,
      IFacebookService facebookService,
      IMessageBoxService messageBoxService)
    {
      this.healthCloudClient = healthCloudClient;
      this.dateTimeService = dateTimeService;
      this.configurationService = configurationService;
      this.tileManagementService = tileManagementService;
      this.facebookService = facebookService;
      this.messageBoxService = messageBoxService;
    }

    public bool IsSocialEnabled
    {
      get => this.configurationService.GetValue<bool>(SocialEngagementServiceBase.IsSocialTileEnabled);
      private set => this.configurationService.SetValue<bool>((GenericConfigurationValue<bool>) SocialEngagementServiceBase.IsSocialTileEnabled, value);
    }

    public bool IsSocialTileUpdatePending { get; set; }

    private async Task DisableSocialAsync()
    {
      if (!this.IsSocialEnabled)
        return;
      FacebookCredentials credentials = await this.facebookService.GetCachedFacebookCredentialsAsync();
      if (credentials == null)
      {
        this.RemoveTile();
      }
      else
      {
        SocialTileStatusResponse statusResponse = (SocialTileStatusResponse) null;
        try
        {
          statusResponse = await this.GetSocialTileDisplayAsync(credentials, CancellationToken.None, true);
        }
        catch (Exception ex)
        {
          ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, ex.ToString());
          int num = (int) await this.messageBoxService.ShowAsync(AppResources.ErrorPageTitle, (string) null, PortableMessageBoxButton.OK);
          throw;
        }
        if (statusResponse == null)
        {
          string errorDetails = "Null statusResponse from GetSocialTileDisplay while unbinding";
          ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, errorDetails);
          int num = (int) await this.messageBoxService.ShowAsync(AppResources.ErrorPageTitle, (string) null, PortableMessageBoxButton.OK);
          throw new HealthCloudException(errorDetails);
        }
        if (statusResponse.Status == SocialTileStatus.Activate)
        {
          this.RemoveTile();
        }
        else
        {
          for (int i = 0; i < 3; ++i)
          {
            try
            {
              using (HttpResponseMessage httpResponseMessage = await this.UnbindForSocialEngagementAsync(CancellationToken.None))
              {
                int num;
                if (num == 5 || num == 6 || httpResponseMessage.IsSuccessStatusCode)
                {
                  try
                  {
                    await this.facebookService.RemoveFacebookPermissionsAsync(credentials.Token);
                    await this.facebookService.DeleteCachedFacebookCredentialsAsync();
                    break;
                  }
                  catch (Exception ex)
                  {
                    ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, ex.ToString());
                    break;
                  }
                  finally
                  {
                    this.RemoveTile();
                  }
                }
                else
                  throw new HealthCloudException(string.Format("Non-success status code from UnbindForSocialEngagement {0}", new object[1]
                  {
                    (object) httpResponseMessage.StatusCode
                  }));
              }
            }
            catch (Exception ex)
            {
              if (i == 2)
              {
                ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, ex.ToString());
                int num = (int) await this.messageBoxService.ShowAsync(AppResources.ErrorPageTitle, (string) null, PortableMessageBoxButton.OK);
                throw;
              }
            }
          }
        }
      }
    }

    private Task EnableSocialAsync()
    {
      this.tileManagementService.HaveTilesBeenUpdated = !this.IsSocialEnabled;
      this.IsSocialEnabled = true;
      return (Task) Task.FromResult<object>((object) null);
    }

    public Task ToggleSocialAsync(SocialRemoveType removeType)
    {
      ApplicationTelemetry.LogSocialTileRemoval(!this.IsSocialEnabled, removeType);
      return !this.IsSocialEnabled ? this.EnableSocialAsync() : this.DisableSocialAsync();
    }

    private void RemoveTile()
    {
      this.tileManagementService.HaveTilesBeenUpdated = this.IsSocialEnabled;
      this.IsSocialEnabled = false;
    }

    public Task<SocialTileStatusResponse> GetSocialTileDisplayAsync(
      FacebookCredentials credentials,
      CancellationToken cancellationToken,
      bool forceCacheUpdate)
    {
      return this.healthCloudClient.GetSocialTileDisplayAsync(credentials?.Token?.Token, credentials?.Profile?.UserId, (int) this.dateTimeService.Now.Offset.TotalMinutes, cancellationToken, forceCacheUpdate);
    }

    public Task SignUpForSocialEngagementAsync(
      FacebookCredentials credentials,
      CancellationToken cancellationToken)
    {
      return this.healthCloudClient.SignUpForSocialEngagementAsync(credentials.Token.Token, cancellationToken);
    }

    public Task<HttpResponseMessage> UnbindForSocialEngagementAsync(
      CancellationToken cancellationToken)
    {
      return this.healthCloudClient.UnbindForSocialEngagementAsync(cancellationToken);
    }

    public Task<Uri> GetSocialSiteUrlAsync(
      string relativeUrl,
      CancellationToken cancellationToken)
    {
      return this.healthCloudClient.GetSocialSiteUrlAsync(relativeUrl, cancellationToken);
    }

    public Task<bool> IsSocialSiteAvailableAsync(
      string urlSuffix,
      CancellationToken cancellationToken)
    {
      return this.healthCloudClient.IsSocialSiteAvailableAsync(urlSuffix, cancellationToken);
    }

    public abstract void ShowFacebookCommentPopup(string commentingUri);
  }
}
