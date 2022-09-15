// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseFilterViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class GolfCourseFilterViewModel
  {
    private readonly IList<GolfCourseFilterItemViewModel> items;
    private readonly GolfCourseFilterCategory category;

    public IList<GolfCourseFilterItemViewModel> Items => (IList<GolfCourseFilterItemViewModel>) this.items.Where<GolfCourseFilterItemViewModel>((Func<GolfCourseFilterItemViewModel, bool>) (p => p.IsSelected)).ToList<GolfCourseFilterItemViewModel>();

    public GolfCourseFilterCategory Category => this.category;

    public GolfCourseFilterViewModel(
      GolfCourseFilterCategory category,
      IList<GolfCourseFilterItemViewModel> items)
    {
      this.category = category;
      this.items = items;
    }

    public IList<GolfCourseFilterItemViewModel> GetRawItems() => this.items;
  }
}
