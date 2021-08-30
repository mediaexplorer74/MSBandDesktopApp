// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.TimeSpanTypeInfo
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System;

namespace Microsoft.Diagnostics.Tracing
{
  internal sealed class TimeSpanTypeInfo : TraceLoggingTypeInfo<TimeSpan>
  {
    public override void WriteMetadata(
      TraceLoggingMetadataCollector collector,
      string name,
      EventFieldFormat format)
    {
      collector.AddScalar(name, Statics.MakeDataType(TraceLoggingDataType.Int64, format));
    }

    public override void WriteData(TraceLoggingDataCollector collector, ref TimeSpan value) => collector.AddScalar(value.Ticks);
  }
}
