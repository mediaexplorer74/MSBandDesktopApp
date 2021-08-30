// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.InjectionParameterValue
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity
{
  public abstract class InjectionParameterValue
  {
    public abstract string ParameterTypeName { get; }

    public abstract bool MatchesType(Type t);

    public abstract IDependencyResolverPolicy GetResolverPolicy(
      Type typeToBuild);

    public static IEnumerable<InjectionParameterValue> ToParameters(
      params object[] values)
    {
      foreach (object obj in values)
        yield return InjectionParameterValue.ToParameter(obj);
    }

    public static InjectionParameterValue ToParameter(object value)
    {
      if (value is InjectionParameterValue injectionParameterValue)
        return injectionParameterValue;
      Type parameterType = value as Type;
      return (object) parameterType != null ? (InjectionParameterValue) new ResolvedParameter(parameterType) : (InjectionParameterValue) new InjectionParameter(value);
    }
  }
}
