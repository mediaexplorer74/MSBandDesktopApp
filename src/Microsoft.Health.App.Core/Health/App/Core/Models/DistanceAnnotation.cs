// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.DistanceAnnotation
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public class DistanceAnnotation
  {
    public DistanceAnnotation(DistanceAnnotationType type, object displayValue)
    {
      this.Type = type;
      this.DisplayValue = displayValue;
    }

    public DistanceAnnotationType Type { get; }

    public object DisplayValue { get; }

    public override int GetHashCode() => new
    {
      Type = this.Type,
      DisplayValue = this.DisplayValue
    }.GetHashCode();

    public override bool Equals(object obj) => obj is DistanceAnnotation distanceAnnotation && this.Type.Equals((object) distanceAnnotation.Type) && this.DisplayValue == distanceAnnotation.DisplayValue;
  }
}
