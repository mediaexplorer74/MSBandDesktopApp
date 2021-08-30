// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.FilterGrouping
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Cirrious.MvvmCross.ViewModels;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  public class FilterGrouping : ObservableCollection<SearchFilter>
  {
    public FilterGrouping(IEnumerable<SearchFilter> items)
      : base(items)
    {
      foreach (MvxNotifyPropertyChanged notifyPropertyChanged in items)
        notifyPropertyChanged.PropertyChanged += (PropertyChangedEventHandler) ((sender, args) => this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)));
    }

    public string FilterName { get; set; }

    public bool IsAnyFilterSelected
    {
      get
      {
        foreach (SearchFilter searchFilter in (Collection<SearchFilter>) this)
        {
          if (searchFilter.IsSelected)
            return true;
        }
        return false;
      }
    }

    public IList<WorkoutSearchFilter> Selected
    {
      get
      {
        List<WorkoutSearchFilter> workoutSearchFilterList = new List<WorkoutSearchFilter>();
        foreach (SearchFilter searchFilter in (Collection<SearchFilter>) this)
        {
          if (searchFilter.IsSelected)
            workoutSearchFilterList.Add(searchFilter.WorkoutFilter);
        }
        return (IList<WorkoutSearchFilter>) workoutSearchFilterList;
      }
    }

    public void SetAllSelection(bool isSelected)
    {
      foreach (SearchFilter searchFilter in (Collection<SearchFilter>) this)
        searchFilter.IsSelected = isSelected;
    }
  }
}
