// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.HttpLoggingHandler
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public sealed class HttpLoggingHandler : DelegatingHandler
  {
    public HttpLoggingHandler(HttpMessageHandler innerHandler)
      : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      DefaultHttpTransaction transaction = (DefaultHttpTransaction) null;
      if (HttpLogging.IsLoggingEnabled)
        transaction = new DefaultHttpTransaction();
      if (transaction != null)
        await HttpLogging.OnHttpRequestBeforeSubmitAsync(transaction, request).ConfigureAwait(false);
      HttpResponseMessage response = (HttpResponseMessage) null;
      try
      {
        response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (HttpLoggingHandler.IsUnreachableHostResponse(response))
          throw new HttpRequestException(string.Format("HTTP request to host {0} failed, host is unreachable.", new object[1]
          {
            (object) request.RequestUri.Host
          }));
      }
      catch (Exception ex)
      {
        if (transaction != null)
          HttpLogging.OnHttpRequestFailed(transaction, ex);
        throw;
      }
      if (transaction != null)
      {
        ConfiguredTaskAwaitable configuredTaskAwaitable;
        if (response.Content != null)
        {
          configuredTaskAwaitable = response.Content.LoadIntoBufferAsync().ConfigureAwait(false);
          await configuredTaskAwaitable;
        }
        configuredTaskAwaitable = HttpLogging.OnHttpResponseReceivedAsync(transaction, response).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
      if (response.IsSuccessStatusCode)
        return response;
      switch (response.StatusCode)
      {
        case HttpStatusCode.NotFound:
          throw new NotFoundException("Content not found.");
        case HttpStatusCode.NotAcceptable:
          throw new NotAcceptableException("Request not acceptable.");
        case HttpStatusCode.Conflict:
          throw new ConflictException("Request conflict");
        default:
          throw new HealthCloudServerException(string.Format("Request status: {0} {1}", new object[2]
          {
            (object) response.StatusCode,
            (object) response.ReasonPhrase
          }));
      }
    }

    private static bool IsUnreachableHostResponse(HttpResponseMessage response)
    {
      if (response.StatusCode == HttpStatusCode.NotFound)
      {
        long? contentLength = response.Content.Headers.ContentLength;
        long num = 0;
        if ((contentLength.GetValueOrDefault() == num ? (contentLength.HasValue ? 1 : 0) : 0) != 0)
          return response.Content.Headers.ContentType == null;
      }
      return false;
    }
  }
}
