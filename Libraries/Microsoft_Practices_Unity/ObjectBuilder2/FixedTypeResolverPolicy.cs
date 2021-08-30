// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.FixedTypeResolverPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class FixedTypeResolverPolicy : IDependencyResolverPolicy, IBuilderPolicy
  {
    private readonly NamedTypeBuildKey keyToBuild;

    public FixedTypeResolverPolicy(Type typeToBuild) => this.keyToBuild = new NamedTypeBuildKey(typeToBuild);

    public object Resolve(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return context.NewBuildUp(this.keyToBuild);
    }
  }
}
