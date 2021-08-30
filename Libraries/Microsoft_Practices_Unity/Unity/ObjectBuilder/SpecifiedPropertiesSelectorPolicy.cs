// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.SpecifiedPropertiesSelectorPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
  public class SpecifiedPropertiesSelectorPolicy : IPropertySelectorPolicy, IBuilderPolicy
  {
    private readonly List<Pair<PropertyInfo, InjectionParameterValue>> propertiesAndValues = new List<Pair<PropertyInfo, InjectionParameterValue>>();

    public void AddPropertyAndValue(PropertyInfo property, InjectionParameterValue value) => this.propertiesAndValues.Add(Pair.Make<PropertyInfo, InjectionParameterValue>(property, value));

    public IEnumerable<SelectedProperty> SelectProperties(
      IBuilderContext context,
      IPolicyList resolverPolicyDestination)
    {
      Type typeToBuild = context.BuildKey.Type;
      ReflectionHelper currentTypeReflector = new ReflectionHelper(context.BuildKey.Type);
      foreach (Pair<PropertyInfo, InjectionParameterValue> pair in this.propertiesAndValues)
      {
        PropertyInfo currentProperty = pair.First;
        if (new ReflectionHelper(pair.First.DeclaringType).IsOpenGeneric)
          currentProperty = currentTypeReflector.Type.GetTypeInfo().GetDeclaredProperty(currentProperty.Name);
        yield return new SelectedProperty(currentProperty, pair.Second.GetResolverPolicy(typeToBuild));
      }
    }
  }
}
