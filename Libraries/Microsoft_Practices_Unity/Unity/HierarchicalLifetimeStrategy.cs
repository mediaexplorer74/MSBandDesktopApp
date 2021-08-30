// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.HierarchicalLifetimeStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.Unity
{
  public class HierarchicalLifetimeStrategy : BuilderStrategy
  {
    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      IPolicyList containingPolicyList;
      if (!(context.PersistentPolicies.Get<ILifetimePolicy>((object) context.BuildKey, out containingPolicyList) is HierarchicalLifetimeManager) || object.ReferenceEquals((object) containingPolicyList, (object) context.PersistentPolicies))
        return;
      HierarchicalLifetimeManager hierarchicalLifetimeManager1 = new HierarchicalLifetimeManager();
      hierarchicalLifetimeManager1.InUse = true;
      HierarchicalLifetimeManager hierarchicalLifetimeManager2 = hierarchicalLifetimeManager1;
      context.PersistentPolicies.Set<ILifetimePolicy>((ILifetimePolicy) hierarchicalLifetimeManager2, (object) context.BuildKey);
      context.Lifetime.Add((object) hierarchicalLifetimeManager2);
    }
  }
}
