// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.TypedInjectionValue
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public abstract class TypedInjectionValue : InjectionParameterValue
  {
    private readonly ReflectionHelper parameterReflector;

    protected TypedInjectionValue(Type parameterType) => this.parameterReflector = new ReflectionHelper(parameterType);

    public virtual Type ParameterType => this.parameterReflector.Type;

    public override string ParameterTypeName => this.parameterReflector.Type.GetTypeInfo().Name;

    public override bool MatchesType(Type t)
    {
      Guard.ArgumentNotNull((object) t, nameof (t));
      ReflectionHelper reflectionHelper = new ReflectionHelper(t);
      return reflectionHelper.IsOpenGeneric && this.parameterReflector.IsOpenGeneric ? (object) reflectionHelper.Type.GetGenericTypeDefinition() == (object) this.parameterReflector.Type.GetGenericTypeDefinition() : t.GetTypeInfo().IsAssignableFrom(this.parameterReflector.Type.GetTypeInfo());
    }
  }
}
