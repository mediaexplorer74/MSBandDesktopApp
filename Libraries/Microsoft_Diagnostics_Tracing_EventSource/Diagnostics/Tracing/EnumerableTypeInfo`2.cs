// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EnumerableTypeInfo`2
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
  internal sealed class EnumerableTypeInfo<IterableType, ElementType> : 
    TraceLoggingTypeInfo<IterableType>
    where IterableType : IEnumerable<ElementType>
  {
    private readonly TraceLoggingTypeInfo<ElementType> elementInfo;

    public EnumerableTypeInfo(TraceLoggingTypeInfo<ElementType> elementInfo) => this.elementInfo = elementInfo;

    public override void WriteMetadata(
      TraceLoggingMetadataCollector collector,
      string name,
      EventFieldFormat format)
    {
      collector.BeginBufferedArray();
      this.elementInfo.WriteMetadata(collector, name, format);
      collector.EndBufferedArray();
    }

    public override void WriteData(TraceLoggingDataCollector collector, ref IterableType value)
    {
      int bookmark = collector.BeginBufferedArray();
      int count = 0;
      if ((object) value != null)
      {
        foreach (ElementType elementType in value)
        {
          this.elementInfo.WriteData(collector, ref elementType);
          ++count;
        }
      }
      collector.EndBufferedArray(bookmark, count);
    }

    public override object GetData(object value)
    {
      IterableType iterableType = (IterableType) value;
      List<object> objectList = new List<object>();
      foreach (ElementType elementType in iterableType)
        objectList.Add(this.elementInfo.GetData((object) elementType));
      return (object) objectList.ToArray();
    }
  }
}
