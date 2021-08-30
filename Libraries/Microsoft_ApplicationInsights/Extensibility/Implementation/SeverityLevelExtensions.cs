// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.SeverityLevelExtensions
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal static class SeverityLevelExtensions
  {
    public static Microsoft.ApplicationInsights.DataContracts.SeverityLevel? TranslateSeverityLevel(
      this Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel? sdkSeverityLevel)
    {
      if (!sdkSeverityLevel.HasValue)
        return new Microsoft.ApplicationInsights.DataContracts.SeverityLevel?();
      switch (sdkSeverityLevel.Value)
      {
        case Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Information:
          return new Microsoft.ApplicationInsights.DataContracts.SeverityLevel?(Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
        case Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Warning:
          return new Microsoft.ApplicationInsights.DataContracts.SeverityLevel?(Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning);
        case Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Error:
          return new Microsoft.ApplicationInsights.DataContracts.SeverityLevel?(Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error);
        case Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Critical:
          return new Microsoft.ApplicationInsights.DataContracts.SeverityLevel?(Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Critical);
        default:
          return new Microsoft.ApplicationInsights.DataContracts.SeverityLevel?(Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Verbose);
      }
    }

    public static Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel? TranslateSeverityLevel(
      this Microsoft.ApplicationInsights.DataContracts.SeverityLevel? dataPlatformSeverityLevel)
    {
      if (!dataPlatformSeverityLevel.HasValue)
        return new Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel?();
      switch (dataPlatformSeverityLevel.Value)
      {
        case Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information:
          return new Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel?(Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Information);
        case Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning:
          return new Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel?(Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Warning);
        case Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error:
          return new Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel?(Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Error);
        case Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Critical:
          return new Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel?(Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Critical);
        default:
          return new Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel?(Microsoft.ApplicationInsights.Extensibility.Implementation.External.SeverityLevel.Verbose);
      }
    }
  }
}
