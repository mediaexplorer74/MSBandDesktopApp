// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseDetails
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Events.Golf.Courses
{
  [DataContract]
  public sealed class GolfCourseDetails : GolfCourseSummary
  {
    public GolfCourseDetails()
    {
      this.Holes = (IList<GolfCourseHole>) new List<GolfCourseHole>();
      this.Tees = (IList<GolfCourseTee>) new List<GolfCourseTee>();
    }

    [DataMember(Name = "county")]
    public string County { get; set; }

    [DataMember(Name = "courseVersion")]
    public int CourseVersion { get; set; }

    [DataMember(Name = "holes")]
    public IList<GolfCourseHole> Holes { get; private set; }

    [DataMember(Name = "tees")]
    public IList<GolfCourseTee> Tees { get; private set; }

    [DataMember(Name = "timezone")]
    public string Timezone { get; set; }
  }
}
