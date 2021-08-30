// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.GenericParameter
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;

namespace Microsoft.Practices.Unity
{
  public class GenericParameter : GenericParameterBase
  {
    public GenericParameter(string genericParameterName)
      : base(genericParameterName)
    {
    }

    public GenericParameter(string genericParameterName, string resolutionKey)
      : base(genericParameterName, resolutionKey)
    {
    }

    protected override IDependencyResolverPolicy DoGetResolverPolicy(
      Type typeToResolve,
      string resolutionKey)
    {
      return (IDependencyResolverPolicy) new NamedTypeDependencyResolverPolicy(typeToResolve, resolutionKey);
    }
  }
}
