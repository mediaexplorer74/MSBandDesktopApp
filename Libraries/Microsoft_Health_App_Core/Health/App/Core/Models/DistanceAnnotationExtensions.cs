// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.DistanceAnnotationExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;

namespace Microsoft.Health.App.Core.Models
{
  public static class DistanceAnnotationExtensions
  {
    public static object ToDistanceAnnotationDisplayValue<T>(
      this DistanceAnnotationType annotationType,
      ref int userGeneratedOrdinal)
      where T : RouteBasedExerciseEvent
    {
      switch (annotationType)
      {
        case DistanceAnnotationType.Start:
          return (object) "\uE413";
        case DistanceAnnotationType.End:
          return (object) "\uE412";
        case DistanceAnnotationType.Split:
          return (object) typeof (T) != (object) typeof (HikeEvent) ? (object) string.Empty : (object) null;
        case DistanceAnnotationType.Pause:
          return (object) typeof (T) != (object) typeof (HikeEvent) ? (object) "\uE125" : (object) "\uE409";
        case DistanceAnnotationType.UserGenerated:
          return (object) ++userGeneratedOrdinal;
        case DistanceAnnotationType.ElevationMax:
          return (object) "\uE415";
        case DistanceAnnotationType.ElevationMin:
          return (object) "\uE414";
        case DistanceAnnotationType.TimeMidPoint:
          return (object) "\uE411";
        case DistanceAnnotationType.Sunrise:
        case DistanceAnnotationType.Sunset:
          return (object) "\uE410";
        default:
          return (object) null;
      }
    }

    public static DistanceAnnotationType ToDistanceAnnotationType(
      this PointOfInterestType pointOfInterestType)
    {
      switch (pointOfInterestType)
      {
        case PointOfInterestType.UserGenerated:
          return DistanceAnnotationType.UserGenerated;
        case PointOfInterestType.ElevationMax:
          return DistanceAnnotationType.ElevationMax;
        case PointOfInterestType.ElevationMin:
          return DistanceAnnotationType.ElevationMin;
        case PointOfInterestType.TimeMidPoint:
          return DistanceAnnotationType.TimeMidPoint;
        case PointOfInterestType.PauseAuto:
          return DistanceAnnotationType.Pause;
        case PointOfInterestType.Sunrise:
          return DistanceAnnotationType.Sunrise;
        case PointOfInterestType.Sunset:
          return DistanceAnnotationType.Sunset;
        default:
          return DistanceAnnotationType.Unknown;
      }
    }

    public static DistanceAnnotationType ToDistanceAnnotationType(
      this MapPointType mapPointType)
    {
      switch (mapPointType)
      {
        case MapPointType.Start:
          return DistanceAnnotationType.Start;
        case MapPointType.End:
          return DistanceAnnotationType.End;
        case MapPointType.Split:
          return DistanceAnnotationType.Split;
        case MapPointType.Pause:
          return DistanceAnnotationType.Pause;
        default:
          return DistanceAnnotationType.Unknown;
      }
    }
  }
}
