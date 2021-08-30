// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Providers.Golf.Courses;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class GolfCourseSummaryViewModel : GolfCourseItemViewModel
  {
    private readonly GolfCourseSummary rawSummary;

    public GolfCourseSummaryViewModel(
      string name,
      string distance,
      GolfCourseType courseType,
      int numberOfHoles,
      string city,
      GolfCourseSummary rawSummary)
      : base(name)
    {
      this.Distance = distance;
      this.CourseType = courseType;
      this.NumberOfHoles = numberOfHoles;
      this.City = city;
      this.rawSummary = rawSummary;
    }

    public string Distance { get; private set; }

    public GolfCourseType CourseType { get; private set; }

    public int NumberOfHoles { get; private set; }

    public string City { get; private set; }

    public GolfCourseSummary GetRawSummary() => this.rawSummary;
  }
}
