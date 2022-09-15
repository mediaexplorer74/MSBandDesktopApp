// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.DefaultUserAgentService
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Services;
using System;
using System.Reflection;

namespace Microsoft.Health.Cloud.Client
{
  public class DefaultUserAgentService : IUserAgentService
  {
    private static readonly Lazy<string> DefaultUserAgent = new Lazy<string>(new Func<string>(DefaultUserAgentService.GetDefaultUserAgent));

    public string UserAgent => DefaultUserAgentService.DefaultUserAgent.Value;

    private static string GetDefaultUserAgent()
    {
      Type type = typeof (HealthCloudClient);
      string version = type.GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
      return string.Format("X-Client/{0} X-Client-AppVersion/{1}", new object[2]
      {
        (object) type.FullName,
        (object) version
      });
    }
  }
}
