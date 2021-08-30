// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSearchFilterOptions
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

namespace Microsoft.Health.Cloud.Client.Events.Golf.Courses
{
  public class GolfCourseSearchFilterOptions
  {
    public GolfCourseSearchFilterOptions(GolfCourseType? type, int? numberOfHoles)
    {
      this.Type = type;
      this.NumberOfHoles = numberOfHoles;
    }

    public GolfCourseType? Type { get; private set; }

    public int? NumberOfHoles { get; private set; }
  }
}
