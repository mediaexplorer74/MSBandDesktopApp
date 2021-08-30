// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseLocalityViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Providers.Golf.Courses;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class GolfCourseLocalityViewModel
  {
    private readonly GolfCourseLocality rawLocality;

    public GolfCourseLocalityViewModel(
      int regionId,
      string name,
      int numberOfSubregions,
      int numberOfCourses,
      GolfCourseLocality rawLocality)
    {
      this.RegionId = regionId;
      this.Name = name;
      this.NumberOfSubregions = numberOfSubregions;
      this.NumberOfCourses = numberOfCourses;
      this.rawLocality = rawLocality;
    }

    public int RegionId { get; private set; }

    public string Name { get; private set; }

    public int NumberOfCourses { get; private set; }

    public int NumberOfSubregions { get; private set; }

    public GolfCourseLocality GetRawLocality() => this.rawLocality;
  }
}
