// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.ConstructorSelectorPolicyBase`1
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public abstract class ConstructorSelectorPolicyBase<TInjectionConstructorMarkerAttribute> : 
    IConstructorSelectorPolicy,
    IBuilderPolicy
    where TInjectionConstructorMarkerAttribute : Attribute
  {
    public SelectedConstructor SelectConstructor(
      IBuilderContext context,
      IPolicyList resolverPolicyDestination)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      Type type = context.BuildKey.Type;
      ConstructorInfo constructorInfo = ConstructorSelectorPolicyBase<TInjectionConstructorMarkerAttribute>.FindInjectionConstructor(type);
      if ((object) constructorInfo == null)
        constructorInfo = ConstructorSelectorPolicyBase<TInjectionConstructorMarkerAttribute>.FindLongestConstructor(type);
      ConstructorInfo ctor = constructorInfo;
      return (object) ctor != null ? this.CreateSelectedConstructor(ctor) : (SelectedConstructor) null;
    }

    private SelectedConstructor CreateSelectedConstructor(ConstructorInfo ctor)
    {
      SelectedConstructor selectedConstructor = new SelectedConstructor(ctor);
      foreach (ParameterInfo parameter in ctor.GetParameters())
        selectedConstructor.AddParameterResolver(this.CreateResolver(parameter));
      return selectedConstructor;
    }

    protected abstract IDependencyResolverPolicy CreateResolver(
      ParameterInfo parameter);

    private static ConstructorInfo FindInjectionConstructor(Type typeToConstruct)
    {
      ConstructorInfo[] array = new ReflectionHelper(typeToConstruct).InstanceConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (ctor => CustomAttributeExtensions.IsDefined(ctor, typeof (TInjectionConstructorMarkerAttribute), true))).ToArray<ConstructorInfo>();
      switch (array.Length)
      {
        case 0:
          return (ConstructorInfo) null;
        case 1:
          return array[0];
        default:
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.MultipleInjectionConstructors, new object[1]
          {
            (object) typeToConstruct.GetTypeInfo().Name
          }));
      }
    }

    private static ConstructorInfo FindLongestConstructor(Type typeToConstruct)
    {
      ConstructorInfo[] array = new ReflectionHelper(typeToConstruct).InstanceConstructors.ToArray<ConstructorInfo>();
      Array.Sort<ConstructorInfo>(array, (IComparer<ConstructorInfo>) new ConstructorSelectorPolicyBase<TInjectionConstructorMarkerAttribute>.ConstructorLengthComparer());
      switch (array.Length)
      {
        case 0:
          return (ConstructorInfo) null;
        case 1:
          return array[0];
        default:
          int length = array[0].GetParameters().Length;
          if (array[1].GetParameters().Length == length)
            throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.AmbiguousInjectionConstructor, new object[2]
            {
              (object) typeToConstruct.GetTypeInfo().Name,
              (object) length
            }));
          return array[0];
      }
    }

    private class ConstructorLengthComparer : IComparer<ConstructorInfo>
    {
      public int Compare(ConstructorInfo x, ConstructorInfo y)
      {
        Guard.ArgumentNotNull((object) x, nameof (x));
        Guard.ArgumentNotNull((object) y, nameof (y));
        return y.GetParameters().Length - x.GetParameters().Length;
      }
    }
  }
}
