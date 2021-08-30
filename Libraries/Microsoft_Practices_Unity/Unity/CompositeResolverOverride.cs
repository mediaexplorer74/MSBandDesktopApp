// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.CompositeResolverOverride
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.Unity
{
  public class CompositeResolverOverride : 
    ResolverOverride,
    IEnumerable<ResolverOverride>,
    IEnumerable
  {
    private readonly List<ResolverOverride> overrides = new List<ResolverOverride>();

    public void Add(ResolverOverride newOverride) => this.overrides.Add(newOverride);

    public void AddRange(IEnumerable<ResolverOverride> newOverrides) => this.overrides.AddRange(newOverrides);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public IEnumerator<ResolverOverride> GetEnumerator() => (IEnumerator<ResolverOverride>) this.overrides.GetEnumerator();

    public override IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType)
    {
      for (int index = this.overrides.Count<ResolverOverride>() - 1; index >= 0; --index)
      {
        IDependencyResolverPolicy resolver = this.overrides[index].GetResolver(context, dependencyType);
        if (resolver != null)
          return resolver;
      }
      return (IDependencyResolverPolicy) null;
    }
  }
}
