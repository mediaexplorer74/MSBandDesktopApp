// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Social.SocialTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Social;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Models.Social;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Social
{
  [PageTaxonomy(new string[] {"Social"})]
  public class SocialTileViewModel : MetricTileViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Social\\SocialTileViewModel.cs");
    private readonly INetworkService networkService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IHttpCacheService httpCacheService;
    private readonly ISocialEngagementService socialEngagementService;
    private readonly SocialViewModel socialViewModel;
    private readonly IFacebookService facebookService;
    private readonly IFormattingService formattingService;
    private readonly IMessageBoxService messageBoxService;
    private SocialTileStatusResponse statusResponse;

    public SocialTileViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      TileFirstTimeUseViewModel firstTimeUse,
      IHealthCloudClient healthCloudClient,
      IHttpCacheService httpCacheService,
      ISocialEngagementService socialEngagementService,
      SocialViewModel socialViewModel,
      IFacebookService facebookService,
      IFormattingService formattingService,
      IMessageBoxService messageBoxService)
      : base(networkService, smoothNavService, messageSender, firstTimeUse)
    {
      socialViewModel.SocialTileViewModel = this;
      this.networkService = networkService;
      this.smoothNavService = smoothNavService;
      this.healthCloudClient = healthCloudClient;
      this.httpCacheService = httpCacheService;
      this.socialEngagementService = socialEngagementService;
      this.socialViewModel = socialViewModel;
      this.facebookService = facebookService;
      this.formattingService = formattingService;
      this.messageBoxService = messageBoxService;
      this.TileIcon = "\uE207";
      this.Pivots.Add(new PivotDefinition(string.Empty, (object) socialViewModel));
      this.ShowPivotHeader = false;
      this.FirstTimeUse.IsSupported = false;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      bool flag;
      try
      {
        SocialTileViewModel.Logger.Debug((object) "<START> loading the social tile");
        FacebookCredentials credentials = await this.facebookService.GetCachedFacebookCredentialsAsync();
        if (credentials != null)
        {
          FacebookPermissions permissionsAsync = await this.facebookService.GetUserPermissionsAsync(credentials.Token);
          if (permissionsAsync == null || !permissionsAsync.CanViewFriends)
          {
            await this.facebookService.DeleteCachedFacebookCredentialsAsync();
            credentials = (FacebookCredentials) null;
            int num = (int) await this.messageBoxService.ShowAsync(AppResources.FacebookFriendsListPermissionError, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
          }
        }
        SocialTileViewModel socialTileViewModel = this;
        SocialTileStatusResponse statusResponse = socialTileViewModel.statusResponse;
        SocialTileStatusResponse tileDisplayAsync = await this.socialEngagementService.GetSocialTileDisplayAsync(credentials, CancellationToken.None, false);
        socialTileViewModel.statusResponse = tileDisplayAsync;
        socialTileViewModel = (SocialTileViewModel) null;
        SocialTileViewModel.Logger.Debug((object) "<END> loading the social tile");
        flag = true;
      }
      catch (Exception ex)
      {
        ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, ex.ToString());
        throw;
      }
      return flag;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.UpdateTileContent();
      this.CanOpen = true;
    }

    protected override Task OnTransitionToNoDataStateAsync() => base.OnTransitionToNoDataStateAsync();

    protected override async void OnNavigatedBack()
    {
      base.OnNavigatedBack();
      if (!this.socialEngagementService.IsSocialTileUpdatePending)
        return;
      this.socialEngagementService.IsSocialTileUpdatePending = false;
      await this.LoadAsync((IDictionary<string, string>) null);
    }

    public void SetTileContent(SocialTileStatusResponse statusResponse)
    {
      this.statusResponse = statusResponse;
      this.UpdateTileContent();
    }

    private void UpdateTileContent()
    {
      SocialTileStatusResponse statusResponse = this.statusResponse;
      if (statusResponse == null)
      {
        this.Header = (StyledSpan) null;
        this.Subheader = this.networkService.IsInternetAvailable ? AppResources.DataErrorMessage : AppResources.InternetRequiredMessage;
      }
      else
      {
        switch (statusResponse.Status)
        {
          case SocialTileStatus.Default:
            this.Subheader = this.statusResponse.Line1;
            this.Header = this.formattingService.FormatSocialMetric(this.statusResponse.NumericValue, this.statusResponse.Line2);
            break;
          case SocialTileStatus.Activate:
            this.Subheader = this.statusResponse.Line1;
            this.Header = (StyledSpan) null;
            break;
          default:
            this.Subheader = this.statusResponse.Line1;
            this.Header = (StyledSpan) null;
            break;
        }
      }
    }
  }
}
