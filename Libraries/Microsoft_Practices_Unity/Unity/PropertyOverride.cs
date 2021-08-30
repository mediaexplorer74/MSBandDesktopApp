// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.PropertyOverride
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.Unity
{
  public class PropertyOverride : ResolverOverride
  {
    private readonly string propertyName;
    private readonly InjectionParameterValue propertyValue;

    public PropertyOverride(string propertyName, object propertyValue)
    {
      this.propertyName = propertyName;
      this.propertyValue = InjectionParameterValue.ToParameter(propertyValue);
    }

    public override IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return context.CurrentOperation is ResolvingPropertyValueOperation currentOperation && currentOperation.PropertyName == this.propertyName ? this.propertyValue.GetResolverPolicy(dependencyType) : (IDependencyResolverPolicy) null;
    }
  }
}
