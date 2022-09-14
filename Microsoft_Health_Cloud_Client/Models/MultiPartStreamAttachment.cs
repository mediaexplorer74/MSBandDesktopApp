// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Models.MultiPartStreamAttachment
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Health.Cloud.Client.Models
{
  public sealed class MultiPartStreamAttachment : IMultiPartAttachment, IDisposable
  {
    public HttpContent Content
    {
      get
      {
        if (this.Data == null)
          return (HttpContent) null;
        StreamContent streamContent = new StreamContent(this.Data);
        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
          Name = "ActivityThumbnail",
          FileName = string.IsNullOrWhiteSpace(this.FileName) ? string.Empty : this.FileName
        };
        streamContent.Headers.ContentType = this.MimeType != null ? MediaTypeHeaderValue.Parse(this.MimeType) : throw new NullReferenceException("MimeType is null for MultiPartStreamAttachment");
        return (HttpContent) streamContent;
      }
    }

    public Stream Data { get; set; }

    public string FileName { get; set; }

    public string MimeType { get; set; }

    public void Dispose()
    {
      if (this.Data == null)
        return;
      this.Data.Dispose();
      this.Data = (Stream) null;
    }
  }
}
