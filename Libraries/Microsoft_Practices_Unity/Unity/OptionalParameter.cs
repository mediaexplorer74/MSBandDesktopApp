// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.OptionalParameter
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.Unity
{
  public class OptionalParameter : TypedInjectionValue
  {
    private readonly string name;

    public OptionalParameter(Type type)
      : this(type, (string) null)
    {
    }

    public OptionalParameter(Type type, string name)
      : base(type)
    {
      this.name = name;
    }

    public override IDependencyResolverPolicy GetResolverPolicy(
      Type typeToBuild)
    {
      Guard.ArgumentNotNull((object) typeToBuild, nameof (typeToBuild));
      ReflectionHelper reflectionHelper = new ReflectionHelper(this.ParameterType);
      Type type = reflectionHelper.Type;
      if (reflectionHelper.IsOpenGeneric)
        type = reflectionHelper.GetClosedParameterType(typeToBuild.GenericTypeArguments);
      return (IDependencyResolverPolicy) new OptionalDependencyResolverPolicy(type, this.name);
    }
  }
}
