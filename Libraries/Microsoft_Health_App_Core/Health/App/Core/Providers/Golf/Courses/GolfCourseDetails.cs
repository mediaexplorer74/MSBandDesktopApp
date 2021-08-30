// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseDetails
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Providers.Golf.Courses
{
  public sealed class GolfCourseDetails : GolfCourseSummary
  {
    private readonly IReadOnlyList<GolfCourseHole> holes;
    private readonly IReadOnlyList<GolfCourseTee> tees;

    public GolfCourseDetails(
      string courseId,
      string name,
      GolfCourseType courseType,
      int numberOfHoles,
      string city,
      string state,
      string region,
      string displayAddress,
      string displayAddress2,
      string displayAddress3,
      string phoneNumber,
      Uri websiteUrl,
      IEnumerable<GolfCourseHole> holes,
      IEnumerable<GolfCourseTee> tees)
      : base(courseId, name, courseType, numberOfHoles, city, state, region, displayAddress, displayAddress2, displayAddress3, phoneNumber, websiteUrl, new Length?())
    {
      Assert.ParamIsNotNull((object) holes, nameof (holes));
      Assert.ParamIsNotNull((object) tees, nameof (tees));
      this.holes = (IReadOnlyList<GolfCourseHole>) new List<GolfCourseHole>(holes);
      this.tees = (IReadOnlyList<GolfCourseTee>) new List<GolfCourseTee>(tees);
    }

    public IReadOnlyList<GolfCourseHole> Holes => this.holes;

    public IReadOnlyList<GolfCourseTee> Tees => this.tees;
  }
}
