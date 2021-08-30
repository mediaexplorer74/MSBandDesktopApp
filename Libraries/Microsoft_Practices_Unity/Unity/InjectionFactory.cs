// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.InjectionFactory
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.Unity
{
  public class InjectionFactory : InjectionMember
  {
    private readonly Func<IUnityContainer, Type, string, object> factoryFunc;

    public InjectionFactory(Func<IUnityContainer, object> factoryFunc)
      : this((Func<IUnityContainer, Type, string, object>) ((c, t, s) => factoryFunc(c)))
    {
    }

    public InjectionFactory(
      Func<IUnityContainer, Type, string, object> factoryFunc)
    {
      Guard.ArgumentNotNull((object) factoryFunc, nameof (factoryFunc));
      this.factoryFunc = factoryFunc;
    }

    public override void AddPolicies(
      Type serviceType,
      Type implementationType,
      string name,
      IPolicyList policies)
    {
      Guard.ArgumentNotNull((object) implementationType, nameof (implementationType));
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      FactoryDelegateBuildPlanPolicy delegateBuildPlanPolicy = new FactoryDelegateBuildPlanPolicy(this.factoryFunc);
      policies.Set<IBuildPlanPolicy>((IBuildPlanPolicy) delegateBuildPlanPolicy, (object) new NamedTypeBuildKey(implementationType, name));
    }
  }
}
