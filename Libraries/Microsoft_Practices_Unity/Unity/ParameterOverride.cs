// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ParameterOverride
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.Unity
{
  public class ParameterOverride : ResolverOverride
  {
    private readonly string parameterName;
    private readonly InjectionParameterValue parameterValue;

    public ParameterOverride(string parameterName, object parameterValue)
    {
      this.parameterName = parameterName;
      this.parameterValue = InjectionParameterValue.ToParameter(parameterValue);
    }

    public override IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return context.CurrentOperation is ConstructorArgumentResolveOperation currentOperation && currentOperation.ParameterName == this.parameterName ? this.parameterValue.GetResolverPolicy(dependencyType) : (IDependencyResolverPolicy) null;
    }
  }
}
