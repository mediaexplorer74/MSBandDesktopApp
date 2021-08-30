// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseLocality
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Events.Golf.Courses
{
  [DataContract]
  public sealed class GolfCourseLocality
  {
    [DataMember(Name = "id")]
    public long Id { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "numberOfCourses")]
    public int NumberOfCourses { get; set; }

    [DataMember(Name = "numberOfStates")]
    public int NumberOfSubregions { get; set; }
  }
}
