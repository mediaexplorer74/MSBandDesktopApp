// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DeferredResolveBuildPlanPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  internal class DeferredResolveBuildPlanPolicy : IBuildPlanPolicy, IBuilderPolicy
  {
    public void BuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      if (context.Existing != null)
        return;
      IUnityContainer currentContainer = context.NewBuildUp<IUnityContainer>();
      Type typeToBuild = DeferredResolveBuildPlanPolicy.GetTypeToBuild(context.BuildKey.Type);
      string name = context.BuildKey.Name;
      Delegate @delegate = !DeferredResolveBuildPlanPolicy.IsResolvingIEnumerable(typeToBuild) ? DeferredResolveBuildPlanPolicy.CreateResolver(currentContainer, typeToBuild, name) : DeferredResolveBuildPlanPolicy.CreateResolveAllResolver(currentContainer, typeToBuild);
      context.Existing = (object) @delegate;
      DynamicMethodConstructorStrategy.SetPerBuildSingleton(context);
    }

    private static Type GetTypeToBuild(Type t) => t.GetTypeInfo().GenericTypeArguments[0];

    private static bool IsResolvingIEnumerable(Type typeToBuild) => typeToBuild.GetTypeInfo().IsGenericType && (object) typeToBuild.GetGenericTypeDefinition() == (object) typeof (IEnumerable<>);

    private static Delegate CreateResolver(
      IUnityContainer currentContainer,
      Type typeToBuild,
      string nameToBuild)
    {
      Type type = typeof (DeferredResolveBuildPlanPolicy.ResolveTrampoline<>).MakeGenericType(typeToBuild);
      Type delegateType = typeof (Func<>).MakeGenericType(typeToBuild);
      MethodInfo declaredMethod = type.GetTypeInfo().GetDeclaredMethod("Resolve");
      object instance = Activator.CreateInstance(type, (object) currentContainer, (object) nameToBuild);
      return declaredMethod.CreateDelegate(delegateType, instance);
    }

    private static Delegate CreateResolveAllResolver(
      IUnityContainer currentContainer,
      Type enumerableType)
    {
      Type type = typeof (DeferredResolveBuildPlanPolicy.ResolveAllTrampoline<>).MakeGenericType(DeferredResolveBuildPlanPolicy.GetTypeToBuild(enumerableType));
      Type delegateType = typeof (Func<>).MakeGenericType(enumerableType);
      MethodInfo declaredMethod = type.GetTypeInfo().GetDeclaredMethod("ResolveAll");
      object instance = Activator.CreateInstance(type, (object) currentContainer);
      return declaredMethod.CreateDelegate(delegateType, instance);
    }

    private class ResolveTrampoline<TItem>
    {
      private readonly IUnityContainer container;
      private readonly string name;

      public ResolveTrampoline(IUnityContainer container, string name)
      {
        this.container = container;
        this.name = name;
      }

      public TItem Resolve() => this.container.Resolve<TItem>(this.name);
    }

    private class ResolveAllTrampoline<TItem>
    {
      private readonly IUnityContainer container;

      public ResolveAllTrampoline(IUnityContainer container) => this.container = container;

      public IEnumerable<TItem> ResolveAll() => this.container.ResolveAll<TItem>();
    }
  }
}
