// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.MapUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class MapUtilities
  {
    public static string ToNiceLatLongString(double latitude, double longitude, bool isLatitude)
    {
      latitude = MapUtilities.FromCloudDoubleToMapDouble(latitude);
      longitude = MapUtilities.FromCloudDoubleToMapDouble(longitude);
      int num1 = (int) Math.Round(Math.Abs(latitude * 3600.0));
      int num2 = num1 / 3600;
      int num3 = num1 % 3600;
      int num4 = num3 / 60;
      int num5 = num3 % 60;
      int num6 = (int) Math.Round(Math.Abs(longitude * 3600.0));
      int num7 = num6 / 3600;
      int num8 = num6 % 3600;
      int num9 = num8 / 60;
      int num10 = num8 % 60;
      string str1 = latitude >= 0.0 ? AppResources.GeoLocationNorthAbbreviation : AppResources.GeoLocationSouthAbbreviation;
      string str2 = longitude >= 0.0 ? AppResources.GeoLocationEastAbbreviation : AppResources.GeoLocationWestAbbreviation;
      return isLatitude ? string.Format(AppResources.HikeLatitudeString, (object) str1, (object) num2, (object) num4, (object) num5) : string.Format(AppResources.HikeLongitudeString, (object) str2, (object) num7, (object) num9, (object) num10);
    }

    public static double FromCloudDoubleToMapDouble(double coordinate) => coordinate / 10000000.0;
  }
}
