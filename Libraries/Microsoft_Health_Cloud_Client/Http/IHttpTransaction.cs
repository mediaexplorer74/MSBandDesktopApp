// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.IHttpTransaction
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.IO;
using System.Net.Http;

namespace Microsoft.Health.Cloud.Client.Http
{
  public interface IHttpTransaction
  {
    DateTimeOffset? BeginRequestTime { get; }

    DateTimeOffset? GotResponseHeadersTime { get; }

    DateTimeOffset? DoneResponseTime { get; }

    HttpRequestMessage Request { get; }

    HttpResponseMessage Response { get; }

    Exception Error { get; }

    Stream OpenRequestStream();

    Stream OpenResponseStream();

    TextReader OpenRequestReader();

    TextReader OpenResponseReader();
  }
}
