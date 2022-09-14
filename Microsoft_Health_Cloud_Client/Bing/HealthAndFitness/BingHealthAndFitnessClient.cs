// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.BingHealthAndFitnessClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Authentication;
using Microsoft.Health.Cloud.Client.Configuration;
using Microsoft.Health.Cloud.Client.Exceptions;
using Microsoft.Health.Cloud.Client.Extensions;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Services;
using Microsoft.Health.Cloud.Client.Tracing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  public class BingHealthAndFitnessClient : IBingHealthAndFitnessClient
  {
    private const string BingWorkoutDetailPath = "FitnessWindowsService.svc/GetWorkoutDetails";
    private const string BingWorkoutDetailUrlPattern = "{0}FitnessWindowsService.svc/GetWorkoutDetails/{1}?market={2}&{3}";
    private const string VideoRequestUriFormat = "http://edge1.catalog.video.msn.com/videobyuuid.aspx?uuid={0}&Client-AppVersion=3.0.2.258";
    private const string VideoRequestForwardLinkUri = "http://go.microsoft.com/fwlink/?LinkID=615600&clcid=0x409";
    private const string SearchWorkoutsRelativeUrl = "FeedsIndexWindowsService.svc/ViewFiltered";
    private const string SearchWorkoutsRelativeUrlFormat = "FeedsIndexWindowsService.svc/ViewFiltered?cluster=fitness&goals=&types=Strength&scenario=workout&market={0}&count=238&";
    private const string SearchWorkoutsQueryFormat = "&query={0}";
    private const string SearchWorkoutsFilterFormat = "&filter={0}";
    private readonly IConnectionInfoProvider connectionInfoProvider;
    private readonly IHttpCacheService cacheService;
    private readonly ICultureService cultureService;
    private readonly HttpClient httpClient;

    public BingHealthAndFitnessClient(
      HttpMessageHandler messageHandler,
      IConnectionInfoProvider connectionInfoProvider,
      IHttpCacheService cacheService,
      ICultureService cultureService)
    {
      this.connectionInfoProvider = connectionInfoProvider;
      this.cacheService = cacheService;
      this.cultureService = cultureService;
      this.httpClient = new HttpClient(messageHandler);
      DefaultHttpTracing defaultHttpTracing = new DefaultHttpTracing();
      this.Trace = (IHttpTracer) defaultHttpTracing;
      this.Configuration = (IHttpTracingConfiguration) new DefaultHttpTracingConfiguration((IHttpTracing) defaultHttpTracing);
    }

    public IHttpTracingConfiguration Configuration { get; private set; }

    private IHttpTracer Trace { get; set; }

    public async Task<WorkoutSearch> SearchWorkoutsAsync(
      WorkoutSearchOptions options,
      CancellationToken cancellationToken)
    {
      bool useCloudCache = true;
      HealthCloudConnectionInfo cloudConnectionInfo = await this.connectionInfoProvider.GetConnectionInfoAsync(cancellationToken).ConfigureAwait(false);
      UriBuilder uriBuilder = new UriBuilder(new Uri(cloudConnectionInfo.HnFEndpoint, "FeedsIndexWindowsService.svc/ViewFiltered"));
      uriBuilder.Query = cloudConnectionInfo.HnFQueryParameters;
      IDictionary<string, string> query = uriBuilder.Uri.ParseQuery();
      if (options != null)
      {
        if (!string.IsNullOrWhiteSpace(options.Cluster))
          query["cluster"] = options.Cluster;
        if (!string.IsNullOrWhiteSpace(options.Goals))
          query["goals"] = options.Goals;
        if (!string.IsNullOrWhiteSpace(options.Types))
          query["types"] = options.Types;
        if (options.Filters.Any<KeyValuePair<string, string>>())
          query["filter"] = string.Join("~", options.Filters.GroupBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (filterItem => filterItem.Key)).Select<IGrouping<string, KeyValuePair<string, string>>, string>((Func<IGrouping<string, KeyValuePair<string, string>>, string>) (group => group.Key + "|" + string.Join(";", group.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (groupItem => groupItem.Value))))));
        if (!string.IsNullOrWhiteSpace(options.Scenario))
          query["scenario"] = options.Scenario;
        if (!string.IsNullOrWhiteSpace(options.Market))
          query["market"] = options.Market;
        int? count = options.Count;
        if (count.HasValue)
        {
          IDictionary<string, string> dictionary = query;
          count = options.Count;
          string str = count.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          dictionary["count"] = str;
        }
        if (!string.IsNullOrWhiteSpace(options.Version))
          query["version"] = options.Version;
        if (!string.IsNullOrWhiteSpace(options.ClientVersion))
          query["ClientAppVersion"] = options.ClientVersion;
        if (!string.IsNullOrWhiteSpace(options.Query))
          query["query"] = options.Query;
        bool? isHealthClient = options.IsHealthClient;
        if (isHealthClient.HasValue)
        {
          IDictionary<string, string> dictionary = query;
          isHealthClient = options.IsHealthClient;
          string str = isHealthClient.Value ? "1" : "0";
          dictionary["isKClient"] = str;
        }
        WorkoutPublisher? publishedBy = options.PublishedBy;
        if (publishedBy.HasValue)
        {
          string str = (string) null;
          publishedBy = options.PublishedBy;
          switch (publishedBy.Value)
          {
            case WorkoutPublisher.Provider:
              str = "Provider";
              break;
            case WorkoutPublisher.Me:
              str = "Me";
              useCloudCache = false;
              break;
            case WorkoutPublisher.All:
              str = "All";
              useCloudCache = false;
              break;
          }
          if (!string.IsNullOrWhiteSpace(str))
            query["PublishedBy"] = str;
        }
      }
      if (!query.ContainsKey("market"))
        query["market"] = this.cultureService.CurrentSupportedUICulture.Name;
      uriBuilder.Query = new NameValueCollection(query).ToQueryString();
      return (await this.GetWithCachingAsync(uriBuilder.Uri, cancellationToken, "WorkoutGeneral", useCache: useCloudCache).ConfigureAwait(false)).ReadJson<WorkoutSearch>();
    }

    public async Task<WorkoutPlanDetail> GetWorkoutAsync(
      string workoutId,
      CancellationToken cancellationToken)
    {
      HealthCloudConnectionInfo cloudConnectionInfo = await this.connectionInfoProvider.GetConnectionInfoAsync(cancellationToken).ConfigureAwait(false);
      return (await this.GetWithCachingAsync(new Uri(string.Format("{0}FitnessWindowsService.svc/GetWorkoutDetails/{1}?market={2}&{3}", (object) cloudConnectionInfo.HnFEndpoint.AbsoluteUri, (object) workoutId, (object) this.cultureService.CurrentSupportedUICulture.Name, (object) cloudConnectionInfo.HnFQueryParameters)), cancellationToken, "WorkoutGeneral", "FitnessWindowsService.svc/GetWorkoutDetails").ConfigureAwait(false)).ReadJson<WorkoutPlanDetail>();
    }

    public async Task<string> GetVideoUrlAsync(
      string id,
      double screenWidth,
      double screenHeight,
      CancellationToken cancellationToken)
    {
      IHttpResponseContent response = await this.GetWithCachingAsync(new Uri(string.Format("http://edge1.catalog.video.msn.com/videobyuuid.aspx?uuid={0}&Client-AppVersion=3.0.2.258", new object[1]
      {
        (object) id
      })), cancellationToken, "WorkoutGeneral").ConfigureAwait(false);
      return BingHealthAndFitnessClient.GetVideo(screenHeight, response);
    }

    public async Task<string> GetGolfIntroVideoUrlAsync(
      double screenWidth,
      double screenHeight,
      CancellationToken cancellationToken)
    {
      IHttpResponseContent response = await this.GetWithCachingAsync(new Uri("http://go.microsoft.com/fwlink/?LinkID=615600&clcid=0x409"), cancellationToken, "WorkoutGeneral").ConfigureAwait(false);
      return BingHealthAndFitnessClient.GetVideo(screenHeight, response);
    }

    private static string GetVideo(double screenHeight, IHttpResponseContent response)
    {
      Video video = response.ReadXml<Video>();
      if (video != null && video.VideoFiles != null)
      {
        List<VideoFile> list = ((IEnumerable<VideoFile>) video.VideoFiles).Where<VideoFile>((Func<VideoFile, bool>) (videoFile => videoFile != null && VideoFile.FormatCodes.SupportedTypes.Contains(videoFile.FormatCode))).OrderByDescending<VideoFile, int>((Func<VideoFile, int>) (videoFile => videoFile.Height)).ToList<VideoFile>();
        VideoFile videoFile1 = (VideoFile) null;
        foreach (VideoFile videoFile2 in list)
        {
          if ((double) videoFile2.Height > screenHeight)
            videoFile1 = videoFile2;
          if ((double) videoFile2.Height <= screenHeight)
            return videoFile1 != null ? videoFile1.Url : videoFile2.Url;
        }
      }
      return (string) null;
    }

    private string GetFilterValueString(ICollection<WorkoutSearchFilter> filters)
    {
      if (filters == null || filters.Count == 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      IOrderedEnumerable<WorkoutSearchFilter> orderedEnumerable = filters.OrderBy<WorkoutSearchFilter, string>((Func<WorkoutSearchFilter, string>) (f => f.Name));
      if (filters != null)
      {
        string str = string.Empty;
        foreach (WorkoutSearchFilter workoutSearchFilter in (IEnumerable<WorkoutSearchFilter>) orderedEnumerable)
        {
          if (workoutSearchFilter.FilterName != str)
          {
            if (!string.IsNullOrWhiteSpace(str))
              stringBuilder.Append("~");
            stringBuilder.AppendFormat("{0}|{1}", new object[2]
            {
              (object) workoutSearchFilter.FilterName,
              (object) workoutSearchFilter.Id
            });
            str = workoutSearchFilter.FilterName;
          }
          else
            stringBuilder.AppendFormat(";{0}", new object[1]
            {
              (object) workoutSearchFilter.Id
            });
        }
      }
      return stringBuilder.ToString();
    }

    private async Task<IHttpResponseContent> GetWithCachingAsync(
      Uri url,
      CancellationToken cancellationToken,
      string cacheArea,
      string telemetryPathOverride = null,
      bool logFullUrl = true,
      bool useCache = true)
    {
      if (this.cacheService != null & useCache)
      {
        IHttpResponseContent responseContent = await this.cacheService.GetAsync(url).ConfigureAwait(false);
        if (responseContent != null)
        {
          this.Trace.CacheResponse(url, responseContent, logFullUrl);
          return responseContent;
        }
      }
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
      request.Properties.Add(HealthCloudClientRequestContext.KeyName, (object) new HealthCloudClientRequestContext()
      {
        TelemetryPathOverride = telemetryPathOverride
      });
      this.Trace.HttpRequest(request, logFullUrl);
      HttpResponseMessage response1 = await this.httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
      this.Trace.HttpResponse(response1, logFullUrl);
      if (!response1.IsSuccessStatusCode)
        throw new HealthCloudServerException();
      DefaultHttpResponseContent response = await DefaultHttpResponseContent.CreateAsync(response1).ConfigureAwait(false);
      if (this.cacheService != null & useCache)
        await this.cacheService.InsertAsync(url, (IHttpResponseContent) response, cacheArea).ConfigureAwait(false);
      return (IHttpResponseContent) response;
    }
  }
}
