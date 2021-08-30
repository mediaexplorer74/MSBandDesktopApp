// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseSummary
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.Providers.Golf.Courses
{
  public class GolfCourseSummary
  {
    private readonly string courseId;
    private readonly string name;
    private readonly GolfCourseType courseType;
    private readonly int numberOfHoles;
    private readonly string city;
    private readonly string state;
    private readonly string region;
    private readonly string displayAddress;
    private readonly string displayAddress2;
    private readonly string displayAddress3;
    private readonly string phoneNumber;
    private readonly Uri websiteUrl;
    private readonly Length? distance;

    public GolfCourseSummary(
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
      Length? distance)
    {
      this.courseId = courseId;
      this.name = name;
      this.courseType = courseType;
      this.numberOfHoles = numberOfHoles;
      this.city = city;
      this.state = state;
      this.region = region;
      this.displayAddress = displayAddress;
      this.displayAddress2 = displayAddress2;
      this.displayAddress3 = displayAddress3;
      this.phoneNumber = phoneNumber;
      this.websiteUrl = websiteUrl;
      this.distance = distance;
    }

    public string CourseId => this.courseId;

    public string Name => this.name;

    public string City => this.city;

    public string State => this.state;

    public string Region => this.region;

    public string DisplayAddress => this.displayAddress;

    public string DisplayAddress2 => this.displayAddress2;

    public string DisplayAddress3 => this.displayAddress3;

    public string PhoneNumber => this.phoneNumber;

    public Uri WebsiteUrl => this.websiteUrl;

    public GolfCourseType CourseType => this.courseType;

    public int NumberOfHoles => this.numberOfHoles;

    public Length? Distance => this.distance;
  }
}
