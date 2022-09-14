// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Logging.CloudTracingInterceptor
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Tracing;
using System;
using System.Net.Http;

namespace Microsoft.Health.App.Core.Utilities.Logging
{
  public class CloudTracingInterceptor : IHttpTracer
  {
    private const string RequestFormat = "Requesting {0}...";
    private const string ResponseFormat = "Received {0} Response For {1}.";
    private const string CacheFormat = "Using Cached Response Content For {0}.";
    private readonly ILog logger;

    public CloudTracingInterceptor(string logName) => this.logger = LogManager.GetLogger(logName);

    public void HttpRequest(HttpRequestMessage request, bool logFullUrl) => this.logger.InfoFormat("Requesting {0}...", (object) CloudTracingInterceptor.GetCleanUrl(request.RequestUri, logFullUrl));

    public void HttpResponse(HttpResponseMessage response, bool logFullUrl) => this.logger.InfoFormat("Received {0} Response For {1}.", (object) response.StatusCode, (object) CloudTracingInterceptor.GetCleanUrl(response.RequestMessage.RequestUri, logFullUrl));

    public void CacheResponse(Uri url, IHttpResponseContent responseContent, bool logFullUrl) => this.logger.InfoFormat("Using Cached Response Content For {0}.", (object) CloudTracingInterceptor.GetCleanUrl(url, logFullUrl));

    private static string GetCleanUrl(Uri url, bool logFullUrl) => url.Host;
  }
}
