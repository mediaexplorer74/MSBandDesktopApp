// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.InjectionMethod
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class InjectionMethod : InjectionMember
  {
    private readonly string methodName;
    private readonly List<InjectionParameterValue> methodParameters;

    public InjectionMethod(string methodName, params object[] methodParameters)
    {
      this.methodName = methodName;
      this.methodParameters = InjectionParameterValue.ToParameters(methodParameters).ToList<InjectionParameterValue>();
    }

    public override void AddPolicies(
      Type serviceType,
      Type implementationType,
      string name,
      IPolicyList policies)
    {
      MethodInfo method = this.FindMethod(implementationType);
      this.ValidateMethodCanBeInjected(method, implementationType);
      InjectionMethod.GetSelectorPolicy(policies, implementationType, name).AddMethodAndParameters(method, (IEnumerable<InjectionParameterValue>) this.methodParameters);
    }

    protected virtual bool MethodNameMatches(MemberInfo targetMethod, string nameToMatch)
    {
      Guard.ArgumentNotNull((object) targetMethod, nameof (targetMethod));
      return targetMethod.Name == nameToMatch;
    }

    private MethodInfo FindMethod(Type typeToCreate)
    {
      ParameterMatcher parameterMatcher = new ParameterMatcher((IEnumerable<InjectionParameterValue>) this.methodParameters);
      foreach (MethodInfo methodInfo in typeToCreate.GetMethodsHierarchical())
      {
        if (this.MethodNameMatches((MemberInfo) methodInfo, this.methodName) && parameterMatcher.Matches((IEnumerable<ParameterInfo>) methodInfo.GetParameters()))
          return methodInfo;
      }
      return (MethodInfo) null;
    }

    private void ValidateMethodCanBeInjected(MethodInfo method, Type typeToCreate)
    {
      this.GuardMethodNotNull(method, typeToCreate);
      this.GuardMethodNotStatic(method, typeToCreate);
      this.GuardMethodNotGeneric(method, typeToCreate);
      this.GuardMethodHasNoOutParams(method, typeToCreate);
      this.GuardMethodHasNoRefParams(method, typeToCreate);
    }

    private void GuardMethodNotNull(MethodInfo info, Type typeToCreate)
    {
      if ((object) info != null)
        return;
      this.ThrowIllegalInjectionMethod(Resources.NoSuchMethod, typeToCreate);
    }

    private void GuardMethodNotStatic(MethodInfo info, Type typeToCreate)
    {
      if (!info.IsStatic)
        return;
      this.ThrowIllegalInjectionMethod(Resources.CannotInjectStaticMethod, typeToCreate);
    }

    private void GuardMethodNotGeneric(MethodInfo info, Type typeToCreate)
    {
      if (!info.IsGenericMethodDefinition)
        return;
      this.ThrowIllegalInjectionMethod(Resources.CannotInjectGenericMethod, typeToCreate);
    }

    private void GuardMethodHasNoOutParams(MethodInfo info, Type typeToCreate)
    {
      if (!((IEnumerable<ParameterInfo>) info.GetParameters()).Any<ParameterInfo>((Func<ParameterInfo, bool>) (param => param.IsOut)))
        return;
      this.ThrowIllegalInjectionMethod(Resources.CannotInjectMethodWithOutParams, typeToCreate);
    }

    private void GuardMethodHasNoRefParams(MethodInfo info, Type typeToCreate)
    {
      if (!((IEnumerable<ParameterInfo>) info.GetParameters()).Any<ParameterInfo>((Func<ParameterInfo, bool>) (param => param.ParameterType.IsByRef)))
        return;
      this.ThrowIllegalInjectionMethod(Resources.CannotInjectMethodWithRefParams, typeToCreate);
    }

    private void ThrowIllegalInjectionMethod(string message, Type typeToCreate) => throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, message, new object[3]
    {
      (object) typeToCreate.GetTypeInfo().Name,
      (object) this.methodName,
      (object) this.methodParameters.JoinStrings<InjectionParameterValue>(", ", (Func<InjectionParameterValue, string>) (mp => mp.ParameterTypeName))
    }));

    private static SpecifiedMethodsSelectorPolicy GetSelectorPolicy(
      IPolicyList policies,
      Type typeToCreate,
      string name)
    {
      NamedTypeBuildKey namedTypeBuildKey = new NamedTypeBuildKey(typeToCreate, name);
      IMethodSelectorPolicy policy = policies.GetNoDefault<IMethodSelectorPolicy>((object) namedTypeBuildKey, false);
      if (policy == null || !(policy is SpecifiedMethodsSelectorPolicy))
      {
        policy = (IMethodSelectorPolicy) new SpecifiedMethodsSelectorPolicy();
        policies.Set<IMethodSelectorPolicy>(policy, (object) namedTypeBuildKey);
      }
      return (SpecifiedMethodsSelectorPolicy) policy;
    }
  }
}
