// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Range`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public class Range<T> where T : IComparable<T>
  {
    public T Low { get; set; }

    public T High { get; set; }

    public BoundaryBehavior LowBoundaryBehavior { get; set; }

    public BoundaryBehavior HighBoundaryBehavior { get; set; }

    public override string ToString()
    {
      if (this.LowBoundaryBehavior == BoundaryBehavior.Infinite && this.HighBoundaryBehavior == BoundaryBehavior.Infinite)
        return string.Format("any");
      return this.LowBoundaryBehavior == BoundaryBehavior.Exclusive && this.HighBoundaryBehavior == BoundaryBehavior.Infinite ? string.Format("over {0}", new object[1]
      {
        (object) this.Low
      }) : (this.LowBoundaryBehavior == BoundaryBehavior.Inclusive && this.HighBoundaryBehavior == BoundaryBehavior.Infinite ? string.Format("{0} and over", new object[1]
      {
        (object) this.Low
      }) : (this.LowBoundaryBehavior == BoundaryBehavior.Infinite && this.HighBoundaryBehavior == BoundaryBehavior.Exclusive ? string.Format("under {0}", new object[1]
      {
        (object) this.High
      }) : (this.LowBoundaryBehavior == BoundaryBehavior.Infinite && this.HighBoundaryBehavior == BoundaryBehavior.Inclusive ? string.Format("{0} and under", new object[1]
      {
        (object) this.High
      }) : (this.LowBoundaryBehavior == BoundaryBehavior.Exclusive && this.HighBoundaryBehavior == BoundaryBehavior.Inclusive ? string.Format("over {0} through {1}", new object[2]
      {
        (object) this.Low,
        (object) this.High
      }) : (this.LowBoundaryBehavior == BoundaryBehavior.Inclusive && this.HighBoundaryBehavior == BoundaryBehavior.Exclusive ? string.Format("{0} through under {1}", new object[2]
      {
        (object) this.Low,
        (object) this.High
      }) : (this.LowBoundaryBehavior == BoundaryBehavior.Exclusive && this.HighBoundaryBehavior == BoundaryBehavior.Exclusive ? string.Format("between {0} and {1}", new object[2]
      {
        (object) this.Low,
        (object) this.High
      }) : string.Format("{0} through {1}", new object[2]
      {
        (object) this.Low,
        (object) this.High
      })))))));
    }

    public bool Contains(T value)
    {
      if (this.LowBoundaryBehavior == BoundaryBehavior.Infinite && this.HighBoundaryBehavior == BoundaryBehavior.Infinite)
        return true;
      if (this.LowBoundaryBehavior == BoundaryBehavior.Exclusive && this.HighBoundaryBehavior == BoundaryBehavior.Infinite)
        return value.CompareTo(this.Low) > 0;
      if (this.LowBoundaryBehavior == BoundaryBehavior.Inclusive && this.HighBoundaryBehavior == BoundaryBehavior.Infinite)
        return value.CompareTo(this.Low) >= 0;
      if (this.LowBoundaryBehavior == BoundaryBehavior.Infinite && this.HighBoundaryBehavior == BoundaryBehavior.Exclusive)
        return value.CompareTo(this.High) < 0;
      if (this.LowBoundaryBehavior == BoundaryBehavior.Infinite && this.HighBoundaryBehavior == BoundaryBehavior.Inclusive)
        return value.CompareTo(this.High) <= 0;
      return this.LowBoundaryBehavior == BoundaryBehavior.Exclusive && this.HighBoundaryBehavior == BoundaryBehavior.Exclusive ? value.CompareTo(this.Low) > 0 && value.CompareTo(this.High) < 0 : (this.LowBoundaryBehavior == BoundaryBehavior.Exclusive && this.HighBoundaryBehavior == BoundaryBehavior.Inclusive ? value.CompareTo(this.Low) > 0 && value.CompareTo(this.High) <= 0 : (this.LowBoundaryBehavior == BoundaryBehavior.Inclusive && this.HighBoundaryBehavior == BoundaryBehavior.Exclusive ? value.CompareTo(this.Low) >= 0 && value.CompareTo(this.High) < 0 : value.CompareTo(this.Low) >= 0 && value.CompareTo(this.High) <= 0));
    }
  }
}
