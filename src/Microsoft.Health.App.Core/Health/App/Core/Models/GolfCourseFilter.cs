// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.GolfCourseFilter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Cirrious.CrossCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.Health.App.Core.Models
{
  public class GolfCourseFilter
  {
    public GolfCourseFilterCategory Category { get; set; }

    public IList<GolfCourseFilterItem> Items { get; set; }

    public override string ToString()
    {
      string str = string.Format("{0}: {1}", new object[2]
      {
        (object) this.GetPropertyNameFromExpression<GolfCourseFilterCategory>((Expression<Func<GolfCourseFilterCategory>>) (() => this.Category)),
        (object) this.Category
      });
      if (this.Items != null && this.Items.Count > 0)
        str = this.Items.Aggregate<GolfCourseFilterItem, string>(str + string.Format(", {0}: ", new object[1]
        {
          (object) this.GetPropertyNameFromExpression<IList<GolfCourseFilterItem>>((Expression<Func<IList<GolfCourseFilterItem>>>) (() => this.Items))
        }), (Func<string, GolfCourseFilterItem, string>) ((current, golfCourseFilterItem) => current + string.Format("{0} ", new object[1]
        {
          (object) golfCourseFilterItem
        })));
      return str;
    }
  }
}
