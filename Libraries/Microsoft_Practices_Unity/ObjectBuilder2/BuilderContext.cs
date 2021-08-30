// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.BuilderContext
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class BuilderContext : IBuilderContext
  {
    private readonly IStrategyChain chain;
    private readonly ILifetimeContainer lifetime;
    private readonly IRecoveryStack recoveryStack = (IRecoveryStack) new Microsoft.Practices.ObjectBuilder2.RecoveryStack();
    private readonly NamedTypeBuildKey originalBuildKey;
    private readonly IPolicyList persistentPolicies;
    private readonly IPolicyList policies;
    private CompositeResolverOverride resolverOverrides;
    private bool ownsOverrides;

    protected BuilderContext()
    {
    }

    public BuilderContext(
      IStrategyChain chain,
      ILifetimeContainer lifetime,
      IPolicyList policies,
      NamedTypeBuildKey originalBuildKey,
      object existing)
    {
      this.chain = chain;
      this.lifetime = lifetime;
      this.originalBuildKey = originalBuildKey;
      this.BuildKey = originalBuildKey;
      this.persistentPolicies = policies;
      this.policies = (IPolicyList) new PolicyList(this.persistentPolicies);
      this.Existing = existing;
      this.resolverOverrides = new CompositeResolverOverride();
      this.ownsOverrides = true;
    }

    public BuilderContext(
      IStrategyChain chain,
      ILifetimeContainer lifetime,
      IPolicyList persistentPolicies,
      IPolicyList transientPolicies,
      NamedTypeBuildKey buildKey,
      object existing)
    {
      this.chain = chain;
      this.lifetime = lifetime;
      this.persistentPolicies = persistentPolicies;
      this.policies = transientPolicies;
      this.originalBuildKey = buildKey;
      this.BuildKey = buildKey;
      this.Existing = existing;
      this.resolverOverrides = new CompositeResolverOverride();
      this.ownsOverrides = true;
    }

    protected BuilderContext(
      IStrategyChain chain,
      ILifetimeContainer lifetime,
      IPolicyList persistentPolicies,
      IPolicyList transientPolicies,
      NamedTypeBuildKey buildKey,
      CompositeResolverOverride resolverOverrides)
    {
      this.chain = chain;
      this.lifetime = lifetime;
      this.persistentPolicies = persistentPolicies;
      this.policies = transientPolicies;
      this.originalBuildKey = buildKey;
      this.BuildKey = buildKey;
      this.Existing = (object) null;
      this.resolverOverrides = resolverOverrides;
      this.ownsOverrides = false;
    }

    public IStrategyChain Strategies => this.chain;

    public NamedTypeBuildKey BuildKey { get; set; }

    public object Existing { get; set; }

    public ILifetimeContainer Lifetime => this.lifetime;

    public NamedTypeBuildKey OriginalBuildKey => this.originalBuildKey;

    public IPolicyList PersistentPolicies => this.persistentPolicies;

    public IPolicyList Policies => this.policies;

    public IRecoveryStack RecoveryStack => this.recoveryStack;

    public bool BuildComplete { get; set; }

    public object CurrentOperation { get; set; }

    public IBuilderContext ChildContext { get; private set; }

    public void AddResolverOverrides(IEnumerable<ResolverOverride> newOverrides)
    {
      if (!this.ownsOverrides)
      {
        CompositeResolverOverride resolverOverrides = this.resolverOverrides;
        this.resolverOverrides = new CompositeResolverOverride();
        this.resolverOverrides.AddRange((IEnumerable<ResolverOverride>) resolverOverrides);
        this.ownsOverrides = true;
      }
      this.resolverOverrides.AddRange(newOverrides);
    }

    public IDependencyResolverPolicy GetOverriddenResolver(
      Type dependencyType)
    {
      return this.resolverOverrides.GetResolver((IBuilderContext) this, dependencyType);
    }

    public object NewBuildUp(NamedTypeBuildKey newBuildKey)
    {
      this.ChildContext = (IBuilderContext) new BuilderContext(this.chain, this.lifetime, this.persistentPolicies, this.policies, newBuildKey, this.resolverOverrides);
      object obj = this.ChildContext.Strategies.ExecuteBuildUp(this.ChildContext);
      this.ChildContext = (IBuilderContext) null;
      return obj;
    }

    public object NewBuildUp(
      NamedTypeBuildKey newBuildKey,
      Action<IBuilderContext> childCustomizationBlock)
    {
      Guard.ArgumentNotNull((object) childCustomizationBlock, nameof (childCustomizationBlock));
      this.ChildContext = (IBuilderContext) new BuilderContext(this.chain, this.lifetime, this.persistentPolicies, this.policies, newBuildKey, this.resolverOverrides);
      childCustomizationBlock(this.ChildContext);
      object obj = this.ChildContext.Strategies.ExecuteBuildUp(this.ChildContext);
      this.ChildContext = (IBuilderContext) null;
      return obj;
    }
  }
}
