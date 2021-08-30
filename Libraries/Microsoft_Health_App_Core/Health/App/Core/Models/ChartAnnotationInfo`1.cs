// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.ChartAnnotationInfo`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public class ChartAnnotationInfo<T>
  {
    public ChartAnnotationInfo(
      double before,
      double after,
      double total,
      object display,
      T horizontalValue,
      double verticalValue)
    {
      this.Before = before;
      this.After = after;
      this.Total = total;
      this.Display = display;
      this.HorizontalValue = horizontalValue;
      this.VerticalValue = verticalValue;
    }

    public ChartAnnotationInfo(
      double before,
      double after,
      double total,
      object display,
      T horizontalValue,
      double verticalValue,
      ChartSeriesType type)
      : this(before, after, total, display, horizontalValue, verticalValue)
    {
      this.Type = type;
    }

    public double Before { get; private set; }

    public double After { get; private set; }

    public double Total { get; private set; }

    public object Display { get; private set; }

    public T HorizontalValue { get; private set; }

    public double VerticalValue { get; private set; }

    public ChartSeriesType Type { get; private set; }
  }
}
