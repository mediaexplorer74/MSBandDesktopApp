// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.RangeExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Utilities;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class RangeExtensions
  {
    public static List<T> GetRange<T>(this List<T> list, Range<int> indexRange)
    {
      if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Infinite && indexRange.HighBoundaryBehavior == BoundaryBehavior.Infinite)
        return list;
      int index;
      int count;
      if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Exclusive && indexRange.HighBoundaryBehavior == BoundaryBehavior.Infinite)
      {
        index = indexRange.Low + 1;
        count = list.Count - (indexRange.Low + 1);
      }
      else if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Inclusive && indexRange.HighBoundaryBehavior == BoundaryBehavior.Infinite)
      {
        index = indexRange.Low;
        count = list.Count - indexRange.Low;
      }
      else if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Infinite && indexRange.HighBoundaryBehavior == BoundaryBehavior.Exclusive)
      {
        index = 0;
        count = indexRange.High;
      }
      else if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Infinite && indexRange.HighBoundaryBehavior == BoundaryBehavior.Inclusive)
      {
        index = 0;
        count = indexRange.High + 1;
      }
      else if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Exclusive && indexRange.HighBoundaryBehavior == BoundaryBehavior.Inclusive)
      {
        index = indexRange.Low + 1;
        count = indexRange.High + 1 - (indexRange.Low + 1);
      }
      else if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Inclusive && indexRange.HighBoundaryBehavior == BoundaryBehavior.Exclusive)
      {
        index = indexRange.Low;
        count = indexRange.High - indexRange.Low;
      }
      else if (indexRange.LowBoundaryBehavior == BoundaryBehavior.Exclusive && indexRange.HighBoundaryBehavior == BoundaryBehavior.Exclusive)
      {
        index = indexRange.Low + 1;
        count = indexRange.High - 1 - (indexRange.Low + 1);
      }
      else
      {
        index = indexRange.Low;
        count = indexRange.High - indexRange.Low + 1;
      }
      return list.GetRange(index, count);
    }
  }
}
