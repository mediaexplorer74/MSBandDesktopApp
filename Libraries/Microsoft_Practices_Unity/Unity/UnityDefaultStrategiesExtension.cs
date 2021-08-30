// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.UnityDefaultStrategiesExtension
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;

namespace Microsoft.Practices.Unity
{
  public class UnityDefaultStrategiesExtension : UnityContainerExtension
  {
    protected override void Initialize()
    {
      this.Context.Strategies.AddNew<BuildKeyMappingStrategy>(UnityBuildStage.TypeMapping);
      this.Context.Strategies.AddNew<HierarchicalLifetimeStrategy>(UnityBuildStage.Lifetime);
      this.Context.Strategies.AddNew<LifetimeStrategy>(UnityBuildStage.Lifetime);
      this.Context.Strategies.AddNew<ArrayResolutionStrategy>(UnityBuildStage.Creation);
      this.Context.Strategies.AddNew<BuildPlanStrategy>(UnityBuildStage.Creation);
      this.Context.BuildPlanStrategies.AddNew<DynamicMethodConstructorStrategy>(UnityBuildStage.Creation);
      this.Context.BuildPlanStrategies.AddNew<DynamicMethodPropertySetterStrategy>(UnityBuildStage.Initialization);
      this.Context.BuildPlanStrategies.AddNew<DynamicMethodCallStrategy>(UnityBuildStage.Initialization);
      this.Context.Policies.SetDefault<IConstructorSelectorPolicy>((IConstructorSelectorPolicy) new DefaultUnityConstructorSelectorPolicy());
      this.Context.Policies.SetDefault<IPropertySelectorPolicy>((IPropertySelectorPolicy) new DefaultUnityPropertySelectorPolicy());
      this.Context.Policies.SetDefault<IMethodSelectorPolicy>((IMethodSelectorPolicy) new DefaultUnityMethodSelectorPolicy());
      this.Context.Policies.SetDefault<IBuildPlanCreatorPolicy>((IBuildPlanCreatorPolicy) new DynamicMethodBuildPlanCreatorPolicy((IStagedStrategyChain) this.Context.BuildPlanStrategies));
      this.Context.Policies.Set<IBuildPlanPolicy>((IBuildPlanPolicy) new DeferredResolveBuildPlanPolicy(), (object) typeof (Func<>));
      this.Context.Policies.Set<ILifetimePolicy>((ILifetimePolicy) new PerResolveLifetimeManager(), (object) typeof (Func<>));
      this.Context.Policies.Set<IBuildPlanCreatorPolicy>((IBuildPlanCreatorPolicy) new LazyDynamicMethodBuildPlanCreatorPolicy(), (object) typeof (Lazy<>));
    }
  }
}
