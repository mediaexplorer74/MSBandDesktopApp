// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.IBuilderContext
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public interface IBuilderContext
  {
    IStrategyChain Strategies { get; }

    ILifetimeContainer Lifetime { get; }

    NamedTypeBuildKey OriginalBuildKey { get; }

    NamedTypeBuildKey BuildKey { get; set; }

    IPolicyList PersistentPolicies { get; }

    IPolicyList Policies { get; }

    IRecoveryStack RecoveryStack { get; }

    object Existing { get; set; }

    bool BuildComplete { get; set; }

    object CurrentOperation { get; set; }

    IBuilderContext ChildContext { get; }

    void AddResolverOverrides(IEnumerable<ResolverOverride> newOverrides);

    IDependencyResolverPolicy GetOverriddenResolver(Type dependencyType);

    object NewBuildUp(NamedTypeBuildKey newBuildKey);

    object NewBuildUp(
      NamedTypeBuildKey newBuildKey,
      Action<IBuilderContext> childCustomizationBlock);
  }
}
