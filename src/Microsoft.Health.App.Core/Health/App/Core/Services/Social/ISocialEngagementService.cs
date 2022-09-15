// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Social.ISocialEngagementService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client.Models.Social;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Social
{
  public interface ISocialEngagementService
  {
    bool IsSocialEnabled { get; }

    bool IsSocialTileUpdatePending { get; set; }

    Task<SocialTileStatusResponse> GetSocialTileDisplayAsync(
      FacebookCredentials facebookCredentials,
      CancellationToken cancellationToken,
      bool forceCacheUpdate);

    Task SignUpForSocialEngagementAsync(
      FacebookCredentials facebookCredentials,
      CancellationToken cancellationToken);

    Task<HttpResponseMessage> UnbindForSocialEngagementAsync(
      CancellationToken cancellationToken);

    Task<Uri> GetSocialSiteUrlAsync(string relativeUrl, CancellationToken cancellationToken);

    Task<bool> IsSocialSiteAvailableAsync(string urlSuffix, CancellationToken cancellationToken);

    void ShowFacebookCommentPopup(string commentingUri);

    Task ToggleSocialAsync(SocialRemoveType removeType);
  }
}
