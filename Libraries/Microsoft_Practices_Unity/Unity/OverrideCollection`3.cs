// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.OverrideCollection`3
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity
{
  public abstract class OverrideCollection<TOverride, TKey, TValue> : 
    ResolverOverride,
    IEnumerable<TOverride>,
    IEnumerable
    where TOverride : ResolverOverride
  {
    private readonly CompositeResolverOverride overrides = new CompositeResolverOverride();

    public void Add(TKey key, TValue value) => this.overrides.Add((ResolverOverride) this.MakeOverride(key, value));

    public override IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType)
    {
      return this.overrides.GetResolver(context, dependencyType);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public IEnumerator<TOverride> GetEnumerator()
    {
      foreach (ResolverOverride o in this.overrides)
        yield return (TOverride) o;
    }

    protected abstract TOverride MakeOverride(TKey key, TValue value);
  }
}
