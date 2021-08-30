// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ResolvedParameter
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.Unity
{
  public class ResolvedParameter : TypedInjectionValue
  {
    private readonly string name;

    public ResolvedParameter(Type parameterType)
      : this(parameterType, (string) null)
    {
    }

    public ResolvedParameter(Type parameterType, string name)
      : base(parameterType)
    {
      this.name = name;
    }

    public override IDependencyResolverPolicy GetResolverPolicy(
      Type typeToBuild)
    {
      Guard.ArgumentNotNull((object) typeToBuild, nameof (typeToBuild));
      ReflectionHelper parameterReflector = new ReflectionHelper(this.ParameterType);
      if (parameterReflector.IsGenericArray)
        return this.CreateGenericArrayResolverPolicy(typeToBuild, parameterReflector);
      return parameterReflector.IsOpenGeneric || parameterReflector.Type.IsGenericParameter ? this.CreateGenericResolverPolicy(typeToBuild, parameterReflector) : this.CreateResolverPolicy(parameterReflector.Type);
    }

    private IDependencyResolverPolicy CreateResolverPolicy(
      Type typeToResolve)
    {
      return (IDependencyResolverPolicy) new NamedTypeDependencyResolverPolicy(typeToResolve, this.name);
    }

    private IDependencyResolverPolicy CreateGenericResolverPolicy(
      Type typeToBuild,
      ReflectionHelper parameterReflector)
    {
      return (IDependencyResolverPolicy) new NamedTypeDependencyResolverPolicy(parameterReflector.GetClosedParameterType(typeToBuild.GenericTypeArguments), this.name);
    }

    private IDependencyResolverPolicy CreateGenericArrayResolverPolicy(
      Type typeToBuild,
      ReflectionHelper parameterReflector)
    {
      return (IDependencyResolverPolicy) new NamedTypeDependencyResolverPolicy(parameterReflector.GetClosedParameterType(typeToBuild.GenericTypeArguments), this.name);
    }
  }
}
