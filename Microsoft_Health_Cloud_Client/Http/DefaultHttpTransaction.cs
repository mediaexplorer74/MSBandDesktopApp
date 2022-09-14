// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.DefaultHttpTransaction
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  internal class DefaultHttpTransaction : IHttpTransaction
  {
    private HttpRequestMessage request;
    private HttpResponseMessage response;
    private MemoryStream requestStream;
    private MemoryStream responseStream;

    public async Task SetRequestAsync(HttpRequestMessage httpRequest)
    {
      this.request = httpRequest;
      if (this.request.Content == null)
        return;
      this.requestStream = new MemoryStream();
      await this.request.Content.CopyToAsync((Stream) this.requestStream);
    }

    public async Task SetResponseAsync(HttpResponseMessage httpResponse)
    {
      this.response = httpResponse;
      this.GotResponseHeadersTime = new DateTimeOffset?(DateTimeOffset.Now);
      this.responseStream = new MemoryStream();
      await this.response.Content.CopyToAsync((Stream) this.responseStream);
    }

    public DateTimeOffset? BeginRequestTime { get; set; }

    public DateTimeOffset? GotResponseHeadersTime { get; set; }

    public DateTimeOffset? DoneResponseTime { get; set; }

    public HttpRequestMessage Request => this.request;

    public HttpResponseMessage Response => this.response;

    public Exception Error { get; set; }

    public Stream OpenRequestStream()
    {
      if (this.requestStream == null)
        return (Stream) null;
      this.requestStream.Seek(0L, SeekOrigin.Begin);
      return (Stream) this.requestStream;
    }

    public Stream OpenResponseStream()
    {
      if (this.response == null)
        return (Stream) null;
      this.responseStream.Seek(0L, SeekOrigin.Begin);
      return (Stream) this.responseStream;
    }

    public TextReader OpenRequestReader()
    {
      Stream stream = this.OpenRequestStream();
      if (stream == null)
        return (TextReader) null;
      Encoding encoding = DefaultHttpTransaction.GetEncoding(this.request);
      return encoding == null ? (TextReader) new StreamReader(stream, true) : (TextReader) new StreamReader(stream, encoding);
    }

    public TextReader OpenResponseReader()
    {
      Stream stream = this.OpenResponseStream();
      if (stream == null)
        return (TextReader) null;
      Encoding encoding = DefaultHttpTransaction.GetEncoding(this.response);
      return encoding == null ? (TextReader) new StreamReader(stream, true) : (TextReader) new StreamReader(stream, encoding);
    }

    private static Encoding GetEncoding(HttpRequestMessage request)
    {
      string name = request.Content.Headers.ContentEncoding.FirstOrDefault<string>();
      return name == null ? (Encoding) null : Encoding.GetEncoding(name);
    }

    private static Encoding GetEncoding(HttpResponseMessage response) => Encoding.GetEncoding(response.Content.Headers.ContentType.CharSet);
  }
}
