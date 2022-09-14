// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Extensions.MultipartFormDataContentExtensions
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Health.Cloud.Client.Extensions
{
  public static class MultipartFormDataContentExtensions
  {
    public const string ContentDispositionHeaderName = "form-data";

    public static void AddStringFormDataAttachmentIfNotNull(
      this MultipartFormDataContent content,
      string name,
      string body)
    {
      if (body == null)
        return;
      content.Add(MultipartFormDataContentExtensions.CreateStringFormDataAttachment(name, body));
    }

    public static void AddStreamFormDataAttachmentIfNotNull(
      this MultipartFormDataContent content,
      string name,
      MultiPartStreamAttachment body)
    {
      if (body == null)
        return;
      content.Add(MultipartFormDataContentExtensions.CreateStreamFormDataAttachment(name, body));
    }

    private static HttpContent CreateStringFormDataAttachment(string name, string body)
    {
      StringContent stringContent = new StringContent(body);
      stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
      {
        Name = name
      };
      return (HttpContent) stringContent;
    }

    private static HttpContent CreateStreamFormDataAttachment(
      string name,
      MultiPartStreamAttachment body)
    {
      StreamContent streamContent = new StreamContent(body.Data);
      streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
      {
        Name = name,
        FileName = string.IsNullOrWhiteSpace(body.FileName) ? string.Empty : body.FileName
      };
      streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(body.MimeType);
      return (HttpContent) streamContent;
    }
  }
}
