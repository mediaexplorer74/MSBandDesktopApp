// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.GenericResolvedArrayParameter
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class GenericResolvedArrayParameter : InjectionParameterValue
  {
    private readonly string genericParameterName;
    private readonly List<InjectionParameterValue> elementValues = new List<InjectionParameterValue>();

    public GenericResolvedArrayParameter(string genericParameterName, params object[] elementValues)
    {
      Guard.ArgumentNotNull((object) genericParameterName, nameof (genericParameterName));
      this.genericParameterName = genericParameterName;
      this.elementValues.AddRange(InjectionParameterValue.ToParameters(elementValues));
    }

    public override string ParameterTypeName => this.genericParameterName + "[]";

    public override bool MatchesType(Type t)
    {
      Guard.ArgumentNotNull((object) t, nameof (t));
      if (!t.IsArray || t.GetArrayRank() != 1)
        return false;
      Type elementType = t.GetElementType();
      return elementType.GetTypeInfo().IsGenericParameter && elementType.GetTypeInfo().Name == this.genericParameterName;
    }

    public override IDependencyResolverPolicy GetResolverPolicy(
      Type typeToBuild)
    {
      this.GuardTypeToBuildIsGeneric(typeToBuild);
      this.GuardTypeToBuildHasMatchingGenericParameter(typeToBuild);
      Type genericParameter = new ReflectionHelper(typeToBuild).GetNamedGenericParameter(this.genericParameterName);
      List<IDependencyResolverPolicy> dependencyResolverPolicyList = new List<IDependencyResolverPolicy>();
      foreach (InjectionParameterValue elementValue in this.elementValues)
        dependencyResolverPolicyList.Add(elementValue.GetResolverPolicy(typeToBuild));
      return (IDependencyResolverPolicy) new ResolvedArrayWithElementsResolverPolicy(genericParameter, dependencyResolverPolicyList.ToArray());
    }

    private void GuardTypeToBuildIsGeneric(Type typeToBuild)
    {
      if (!typeToBuild.GetTypeInfo().IsGenericType)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.NotAGenericType, new object[2]
        {
          (object) typeToBuild.GetTypeInfo().Name,
          (object) this.genericParameterName
        }));
    }

    private void GuardTypeToBuildHasMatchingGenericParameter(Type typeToBuild)
    {
      foreach (Type genericTypeParameter in typeToBuild.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters)
      {
        if (genericTypeParameter.GetTypeInfo().Name == this.genericParameterName)
          return;
      }
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.NoMatchingGenericArgument, new object[2]
      {
        (object) typeToBuild.GetTypeInfo().Name,
        (object) this.genericParameterName
      }));
    }
  }
}
