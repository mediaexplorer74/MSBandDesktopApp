// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DynamicMethodConstructorStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity;
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
  public class DynamicMethodConstructorStrategy : BuilderStrategy
  {
    private static readonly MethodInfo ThrowForNullExistingObjectMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.ThrowForNullExistingObject(default (IBuilderContext))));
    private static readonly MethodInfo ThrowForNullExistingObjectWithInvalidConstructorMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.ThrowForNullExistingObjectWithInvalidConstructor(default (IBuilderContext), default (string))));
    private static readonly MethodInfo ThrowForAttemptingToConstructInterfaceMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.ThrowForAttemptingToConstructInterface(default (IBuilderContext))));
    private static readonly MethodInfo ThrowForAttemptingToConstructAbstractClassMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.ThrowForAttemptingToConstructAbstractClass(default (IBuilderContext))));
    private static readonly MethodInfo ThrowForAttemptingToConstructDelegateMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.ThrowForAttemptingToConstructDelegate(default (IBuilderContext))));
    private static readonly MethodInfo SetCurrentOperationToResolvingParameterMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.SetCurrentOperationToResolvingParameter(default (string), default (string), default (IBuilderContext))));
    private static readonly MethodInfo SetCurrentOperationToInvokingConstructorMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.SetCurrentOperationToInvokingConstructor(default (string), default (IBuilderContext))));
    private static readonly MethodInfo SetPerBuildSingletonMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodConstructorStrategy.SetPerBuildSingleton(default (IBuilderContext))));

    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      DynamicBuildPlanGenerationContext existing = (DynamicBuildPlanGenerationContext) context.Existing;
      DynamicMethodConstructorStrategy.GuardTypeIsNonPrimitive(context);
      existing.AddToBuildPlan((Expression) Expression.IfThen((Expression) Expression.Equal(existing.GetExistingObjectExpression(), (Expression) Expression.Constant((object) null)), this.CreateInstanceBuildupExpression(existing, context)));
      existing.AddToBuildPlan((Expression) Expression.Call((Expression) null, DynamicMethodConstructorStrategy.SetPerBuildSingletonMethod, (Expression) existing.ContextParameter));
    }

    internal Expression CreateInstanceBuildupExpression(
      DynamicBuildPlanGenerationContext buildContext,
      IBuilderContext context)
    {
      TypeInfo typeInfo = context.BuildKey.Type.GetTypeInfo();
      if (typeInfo.IsInterface)
        return DynamicMethodConstructorStrategy.CreateThrowWithContext(buildContext, DynamicMethodConstructorStrategy.ThrowForAttemptingToConstructInterfaceMethod);
      if (typeInfo.IsAbstract)
        return DynamicMethodConstructorStrategy.CreateThrowWithContext(buildContext, DynamicMethodConstructorStrategy.ThrowForAttemptingToConstructAbstractClassMethod);
      if (typeInfo.IsSubclassOf(typeof (Delegate)))
        return DynamicMethodConstructorStrategy.CreateThrowWithContext(buildContext, DynamicMethodConstructorStrategy.ThrowForAttemptingToConstructDelegateMethod);
      IPolicyList containingPolicyList;
      SelectedConstructor selectedConstructor = context.Policies.Get<IConstructorSelectorPolicy>((object) context.BuildKey, out containingPolicyList).SelectConstructor(context, containingPolicyList);
      if (selectedConstructor == null)
        return DynamicMethodConstructorStrategy.CreateThrowWithContext(buildContext, DynamicMethodConstructorStrategy.ThrowForNullExistingObjectMethod);
      string signatureString = DynamicMethodConstructorStrategy.CreateSignatureString(selectedConstructor.Constructor);
      return DynamicMethodConstructorStrategy.IsInvalidConstructor(selectedConstructor) ? DynamicMethodConstructorStrategy.CreateThrowForNullExistingObjectWithInvalidConstructor(buildContext, signatureString) : (Expression) Expression.Block(this.CreateNewBuildupSequence(buildContext, selectedConstructor, signatureString));
    }

    private static bool IsInvalidConstructor(SelectedConstructor selectedConstructor) => ((IEnumerable<ParameterInfo>) selectedConstructor.Constructor.GetParameters()).Any<ParameterInfo>((Func<ParameterInfo, bool>) (pi => pi.ParameterType.IsByRef));

    private static Expression CreateThrowWithContext(
      DynamicBuildPlanGenerationContext buildContext,
      MethodInfo throwMethod)
    {
      return (Expression) Expression.Call((Expression) null, throwMethod, (Expression) buildContext.ContextParameter);
    }

    private static Expression CreateThrowForNullExistingObjectWithInvalidConstructor(
      DynamicBuildPlanGenerationContext buildContext,
      string signature)
    {
      return (Expression) Expression.Call((Expression) null, DynamicMethodConstructorStrategy.ThrowForNullExistingObjectWithInvalidConstructorMethod, (Expression) buildContext.ContextParameter, (Expression) Expression.Constant((object) signature, typeof (string)));
    }

    private IEnumerable<Expression> CreateNewBuildupSequence(
      DynamicBuildPlanGenerationContext buildContext,
      SelectedConstructor selectedConstructor,
      string signature)
    {
      IEnumerable<Expression> parameterExpressions = this.BuildConstructionParameterExpressions(buildContext, selectedConstructor, signature);
      Expression.Variable(selectedConstructor.Constructor.DeclaringType, "newItem");
      yield return (Expression) Expression.Call((Expression) null, DynamicMethodConstructorStrategy.SetCurrentOperationToInvokingConstructorMethod, (Expression) Expression.Constant((object) signature), (Expression) buildContext.ContextParameter);
      yield return (Expression) Expression.Assign(buildContext.GetExistingObjectExpression(), (Expression) Expression.Convert((Expression) Expression.New(selectedConstructor.Constructor, parameterExpressions), typeof (object)));
      yield return buildContext.GetClearCurrentOperationExpression();
    }

    private IEnumerable<Expression> BuildConstructionParameterExpressions(
      DynamicBuildPlanGenerationContext buildContext,
      SelectedConstructor selectedConstructor,
      string constructorSignature)
    {
      int i = 0;
      ParameterInfo[] constructionParameters = selectedConstructor.Constructor.GetParameters();
      foreach (IDependencyResolverPolicy parameterResolver in selectedConstructor.GetParameterResolvers())
      {
        yield return buildContext.CreateParameterExpression(parameterResolver, constructionParameters[i].ParameterType, (Expression) Expression.Call((Expression) null, DynamicMethodConstructorStrategy.SetCurrentOperationToResolvingParameterMethod, (Expression) Expression.Constant((object) constructionParameters[i].Name, typeof (string)), (Expression) Expression.Constant((object) constructorSignature), (Expression) buildContext.ContextParameter));
        ++i;
      }
    }

    public static void SetPerBuildSingleton(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      if (!(context.Policies.Get<ILifetimePolicy>((object) context.BuildKey) is PerResolveLifetimeManager))
        return;
      PerResolveLifetimeManager resolveLifetimeManager = new PerResolveLifetimeManager(context.Existing);
      context.Policies.Set<ILifetimePolicy>((ILifetimePolicy) resolveLifetimeManager, (object) context.BuildKey);
    }

    public static string CreateSignatureString(ConstructorInfo constructor)
    {
      Guard.ArgumentNotNull((object) constructor, nameof (constructor));
      string fullName = constructor.DeclaringType.FullName;
      ParameterInfo[] parameters = constructor.GetParameters();
      string[] strArray = new string[parameters.Length];
      for (int index = 0; index < parameters.Length; ++index)
        strArray[index] = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} {1}", new object[2]
        {
          (object) parameters[index].ParameterType.FullName,
          (object) parameters[index].Name
        });
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}({1})", new object[2]
      {
        (object) fullName,
        (object) string.Join(", ", strArray)
      });
    }

    private static void GuardTypeIsNonPrimitive(IBuilderContext context)
    {
      Type type = context.BuildKey.Type;
      if (!type.GetTypeInfo().IsInterface && (object) type == (object) typeof (string))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TypeIsNotConstructable, new object[1]
        {
          (object) type.GetTypeInfo().Name
        }));
    }

    public static void SetCurrentOperationToResolvingParameter(
      string parameterName,
      string constructorSignature,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      context.CurrentOperation = (object) new ConstructorArgumentResolveOperation(context.BuildKey.Type, constructorSignature, parameterName);
    }

    public static void SetCurrentOperationToInvokingConstructor(
      string constructorSignature,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      context.CurrentOperation = (object) new InvokingConstructorOperation(context.BuildKey.Type, constructorSignature);
    }

    public static void ThrowForAttemptingToConstructInterface(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.CannotConstructInterface, new object[2]
      {
        (object) context.BuildKey.Type,
        (object) context.BuildKey
      }));
    }

    public static void ThrowForAttemptingToConstructAbstractClass(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.CannotConstructAbstractClass, new object[2]
      {
        (object) context.BuildKey.Type,
        (object) context.BuildKey
      }));
    }

    public static void ThrowForAttemptingToConstructDelegate(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.CannotConstructDelegate, new object[2]
      {
        (object) context.BuildKey.Type,
        (object) context.BuildKey
      }));
    }

    public static void ThrowForNullExistingObject(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.NoConstructorFound, new object[1]
      {
        (object) context.BuildKey.Type.GetTypeInfo().Name
      }));
    }

    public static void ThrowForNullExistingObjectWithInvalidConstructor(
      IBuilderContext context,
      string signature)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.SelectedConstructorHasRefParameters, new object[2]
      {
        (object) context.BuildKey.Type.GetTypeInfo().Name,
        (object) signature
      }));
    }
  }
}
