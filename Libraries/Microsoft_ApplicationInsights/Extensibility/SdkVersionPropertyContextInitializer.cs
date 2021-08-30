// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.SdkVersionPropertyContextInitializer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Microsoft.ApplicationInsights.Extensibility
{
  internal sealed class SdkVersionPropertyContextInitializer : IContextInitializer
  {
    private const string SDKVersion = "SDKVersion";
    private string sdkVersion;

    public void Initialize(TelemetryContext context)
    {
      string str = LazyInitializer.EnsureInitialized<string>(ref this.sdkVersion, new Func<string>(this.GetAssemblyVersion));
      if (!string.IsNullOrEmpty(context.Internal.SdkVersion))
        return;
      context.Internal.SdkVersion = str;
    }

    private string GetAssemblyVersion() => Enumerable.OfType<AssemblyFileVersionAttribute>(typeof (SdkVersionPropertyContextInitializer).Assembly.GetCustomAttributes(false)).First<AssemblyFileVersionAttribute>().Version;
  }
}
