// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.WebUtil
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopSyncApp
{
  public static class WebUtil
  {
    private static readonly string[] ContentTypeNamesJson = new string[3]
    {
      "text/json",
      "application/json",
      "text/javascript"
    };

    public static void VerifyContentType(
      WebHeaderCollection headers,
      IEnumerable<string> requiredMediaTypes)
    {
      string contentType1 = headers.Get("Content-Type");
      if (contentType1 == null || contentType1 == "")
        return;
      ContentType contentType2 = new ContentType(contentType1);
      string str1 = (string) null;
      string lower = contentType2.MediaType.ToLower();
      string str2 = "";
      if (contentType2.CharSet != null)
        str2 = contentType2.CharSet.ToLower();
      foreach (string requiredMediaType in requiredMediaTypes)
      {
        if (str1 == null)
          str1 = requiredMediaType;
        if (requiredMediaType.ToLower() == lower)
          goto label_16;
      }
      if (str1 != null)
        throw new Exception(string.Format("Unsupported content media type; require \"{0}\"", (object) str1));
label_16:
      if (str2 != "" && str2 != "utf-8")
        throw new Exception(string.Format("Unsupported character set; require UTF-8 or unspecified"));
    }

    public static void VerifyContentTypeIsJson(WebHeaderCollection headers) => WebUtil.VerifyContentType(headers, (IEnumerable<string>) WebUtil.ContentTypeNamesJson);

    public static async Task<HttpResponseMessage> DownloadFile(
      Uri downloadFileUrl,
      IDictionary<string, string> headers,
      TimeSpan timeout)
    {
      string empty = string.Empty;
      HttpResponseMessage httpResponseMessage = (HttpResponseMessage) null;
      using (HttpClient client = new HttpClient())
      {
        client.Timeout = timeout;
        using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, downloadFileUrl))
        {
          foreach (KeyValuePair<string, string> header in (IEnumerable<KeyValuePair<string, string>>) headers)
            requestMessage.Headers.Add(header.Key, header.Value);
          if (!requestMessage.Headers.Contains("User-Agent"))
            requestMessage.Headers.Add("User-Agent", Globals.DefaultUserAgent);
          httpResponseMessage = await WebUtil.Send(client, requestMessage, CancellationToken.None);
        }
      }
      return httpResponseMessage;
    }

    private static async Task<HttpResponseMessage> Send(
      HttpClient client,
      HttpRequestMessage requestMessage,
      CancellationToken cancellationToken)
    {
      HttpResponseMessage httpResponseMessage;
      try
      {
        httpResponseMessage = await client.SendAsync(requestMessage, cancellationToken);
      }
      catch (AggregateException ex)
      {
        if (ex.InnerExceptions.Count == 1)
        {
          if (ex.InnerException is TaskCanceledException)
          {
            cancellationToken.ThrowIfCancellationRequested();
            throw new TimeoutException();
          }
          throw ex.InnerException;
        }
        throw;
      }
      return httpResponseMessage;
    }
  }
}
