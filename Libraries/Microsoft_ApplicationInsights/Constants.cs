// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Constants
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

namespace Microsoft.ApplicationInsights
{
  internal class Constants
  {
    internal const string TelemetryServiceEndpoint = "https://dc.services.visualstudio.com/v2/track";
    internal const string TelemetryNamePrefix = "Microsoft.ApplicationInsights.";
    internal const string DevModeTelemetryNamePrefix = "Microsoft.ApplicationInsights.Dev.";
    internal const string TelemetryGroup = "{0d943590-b235-5bdb-f854-89520f32fc0b}";
    internal const string DevModeTelemetryGroup = "{ba84f32b-8af2-5006-f147-5030cdd7f22d}";
    internal const string EventSourceGroupTraitKey = "ETW_GROUP";
    internal const int MaxExceptionCountToSave = 10;
  }
}
