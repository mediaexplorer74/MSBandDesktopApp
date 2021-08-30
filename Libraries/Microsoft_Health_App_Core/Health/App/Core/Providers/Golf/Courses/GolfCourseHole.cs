// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseHole
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Providers.Golf.Courses
{
  public sealed class GolfCourseHole
  {
    private readonly string id;
    private readonly string name;
    private readonly IReadOnlyList<GolfCourseHoleTee> tees;

    public GolfCourseHole(string id, string name, IEnumerable<GolfCourseHoleTee> tees)
    {
      Assert.ParamIsNotNull((object) tees, nameof (tees));
      this.id = id;
      this.name = name;
      this.tees = (IReadOnlyList<GolfCourseHoleTee>) new List<GolfCourseHoleTee>(tees);
    }

    public string Id => this.id;

    public string Name => this.name;

    public IReadOnlyList<GolfCourseHoleTee> Tees => this.tees;
  }
}
