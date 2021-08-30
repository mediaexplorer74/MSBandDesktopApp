// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.ParameterReflectionHelper
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Reflection;

namespace Microsoft.Practices.Unity.Utility
{
  public class ParameterReflectionHelper : ReflectionHelper
  {
    public ParameterReflectionHelper(ParameterInfo parameter)
      : base(ParameterReflectionHelper.TypeFromParameterInfo(parameter))
    {
    }

    private static Type TypeFromParameterInfo(ParameterInfo parameter)
    {
      Guard.ArgumentNotNull((object) parameter, nameof (parameter));
      return parameter.ParameterType;
    }
  }
}
