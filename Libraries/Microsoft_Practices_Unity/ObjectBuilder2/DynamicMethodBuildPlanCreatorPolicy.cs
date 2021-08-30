// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DynamicMethodBuildPlanCreatorPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class DynamicMethodBuildPlanCreatorPolicy : IBuildPlanCreatorPolicy, IBuilderPolicy
  {
    private IStagedStrategyChain strategies;

    public DynamicMethodBuildPlanCreatorPolicy(IStagedStrategyChain strategies) => this.strategies = strategies;

    public IBuildPlanPolicy CreatePlan(
      IBuilderContext context,
      NamedTypeBuildKey buildKey)
    {
      Guard.ArgumentNotNull((object) buildKey, nameof (buildKey));
      DynamicBuildPlanGenerationContext generatorContext = new DynamicBuildPlanGenerationContext(buildKey.Type);
      IBuilderContext context1 = this.GetContext(context, buildKey, generatorContext);
      context1.Strategies.ExecuteBuildUp(context1);
      return (IBuildPlanPolicy) new DynamicMethodBuildPlan(generatorContext.GetBuildMethod());
    }

    private IBuilderContext GetContext(
      IBuilderContext originalContext,
      NamedTypeBuildKey buildKey,
      DynamicBuildPlanGenerationContext generatorContext)
    {
      return (IBuilderContext) new BuilderContext(this.strategies.MakeStrategyChain(), originalContext.Lifetime, originalContext.PersistentPolicies, originalContext.Policies, buildKey, (object) generatorContext);
    }
  }
}
