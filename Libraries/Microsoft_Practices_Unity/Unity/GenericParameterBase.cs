// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.GenericParameterBase
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public abstract class GenericParameterBase : InjectionParameterValue
  {
    private readonly string genericParameterName;
    private readonly bool isArray;
    private readonly string resolutionKey;

    protected GenericParameterBase(string genericParameterName)
      : this(genericParameterName, (string) null)
    {
    }

    protected GenericParameterBase(string genericParameterName, string resolutionKey)
    {
      Guard.ArgumentNotNull((object) genericParameterName, nameof (genericParameterName));
      if (genericParameterName.EndsWith("[]", StringComparison.Ordinal) || genericParameterName.EndsWith("()", StringComparison.Ordinal))
      {
        this.genericParameterName = genericParameterName.Replace("[]", string.Empty).Replace("()", string.Empty);
        this.isArray = true;
      }
      else
      {
        this.genericParameterName = genericParameterName;
        this.isArray = false;
      }
      this.resolutionKey = resolutionKey;
    }

    public override string ParameterTypeName => this.genericParameterName;

    public override bool MatchesType(Type t)
    {
      Guard.ArgumentNotNull((object) t, nameof (t));
      return !this.isArray ? t.GetTypeInfo().IsGenericParameter && t.GetTypeInfo().Name == this.genericParameterName : t.IsArray && t.GetElementType().GetTypeInfo().IsGenericParameter && t.GetElementType().GetTypeInfo().Name == this.genericParameterName;
    }

    public override IDependencyResolverPolicy GetResolverPolicy(
      Type typeToBuild)
    {
      this.GuardTypeToBuildIsGeneric(typeToBuild);
      this.GuardTypeToBuildHasMatchingGenericParameter(typeToBuild);
      Type typeToResolve = new ReflectionHelper(typeToBuild).GetNamedGenericParameter(this.genericParameterName);
      if (this.isArray)
        typeToResolve = typeToResolve.MakeArrayType();
      return this.DoGetResolverPolicy(typeToResolve, this.resolutionKey);
    }

    protected abstract IDependencyResolverPolicy DoGetResolverPolicy(
      Type typeToResolve,
      string resolutionKey);

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
