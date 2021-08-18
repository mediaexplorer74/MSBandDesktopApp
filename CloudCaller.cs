// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.CloudCaller
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopSyncApp
{
  public class CloudCaller
  {
    private const string AuthorizationHeader = "Authorization";
    private const string UserAgentHeader = "UserAgent";
    private ServiceInfo serviceInfo;

    public CloudCaller(ServiceInfo serviceInfo) => this.serviceInfo = serviceInfo;

    public async Task<IList<ExerciseTag>> GetExerciseTagsAsync(
      CancellationToken cancellationToken)
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds(20.0);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{0}/{1}", (object) this.serviceInfo.PodAddress, (object) "v2/exercisetagging/getalltags");
      IList<ExerciseTag> exerciseTagList;
      using (HttpClient client = this.CreateHttpClient(CloudCaller.AuthorizationNeeded.Pod, (Dictionary<string, string>) null))
      {
        client.Timeout = timeSpan;
        using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, stringBuilder.ToString()))
        {
          using (HttpResponseMessage responseMessage = await client.SendAsync(requestMessage, cancellationToken))
          {
            if (!responseMessage.IsSuccessStatusCode)
              throw new Exception("Unable to get Exercise Tags");
            exerciseTagList = (IList<ExerciseTag>) await responseMessage.ReadJsonAsync<List<ExerciseTag>>();
          }
        }
      }
      return exerciseTagList;
    }

    public async Task<MsaUserProfile> GetMsaUserProfileAsync(
      CancellationToken cancellationToken)
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds(20.0);
      StringBuilder stringBuilder = new StringBuilder();
      Dictionary<string, string> additionalHeaders = new Dictionary<string, string>();
      stringBuilder.AppendFormat("https://{0}/{1}", (object) this.serviceInfo.DiscoveryServiceAddress, (object) "api/v1/user/live");
      additionalHeaders.Add("Region", RegionInfo.CurrentRegion.TwoLetterISORegionName);
      additionalHeaders.Add("Accept-Language", CultureInfo.CurrentUICulture.Name);
      using (HttpClient client = this.CreateHttpClient(CloudCaller.AuthorizationNeeded.Discovery, additionalHeaders))
      {
        client.Timeout = timeSpan;
        using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, stringBuilder.ToString()))
        {
          using (HttpResponseMessage responseMessage = await client.SendAsync(requestMessage, cancellationToken))
            return !responseMessage.IsSuccessStatusCode ? (MsaUserProfile) null : await responseMessage.ReadJsonAsync<MsaUserProfile>();
        }
      }
    }

    private HttpClient CreateHttpClient(
      CloudCaller.AuthorizationNeeded authorizationNeeded,
      Dictionary<string, string> additionalHeaders)
    {
      HttpClient httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("UserAgent", Globals.DefaultUserAgent);
      switch (authorizationNeeded)
      {
        case CloudCaller.AuthorizationNeeded.Pod:
          httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("WRAP access_token=\"{0}\"", (object) this.serviceInfo.AccessToken));
          break;
        case CloudCaller.AuthorizationNeeded.Discovery:
          httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", this.serviceInfo.DiscoveryServiceAccessToken);
          break;
      }
      if (additionalHeaders != null)
      {
        foreach (KeyValuePair<string, string> additionalHeader in additionalHeaders)
        {
          if (string.Compare("UserAgent", additionalHeader.Key, StringComparison.OrdinalIgnoreCase) != 0 && string.Compare("Authorization", additionalHeader.Key, StringComparison.OrdinalIgnoreCase) != 0)
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(additionalHeader.Key, additionalHeader.Value);
        }
      }
      return httpClient;
    }

    private enum AuthorizationNeeded
    {
      None,
      Pod,
      Discovery,
    }

    private static class EndPoints
    {
      public const string ExerciseTags = "v2/exercisetagging/getalltags";
      public const string MsaUserProfile = "api/v1/user/live";
    }
  }
}
