// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.GolfCourseSummaryExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Providers.Golf.Courses;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class GolfCourseSummaryExtensions
  {
    public static IList<string> GetAddressLines(this GolfCourseSummary summary)
    {
      List<string> stringList = new List<string>();
      if (!string.IsNullOrEmpty(summary.DisplayAddress))
        stringList.Add(summary.DisplayAddress);
      if (!string.IsNullOrEmpty(summary.DisplayAddress2))
        stringList.Add(summary.DisplayAddress2);
      if (!string.IsNullOrEmpty(summary.DisplayAddress3))
        stringList.Add(summary.DisplayAddress3);
      return (IList<string>) stringList;
    }
  }
}
