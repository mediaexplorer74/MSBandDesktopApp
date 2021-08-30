// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.DependencyAttribute
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;

namespace Microsoft.Practices.Unity
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
  public sealed class DependencyAttribute : DependencyResolutionAttribute
  {
    private readonly string name;

    public DependencyAttribute()
      : this((string) null)
    {
    }

    public DependencyAttribute(string name) => this.name = name;

    public string Name => this.name;

    public override IDependencyResolverPolicy CreateResolver(
      Type typeToResolve)
    {
      return (IDependencyResolverPolicy) new NamedTypeDependencyResolverPolicy(typeToResolve, this.name);
    }
  }
}
