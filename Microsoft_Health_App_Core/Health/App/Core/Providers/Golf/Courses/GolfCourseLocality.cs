// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseLocality
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Providers.Golf.Courses
{
  public sealed class GolfCourseLocality
  {
    private readonly string id;
    private readonly string name;
    private readonly int numberOfSubregions;
    private readonly int numberOfCourses;

    public GolfCourseLocality(string id, string name, int numberOfSubregions, int numberOfCourses)
    {
      this.id = id;
      this.name = name;
      this.numberOfSubregions = numberOfSubregions;
      this.numberOfCourses = numberOfCourses;
    }

    public string Id => this.id;

    public string Name => this.name;

    public int NumberOfSubregions => this.numberOfSubregions;

    public int NumberOfCourses => this.numberOfCourses;
  }
}
