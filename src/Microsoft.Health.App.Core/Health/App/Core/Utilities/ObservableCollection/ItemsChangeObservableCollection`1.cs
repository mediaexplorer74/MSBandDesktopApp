// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.ObservableCollection.ItemsChangeObservableCollection`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Microsoft.Health.App.Core.Utilities.ObservableCollection
{
  public class ItemsChangeObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
    where T : INotifyPropertyChanged
  {
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
        this.RegisterPropertyChanged(e.NewItems);
      else if (e.Action == NotifyCollectionChangedAction.Remove)
        this.UnRegisterPropertyChanged(e.OldItems);
      else if (e.Action == NotifyCollectionChangedAction.Replace)
      {
        this.UnRegisterPropertyChanged(e.OldItems);
        this.RegisterPropertyChanged(e.NewItems);
      }
      base.OnCollectionChanged(e);
    }

    protected override void ClearItems()
    {
      this.UnRegisterPropertyChanged((IList) this);
      base.ClearItems();
    }

    private void RegisterPropertyChanged(IList items)
    {
      foreach (INotifyPropertyChanged notifyPropertyChanged in (IEnumerable) items)
      {
        if (notifyPropertyChanged != null)
          notifyPropertyChanged.PropertyChanged += new PropertyChangedEventHandler(this.ItemPropertyChanged);
      }
    }

    private void UnRegisterPropertyChanged(IList items)
    {
      foreach (INotifyPropertyChanged notifyPropertyChanged in (IEnumerable) items)
      {
        if (notifyPropertyChanged != null)
          notifyPropertyChanged.PropertyChanged -= new PropertyChangedEventHandler(this.ItemPropertyChanged);
      }
    }

    private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e) => base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
  }
}
