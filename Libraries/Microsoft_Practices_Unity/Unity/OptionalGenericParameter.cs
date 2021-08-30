// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.OptionalGenericParameter
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;

namespace Microsoft.Practices.Unity
{
  public class OptionalGenericParameter : GenericParameterBase
  {
    public OptionalGenericParameter(string genericParameterName)
      : base(genericParameterName)
    {
    }

    public OptionalGenericParameter(string genericParameterName, string resolutionKey)
      : base(genericParameterName, resolutionKey)
    {
    }

    protected override IDependencyResolverPolicy DoGetResolverPolicy(
      Type typeToResolve,
      string resolutionKey)
    {
      return (IDependencyResolverPolicy) new OptionalDependencyResolverPolicy(typeToResolve, resolutionKey);
    }
  }
}
