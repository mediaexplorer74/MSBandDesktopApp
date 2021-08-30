// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.DependencyOverride
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;

namespace Microsoft.Practices.Unity
{
  public class DependencyOverride : ResolverOverride
  {
    private readonly InjectionParameterValue dependencyValue;
    private readonly Type typeToConstruct;

    public DependencyOverride(Type typeToConstruct, object dependencyValue)
    {
      this.typeToConstruct = typeToConstruct;
      this.dependencyValue = InjectionParameterValue.ToParameter(dependencyValue);
    }

    public override IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType)
    {
      IDependencyResolverPolicy dependencyResolverPolicy = (IDependencyResolverPolicy) null;
      if ((object) dependencyType == (object) this.typeToConstruct)
        dependencyResolverPolicy = this.dependencyValue.GetResolverPolicy(dependencyType);
      return dependencyResolverPolicy;
    }
  }
}
