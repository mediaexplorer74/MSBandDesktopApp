// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.BandHttpException
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  public class BandHttpException : BandCloudException
  {
    internal BandHttpException(string responseContent, string message, Exception innerException)
      : base(message, innerException)
    {
      this.ResponseContent = responseContent;
    }

    internal string ResponseContent { get; private set; }
  }
}
