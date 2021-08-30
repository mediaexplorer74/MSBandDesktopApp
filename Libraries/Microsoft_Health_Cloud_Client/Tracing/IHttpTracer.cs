// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Tracing.IHttpTracer
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Net.Http;

namespace Microsoft.Health.Cloud.Client.Tracing
{
  public interface IHttpTracer
  {
    void HttpRequest(HttpRequestMessage request, bool logFullUrl);

    void HttpResponse(HttpResponseMessage response, bool logFullUrl);

    void CacheResponse(Uri url, IHttpResponseContent responseContent, bool logFullUrl);
  }
}
