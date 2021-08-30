// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.SpecifiedConstructorSelectorPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
  public class SpecifiedConstructorSelectorPolicy : IConstructorSelectorPolicy, IBuilderPolicy
  {
    private readonly ConstructorInfo ctor;
    private readonly MethodReflectionHelper ctorReflector;
    private readonly InjectionParameterValue[] parameterValues;

    public SpecifiedConstructorSelectorPolicy(
      ConstructorInfo ctor,
      InjectionParameterValue[] parameterValues)
    {
      this.ctor = ctor;
      this.ctorReflector = new MethodReflectionHelper((MethodBase) ctor);
      this.parameterValues = parameterValues;
    }

    public SelectedConstructor SelectConstructor(
      IBuilderContext context,
      IPolicyList resolverPolicyDestination)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      Type type = context.BuildKey.Type;
      ReflectionHelper reflectionHelper = new ReflectionHelper(this.ctor.DeclaringType);
      SelectedConstructor selectedConstructor;
      if (!this.ctorReflector.MethodHasOpenGenericParameters && !reflectionHelper.IsOpenGeneric)
      {
        selectedConstructor = new SelectedConstructor(this.ctor);
      }
      else
      {
        Type[] closedParameterTypes = this.ctorReflector.GetClosedParameterTypes(type.GenericTypeArguments);
        selectedConstructor = new SelectedConstructor(TypeReflectionExtensions.GetConstructor(type, closedParameterTypes));
      }
      SpecifiedMemberSelectorHelper.AddParameterResolvers(type, resolverPolicyDestination, (IEnumerable<InjectionParameterValue>) this.parameterValues, (SelectedMemberWithParameters) selectedConstructor);
      return selectedConstructor;
    }
  }
}
