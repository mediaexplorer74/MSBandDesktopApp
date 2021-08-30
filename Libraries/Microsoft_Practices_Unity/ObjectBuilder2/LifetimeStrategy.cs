// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.LifetimeStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class LifetimeStrategy : BuilderStrategy
  {
    private readonly object genericLifetimeManagerLock = new object();

    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      if (context.Existing != null)
        return;
      ILifetimePolicy lifetimePolicy = this.GetLifetimePolicy(context);
      if (lifetimePolicy is IRequiresRecovery recovery)
        context.RecoveryStack.Add(recovery);
      object obj = lifetimePolicy.GetValue();
      if (obj == null)
        return;
      context.Existing = obj;
      context.BuildComplete = true;
    }

    public override void PostBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      this.GetLifetimePolicy(context).SetValue(context.Existing);
    }

    private ILifetimePolicy GetLifetimePolicy(IBuilderContext context)
    {
      ILifetimePolicy policy = context.Policies.GetNoDefault<ILifetimePolicy>((object) context.BuildKey, false);
      if (policy == null && context.BuildKey.Type.GetTypeInfo().IsGenericType)
        policy = this.GetLifetimePolicyForGenericType(context);
      if (policy == null)
      {
        policy = (ILifetimePolicy) new TransientLifetimeManager();
        context.PersistentPolicies.Set<ILifetimePolicy>(policy, (object) context.BuildKey);
      }
      return policy;
    }

    private ILifetimePolicy GetLifetimePolicyForGenericType(IBuilderContext context)
    {
      object buildKey = (object) new NamedTypeBuildKey(context.BuildKey.Type.GetGenericTypeDefinition(), context.BuildKey.Name);
      IPolicyList containingPolicyList;
      ILifetimeFactoryPolicy lifetimeFactoryPolicy = context.Policies.Get<ILifetimeFactoryPolicy>(buildKey, out containingPolicyList);
      if (lifetimeFactoryPolicy == null)
        return (ILifetimePolicy) null;
      ILifetimePolicy lifetimePolicy1 = lifetimeFactoryPolicy.CreateLifetimePolicy();
      lock (this.genericLifetimeManagerLock)
      {
        ILifetimePolicy lifetimePolicy2 = containingPolicyList.GetNoDefault<ILifetimePolicy>((object) context.BuildKey, false);
        if (lifetimePolicy2 == null)
        {
          containingPolicyList.Set<ILifetimePolicy>(lifetimePolicy1, (object) context.BuildKey);
          lifetimePolicy2 = lifetimePolicy1;
        }
        return lifetimePolicy2;
      }
    }
  }
}
