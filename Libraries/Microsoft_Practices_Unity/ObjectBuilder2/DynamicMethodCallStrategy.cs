// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DynamicMethodCallStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class DynamicMethodCallStrategy : BuilderStrategy
  {
    private static readonly MethodInfo SetCurrentOperationToResolvingParameterMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodCallStrategy.SetCurrentOperationToResolvingParameter(default (string), default (string), default (IBuilderContext))));
    private static readonly MethodInfo SetCurrentOperationToInvokingMethodInfo = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodCallStrategy.SetCurrentOperationToInvokingMethod(default (string), default (IBuilderContext))));

    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      DynamicBuildPlanGenerationContext existing = (DynamicBuildPlanGenerationContext) context.Existing;
      IPolicyList containingPolicyList;
      IMethodSelectorPolicy methodSelectorPolicy = context.Policies.Get<IMethodSelectorPolicy>((object) context.BuildKey, out containingPolicyList);
      bool flag = false;
      foreach (SelectedMethod selectMethod in methodSelectorPolicy.SelectMethods(context, containingPolicyList))
      {
        flag = true;
        string methodSignature = DynamicMethodCallStrategy.GetMethodSignature((MethodBase) selectMethod.Method);
        DynamicMethodCallStrategy.GuardMethodIsNotOpenGeneric(selectMethod.Method);
        DynamicMethodCallStrategy.GuardMethodHasNoOutParams(selectMethod.Method);
        DynamicMethodCallStrategy.GuardMethodHasNoRefParams(selectMethod.Method);
        existing.AddToBuildPlan((Expression) Expression.Block((Expression) Expression.Call((Expression) null, DynamicMethodCallStrategy.SetCurrentOperationToInvokingMethodInfo, (Expression) Expression.Constant((object) methodSignature), (Expression) existing.ContextParameter), (Expression) Expression.Call((Expression) Expression.Convert(existing.GetExistingObjectExpression(), existing.TypeToBuild), selectMethod.Method, this.BuildMethodParameterExpressions(existing, selectMethod, methodSignature))));
      }
      if (!flag)
        return;
      existing.AddToBuildPlan(existing.GetClearCurrentOperationExpression());
    }

    private IEnumerable<Expression> BuildMethodParameterExpressions(
      DynamicBuildPlanGenerationContext context,
      SelectedMethod method,
      string methodSignature)
    {
      int i = 0;
      ParameterInfo[] methodParameters = method.Method.GetParameters();
      foreach (IDependencyResolverPolicy parameterResolver in method.GetParameterResolvers())
      {
        yield return context.CreateParameterExpression(parameterResolver, methodParameters[i].ParameterType, (Expression) Expression.Call((Expression) null, DynamicMethodCallStrategy.SetCurrentOperationToResolvingParameterMethod, (Expression) Expression.Constant((object) methodParameters[i].Name, typeof (string)), (Expression) Expression.Constant((object) methodSignature), (Expression) context.ContextParameter));
        ++i;
      }
    }

    private static void GuardMethodIsNotOpenGeneric(MethodInfo method)
    {
      if (!method.IsGenericMethodDefinition)
        return;
      DynamicMethodCallStrategy.ThrowIllegalInjectionMethod(Resources.CannotInjectOpenGenericMethod, method);
    }

    private static void GuardMethodHasNoOutParams(MethodInfo method)
    {
      if (!((IEnumerable<ParameterInfo>) method.GetParameters()).Any<ParameterInfo>((Func<ParameterInfo, bool>) (param => param.IsOut)))
        return;
      DynamicMethodCallStrategy.ThrowIllegalInjectionMethod(Resources.CannotInjectMethodWithOutParam, method);
    }

    private static void GuardMethodHasNoRefParams(MethodInfo method)
    {
      if (!((IEnumerable<ParameterInfo>) method.GetParameters()).Any<ParameterInfo>((Func<ParameterInfo, bool>) (param => param.ParameterType.IsByRef)))
        return;
      DynamicMethodCallStrategy.ThrowIllegalInjectionMethod(Resources.CannotInjectMethodWithOutParam, method);
    }

    private static void ThrowIllegalInjectionMethod(string format, MethodInfo method) => throw new IllegalInjectionMethodException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, new object[2]
    {
      (object) method.DeclaringType.GetTypeInfo().Name,
      (object) method.Name
    }));

    public static void SetCurrentOperationToResolvingParameter(
      string parameterName,
      string methodSignature,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      context.CurrentOperation = (object) new MethodArgumentResolveOperation(context.BuildKey.Type, methodSignature, parameterName);
    }

    public static void SetCurrentOperationToInvokingMethod(
      string methodSignature,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      context.CurrentOperation = (object) new InvokingMethodOperation(context.BuildKey.Type, methodSignature);
    }

    private static string GetMethodSignature(MethodBase method)
    {
      string name = method.Name;
      ParameterInfo[] parameters = method.GetParameters();
      string[] strArray = new string[parameters.Length];
      for (int index = 0; index < parameters.Length; ++index)
        strArray[index] = parameters[index].ParameterType.FullName + " " + parameters[index].Name;
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}({1})", new object[2]
      {
        (object) name,
        (object) string.Join(", ", strArray)
      });
    }
  }
}
