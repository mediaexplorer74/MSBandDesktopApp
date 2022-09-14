// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HealthCloudClientBase
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Configuration;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Tracing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  public abstract class HealthCloudClientBase
  {
    public static readonly TimeSpan TokenRefreshBuffer = TimeSpan.FromMinutes(5.0);
    private static readonly Microsoft.Health.Cloud.Client.Caching.ResponseSharer<IHttpResponseContent> ResponseSharer = new Microsoft.Health.Cloud.Client.Caching.ResponseSharer<IHttpResponseContent>();
    private readonly HttpMessageHandler messageHandler;
    private readonly Func<CancellationToken, Task<Uri>> baseUriSelector;

    protected HealthCloudClientBase(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHttpCacheService cacheService = null)
    {
      this.messageHandler = messageHandler;
      this.baseUriSelector = baseUriSelector;
      this.CacheService = cacheService;
      DefaultHttpTracing defaultHttpTracing = new DefaultHttpTracing();
      this.Trace = (IHttpTracer) defaultHttpTracing;
      this.Configuration = (IHttpTracingConfiguration) new DefaultHttpTracingConfiguration((IHttpTracing) defaultHttpTracing);
    }

    protected HttpMessageHandler MessageHandler => this.messageHandler;

    protected Func<CancellationToken, Task<Uri>> BaseUriSelector => this.baseUriSelector;

    public IHttpTracingConfiguration Configuration { get; private set; }

    protected IHttpTracer Trace { get; private set; }

    protected IHttpCacheService CacheService { get; private set; }

    protected async Task<T> GetJsonAsync<T>(
      string relativeUrl,
      CancellationToken cancellationToken = default (CancellationToken),
      NameValueCollection parameters = null,
      string cacheArea = null,
      IEnumerable<string> cacheTags = null,
      bool logFullUrl = true,
      bool forceCacheUpdate = false)
    {
      return await this.GetJsonAsync<T>(await this.CreateUrlAsync(relativeUrl, cancellationToken, parameters).ConfigureAwait(false), cancellationToken, cacheArea, cacheTags, logFullUrl, forceCacheUpdate).ConfigureAwait(false);
    }

    protected async Task<T> GetJsonAsync<T>(
      Uri url,
      CancellationToken cancellationToken = default (CancellationToken),
      string cacheArea = null,
      IEnumerable<string> cacheTags = null,
      bool logFullUrl = true,
      bool forceCacheUpdate = false)
    {
      T jsonAsync;
      using (HttpRequestMessage request = await this.CreateHttpRequestAsync(HttpMethod.Get, url, cancellationToken).ConfigureAwait(false))
        jsonAsync = await this.GetJsonAsync<T>(request, cancellationToken, cacheArea, cacheTags, logFullUrl, forceCacheUpdate);
      return jsonAsync;
    }

    protected async Task<T> GetJsonAsync<T>(
      HttpRequestMessage request,
      CancellationToken cancellationToken = default (CancellationToken),
      string cacheArea = null,
      IEnumerable<string> cacheTags = null,
      bool logFullUrl = true,
      bool forceCacheUpdate = false)
    {
      if (cacheArea != null)
        return await this.SendWithCachingAsync<T>(request, cancellationToken, cacheArea, cacheTags, logFullUrl, forceCacheUpdate).ConfigureAwait(false);
      using (HttpResponseMessage response = await this.SendAsync(request, cancellationToken, logFullUrl).ConfigureAwait(false))
        return await response.ReadJsonAsync<T>().ConfigureAwait(false);
    }

    protected async Task<IList<T>> GetODataJsonAsync<T>(
      string relativeUrl,
      CancellationToken cancellationToken,
      string cacheArea = null,
      IEnumerable<string> cacheTags = null)
    {
      HttpRequestMessage request = await this.CreateHttpRequestAsync(HttpMethod.Get, await this.CreateUrlAsync(relativeUrl, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
      ODataResponse<IList<T>> odataResponse;
      if (cacheArea == null)
      {
        using (HttpResponseMessage response = await this.SendAsync(request, cancellationToken).ConfigureAwait(false))
          odataResponse = await response.ReadJsonAsync<ODataResponse<IList<T>>>().ConfigureAwait(false);
      }
      else
        odataResponse = await this.SendWithCachingAsync<ODataResponse<IList<T>>>(request, cancellationToken, cacheArea, cacheTags).ConfigureAwait(false);
      return odataResponse.Value;
    }

    protected async Task<Stream> GetResponseStreamAsync(
      string relativeUrl,
      CancellationToken cancellationToken,
      NameValueCollection parameters = null)
    {
      return await (await this.SendAsync(await this.CreateHttpRequestAsync(HttpMethod.Get, await this.CreateUrlAsync(relativeUrl, cancellationToken, parameters).ConfigureAwait(false), cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false)).Content.ReadAsStreamAsync().ConfigureAwait(false);
    }

    protected async Task DeleteAsync(
      string relativeUrl,
      CancellationToken cancellationToken,
      NameValueCollection parameters = null)
    {
      HttpResponseMessage httpResponseMessage = await this.SendAsync(await this.CreateHttpRequestAsync(HttpMethod.Delete, await this.CreateUrlAsync(relativeUrl, cancellationToken, parameters).ConfigureAwait(false), cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
    }

    protected async Task DeleteAsync<T>(
      string relativeUrl,
      NameValueCollection parameters,
      T body,
      CancellationToken cancellationToken)
    {
      HttpResponseMessage httpResponseMessage = await this.SendJsonInternalAsync<T>(relativeUrl, parameters, body, HttpMethod.Delete, string.Empty, string.Empty, cancellationToken).ConfigureAwait(false);
    }

    protected async Task PatchAsync(
      string relativeUrl,
      byte[] package,
      CancellationToken cancellationToken)
    {
      HttpRequestMessage request = await this.CreateHttpRequestAsync(new HttpMethod("PATCH"), await this.CreateUrlAsync(relativeUrl, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
      request.Content = (HttpContent) new ByteArrayContent(package);
      request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      HttpResponseMessage httpResponseMessage = await this.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    protected Task PatchJsonAsync<T>(
      string relativeUrl,
      NameValueCollection parameters,
      T body,
      CancellationToken token)
    {
      return (Task) this.SendJsonInternalAsync<T>(relativeUrl, parameters, body, new HttpMethod("PATCH"), string.Empty, string.Empty, token);
    }

    protected async Task PutAsync(string relativeUrl, CancellationToken cancellationToken)
    {
      HttpResponseMessage httpResponseMessage = await this.SendAsync(await this.CreateHttpRequestAsync(HttpMethod.Put, await this.CreateUrlAsync(relativeUrl, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
    }

    protected Task PutJsonAsync<T>(
      string relativeUrl,
      NameValueCollection parameters,
      T body,
      CancellationToken cancellationToken)
    {
      return (Task) this.SendJsonInternalAsync<T>(relativeUrl, parameters, body, HttpMethod.Put, string.Empty, string.Empty, cancellationToken);
    }

    protected Task<HttpResponseMessage> PostJsonAsync<T>(
      string relativeUrl,
      NameValueCollection parameters,
      T body,
      CancellationToken cancellationToken)
    {
      return this.SendJsonInternalAsync<T>(relativeUrl, parameters, body, HttpMethod.Post, string.Empty, string.Empty, cancellationToken);
    }

    protected Task<HttpResponseMessage> PostJsonAsync<T>(
      string relativeUrl,
      NameValueCollection parameters,
      T body,
      string deviceMetadataString,
      string uploadMetadataString,
      CancellationToken cancellationToken)
    {
      return this.SendJsonInternalAsync<T>(relativeUrl, parameters, body, HttpMethod.Post, deviceMetadataString, uploadMetadataString, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendJsonInternalAsync<T>(
      string relativeUrl,
      NameValueCollection parameters,
      T body,
      HttpMethod method,
      string deviceMetadataString,
      string uploadMetadataString,
      CancellationToken cancellationToken)
    {
      Uri url = await this.CreateUrlAsync(relativeUrl, cancellationToken, parameters).ConfigureAwait(false);
      HttpRequestMessage request = await this.CreateHttpRequestAsync(method, url, cancellationToken).ConfigureAwait(false);
      if (!string.IsNullOrEmpty(deviceMetadataString))
        request.Headers.Add("DeviceMetadataHint", deviceMetadataString);
      if (!string.IsNullOrEmpty(uploadMetadataString))
        request.Headers.Add("UploadMetaData", uploadMetadataString);
      request.SetJsonContent<T>(body);
      return await this.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static Uri CreateUrl(
      Uri baseUrl,
      string relativeUrl,
      NameValueCollection parameters = null)
    {
      Uri uri = new Uri(baseUrl, relativeUrl);
      if (parameters != null && parameters.Count > 0)
        uri = new UriBuilder(uri)
        {
          Query = parameters.ToQueryString()
        }.Uri;
      return uri;
    }

    protected Task<Uri> CreateUrlAsync(
      string relativeUrl,
      CancellationToken cancellationToken,
      NameValueCollection parameters = null)
    {
      return this.CreateUrlAsync(this.baseUriSelector, relativeUrl, cancellationToken, parameters);
    }

    protected async Task<Uri> CreateUrlAsync(
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      string relativeUrl,
      CancellationToken cancellationToken,
      NameValueCollection parameters = null)
    {
      return HealthCloudClientBase.CreateUrl(await baseUriSelector(cancellationToken).ConfigureAwait(false), relativeUrl, parameters);
    }

    protected virtual Task<HttpRequestMessage> CreateHttpRequestAsync(
      HttpMethod method,
      Uri url,
      CancellationToken cancellationToken)
    {
      return Task.FromResult<HttpRequestMessage>(new HttpRequestMessage(method, url)
      {
        Properties = {
          {
            HealthCloudClientRequestContext.KeyName,
            (object) new HealthCloudClientRequestContext()
            {
              RequestId = Guid.NewGuid().ToString("N")
            }
          }
        }
      });
    }

    protected async Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken = default (CancellationToken),
      bool logFullUrl = true)
    {
      this.Trace.HttpRequest(request, logFullUrl);
      HttpResponseMessage response = await new HttpClient(this.MessageHandler).SendAsync(request, cancellationToken).ConfigureAwait(false);
      this.Trace.HttpResponse(response, logFullUrl);
      return response;
    }

    private async Task<T> SendWithCachingAsync<T>(
      HttpRequestMessage request,
      CancellationToken cancellationToken,
      string cacheArea = null,
      IEnumerable<string> cacheTags = null,
      bool logFullUrl = true,
      bool forceCacheUpdate = false)
    {
      Uri url = request.RequestUri;
      if (this.CacheService != null)
      {
        if (forceCacheUpdate)
        {
          await this.CacheService.RemoveAsync(url).ConfigureAwait(false);
        }
        else
        {
          IHttpResponseContent httpResponseContent = await this.CacheService.GetAsync(url).ConfigureAwait(false);
          if (httpResponseContent != null)
          {
            this.Trace.CacheResponse(url, httpResponseContent, logFullUrl);
            return httpResponseContent.ReadJson<T>();
          }
        }
      }
      Func<Task<IHttpResponseContent>> fetcher = (Func<Task<IHttpResponseContent>>) (async () => (IHttpResponseContent) await DefaultHttpResponseContent.CreateAsync(await this.SendAsync(request, cancellationToken, logFullUrl).ConfigureAwait(false)).ConfigureAwait(false));
      IHttpResponseContent response;
      if (this.CacheService != null && cacheArea != null)
        response = await HealthCloudClientBase.ResponseSharer.GetResponseWithSharingAsync(url, fetcher).ConfigureAwait(false);
      else
        response = await fetcher().ConfigureAwait(false);
      T result = response.ReadJson<T>();
      if (this.CacheService != null)
        await this.CacheService.InsertAsync(url, response, cacheArea, cacheTags).ConfigureAwait(false);
      return result;
    }

    protected static string ToIsoDateTime(DateTimeOffset date) => Uri.EscapeDataString(date.ToString("o"));
  }
}
