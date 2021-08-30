// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ResolvedArrayParameter
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Practices.Unity
{
  public class ResolvedArrayParameter : TypedInjectionValue
  {
    private readonly Type elementType;
    private readonly List<InjectionParameterValue> elementValues = new List<InjectionParameterValue>();

    public ResolvedArrayParameter(Type elementType, params object[] elementValues)
      : this(ResolvedArrayParameter.GetArrayType(elementType), elementType, elementValues)
    {
    }

    protected ResolvedArrayParameter(
      Type arrayParameterType,
      Type elementType,
      params object[] elementValues)
      : base(arrayParameterType)
    {
      Guard.ArgumentNotNull((object) elementType, nameof (elementType));
      Guard.ArgumentNotNull((object) elementValues, nameof (elementValues));
      this.elementType = elementType;
      this.elementValues.AddRange(InjectionParameterValue.ToParameters(elementValues));
      foreach (InjectionParameterValue elementValue in this.elementValues)
      {
        if (!elementValue.MatchesType(elementType))
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TypesAreNotAssignable, new object[2]
          {
            (object) elementType,
            (object) elementValue.ParameterTypeName
          }));
      }
    }

    public override IDependencyResolverPolicy GetResolverPolicy(
      Type typeToBuild)
    {
      Guard.ArgumentNotNull((object) typeToBuild, nameof (typeToBuild));
      List<IDependencyResolverPolicy> dependencyResolverPolicyList = new List<IDependencyResolverPolicy>();
      foreach (InjectionParameterValue elementValue in this.elementValues)
        dependencyResolverPolicyList.Add(elementValue.GetResolverPolicy(this.elementType));
      return (IDependencyResolverPolicy) new ResolvedArrayWithElementsResolverPolicy(this.elementType, dependencyResolverPolicyList.ToArray());
    }

    private static Type GetArrayType(Type elementType)
    {
      Guard.ArgumentNotNull((object) elementType, nameof (elementType));
      return elementType.MakeArrayType();
    }
  }
}
