// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.ChartSeriesInfoExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class ChartSeriesInfoExtensions
  {
    public static IList<IList<T>> GetSeriesDataFragments<T>(
      this ChartSeriesInfoBase<T> seriesInfo,
      double missingValue = 0.0)
      where T : ChartPointBase
    {
      if (!seriesInfo.SkipZeroes)
        return (IList<IList<T>>) new List<IList<T>>()
        {
          seriesInfo.SeriesData
        };
      List<IList<T>> objListList = new List<IList<T>>();
      List<T> objList = new List<T>();
      bool flag1 = true;
      foreach (T obj in (IEnumerable<T>) seriesInfo.SeriesData)
      {
        bool flag2 = obj.Value == missingValue;
        if (!flag1 & flag2)
        {
          objListList.Add((IList<T>) objList);
          objList = new List<T>();
        }
        else if (!flag2)
          objList.Add(obj);
        flag1 = flag2;
      }
      if (objList.Count > 0)
        objListList.Add((IList<T>) objList);
      if (objListList.Count != 0)
        return (IList<IList<T>>) objListList;
      return (IList<IList<T>>) new List<IList<T>>()
      {
        (IList<T>) new List<T>()
      };
    }
  }
}
