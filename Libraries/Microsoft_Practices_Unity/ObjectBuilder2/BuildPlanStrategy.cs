// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.BuildPlanStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class BuildPlanStrategy : BuilderStrategy
  {
    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      IPolicyList containingPolicyList1;
      IBuildPlanPolicy plan = context.Policies.Get<IBuildPlanPolicy>((object) context.BuildKey, out containingPolicyList1);
      if (plan == null || plan is OverriddenBuildPlanMarkerPolicy)
      {
        IPolicyList containingPolicyList2;
        IBuildPlanCreatorPolicy planCreatorPolicy = context.Policies.Get<IBuildPlanCreatorPolicy>((object) context.BuildKey, out containingPolicyList2);
        if (planCreatorPolicy != null)
        {
          plan = planCreatorPolicy.CreatePlan(context, context.BuildKey);
          (containingPolicyList1 ?? containingPolicyList2).Set<IBuildPlanPolicy>(plan, (object) context.BuildKey);
        }
      }
      plan?.BuildUp(context);
    }
  }
}
