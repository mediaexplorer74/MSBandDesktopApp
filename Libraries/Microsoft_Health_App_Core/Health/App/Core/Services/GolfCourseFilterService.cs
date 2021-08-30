// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.GolfCourseFilterService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.Health.App.Core.Services
{
  public class GolfCourseFilterService : IGolfCourseFilterService
  {
    private static IEnumerable<GolfCourseFilter> CreateFiltersList()
    {
      List<GolfCourseFilter> golfCourseFilterList = new List<GolfCourseFilter>();
      GolfCourseFilter golfCourseFilter1 = new GolfCourseFilter();
      golfCourseFilter1.Category = GolfCourseFilterCategory.Type;
      ObservableCollection<GolfCourseFilterItem> observableCollection1 = new ObservableCollection<GolfCourseFilterItem>();
      observableCollection1.Add(new GolfCourseFilterItem()
      {
        Subcategory = GolfCourseFilterSubcategory.Public
      });
      observableCollection1.Add(new GolfCourseFilterItem()
      {
        Subcategory = GolfCourseFilterSubcategory.Private
      });
      golfCourseFilter1.Items = (IList<GolfCourseFilterItem>) observableCollection1;
      golfCourseFilterList.Add(golfCourseFilter1);
      GolfCourseFilter golfCourseFilter2 = new GolfCourseFilter();
      golfCourseFilter2.Category = GolfCourseFilterCategory.Hole;
      ObservableCollection<GolfCourseFilterItem> observableCollection2 = new ObservableCollection<GolfCourseFilterItem>();
      observableCollection2.Add(new GolfCourseFilterItem()
      {
        Subcategory = GolfCourseFilterSubcategory.Nine
      });
      observableCollection2.Add(new GolfCourseFilterItem()
      {
        Subcategory = GolfCourseFilterSubcategory.Eighteen
      });
      golfCourseFilter2.Items = (IList<GolfCourseFilterItem>) observableCollection2;
      golfCourseFilterList.Add(golfCourseFilter2);
      return (IEnumerable<GolfCourseFilter>) golfCourseFilterList;
    }

    public void Initialize()
    {
      this.EnabledFilters = (IEnumerable<GolfCourseFilter>) new List<GolfCourseFilter>(GolfCourseFilterService.CreateFiltersList());
      this.ResetFilters();
    }

    public void ResetFilters()
    {
      this.ClearFilters();
      foreach (GolfCourseFilter pendingFilter in this.PendingFilters)
      {
        GolfCourseFilter golfCourseFilter = pendingFilter;
        foreach (GolfCourseFilterItem courseFilterItem in (IEnumerable<GolfCourseFilterItem>) golfCourseFilter.Items)
        {
          GolfCourseFilterItem golfCourseFilterItem = courseFilterItem;
          golfCourseFilterItem.IsSelected = this.EnabledFilters.First<GolfCourseFilter>((Func<GolfCourseFilter, bool>) (p => p.Category == golfCourseFilter.Category)).Items.First<GolfCourseFilterItem>((Func<GolfCourseFilterItem, bool>) (s => s.Subcategory == golfCourseFilterItem.Subcategory)).IsSelected;
        }
      }
    }

    public void ClearFilters() => this.PendingFilters = (IEnumerable<GolfCourseFilter>) new List<GolfCourseFilter>(GolfCourseFilterService.CreateFiltersList());

    public IEnumerable<GolfCourseFilter> EnabledFilters { get; set; }

    public IEnumerable<GolfCourseFilter> PendingFilters { get; set; }
  }
}
