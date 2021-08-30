// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.InjectionConstructor
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class InjectionConstructor : InjectionMember
  {
    private readonly List<InjectionParameterValue> parameterValues;

    public InjectionConstructor(params object[] parameterValues) => this.parameterValues = InjectionParameterValue.ToParameters(parameterValues).ToList<InjectionParameterValue>();

    public override void AddPolicies(
      Type serviceType,
      Type implementationType,
      string name,
      IPolicyList policies)
    {
      ConstructorInfo constructor = this.FindConstructor(implementationType);
      policies.Set<IConstructorSelectorPolicy>((IConstructorSelectorPolicy) new SpecifiedConstructorSelectorPolicy(constructor, this.parameterValues.ToArray()), (object) new NamedTypeBuildKey(implementationType, name));
    }

    private ConstructorInfo FindConstructor(Type typeToCreate)
    {
      ParameterMatcher parameterMatcher = new ParameterMatcher((IEnumerable<InjectionParameterValue>) this.parameterValues);
      foreach (ConstructorInfo instanceConstructor in new ReflectionHelper(typeToCreate).InstanceConstructors)
      {
        if (parameterMatcher.Matches((IEnumerable<ParameterInfo>) instanceConstructor.GetParameters()))
          return instanceConstructor;
      }
      string str = string.Join(", ", this.parameterValues.Select<InjectionParameterValue, string>((Func<InjectionParameterValue, string>) (p => p.ParameterTypeName)).ToArray<string>());
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.NoSuchConstructor, new object[2]
      {
        (object) typeToCreate.FullName,
        (object) str
      }));
    }
  }
}
