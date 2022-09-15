// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IFacebookService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IFacebookService
  {
    Task<FacebookCredentials> GetCachedFacebookCredentialsAsync();

    Task SetCachedFacebookCredentialsAsync(FacebookCredentials credentials);

    Task DeleteCachedFacebookCredentialsAsync();

    Task RemoveFacebookPermissionsAsync(FacebookAccessToken accessToken);

    Task<FacebookAccessToken> LoginAsync();

    Task SetLoginResultAsync(FacebookAccessToken token);

    Task<FacebookAccessToken> GetLongLivedAccessTokenAsync(
      FacebookAccessToken accessToken);

    Task<FacebookUserProfileFragment> GetUserProfileAsync(
      FacebookAccessToken accessToken);

    Task<FacebookPermissions> GetUserPermissionsAsync(
      FacebookAccessToken accessToken);

    Task<Uri> GetFacebookAppLinkUrlAsync(CancellationToken cancellationToken);

    Task InviteFriendsAsync();
  }
}
