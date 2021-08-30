// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.MultiPartByteArrayAttachment
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Net.Http;

namespace Microsoft.Health.Cloud.Client.Http
{
  public class MultiPartByteArrayAttachment : IMultiPartAttachment, IDisposable
  {
    public HttpContent Content { get; set; }

    public string FileName { get; set; }

    public string MimeType { get; set; }

    public void Dispose()
    {
      if (this.Content == null)
        return;
      this.Content.Dispose();
      this.Content = (HttpContent) null;
    }
  }
}
