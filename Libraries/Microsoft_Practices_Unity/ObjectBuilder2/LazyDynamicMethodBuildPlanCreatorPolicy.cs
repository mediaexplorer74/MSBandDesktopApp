// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.LazyDynamicMethodBuildPlanCreatorPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class LazyDynamicMethodBuildPlanCreatorPolicy : IBuildPlanCreatorPolicy, IBuilderPolicy
  {
    private static readonly MethodInfo BuildResolveLazyMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => LazyDynamicMethodBuildPlanCreatorPolicy.BuildResolveLazy<object>(default (IBuilderContext)))).GetGenericMethodDefinition();
    private static readonly MethodInfo BuildResolveAllLazyMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => LazyDynamicMethodBuildPlanCreatorPolicy.BuildResolveAllLazy<object>(default (IBuilderContext)))).GetGenericMethodDefinition();

    public IBuildPlanPolicy CreatePlan(
      IBuilderContext context,
      NamedTypeBuildKey buildKey)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      Guard.ArgumentNotNull((object) buildKey, nameof (buildKey));
      return (IBuildPlanPolicy) new DynamicMethodBuildPlan(LazyDynamicMethodBuildPlanCreatorPolicy.CreateBuildPlanMethod(buildKey.Type));
    }

    private static DynamicBuildPlanMethod CreateBuildPlanMethod(Type lazyType)
    {
      Type genericTypeArgument = lazyType.GetTypeInfo().GenericTypeArguments[0];
      MethodInfo methodInfo;
      if (genericTypeArgument.GetTypeInfo().IsGenericType && (object) genericTypeArgument.GetGenericTypeDefinition() == (object) typeof (IEnumerable<>))
        methodInfo = LazyDynamicMethodBuildPlanCreatorPolicy.BuildResolveAllLazyMethod.MakeGenericMethod(genericTypeArgument.GetTypeInfo().GenericTypeArguments[0]);
      else
        methodInfo = LazyDynamicMethodBuildPlanCreatorPolicy.BuildResolveLazyMethod.MakeGenericMethod(genericTypeArgument);
      return (DynamicBuildPlanMethod) methodInfo.CreateDelegate(typeof (DynamicBuildPlanMethod));
    }

    private static void BuildResolveLazy<T>(IBuilderContext context)
    {
      if (context.Existing == null)
      {
        string name = context.BuildKey.Name;
        IUnityContainer container = context.NewBuildUp<IUnityContainer>();
        context.Existing = (object) new Lazy<T>((Func<T>) (() => container.Resolve<T>(name)));
      }
      DynamicMethodConstructorStrategy.SetPerBuildSingleton(context);
    }

    private static void BuildResolveAllLazy<T>(IBuilderContext context)
    {
      if (context.Existing == null)
      {
        IUnityContainer container = context.NewBuildUp<IUnityContainer>();
        context.Existing = (object) new Lazy<IEnumerable<T>>((Func<IEnumerable<T>>) (() => container.ResolveAll<T>()));
      }
      DynamicMethodConstructorStrategy.SetPerBuildSingleton(context);
    }
  }
}
