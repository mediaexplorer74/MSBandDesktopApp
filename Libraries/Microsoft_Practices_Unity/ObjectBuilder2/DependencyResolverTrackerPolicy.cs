// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DependencyResolverTrackerPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class DependencyResolverTrackerPolicy : IDependencyResolverTrackerPolicy, IBuilderPolicy
  {
    private List<object> keys = new List<object>();

    public void AddResolverKey(object key)
    {
      lock (this.keys)
        this.keys.Add(key);
    }

    public void RemoveResolvers(IPolicyList policies)
    {
      List<object> objectList = new List<object>();
      lock (this.keys)
      {
        objectList.AddRange((IEnumerable<object>) this.keys);
        this.keys.Clear();
      }
      foreach (object buildKey in objectList)
        policies.Clear<IDependencyResolverPolicy>(buildKey);
    }

    public static IDependencyResolverTrackerPolicy GetTracker(
      IPolicyList policies,
      object buildKey)
    {
      IDependencyResolverTrackerPolicy policy = policies.Get<IDependencyResolverTrackerPolicy>(buildKey);
      if (policy == null)
      {
        policy = (IDependencyResolverTrackerPolicy) new DependencyResolverTrackerPolicy();
        policies.Set<IDependencyResolverTrackerPolicy>(policy, buildKey);
      }
      return policy;
    }

    public static void TrackKey(IPolicyList policies, object buildKey, object resolverKey) => DependencyResolverTrackerPolicy.GetTracker(policies, buildKey).AddResolverKey(resolverKey);

    public static void RemoveResolvers(IPolicyList policies, object buildKey) => policies.Get<IDependencyResolverTrackerPolicy>(buildKey)?.RemoveResolvers(policies);
  }
}
