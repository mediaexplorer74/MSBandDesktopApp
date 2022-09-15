// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.DistanceChartPoint
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Models
{
  public class DistanceChartPoint : ChartPointBase
  {
    private readonly HashSet<DistanceAnnotation> annotations = new HashSet<DistanceAnnotation>();

    public double ScaledDistance { get; set; }

    public double ScaledPace { get; set; }

    public Length Distance { get; set; }

    public double DistanceUnitConversion { get; set; }

    public double ValueUnitConversion { get; set; }

    public TimeSpan ElapsedSeconds { get; set; }

    public ICollection<DistanceAnnotation> Annotations => (ICollection<DistanceAnnotation>) this.annotations;

    public bool HasAnnotation(DistanceAnnotationType annotationType) => this.Annotations.Any<DistanceAnnotation>((Func<DistanceAnnotation, bool>) (s => s.Type == annotationType));
  }
}
