// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.InvokingConstructorOperation
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class InvokingConstructorOperation : BuildOperation
  {
    private readonly string constructorSignature;

    public InvokingConstructorOperation(Type typeBeingConstructed, string constructorSignature)
      : base(typeBeingConstructed)
    {
      this.constructorSignature = constructorSignature;
    }

    public string ConstructorSignature => this.constructorSignature;

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.InvokingConstructorOperation, new object[1]
    {
      (object) this.constructorSignature
    });
  }
}
