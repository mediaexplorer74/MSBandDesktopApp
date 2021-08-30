// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Channel.Transmission
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Channel
{
  internal class Transmission
  {
    private const string ContentTypeHeader = "Content-Type";
    private const string ContentEncodingHeader = "Content-Encoding";
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100.0);
    private readonly Uri address;
    private readonly byte[] content;
    private readonly string contentType;
    private readonly string contentEncoding;
    private readonly TimeSpan timeout;
    private int isSending;

    public Transmission(
      Uri address,
      byte[] content,
      string contentType,
      string contentEncoding,
      TimeSpan timeout = default (TimeSpan))
    {
      if (address == (Uri) null)
        throw new ArgumentNullException(nameof (address));
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (contentType == null)
        throw new ArgumentNullException(nameof (contentType));
      this.address = address;
      this.content = content;
      this.contentType = contentType;
      this.contentEncoding = contentEncoding;
      this.timeout = timeout == new TimeSpan() ? Transmission.DefaultTimeout : timeout;
    }

    protected Transmission()
    {
    }

    public Uri EndpointAddress => this.address;

    public byte[] Content => this.content;

    public string ContentType => this.contentType;

    public string ContentEncoding => this.contentEncoding;

    public TimeSpan Timeout => this.timeout;

    public static async Task<Transmission> LoadAsync(Stream stream)
    {
      StreamReader reader = new StreamReader(stream);
      Uri address = await Transmission.ReadAddressAsync((TextReader) reader).ConfigureAwait<Uri>(false);
      string contentType = await Transmission.ReadHeaderAsync((TextReader) reader, "Content-Type").ConfigureAwait<string>(false);
      string contentEncoding = await Transmission.ReadHeaderAsync((TextReader) reader, "Content-Encoding").ConfigureAwait<string>(false);
      byte[] content = await Transmission.ReadContentAsync((TextReader) reader).ConfigureAwait<byte[]>(false);
      return new Transmission(address, content, contentType, contentEncoding);
    }

    public virtual async Task SaveAsync(Stream stream)
    {
      StreamWriter writer = new StreamWriter(stream);
      try
      {
        await AsyncExtensions.WriteLineAsync((TextWriter) writer, this.address.ToString()).ConfigureAwait(false);
        await AsyncExtensions.WriteLineAsync((TextWriter) writer, "Content-Type:" + this.contentType).ConfigureAwait(false);
        await AsyncExtensions.WriteLineAsync((TextWriter) writer, "Content-Encoding:" + this.contentEncoding).ConfigureAwait(false);
        await AsyncExtensions.WriteLineAsync((TextWriter) writer, string.Empty).ConfigureAwait(false);
        await AsyncExtensions.WriteAsync((TextWriter) writer, Convert.ToBase64String(this.content)).ConfigureAwait(false);
      }
      finally
      {
        writer.Flush();
      }
    }

    public virtual async Task SendAsync()
    {
      if (Interlocked.CompareExchange(ref this.isSending, 1, 0) != 0)
        throw new InvalidOperationException("SendAsync is already in progress.");
      try
      {
        WebRequest request = this.CreateRequest(this.EndpointAddress);
        Task timeoutTask = TaskEx.Delay((TimeSpan) this.timeout);
        Task sendTask = this.SendRequestAsync(request);
        Task completedTask = await TaskEx.WhenAny(timeoutTask, sendTask).ConfigureAwait<Task>(false);
        if (completedTask == timeoutTask)
          request.Abort();
        await sendTask.ConfigureAwait(false);
      }
      finally
      {
        Interlocked.Exchange(ref this.isSending, 0);
      }
    }

    protected static async Task<string> ReadHeaderAsync(TextReader reader, string headerName)
    {
      string line = await AsyncExtensions.ReadLineAsync((TextReader) reader).ConfigureAwait<string>(false);
      string[] parts = !string.IsNullOrEmpty(line) ? line.Split(':') : throw new FormatException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} header is expected.", (object) headerName));
      if (parts.Length != 2)
        throw new FormatException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unexpected header format. {0} header is expected. Actual header: {1}", (object) headerName, (object) line));
      return !(parts[0] != headerName) ? parts[1].Trim() : throw new FormatException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} header is expected. Actual header: {1}", (object) headerName, (object) line));
    }

    protected virtual WebRequest CreateRequest(Uri address)
    {
      WebRequest webRequest = WebRequest.Create(address);
      webRequest.Method = "POST";
      if (!string.IsNullOrEmpty(this.contentType))
        webRequest.ContentType = this.contentType;
      if (!string.IsNullOrEmpty(this.contentEncoding))
        webRequest.Headers["Content-Encoding"] = this.contentEncoding;
      webRequest.ContentLength = (long) this.content.Length;
      return webRequest;
    }

    private static async Task<Uri> ReadAddressAsync(TextReader reader)
    {
      string addressLine = await AsyncExtensions.ReadLineAsync((TextReader) reader).ConfigureAwait<string>(false);
      Uri address = !string.IsNullOrEmpty(addressLine) ? new Uri(addressLine) : throw new FormatException("Transmission address is expected.");
      return address;
    }

    private static async Task<byte[]> ReadContentAsync(TextReader reader)
    {
      string content = await AsyncExtensions.ReadToEndAsync((TextReader) reader).ConfigureAwait<string>(false);
      return !string.IsNullOrEmpty(content) && !(content == Environment.NewLine) ? Convert.FromBase64String(content) : throw new FormatException("Content is expected.");
    }

    private async Task SendRequestAsync(WebRequest request)
    {
      using (Stream requestStream = await ((Task<Stream>) request.GetRequestStreamAsync()).ConfigureAwait<Stream>(false))
        await AsyncExtensions.WriteAsync((Stream) requestStream, this.content, 0, this.content.Length).ConfigureAwait(false);
      using (await request.GetResponseAsync().ConfigureAwait<WebResponse>(false))
        ;
    }
  }
}
