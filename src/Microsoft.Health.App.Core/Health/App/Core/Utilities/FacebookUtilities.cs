// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.FacebookUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class FacebookUtilities
  {
    private const string FacebookSocialAppId = "1537258739822978";
    private const string FacebookBaseWwwUri = "https://www.facebook.com";
    private const string FacebookBaseGraphUri = "https://graph.facebook.com";
    private const string FacebookLoginDialogPath = "dialog/oauth";
    private const string FacebookLoginDialogRerequestArgument = "&auth_type=rerequest";
    private const string FacebookLoginDialogFinalRedirectUri = "https://www.facebook.com/connect/login_success.html";
    private const string FacebookLoginDialogFinalRedirectUriComplete = "https://www.facebook.com/connect/login_success.html";
    private const string FacebookLoginDialogUriFormat = "{0}/{1}?client_id={2}&response_type=token&scope=user_friends&display=touch&redirect_uri={3}{4}";
    private const string FacebookGetUserProfileGraphPath = "me";
    private const string FacebookGetUserProfileGraphUriFormat = "{0}/{1}?access_token={2}";
    private const string FacebookGetUserPermissionsGraphPath = "me/permissions";
    private const string FacebookGetUserPermissionsGraphUriFormat = "{0}/{1}?access_token={2}";
    private const string FacebookShareDialogPath = "dialog/share";
    private const string FacebookShareDialogFinalRedirectUri = "http://www.facebook.com";
    private const string FacebookShareDialogFinalRedirectUriComplete = "http://www.facebook.com/";
    private const string FacebookShareDialogUriFormat = "{0}/{1}?app_id={2}&href={3}&display=touch&redirect_uri={4}";
    private const string FacebookSendMessageDialogPath = "dialog/send";
    private const string FacebookSendMessageDialogFinalRedirectUri = "http://www.facebook.com";
    private const string FacebookSendMessageDialogFinalRedirectUriComplete = "http://www.facebook.com/";
    private const string FacebookSendMessageWithLinkDialogUriFormat = "{0}/{1}?app_id={2}&link={3}&display=popup&redirect_uri={4}";

    public static Uri GenerateFacebookLoginDialogUrl(bool isRerequest) => new Uri(string.Format("{0}/{1}?client_id={2}&response_type=token&scope=user_friends&display=touch&redirect_uri={3}{4}", (object) "https://www.facebook.com", (object) "dialog/oauth", (object) "1537258739822978", (object) "https://www.facebook.com/connect/login_success.html", isRerequest ? (object) "&auth_type=rerequest" : (object) string.Empty));

    public static bool IsFacebookLoginCompletedUrl(Uri uri) => string.Format("{0}://{1}{2}", new object[3]
    {
      (object) uri.Scheme,
      (object) uri.Host,
      (object) uri.AbsolutePath
    }) == "https://www.facebook.com/connect/login_success.html";

    public static Task<FacebookAccessToken> ParseFacebookLoginFinishedUriAsync(
      Uri uri)
    {
      string data = uri.Fragment.Substring(1, uri.Fragment.Length - 1);
      return Task.Run<FacebookAccessToken>((Func<FacebookAccessToken>) (() => FacebookUtilities.ParseFacebookAccessTokenResponse(data)));
    }

    public static Task<FacebookAccessToken> ParseFacebookAccessTokenResponseAsync(
      string data)
    {
      return Task.Run<FacebookAccessToken>((Func<FacebookAccessToken>) (() => FacebookUtilities.ParseFacebookAccessTokenResponse(data)));
    }

    private static FacebookAccessToken ParseFacebookAccessTokenResponse(
      string data)
    {
      string[] strArray1 = data.Split('&');
      FacebookAccessToken facebookAccessToken = new FacebookAccessToken();
      foreach (string str1 in strArray1)
      {
        char[] separator = new char[1]{ '=' };
        string[] strArray2 = str1.Split(separator, 2);
        if (strArray2.Length >= 2)
        {
          string str2 = strArray2[0];
          if (!(str2 == "access_token"))
          {
            if (str2 == "expires_in" || str2 == "expires")
              facebookAccessToken.Expires = new DateTime?(DateTime.UtcNow.AddSeconds((double) Convert.ToInt32(strArray2[1])));
          }
          else
            facebookAccessToken.Token = strArray2[1];
        }
      }
      return !string.IsNullOrWhiteSpace(facebookAccessToken.Token) && facebookAccessToken.Expires.HasValue ? facebookAccessToken : (FacebookAccessToken) null;
    }

    public static Uri GenerateFacebookGetUserProfileUrl(FacebookAccessToken accessToken) => new Uri(string.Format("{0}/{1}?access_token={2}", new object[3]
    {
      (object) "https://graph.facebook.com",
      (object) "me",
      (object) accessToken.Token
    }));

    public static Uri GenerateFacebookGetUserPermissionsUrl(FacebookAccessToken accessToken) => new Uri(string.Format("{0}/{1}?access_token={2}", new object[3]
    {
      (object) "https://graph.facebook.com",
      (object) "me/permissions",
      (object) accessToken.Token
    }));

    public static Uri GenerateFacebookShareDialogUrl(string dashboardShareUrl) => new Uri(string.Format("{0}/{1}?app_id={2}&href={3}&display=touch&redirect_uri={4}", (object) "https://www.facebook.com", (object) "dialog/share", (object) "1537258739822978", (object) dashboardShareUrl, (object) "http://www.facebook.com"));

    public static bool IsFacebookShareCompletedUrl(Uri url) => string.Format("{0}://{1}{2}", new object[3]
    {
      (object) url.Scheme,
      (object) url.Host,
      (object) url.AbsolutePath
    }) == "http://www.facebook.com/";

    public static Uri GenerateFacebookSendMessageWithLinkDialogUrl(Uri messageLink) => new Uri(string.Format("{0}/{1}?app_id={2}&link={3}&display=popup&redirect_uri={4}", (object) "https://www.facebook.com", (object) "dialog/send", (object) "1537258739822978", (object) messageLink, (object) "http://www.facebook.com"));

    public static bool IsFacebookSendMessageCompletedUrl(Uri url) => string.Format("{0}://{1}{2}", new object[3]
    {
      (object) url.Scheme,
      (object) url.Host,
      (object) url.AbsolutePath
    }) == "http://www.facebook.com/";
  }
}
