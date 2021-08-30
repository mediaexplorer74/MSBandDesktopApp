// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.MethodReflectionHelper
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity.Utility
{
  public class MethodReflectionHelper
  {
    private readonly MethodBase method;

    public MethodReflectionHelper(MethodBase method) => this.method = method;

    public bool MethodHasOpenGenericParameters => this.GetParameterReflectors().Any<ParameterReflectionHelper>((Func<ParameterReflectionHelper, bool>) (r => r.IsOpenGeneric));

    public IEnumerable<Type> ParameterTypes
    {
      get
      {
        foreach (ParameterInfo parameter in this.method.GetParameters())
          yield return parameter.ParameterType;
      }
    }

    public Type[] GetClosedParameterTypes(Type[] genericTypeArguments) => this.GetClosedParameterTypesSequence(genericTypeArguments).ToArray<Type>();

    private IEnumerable<ParameterReflectionHelper> GetParameterReflectors()
    {
      foreach (ParameterInfo parameter in this.method.GetParameters())
        yield return new ParameterReflectionHelper(parameter);
    }

    private IEnumerable<Type> GetClosedParameterTypesSequence(
      Type[] genericTypeArguments)
    {
      foreach (ParameterReflectionHelper r in this.GetParameterReflectors())
        yield return r.GetClosedParameterType(genericTypeArguments);
    }
  }
}
