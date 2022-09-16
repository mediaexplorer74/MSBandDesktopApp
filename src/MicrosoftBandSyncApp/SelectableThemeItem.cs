// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.SelectableThemeItem
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.ComponentModel;

namespace DesktopSyncApp
{
  public abstract class SelectableThemeItem : INotifyPropertyChanged
  {
    protected DeviceThemeManager manager;

    public event PropertyChangedEventHandler PropertyChanged;

    public SelectableThemeItem(DeviceThemeManager manager, int index, int id, string name)
    {
      this.manager = manager;
      this.Index = index;
      this.Id = id;
      this.Name = name;
    }

    public int Index { get; private set; }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public abstract bool IsSelected { get; set; }

    public virtual void FireIsSelectedChanged() => this.OnPropertyChanged("IsSelected", this.PropertyChanged);
  }
}
