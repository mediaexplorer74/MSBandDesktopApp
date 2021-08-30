// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.PropertyAnalysis
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
  internal sealed class PropertyAnalysis
  {
    internal readonly string name;
    internal readonly MethodInfo getterInfo;
    internal readonly TraceLoggingTypeInfo typeInfo;
    internal readonly EventFieldAttribute fieldAttribute;

    public PropertyAnalysis(
      string name,
      MethodInfo getterInfo,
      TraceLoggingTypeInfo typeInfo,
      EventFieldAttribute fieldAttribute)
    {
      this.name = name;
      this.getterInfo = getterInfo;
      this.typeInfo = typeInfo;
      this.fieldAttribute = fieldAttribute;
    }
  }
}
