// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.InvokingMethodOperation
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class InvokingMethodOperation : BuildOperation
  {
    private readonly string methodSignature;

    public InvokingMethodOperation(Type typeBeingConstructed, string methodSignature)
      : base(typeBeingConstructed)
    {
      this.methodSignature = methodSignature;
    }

    public string MethodSignature => this.methodSignature;

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.InvokingMethodOperation, new object[2]
    {
      (object) this.TypeBeingConstructed.GetTypeInfo().Name,
      (object) this.methodSignature
    });
  }
}
