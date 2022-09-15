// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.MapPointExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models
{
  public static class MapPointExtensions
  {
    public static readonly Range<int> TotalLocationPathsAllowedRange = Range.GetInclusiveHighOnly<int>(0);
    public static readonly Range<int> ValidSpeedOverGroundRange = Range.GetInclusiveLowOnly<int>(30);
    public static readonly Range<int> ValidEhpeRange = Range.GetExclusiveHighOnly<int>(3000);
    public static readonly Range<int> SlowScaledPaceRange = Range.GetInclusiveHighOnly<int>(33);
    public static readonly Range<int> MediumScaledPaceRange = Range.GetExclusiveLow<int>(33, 66);
    public static readonly Range<int> FastScaledPaceRange = Range.GetExclusiveLowOnly<int>(66);
    private static readonly double GpsCoverageThreshold = 0.3;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Models\\MapPointExtensions.cs");

    public static bool IsValid(this MapPoint mapPoint, MapType mapType)
    {
      if (mapPoint == null || mapPoint.Location == null)
        return false;
      if (mapType != MapType.Run)
        return true;
      return MapPointExtensions.ValidSpeedOverGroundRange.Contains((int) mapPoint.Location.SpeedOverGround.CentimetersPerSecond) && MapPointExtensions.ValidEhpeRange.Contains(mapPoint.Location.Ehpe);
    }

    public static bool HasEnoughGpsToMap(
      this IList<MapPoint> mapPoints,
      MapType mapType,
      Length totalDistance)
    {
      if (mapPoints == null || mapPoints.Count <= 0 || totalDistance.TotalCentimeters <= 0.0)
        return false;
      Length length1 = Length.FromCentimeters(0.0);
      Length length2 = Length.FromCentimeters(0.0);
      bool flag = false;
      foreach (MapPoint mapPoint in (IEnumerable<MapPoint>) mapPoints)
      {
        int num = mapPoint.IsValid(mapType) ? 1 : 0;
        if ((num & (flag ? 1 : 0)) != 0)
          length1 += mapPoint.TotalDistance - length2;
        length2 = mapPoint.TotalDistance;
        flag = num != 0;
      }
      return length1.TotalCentimeters / totalDistance.TotalCentimeters >= MapPointExtensions.GpsCoverageThreshold;
    }
  }
}
