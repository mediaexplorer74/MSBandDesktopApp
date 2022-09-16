// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ThemePatternCollection
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Collections;
using System.Collections.Specialized;

namespace DesktopSyncApp
{
  public class ThemePatternCollection : ThemeItemCollection<ThemePattern>, INotifyCollectionChanged
  {
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public ThemePatternCollection(DeviceThemeManager manager, ThemePattern[] patterns)
      : base(manager, patterns)
    {
      manager.ColorSets.SelectedIndexChanged += new PropertyValueChangedEventHandler(this.ColorSets_SelectedIndexChanged);
    }

    private void ColorSets_SelectedIndexChanged(object sender, PropertyValueChangedEventArgs e)
    {
      if (this.CollectionChanged != null)
        this.CollectionChanged((object) this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      this.FireSelectedItemChanged();
    }

    private class ThemePattern2Enumerator : IEnumerator
    {
      private ThemePatternCollection collection;
      private int index;

      internal ThemePattern2Enumerator(ThemePatternCollection collection)
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

      public ThemePattern Current
      {
        get
        {
          if (this.index == -1 || this.index == this.collection.Count)
            throw new InvalidOperationException();
          return this.collection[this.index];
        }
      }
    }
  }
}
