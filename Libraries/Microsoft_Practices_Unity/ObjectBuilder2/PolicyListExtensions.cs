// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.PolicyListExtensions
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.ObjectBuilder2
{
  public static class PolicyListExtensions
  {
    public static void Clear<TPolicyInterface>(this IPolicyList policies, object buildKey) where TPolicyInterface : IBuilderPolicy
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      policies.Clear(typeof (TPolicyInterface), buildKey);
    }

    public static void ClearDefault<TPolicyInterface>(this IPolicyList policies) where TPolicyInterface : IBuilderPolicy
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      policies.ClearDefault(typeof (TPolicyInterface));
    }

    public static TPolicyInterface Get<TPolicyInterface>(this IPolicyList policies, object buildKey) where TPolicyInterface : IBuilderPolicy => (TPolicyInterface) policies.Get(typeof (TPolicyInterface), buildKey, false);

    public static TPolicyInterface Get<TPolicyInterface>(
      this IPolicyList policies,
      object buildKey,
      out IPolicyList containingPolicyList)
      where TPolicyInterface : IBuilderPolicy
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      return (TPolicyInterface) policies.Get(typeof (TPolicyInterface), buildKey, false, out containingPolicyList);
    }

    public static IBuilderPolicy Get(
      this IPolicyList policies,
      Type policyInterface,
      object buildKey)
    {
      return policies.Get(policyInterface, buildKey, false);
    }

    public static IBuilderPolicy Get(
      this IPolicyList policies,
      Type policyInterface,
      object buildKey,
      out IPolicyList containingPolicyList)
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      return policies.Get(policyInterface, buildKey, false, out containingPolicyList);
    }

    public static TPolicyInterface Get<TPolicyInterface>(
      this IPolicyList policies,
      object buildKey,
      bool localOnly)
      where TPolicyInterface : IBuilderPolicy
    {
      return (TPolicyInterface) policies.Get(typeof (TPolicyInterface), buildKey, localOnly);
    }

    public static TPolicyInterface Get<TPolicyInterface>(
      this IPolicyList policies,
      object buildKey,
      bool localOnly,
      out IPolicyList containingPolicyList)
      where TPolicyInterface : IBuilderPolicy
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      return (TPolicyInterface) policies.Get(typeof (TPolicyInterface), buildKey, localOnly, out containingPolicyList);
    }

    public static IBuilderPolicy Get(
      this IPolicyList policies,
      Type policyInterface,
      object buildKey,
      bool localOnly)
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      return policies.Get(policyInterface, buildKey, localOnly, out IPolicyList _);
    }

    public static TPolicyInterface GetNoDefault<TPolicyInterface>(
      this IPolicyList policies,
      object buildKey,
      bool localOnly)
      where TPolicyInterface : IBuilderPolicy
    {
      return (TPolicyInterface) policies.GetNoDefault(typeof (TPolicyInterface), buildKey, localOnly);
    }

    public static TPolicyInterface GetNoDefault<TPolicyInterface>(
      this IPolicyList policies,
      object buildKey,
      bool localOnly,
      out IPolicyList containingPolicyList)
      where TPolicyInterface : IBuilderPolicy
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      return (TPolicyInterface) policies.GetNoDefault(typeof (TPolicyInterface), buildKey, localOnly, out containingPolicyList);
    }

    public static IBuilderPolicy GetNoDefault(
      this IPolicyList policies,
      Type policyInterface,
      object buildKey,
      bool localOnly)
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      return policies.GetNoDefault(policyInterface, buildKey, localOnly, out IPolicyList _);
    }

    public static void Set<TPolicyInterface>(
      this IPolicyList policies,
      TPolicyInterface policy,
      object buildKey)
      where TPolicyInterface : IBuilderPolicy
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      policies.Set(typeof (TPolicyInterface), (IBuilderPolicy) policy, buildKey);
    }

    public static void SetDefault<TPolicyInterface>(
      this IPolicyList policies,
      TPolicyInterface policy)
      where TPolicyInterface : IBuilderPolicy
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      policies.SetDefault(typeof (TPolicyInterface), (IBuilderPolicy) policy);
    }
  }
}
