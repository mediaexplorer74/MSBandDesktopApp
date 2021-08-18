// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ThemeItemCollection`1
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DesktopSyncApp
{
  public class ThemeItemCollection<T> : IEnumerable, INotifyPropertyChanged
    where T : SelectableThemeItem
  {
    private DeviceThemeManager manager;
    private T[] items;
    private int selectedIndex = -1;
    private Dictionary<int, T> idIndex;

    public event PropertyChangedEventHandler PropertyChanged;

    public event PropertyValueChangedEventHandler SelectedIndexChanged;

    public ThemeItemCollection(DeviceThemeManager manager, T[] items)
    {
      this.manager = manager;
      this.items = items;
      this.idIndex = new Dictionary<int, T>();
      foreach (T obj in items)
        this.idIndex[obj.Id] = obj;
    }

    public int Count => this.items.Length;

    public T this[int index] => this.items[index];

    public int SelectedIndex
    {
      get => this.selectedIndex;
      set
      {
        if (value == this.selectedIndex)
          return;
        int selectedIndex = this.selectedIndex;
        this.selectedIndex = value;
        if (selectedIndex > -1)
          this.items[selectedIndex].FireIsSelectedChanged();
        if (this.selectedIndex > -1)
          this.items[this.selectedIndex].FireIsSelectedChanged();
        if (this.SelectedIndexChanged != null)
          this.SelectedIndexChanged((object) this, new PropertyValueChangedEventArgs(nameof (SelectedIndex), (object) selectedIndex, (object) this.selectedIndex));
        this.OnPropertyChanged(nameof (SelectedIndex), this.PropertyChanged, this.SelectedIndexChanged, (object) selectedIndex, (object) this.selectedIndex);
        this.OnPropertyChanged("SelectedItem", this.PropertyChanged);
      }
    }

    public T SelectedItem
    {
      get => this.selectedIndex > -1 ? this[this.selectedIndex] : default (T);
      set
      {
        if (this.selectedIndex == value.Index)
          return;
        this.SelectedIndex = value.Index;
      }
    }

    public bool TryGetItemById(int id, out T item) => this.idIndex.TryGetValue(id, out item);

    public bool TrySetSelectedItemById(int id)
    {
      T obj;
      if (!this.idIndex.TryGetValue(id, out obj))
        return false;
      this.SelectedItem = obj;
      return true;
    }

    protected void FireSelectedItemChanged() => this.OnPropertyChanged("SelectedItem", this.PropertyChanged);

    public IEnumerator GetEnumerator() => (IEnumerator) new ThemeItemCollection<T>.ThemeItemEnumerator(this);

    private class ThemeItemEnumerator : IEnumerator<T>, IDisposable, IEnumerator
    {
      private ThemeItemCollection<T> collection;
      private int index;

      internal ThemeItemEnumerator(ThemeItemCollection<T> collection)
      {
        this.collection = collection;
        this.index = -1;
      }

      public bool MoveNext()
      {
        if (this.index < this.collection.Count)
          ++this.index;
        return this.index < this.collection.Count;
      }

      public void Reset() => this.index = -1;

      object IEnumerator.Current => (object) this.Current;

      public T Current
      {
        get
        {
          if (this.index == -1 || this.index == this.collection.Count)
            throw new InvalidOperationException();
          return this.collection[this.index];
        }
      }

      public void Dispose()
      {
      }
    }
  }
}
