// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.InjectionParameter
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Properties;
using System;

namespace Microsoft.Practices.Unity
{
  public class InjectionParameter : TypedInjectionValue
  {
    private readonly object parameterValue;

    public InjectionParameter(object parameterValue)
      : this(InjectionParameter.GetParameterType(parameterValue), parameterValue)
    {
    }

    private static Type GetParameterType(object parameterValue) => parameterValue != null ? parameterValue.GetType() : throw new ArgumentNullException(nameof (parameterValue), Resources.ExceptionNullParameterValue);

    public InjectionParameter(Type parameterType, object parameterValue)
      : base(parameterType)
    {
      this.parameterValue = parameterValue;
    }

    public override IDependencyResolverPolicy GetResolverPolicy(
      Type typeToBuild)
    {
      return (IDependencyResolverPolicy) new LiteralValueDependencyResolverPolicy(this.parameterValue);
    }
  }
}
