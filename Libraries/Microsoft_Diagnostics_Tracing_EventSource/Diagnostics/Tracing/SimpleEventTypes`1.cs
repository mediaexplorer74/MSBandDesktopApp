// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.SimpleEventTypes`1
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
  internal class SimpleEventTypes<T> : TraceLoggingEventTypes
  {
    private static SimpleEventTypes<T> instance;
    internal readonly TraceLoggingTypeInfo<T> typeInfo;

    private SimpleEventTypes(TraceLoggingTypeInfo<T> typeInfo)
      : base(typeInfo.Name, typeInfo.Tags, (TraceLoggingTypeInfo) typeInfo)
    {
      this.typeInfo = typeInfo;
    }

    public static SimpleEventTypes<T> Instance => SimpleEventTypes<T>.instance ?? SimpleEventTypes<T>.InitInstance();

    private static SimpleEventTypes<T> InitInstance()
    {
      SimpleEventTypes<T> simpleEventTypes = new SimpleEventTypes<T>(TraceLoggingTypeInfo<T>.Instance);
      Interlocked.CompareExchange<SimpleEventTypes<T>>(ref SimpleEventTypes<T>.instance, simpleEventTypes, (SimpleEventTypes<T>) null);
      return SimpleEventTypes<T>.instance;
    }
  }
}
