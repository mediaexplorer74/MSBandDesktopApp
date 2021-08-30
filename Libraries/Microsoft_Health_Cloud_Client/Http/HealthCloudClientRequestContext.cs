// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.HealthCloudClientRequestContext
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;

namespace Microsoft.Health.Cloud.Client.Http
{
  public sealed class HealthCloudClientRequestContext
  {
    private static readonly Lazy<string> KeyNameProvider = new Lazy<string>((Func<string>) (() => typeof (HealthCloudClientRequestContext).FullName));

    public static string KeyName => HealthCloudClientRequestContext.KeyNameProvider.Value;

    public string RequestId { get; set; }

    public string TelemetryPathOverride { get; set; }
  }
}
