// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.OptionalDependencyAttribute
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;

namespace Microsoft.Practices.Unity
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class OptionalDependencyAttribute : DependencyResolutionAttribute
  {
    private readonly string name;

    public OptionalDependencyAttribute()
      : this((string) null)
    {
    }

    public OptionalDependencyAttribute(string name) => this.name = name;

    public string Name => this.name;

    public override IDependencyResolverPolicy CreateResolver(
      Type typeToResolve)
    {
      return (IDependencyResolverPolicy) new OptionalDependencyResolverPolicy(typeToResolve, this.name);
    }
  }
}
