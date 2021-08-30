// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.DesignGolfCourseFilterCategoryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class DesignGolfCourseFilterCategoryViewModel
  {
    public IList<GolfCourseFilter> Filters
    {
      get
      {
        ObservableCollection<GolfCourseFilter> observableCollection1 = new ObservableCollection<GolfCourseFilter>();
        GolfCourseFilter golfCourseFilter1 = new GolfCourseFilter();
        golfCourseFilter1.Category = GolfCourseFilterCategory.Type;
        ObservableCollection<GolfCourseFilterItem> observableCollection2 = new ObservableCollection<GolfCourseFilterItem>();
        observableCollection2.Add(new GolfCourseFilterItem()
        {
          IsSelected = true,
          Subcategory = GolfCourseFilterSubcategory.Public
        });
        golfCourseFilter1.Items = (IList<GolfCourseFilterItem>) observableCollection2;
        observableCollection1.Add(golfCourseFilter1);
        GolfCourseFilter golfCourseFilter2 = new GolfCourseFilter();
        golfCourseFilter2.Category = GolfCourseFilterCategory.Hole;
        ObservableCollection<GolfCourseFilterItem> observableCollection3 = new ObservableCollection<GolfCourseFilterItem>();
        observableCollection3.Add(new GolfCourseFilterItem()
        {
          IsSelected = true,
          Subcategory = GolfCourseFilterSubcategory.Nine
        });
        observableCollection3.Add(new GolfCourseFilterItem()
        {
          IsSelected = true,
          Subcategory = GolfCourseFilterSubcategory.Eighteen
        });
        golfCourseFilter2.Items = (IList<GolfCourseFilterItem>) observableCollection3;
        observableCollection1.Add(golfCourseFilter2);
        return (IList<GolfCourseFilter>) observableCollection1;
      }
    }

    public bool ShowClearAll => true;

    public ICommand NavigateToFilterCommand => (ICommand) null;

    public ICommand ClearAllCommand => (ICommand) null;

    public ICommand ConfirmCommand => (ICommand) null;

    public ICommand CancelCommand => (ICommand) null;
  }
}
