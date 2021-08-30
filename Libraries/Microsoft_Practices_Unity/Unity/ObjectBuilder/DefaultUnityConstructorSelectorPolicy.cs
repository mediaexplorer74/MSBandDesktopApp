// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.DefaultUnityConstructorSelectorPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
  public class DefaultUnityConstructorSelectorPolicy : 
    ConstructorSelectorPolicyBase<InjectionConstructorAttribute>
  {
    protected override IDependencyResolverPolicy CreateResolver(
      ParameterInfo parameter)
    {
      Guard.ArgumentNotNull((object) parameter, nameof (parameter));
      List<DependencyResolutionAttribute> list = CustomAttributeExtensions.GetCustomAttributes(parameter, false).OfType<DependencyResolutionAttribute>().ToList<DependencyResolutionAttribute>();
      return list.Count > 0 ? list[0].CreateResolver(parameter.ParameterType) : (IDependencyResolverPolicy) new NamedTypeDependencyResolverPolicy(parameter.ParameterType, (string) null);
    }
  }
}
