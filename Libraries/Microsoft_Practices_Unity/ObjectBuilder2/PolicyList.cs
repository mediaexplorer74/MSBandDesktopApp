// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.PolicyList
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class PolicyList : IPolicyList
  {
    private readonly IPolicyList innerPolicyList;
    private readonly object lockObject = new object();
    private Dictionary<PolicyList.PolicyKey, IBuilderPolicy> policies = new Dictionary<PolicyList.PolicyKey, IBuilderPolicy>((IEqualityComparer<PolicyList.PolicyKey>) PolicyList.PolicyKeyEqualityComparer.Default);

    public PolicyList()
      : this((IPolicyList) null)
    {
    }

    public PolicyList(IPolicyList innerPolicyList) => this.innerPolicyList = innerPolicyList ?? (IPolicyList) new PolicyList.NullPolicyList();

    public int Count => this.policies.Count;

    public void Clear(Type policyInterface, object buildKey)
    {
      lock (this.lockObject)
      {
        Dictionary<PolicyList.PolicyKey, IBuilderPolicy> dictionary = this.ClonePolicies();
        dictionary.Remove(new PolicyList.PolicyKey(policyInterface, buildKey));
        this.policies = dictionary;
      }
    }

    public void ClearAll()
    {
      lock (this.lockObject)
        this.policies = new Dictionary<PolicyList.PolicyKey, IBuilderPolicy>((IEqualityComparer<PolicyList.PolicyKey>) PolicyList.PolicyKeyEqualityComparer.Default);
    }

    public void ClearDefault(Type policyInterface) => this.Clear(policyInterface, (object) null);

    public IBuilderPolicy Get(
      Type policyInterface,
      object buildKey,
      bool localOnly,
      out IPolicyList containingPolicyList)
    {
      Type type;
      PolicyList.TryGetType(buildKey, out type);
      return this.GetPolicyForKey(policyInterface, buildKey, localOnly, out containingPolicyList) ?? this.GetPolicyForOpenGenericKey(policyInterface, buildKey, type, localOnly, out containingPolicyList) ?? this.GetPolicyForType(policyInterface, type, localOnly, out containingPolicyList) ?? this.GetPolicyForOpenGenericType(policyInterface, type, localOnly, out containingPolicyList) ?? this.GetDefaultForPolicy(policyInterface, localOnly, out containingPolicyList);
    }

    private IBuilderPolicy GetPolicyForKey(
      Type policyInterface,
      object buildKey,
      bool localOnly,
      out IPolicyList containingPolicyList)
    {
      if (buildKey != null)
        return this.GetNoDefault(policyInterface, buildKey, localOnly, out containingPolicyList);
      containingPolicyList = (IPolicyList) null;
      return (IBuilderPolicy) null;
    }

    private IBuilderPolicy GetPolicyForOpenGenericKey(
      Type policyInterface,
      object buildKey,
      Type buildType,
      bool localOnly,
      out IPolicyList containingPolicyList)
    {
      if ((object) buildType != null && buildType.GetTypeInfo().IsGenericType)
        return this.GetNoDefault(policyInterface, PolicyList.ReplaceType(buildKey, buildType.GetGenericTypeDefinition()), localOnly, out containingPolicyList);
      containingPolicyList = (IPolicyList) null;
      return (IBuilderPolicy) null;
    }

    private IBuilderPolicy GetPolicyForType(
      Type policyInterface,
      Type buildType,
      bool localOnly,
      out IPolicyList containingPolicyList)
    {
      if ((object) buildType != null)
        return this.GetNoDefault(policyInterface, (object) buildType, localOnly, out containingPolicyList);
      containingPolicyList = (IPolicyList) null;
      return (IBuilderPolicy) null;
    }

    private IBuilderPolicy GetPolicyForOpenGenericType(
      Type policyInterface,
      Type buildType,
      bool localOnly,
      out IPolicyList containingPolicyList)
    {
      if ((object) buildType != null && buildType.GetTypeInfo().IsGenericType)
        return this.GetNoDefault(policyInterface, (object) buildType.GetGenericTypeDefinition(), localOnly, out containingPolicyList);
      containingPolicyList = (IPolicyList) null;
      return (IBuilderPolicy) null;
    }

    private IBuilderPolicy GetDefaultForPolicy(
      Type policyInterface,
      bool localOnly,
      out IPolicyList containingPolicyList)
    {
      return this.GetNoDefault(policyInterface, (object) null, localOnly, out containingPolicyList);
    }

    public IBuilderPolicy GetNoDefault(
      Type policyInterface,
      object buildKey,
      bool localOnly,
      out IPolicyList containingPolicyList)
    {
      containingPolicyList = (IPolicyList) null;
      IBuilderPolicy builderPolicy;
      if (this.policies.TryGetValue(new PolicyList.PolicyKey(policyInterface, buildKey), out builderPolicy))
      {
        containingPolicyList = (IPolicyList) this;
        return builderPolicy;
      }
      return localOnly ? (IBuilderPolicy) null : this.innerPolicyList.GetNoDefault(policyInterface, buildKey, false, out containingPolicyList);
    }

    public void Set(Type policyInterface, IBuilderPolicy policy, object buildKey)
    {
      lock (this.lockObject)
      {
        Dictionary<PolicyList.PolicyKey, IBuilderPolicy> dictionary = this.ClonePolicies();
        dictionary[new PolicyList.PolicyKey(policyInterface, buildKey)] = policy;
        this.policies = dictionary;
      }
    }

    public void SetDefault(Type policyInterface, IBuilderPolicy policy) => this.Set(policyInterface, policy, (object) null);

    private Dictionary<PolicyList.PolicyKey, IBuilderPolicy> ClonePolicies() => new Dictionary<PolicyList.PolicyKey, IBuilderPolicy>((IDictionary<PolicyList.PolicyKey, IBuilderPolicy>) this.policies, (IEqualityComparer<PolicyList.PolicyKey>) PolicyList.PolicyKeyEqualityComparer.Default);

    private static bool TryGetType(object buildKey, out Type type)
    {
      type = buildKey as Type;
      if ((object) type == null)
      {
        NamedTypeBuildKey namedTypeBuildKey = buildKey as NamedTypeBuildKey;
        if (namedTypeBuildKey != (NamedTypeBuildKey) null)
          type = namedTypeBuildKey.Type;
      }
      return (object) type != null;
    }

    private static object ReplaceType(object buildKey, Type newType)
    {
      if ((object) (buildKey as Type) != null)
        return (object) newType;
      NamedTypeBuildKey namedTypeBuildKey = buildKey as NamedTypeBuildKey;
      if (namedTypeBuildKey != (NamedTypeBuildKey) null)
        return (object) new NamedTypeBuildKey(newType, namedTypeBuildKey.Name);
      throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.CannotExtractTypeFromBuildKey, new object[1]
      {
        buildKey
      }), nameof (buildKey));
    }

    private class NullPolicyList : IPolicyList
    {
      public void Clear(Type policyInterface, object buildKey) => throw new NotImplementedException();

      public void ClearAll() => throw new NotImplementedException();

      public void ClearDefault(Type policyInterface) => throw new NotImplementedException();

      public IBuilderPolicy Get(
        Type policyInterface,
        object buildKey,
        bool localOnly,
        out IPolicyList containingPolicyList)
      {
        containingPolicyList = (IPolicyList) null;
        return (IBuilderPolicy) null;
      }

      public IBuilderPolicy GetNoDefault(
        Type policyInterface,
        object buildKey,
        bool localOnly,
        out IPolicyList containingPolicyList)
      {
        containingPolicyList = (IPolicyList) null;
        return (IBuilderPolicy) null;
      }

      public void Set(Type policyInterface, IBuilderPolicy policy, object buildKey) => throw new NotImplementedException();

      public void SetDefault(Type policyInterface, IBuilderPolicy policy) => throw new NotImplementedException();
    }

    private struct PolicyKey
    {
      public readonly object BuildKey;
      public readonly Type PolicyType;

      public PolicyKey(Type policyType, object buildKey)
      {
        this.PolicyType = policyType;
        this.BuildKey = buildKey;
      }

      public override bool Equals(object obj) => obj != null && (object) obj.GetType() == (object) typeof (PolicyList.PolicyKey) && this == (PolicyList.PolicyKey) obj;

      public override int GetHashCode() => PolicyList.PolicyKey.SafeGetHashCode((object) this.PolicyType) * 37 + PolicyList.PolicyKey.SafeGetHashCode(this.BuildKey);

      public static bool operator ==(PolicyList.PolicyKey left, PolicyList.PolicyKey right) => (object) left.PolicyType == (object) right.PolicyType && object.Equals(left.BuildKey, right.BuildKey);

      public static bool operator !=(PolicyList.PolicyKey left, PolicyList.PolicyKey right) => !(left == right);

      private static int SafeGetHashCode(object obj) => obj == null ? 0 : obj.GetHashCode();
    }

    private class PolicyKeyEqualityComparer : IEqualityComparer<PolicyList.PolicyKey>
    {
      public static readonly PolicyList.PolicyKeyEqualityComparer Default = new PolicyList.PolicyKeyEqualityComparer();

      public bool Equals(PolicyList.PolicyKey x, PolicyList.PolicyKey y) => (object) x.PolicyType == (object) y.PolicyType && object.Equals(x.BuildKey, y.BuildKey);

      public int GetHashCode(PolicyList.PolicyKey obj) => obj.GetHashCode();
    }
  }
}
