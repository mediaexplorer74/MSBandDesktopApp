// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.NamedTypesRegistry
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.Unity
{
  internal class NamedTypesRegistry
  {
    private readonly Dictionary<Type, List<string>> registeredKeys;
    private readonly NamedTypesRegistry parent;

    public NamedTypesRegistry()
      : this((NamedTypesRegistry) null)
    {
    }

    public NamedTypesRegistry(NamedTypesRegistry parent)
    {
      this.parent = parent;
      this.registeredKeys = new Dictionary<Type, List<string>>();
    }

    public void RegisterType(Type t, string name)
    {
      if (!this.registeredKeys.ContainsKey(t))
        this.registeredKeys[t] = new List<string>();
      this.RemoveMatchingKeys(t, name);
      this.registeredKeys[t].Add(name);
    }

    public IEnumerable<string> GetKeys(Type t)
    {
      IEnumerable<string> first = Enumerable.Empty<string>();
      if (this.parent != null)
        first = first.Concat<string>(this.parent.GetKeys(t));
      if (this.registeredKeys.ContainsKey(t))
        first = first.Concat<string>((IEnumerable<string>) this.registeredKeys[t]);
      return first;
    }

    public IEnumerable<Type> RegisteredTypes => (IEnumerable<Type>) this.registeredKeys.Keys;

    public void Clear() => this.registeredKeys.Clear();

    private void RemoveMatchingKeys(Type t, string name)
    {
      IEnumerable<string> source = this.registeredKeys[t].Where<string>((Func<string, bool>) (registeredName => registeredName != name));
      this.registeredKeys[t] = source.ToList<string>();
    }
  }
}
