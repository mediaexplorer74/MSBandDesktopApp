// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Events.Golf.Courses
{
  internal sealed class GolfCourseClient : HealthCloudClientBase, IGolfCourseClient
  {
    public GolfCourseClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHttpCacheService cacheService = null)
      : base(messageHandler, baseUriSelector, cacheService)
    {
    }

    public Task<GolfCourseRegionSearchResults> GetRegionsAsync(
      CancellationToken token)
    {
      return this.GetJsonAsync<GolfCourseRegionSearchResults>(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/regions", new object[1]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0/course"
      }), token);
    }

    public Task<GolfCourseStateSearchResults> GetStatesAsync(
      long regionId,
      CancellationToken token)
    {
      return this.GetJsonAsync<GolfCourseStateSearchResults>(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/regions/{1}/states", new object[2]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0/course",
        (object) regionId
      }), token);
    }

    public async Task<GolfCourseSearchResults> GetRecentCoursesAsync(
      CancellationToken token)
    {
      return await this.GetJsonAsync<GolfCourseSearchResults>(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/recent", new object[1]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0"
      }), token, cacheArea: "GolfRecent").ConfigureAwait(false) ?? new GolfCourseSearchResults();
    }

    public async Task<GolfCourseSearchResults> GetCoursesByLocationAsync(
      double latitude,
      double longitude,
      CancellationToken token,
      GolfCourseSearchPagingOptions pagingOptions = null,
      GolfCourseSearchFilterOptions filterOptions = null,
      bool logFullUrl = true)
    {
      string relativeUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/search", new object[1]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0/course"
      });
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add(nameof (latitude), latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      nameValueCollection.Add(nameof (longitude), longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      NameValueCollection parameters = nameValueCollection;
      GolfCourseClient.AddPagingOptions(parameters, pagingOptions);
      GolfCourseClient.AddFilterOptions(parameters, filterOptions);
      return await this.GetJsonAsync<GolfCourseSearchResults>(relativeUrl, token, parameters, logFullUrl: logFullUrl).ConfigureAwait(false) ?? new GolfCourseSearchResults();
    }

    public async Task<GolfCourseSearchResults> GetCoursesByStateAsync(
      long stateId,
      CancellationToken token,
      GolfCourseSearchPagingOptions pagingOptions = null,
      GolfCourseSearchFilterOptions filterOptions = null)
    {
      string relativeUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/states/{1}", new object[2]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0/course",
        (object) stateId
      });
      NameValueCollection parameters = new NameValueCollection();
      GolfCourseClient.AddPagingOptions(parameters, pagingOptions);
      GolfCourseClient.AddFilterOptions(parameters, filterOptions);
      return await this.GetJsonAsync<GolfCourseSearchResults>(relativeUrl, token, parameters).ConfigureAwait(false) ?? new GolfCourseSearchResults();
    }

    public async Task<GolfCourseSearchResults> GetCoursesByNameAsync(
      string query,
      CancellationToken token,
      GolfCourseSearchPagingOptions pagingOptions = null,
      GolfCourseSearchFilterOptions filterOptions = null)
    {
      string relativeUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/search", new object[1]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0/course"
      });
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add("q", query);
      NameValueCollection parameters = nameValueCollection;
      GolfCourseClient.AddPagingOptions(parameters, pagingOptions);
      GolfCourseClient.AddFilterOptions(parameters, filterOptions);
      return await this.GetJsonAsync<GolfCourseSearchResults>(relativeUrl, token, parameters).ConfigureAwait(false) ?? new GolfCourseSearchResults();
    }

    public Task<GolfCourseDetails> GetCourseDetailsAsync(
      long courseId,
      CancellationToken token)
    {
      return this.GetJsonAsync<GolfCourseDetails>(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/details/{1}", new object[2]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0/course",
        (object) courseId
      }), token, cacheArea: "Golf");
    }

    protected override async Task<HttpRequestMessage> CreateHttpRequestAsync(
      HttpMethod method,
      Uri url,
      CancellationToken cancellationToken)
    {
      HttpRequestMessage httpRequestMessage = await base.CreateHttpRequestAsync(method, url, cancellationToken).ConfigureAwait(false);
      httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
      return httpRequestMessage;
    }

    private static void AddPagingOptions(
      NameValueCollection parameters,
      GolfCourseSearchPagingOptions pagingOptions)
    {
      if (pagingOptions == null)
        return;
      parameters.Add("page", pagingOptions.PageNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      parameters.Add("per_page", pagingOptions.ResultsPerPage.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    private static void AddFilterOptions(
      NameValueCollection parameters,
      GolfCourseSearchFilterOptions filtergOptions)
    {
      if (filtergOptions == null)
        return;
      if (filtergOptions.Type.HasValue)
        parameters.Add("course_type", filtergOptions.Type.Value.ToString("G").ToLowerInvariant());
      if (!filtergOptions.NumberOfHoles.HasValue)
        return;
      parameters.Add("hole_count", filtergOptions.NumberOfHoles.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }
  }
}
