// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Range
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class Range
  {
    public static Range<T> GetUnbounded<T>() where T : IComparable<T> => new Range<T>()
    {
      LowBoundaryBehavior = BoundaryBehavior.Infinite,
      HighBoundaryBehavior = BoundaryBehavior.Infinite
    };

    public static Range<T> GetInclusive<T>(T low, T high) where T : IComparable<T> => new Range<T>()
    {
      Low = low,
      LowBoundaryBehavior = BoundaryBehavior.Inclusive,
      High = high,
      HighBoundaryBehavior = BoundaryBehavior.Inclusive
    };

    public static Range<T> GetExclusive<T>(T low, T high) where T : IComparable<T> => new Range<T>()
    {
      Low = low,
      LowBoundaryBehavior = BoundaryBehavior.Exclusive,
      High = high,
      HighBoundaryBehavior = BoundaryBehavior.Exclusive
    };

    public static Range<T> GetExclusiveHigh<T>(T low, T high) where T : IComparable<T> => new Range<T>()
    {
      Low = low,
      LowBoundaryBehavior = BoundaryBehavior.Inclusive,
      High = high,
      HighBoundaryBehavior = BoundaryBehavior.Exclusive
    };

    public static Range<T> GetExclusiveLow<T>(T low, T high) where T : IComparable<T> => new Range<T>()
    {
      Low = low,
      LowBoundaryBehavior = BoundaryBehavior.Exclusive,
      High = high,
      HighBoundaryBehavior = BoundaryBehavior.Inclusive
    };

    public static Range<T> GetInclusiveLowOnly<T>(T low) where T : IComparable<T> => new Range<T>()
    {
      Low = low,
      LowBoundaryBehavior = BoundaryBehavior.Inclusive,
      HighBoundaryBehavior = BoundaryBehavior.Infinite
    };

    public static Range<T> GetInclusiveHighOnly<T>(T high) where T : IComparable<T> => new Range<T>()
    {
      LowBoundaryBehavior = BoundaryBehavior.Infinite,
      High = high,
      HighBoundaryBehavior = BoundaryBehavior.Inclusive
    };

    public static Range<T> GetExclusiveLowOnly<T>(T low) where T : IComparable<T> => new Range<T>()
    {
      Low = low,
      LowBoundaryBehavior = BoundaryBehavior.Exclusive,
      HighBoundaryBehavior = BoundaryBehavior.Infinite
    };

    public static Range<T> GetExclusiveHighOnly<T>(T high) where T : IComparable<T> => new Range<T>()
    {
      LowBoundaryBehavior = BoundaryBehavior.Infinite,
      High = high,
      HighBoundaryBehavior = BoundaryBehavior.Exclusive
    };
  }
}
