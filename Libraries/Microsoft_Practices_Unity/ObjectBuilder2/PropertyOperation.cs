// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.PropertyOperation
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public abstract class PropertyOperation : BuildOperation
  {
    private readonly string propertyName;

    protected PropertyOperation(Type typeBeingConstructed, string propertyName)
      : base(typeBeingConstructed)
    {
      this.propertyName = propertyName;
    }

    public string PropertyName => this.propertyName;

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, this.GetDescriptionFormat(), new object[2]
    {
      (object) this.TypeBeingConstructed.GetTypeInfo().Name,
      (object) this.propertyName
    });

    protected abstract string GetDescriptionFormat();
  }
}
