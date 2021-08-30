// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseTee
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;

namespace Microsoft.Health.App.Core.Providers.Golf.Courses
{
  public sealed class GolfCourseTee
  {
    private readonly string id;
    private readonly string name;
    private readonly int par;
    private readonly Length length;
    private readonly double rating;
    private readonly double slope;
    private readonly bool isDefault;

    public GolfCourseTee(
      string id,
      string name,
      int par,
      Length length,
      double rating,
      double slope,
      bool isDefault)
    {
      this.id = id;
      this.name = name;
      this.par = par;
      this.length = length;
      this.rating = rating;
      this.slope = slope;
      this.isDefault = isDefault;
    }

    public string Name => this.name;

    public bool IsDefault => this.isDefault;

    public int Par => this.par;

    public double Rating => this.rating;

    public double Slope => this.slope;

    public Length Length => this.length;

    public string Id => this.id;
  }
}
