// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Events.Golf.Courses
{
  [DataContract]
  public class GolfCourseSummary : DeserializableObjectBase
  {
    [DataMember(Name = "courseId")]
    public long CourseId { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "street")]
    public string Street { get; set; }

    [DataMember(Name = "city")]
    public string City { get; set; }

    [DataMember(Name = "state")]
    public string State { get; set; }

    [DataMember(Name = "zipcode")]
    public string Zipcode { get; set; }

    [DataMember(Name = "country")]
    public string Country { get; set; }

    [DataMember(Name = "displayAddress")]
    public string DisplayAddress { get; set; }

    [DataMember(Name = "displayAddress2")]
    public string DisplayAddress2 { get; set; }

    [DataMember(Name = "displayAddress3")]
    public string DisplayAddress3 { get; set; }

    [DataMember(Name = "phoneNumber")]
    public string PhoneNumber { get; set; }

    [DataMember(Name = "website")]
    public string Website { get; set; }

    [DataMember(Name = "courseType")]
    public GolfCourseType CourseType { get; set; }

    [DataMember(Name = "longitude")]
    public double Longitude { get; set; }

    [DataMember(Name = "latitude")]
    public double Latitude { get; set; }

    [DataMember(Name = "numberOfHoles")]
    public int NumberOfHoles { get; set; }

    [DataMember(Name = "distance")]
    public double? Distance { get; set; }

    protected override void Validate() => base.Validate();
  }
}
