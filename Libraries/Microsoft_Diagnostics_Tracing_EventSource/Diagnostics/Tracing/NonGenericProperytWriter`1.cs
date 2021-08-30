// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.NonGenericProperytWriter`1
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
  internal class NonGenericProperytWriter<ContainerType> : PropertyAccessor<ContainerType>
  {
    private readonly TraceLoggingTypeInfo typeInfo;
    private readonly MethodInfo getterInfo;

    public NonGenericProperytWriter(PropertyAnalysis property)
    {
      this.getterInfo = property.getterInfo;
      this.typeInfo = property.typeInfo;
    }

    public override void Write(TraceLoggingDataCollector collector, ref ContainerType container)
    {
      object obj = (object) container == null ? (object) null : this.getterInfo.Invoke((object) container, (object[]) null);
      this.typeInfo.WriteObjectData(collector, obj);
    }

    public override object GetData(ContainerType container) => (object) container != null ? this.getterInfo.Invoke((object) container, (object[]) null) : (object) null;
  }
}
