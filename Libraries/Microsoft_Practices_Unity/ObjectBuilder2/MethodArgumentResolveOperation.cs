// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.MethodArgumentResolveOperation
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class MethodArgumentResolveOperation : BuildOperation
  {
    private readonly string methodSignature;
    private readonly string parameterName;

    public MethodArgumentResolveOperation(
      Type typeBeingConstructed,
      string methodSignature,
      string parameterName)
      : base(typeBeingConstructed)
    {
      this.methodSignature = methodSignature;
      this.parameterName = parameterName;
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.MethodArgumentResolveOperation, new object[3]
    {
      (object) this.parameterName,
      (object) this.TypeBeingConstructed.GetTypeInfo().Name,
      (object) this.methodSignature
    });

    public string MethodSignature => this.methodSignature;

    public string ParameterName => this.parameterName;
  }
}
