// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Telemetry
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Globalization;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal static class Telemetry
  {
    public static void WriteEnvelopeProperties(this ITelemetry telemetry, IJsonWriter json)
    {
      json.WriteProperty("time", new DateTimeOffset?(telemetry.Timestamp));
      json.WriteProperty("seq", telemetry.Sequence);
      ((IJsonSerializable) telemetry.Context).Serialize(json);
    }

    public static void WriteTelemetryName(
      this ITelemetry telemetry,
      IJsonWriter json,
      string telemetryName)
    {
      bool result = false;
      string str1;
      if (telemetry is ISupportProperties supportProperties && supportProperties.Properties.TryGetValue("DeveloperMode", out str1))
        bool.TryParse(str1, out result);
      string str2 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}{2}", result ? (object) "Microsoft.ApplicationInsights.Dev." : (object) "Microsoft.ApplicationInsights.", (object) Telemetry.NormalizeInstrumentationKey(telemetry.Context.InstrumentationKey), (object) telemetryName);
      json.WriteProperty("name", str2);
    }

    private static string NormalizeInstrumentationKey(string instrumentationKey) => instrumentationKey.IsNullOrWhiteSpace() ? string.Empty : instrumentationKey.Replace("-", string.Empty).ToLowerInvariant() + ".";
  }
}
