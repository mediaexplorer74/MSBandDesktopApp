// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.MethodSelectorPolicyBase`1
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public abstract class MethodSelectorPolicyBase<TMarkerAttribute> : 
    IMethodSelectorPolicy,
    IBuilderPolicy
    where TMarkerAttribute : Attribute
  {
    public virtual IEnumerable<SelectedMethod> SelectMethods(
      IBuilderContext context,
      IPolicyList resolverPolicyDestination)
    {
      Type t = context.BuildKey.Type;
      IEnumerable<MethodInfo> candidateMethods = t.GetMethodsHierarchical().Where<MethodInfo>((Func<MethodInfo, bool>) (m => !m.IsStatic && m.IsPublic));
      foreach (MethodInfo method in candidateMethods)
      {
        if (CustomAttributeExtensions.IsDefined(method, typeof (TMarkerAttribute), false))
          yield return this.CreateSelectedMethod(method);
      }
    }

    private SelectedMethod CreateSelectedMethod(MethodInfo method)
    {
      SelectedMethod selectedMethod = new SelectedMethod(method);
      foreach (ParameterInfo parameter in method.GetParameters())
        selectedMethod.AddParameterResolver(this.CreateResolver(parameter));
      return selectedMethod;
    }

    protected abstract IDependencyResolverPolicy CreateResolver(
      ParameterInfo parameter);
  }
}
