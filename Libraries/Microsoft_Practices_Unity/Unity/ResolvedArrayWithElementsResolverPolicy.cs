// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ResolvedArrayWithElementsResolverPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class ResolvedArrayWithElementsResolverPolicy : IDependencyResolverPolicy, IBuilderPolicy
  {
    private readonly ResolvedArrayWithElementsResolverPolicy.Resolver resolver;
    private readonly IDependencyResolverPolicy[] elementPolicies;

    public ResolvedArrayWithElementsResolverPolicy(
      Type elementType,
      params IDependencyResolverPolicy[] elementPolicies)
    {
      Guard.ArgumentNotNull((object) elementType, nameof (elementType));
      this.resolver = (ResolvedArrayWithElementsResolverPolicy.Resolver) typeof (ResolvedArrayWithElementsResolverPolicy).GetTypeInfo().GetDeclaredMethod("DoResolve").MakeGenericMethod(elementType).CreateDelegate(typeof (ResolvedArrayWithElementsResolverPolicy.Resolver));
      this.elementPolicies = elementPolicies;
    }

    public object Resolve(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return this.resolver(context, this.elementPolicies);
    }

    private static object DoResolve<T>(
      IBuilderContext context,
      IDependencyResolverPolicy[] elementPolicies)
    {
      T[] objArray = new T[elementPolicies.Length];
      for (int index = 0; index < elementPolicies.Length; ++index)
        objArray[index] = (T) elementPolicies[index].Resolve(context);
      return (object) objArray;
    }

    private delegate object Resolver(
      IBuilderContext context,
      IDependencyResolverPolicy[] elementPolicies);
  }
}
