// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.DefaultUnityPropertySelectorPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
  public class DefaultUnityPropertySelectorPolicy : 
    PropertySelectorBase<DependencyResolutionAttribute>
  {
    protected override IDependencyResolverPolicy CreateResolver(
      PropertyInfo property)
    {
      Guard.ArgumentNotNull((object) property, nameof (property));
      return CustomAttributeExtensions.GetCustomAttributes(property, typeof (DependencyResolutionAttribute), false).OfType<DependencyResolutionAttribute>().ToList<DependencyResolutionAttribute>()[0].CreateResolver(property.PropertyType);
    }
  }
}
