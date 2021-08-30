// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.PropertySelectorBase`1
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
  public abstract class PropertySelectorBase<TResolutionAttribute> : 
    IPropertySelectorPolicy,
    IBuilderPolicy
    where TResolutionAttribute : Attribute
  {
    public virtual IEnumerable<SelectedProperty> SelectProperties(
      IBuilderContext context,
      IPolicyList resolverPolicyDestination)
    {
      Type t = context.BuildKey.Type;
      foreach (PropertyInfo prop in t.GetPropertiesHierarchical().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.CanWrite)))
      {
        MethodInfo methodInfo = prop.SetMethod;
        if ((object) methodInfo == null)
          methodInfo = prop.GetMethod;
        MethodInfo propertyMethod = methodInfo;
        if (!propertyMethod.IsStatic && prop.GetIndexParameters().Length == 0 && CustomAttributeExtensions.IsDefined(prop, typeof (TResolutionAttribute), false))
          yield return this.CreateSelectedProperty(prop);
      }
    }

    private SelectedProperty CreateSelectedProperty(PropertyInfo property)
    {
      IDependencyResolverPolicy resolver = this.CreateResolver(property);
      return new SelectedProperty(property, resolver);
    }

    protected abstract IDependencyResolverPolicy CreateResolver(
      PropertyInfo property);
  }
}
