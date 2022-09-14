// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.ProviderLogLevelExtensions
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  internal static class ProviderLogLevelExtensions
  {
    internal static LogLevel ToLogLevel(this ProviderLogLevel level)
    {
      switch (level)
      {
        case ProviderLogLevel.Off:
          return LogLevel.Off;
        case ProviderLogLevel.Fatal:
          return LogLevel.Fatal;
        case ProviderLogLevel.Error:
          return LogLevel.Error;
        case ProviderLogLevel.Warning:
          return LogLevel.Warning;
        case ProviderLogLevel.Info:
          return LogLevel.Info;
        case ProviderLogLevel.Performance:
          return LogLevel.Performance;
        case ProviderLogLevel.Verbose:
          return LogLevel.Verbose;
        default:
          throw new ArgumentException("Unknown LogLevel value.");
      }
    }
  }
}
