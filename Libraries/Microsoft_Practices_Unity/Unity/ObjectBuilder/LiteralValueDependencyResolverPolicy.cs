// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.LiteralValueDependencyResolverPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
  public class LiteralValueDependencyResolverPolicy : IDependencyResolverPolicy, IBuilderPolicy
  {
    private object dependencyValue;

    public LiteralValueDependencyResolverPolicy(object dependencyValue) => this.dependencyValue = dependencyValue;

    public object Resolve(IBuilderContext context) => this.dependencyValue;
  }
}
