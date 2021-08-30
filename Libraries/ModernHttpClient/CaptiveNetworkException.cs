// Decompiled with JetBrains decompiler
// Type: ModernHttpClient.CaptiveNetworkException
// Assembly: ModernHttpClient, Version=2.4.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6217D996-1B38-42C3-A52D-8A884E871EC8
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\ModernHttpClient.dll

using System;
using System.Net;

namespace ModernHttpClient
{
  public class CaptiveNetworkException : WebException
  {
    private const string DefaultCaptiveNetworkErrorMessage = "Hostnames don't match, you are probably on a captive network";

    public CaptiveNetworkException(Uri sourceUri, Uri destinationUri)
      : base("Hostnames don't match, you are probably on a captive network")
    {
      this.SourceUri = sourceUri;
      this.DestinationUri = destinationUri;
    }

    public Uri SourceUri { get; private set; }

    public Uri DestinationUri { get; private set; }
  }
}
