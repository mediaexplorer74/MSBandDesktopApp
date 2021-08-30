// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.NamedTypeDependencyResolverPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
  public class NamedTypeDependencyResolverPolicy : IDependencyResolverPolicy, IBuilderPolicy
  {
    private Type type;
    private string name;

    public NamedTypeDependencyResolverPolicy(Type type, string name)
    {
      this.type = type;
      this.name = name;
    }

    public object Resolve(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return context.NewBuildUp(new NamedTypeBuildKey(this.type, this.name));
    }

    public Type Type => this.type;

    public string Name => this.name;
  }
}
