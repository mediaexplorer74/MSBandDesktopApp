// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ArrayResolutionStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class ArrayResolutionStrategy : BuilderStrategy
  {
    private static readonly MethodInfo GenericResolveArrayMethod = typeof (ArrayResolutionStrategy).GetTypeInfo().DeclaredMethods.First<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "ResolveArray" && !m.IsPublic && m.IsStatic));

    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      Type type = context.BuildKey.Type;
      if (!type.IsArray || type.GetArrayRank() != 1)
        return;
      Type elementType = type.GetElementType();
      ArrayResolutionStrategy.ArrayResolver arrayResolver = (ArrayResolutionStrategy.ArrayResolver) ArrayResolutionStrategy.GenericResolveArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof (ArrayResolutionStrategy.ArrayResolver));
      context.Existing = arrayResolver(context);
      context.BuildComplete = true;
    }

    private static object ResolveArray<T>(IBuilderContext context)
    {
      IRegisteredNamesPolicy registeredNamesPolicy = context.Policies.Get<IRegisteredNamesPolicy>((object) null);
      if (registeredNamesPolicy == null)
        return (object) new T[0];
      IEnumerable<string> strings = registeredNamesPolicy.GetRegisteredNames(typeof (T));
      if (typeof (T).GetTypeInfo().IsGenericType)
        strings = strings.Concat<string>(registeredNamesPolicy.GetRegisteredNames(typeof (T).GetGenericTypeDefinition()));
      return (object) strings.Distinct<string>().Select<string, T>((Func<string, T>) (n => context.NewBuildUp<T>(n))).ToArray<T>();
    }

    private delegate object ArrayResolver(IBuilderContext context);
  }
}
