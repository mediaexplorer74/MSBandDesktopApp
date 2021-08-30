// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.DesignGolfCourseFilterSubcategoryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class DesignGolfCourseFilterSubcategoryViewModel
  {
    public IList<GolfCourseFilterItem> Subfilters
    {
      get
      {
        ObservableCollection<GolfCourseFilterItem> observableCollection = new ObservableCollection<GolfCourseFilterItem>();
        observableCollection.Add(new GolfCourseFilterItem()
        {
          IsSelected = true,
          Subcategory = GolfCourseFilterSubcategory.Public
        });
        observableCollection.Add(new GolfCourseFilterItem()
        {
          IsSelected = false,
          Subcategory = GolfCourseFilterSubcategory.Private
        });
        return (IList<GolfCourseFilterItem>) observableCollection;
      }
    }

    public GolfCourseFilterCategory Category => GolfCourseFilterCategory.Type;

    public ICommand ConfirmCommand => (ICommand) null;

    public ICommand CancelCommand => (ICommand) null;
  }
}
