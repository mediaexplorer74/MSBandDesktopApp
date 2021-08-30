// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.SpecifiedMethodsSelectorPolicy
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
  public class SpecifiedMethodsSelectorPolicy : IMethodSelectorPolicy, IBuilderPolicy
  {
    private readonly List<Pair<MethodInfo, IEnumerable<InjectionParameterValue>>> methods = new List<Pair<MethodInfo, IEnumerable<InjectionParameterValue>>>();

    public void AddMethodAndParameters(
      MethodInfo method,
      IEnumerable<InjectionParameterValue> parameters)
    {
      this.methods.Add(Pair.Make<MethodInfo, IEnumerable<InjectionParameterValue>>(method, parameters));
    }

    public IEnumerable<SelectedMethod> SelectMethods(
      IBuilderContext context,
      IPolicyList resolverPolicyDestination)
    {
      foreach (Pair<MethodInfo, IEnumerable<InjectionParameterValue>> method in this.methods)
      {
        Type typeToBuild = context.BuildKey.Type;
        ReflectionHelper typeReflector = new ReflectionHelper(method.First.DeclaringType);
        MethodReflectionHelper methodReflector = new MethodReflectionHelper((MethodBase) method.First);
        SelectedMethod selectedMethod = methodReflector.MethodHasOpenGenericParameters || typeReflector.IsOpenGeneric ? new SelectedMethod(typeToBuild.GetMethodHierarchical(method.First.Name, methodReflector.GetClosedParameterTypes(typeToBuild.GetTypeInfo().GenericTypeArguments))) : new SelectedMethod(method.First);
        SpecifiedMemberSelectorHelper.AddParameterResolvers(typeToBuild, resolverPolicyDestination, method.Second, (SelectedMemberWithParameters) selectedMethod);
        yield return selectedMethod;
      }
    }
  }
}
