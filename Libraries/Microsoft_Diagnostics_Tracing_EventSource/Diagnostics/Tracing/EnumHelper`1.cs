// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EnumHelper`1
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
  internal static class EnumHelper<UnderlyingType>
  {
    private static readonly MethodInfo IdentityInfo = Statics.GetDeclaredStaticMethod(typeof (EnumHelper<UnderlyingType>), "Identity");

    public static UnderlyingType Cast<ValueType>(ValueType value) => EnumHelper<UnderlyingType>.Caster<ValueType>.Instance(value);

    internal static UnderlyingType Identity(UnderlyingType value) => value;

    private delegate UnderlyingType Transformer<ValueType>(ValueType value);

    private static class Caster<ValueType>
    {
      public static readonly EnumHelper<UnderlyingType>.Transformer<ValueType> Instance = (EnumHelper<UnderlyingType>.Transformer<ValueType>) Statics.CreateDelegate(typeof (EnumHelper<UnderlyingType>.Transformer<ValueType>), EnumHelper<UnderlyingType>.IdentityInfo);
    }
  }
}
