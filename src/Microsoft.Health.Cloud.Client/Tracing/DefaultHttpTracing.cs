// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Tracing.DefaultHttpTracing
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.Health.Cloud.Client.Tracing
{
  internal class DefaultHttpTracing : IHttpTracing, IHttpTracer
  {
    private readonly List<IHttpTracer> interceptors;

    public DefaultHttpTracing() => this.interceptors = new List<IHttpTracer>();

    public void AddTracingInterceptor(IHttpTracer interceptor)
    {
      if (interceptor == null)
        throw new ArgumentNullException(nameof (interceptor));
      this.interceptors.Add(interceptor);
    }

    public void HttpRequest(HttpRequestMessage request, bool logFullUrl)
    {
      foreach (IHttpTracer interceptor in this.interceptors)
      {
        try
        {
          interceptor.HttpRequest(request, logFullUrl);
        }
        catch (Exception ex)
        {
        }
      }
    }

    public void HttpResponse(HttpResponseMessage response, bool logFullUrl)
    {
      foreach (IHttpTracer interceptor in this.interceptors)
      {
        try
        {
          interceptor.HttpResponse(response, logFullUrl);
        }
        catch (Exception ex)
        {
        }
      }
    }

    public void CacheResponse(Uri url, IHttpResponseContent responseContent, bool logFullUrl)
    {
      foreach (IHttpTracer interceptor in this.interceptors)
      {
        try
        {
          interceptor.CacheResponse(url, responseContent, logFullUrl);
        }
        catch (Exception ex)
        {
        }
      }
    }
  }
}
