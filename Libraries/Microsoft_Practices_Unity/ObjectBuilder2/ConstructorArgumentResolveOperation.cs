// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.ConstructorArgumentResolveOperation
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class ConstructorArgumentResolveOperation : BuildOperation
  {
    private readonly string constructorSignature;
    private readonly string parameterName;

    public ConstructorArgumentResolveOperation(
      Type typeBeingConstructed,
      string constructorSignature,
      string parameterName)
      : base(typeBeingConstructed)
    {
      this.constructorSignature = constructorSignature;
      this.parameterName = parameterName;
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.ConstructorArgumentResolveOperation, new object[2]
    {
      (object) this.parameterName,
      (object) this.constructorSignature
    });

    public string ConstructorSignature => this.constructorSignature;

    public string ParameterName => this.parameterName;
  }
}
