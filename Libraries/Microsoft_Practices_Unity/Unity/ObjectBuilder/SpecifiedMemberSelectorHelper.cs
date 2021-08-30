// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ObjectBuilder.SpecifiedMemberSelectorHelper
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
  public static class SpecifiedMemberSelectorHelper
  {
    public static void AddParameterResolvers(
      Type typeToBuild,
      IPolicyList policies,
      IEnumerable<InjectionParameterValue> parameterValues,
      SelectedMemberWithParameters result)
    {
      Guard.ArgumentNotNull((object) policies, nameof (policies));
      Guard.ArgumentNotNull((object) parameterValues, nameof (parameterValues));
      Guard.ArgumentNotNull((object) result, nameof (result));
      foreach (InjectionParameterValue parameterValue in parameterValues)
      {
        IDependencyResolverPolicy resolverPolicy = parameterValue.GetResolverPolicy(typeToBuild);
        result.AddParameterResolver(resolverPolicy);
      }
    }
  }
}
