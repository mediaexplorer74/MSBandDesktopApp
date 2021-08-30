// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.PropertyAccessor`1
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System;

namespace Microsoft.Diagnostics.Tracing
{
  internal abstract class PropertyAccessor<ContainerType>
  {
    public abstract void Write(TraceLoggingDataCollector collector, ref ContainerType value);

    public abstract object GetData(ContainerType value);

    public static PropertyAccessor<ContainerType> Create(PropertyAnalysis property)
    {
      Type returnType = property.getterInfo.ReturnType;
      if (!Statics.IsValueType(typeof (ContainerType)))
      {
        if ((object) returnType == (object) typeof (int))
          return (PropertyAccessor<ContainerType>) new ClassPropertyWriter<ContainerType, int>(property);
        if ((object) returnType == (object) typeof (long))
          return (PropertyAccessor<ContainerType>) new ClassPropertyWriter<ContainerType, long>(property);
        if ((object) returnType == (object) typeof (string))
          return (PropertyAccessor<ContainerType>) new ClassPropertyWriter<ContainerType, string>(property);
      }
      return (PropertyAccessor<ContainerType>) new NonGenericProperytWriter<ContainerType>(property);
    }
  }
}
