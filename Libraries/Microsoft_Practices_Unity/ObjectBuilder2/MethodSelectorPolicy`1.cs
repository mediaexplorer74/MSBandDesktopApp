// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.MethodSelectorPolicy`1
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class MethodSelectorPolicy<TMarkerAttribute> : MethodSelectorPolicyBase<TMarkerAttribute>
    where TMarkerAttribute : Attribute
  {
    protected override IDependencyResolverPolicy CreateResolver(
      ParameterInfo parameter)
    {
      Guard.ArgumentNotNull((object) parameter, nameof (parameter));
      return (IDependencyResolverPolicy) new FixedTypeResolverPolicy(parameter.ParameterType);
    }
  }
}
