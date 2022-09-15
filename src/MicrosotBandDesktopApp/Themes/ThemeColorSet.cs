// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ThemeColorSet
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.ComponentModel;

namespace DesktopSyncApp
{
  public class ThemeColorSet : SelectableThemeItem
  {
    public new event PropertyChangedEventHandler PropertyChanged;

    public ThemeColorSet(
      DeviceThemeManager manager,
      int index,
      int id,
      string name,
      string colorKey,
      ThemeColorPaletteProxy colors)
      : base(manager, index, id, name)
    {
      this.ColorKey = colorKey;
      this.Colors = colors;
    }

    public string ColorKey { get; private set; }

    public ThemeColorPaletteProxy Colors { get; private set; }

    public override bool IsSelected
    {
      get => this.manager.ColorSets.SelectedIndex == this.Index;
      set
      {
        if (value)
        {
          this.manager.ColorSets.SelectedIndex = this.Index;
        }
        else
        {
          if (!this.IsSelected)
            return;
          this.manager.ColorSets.SelectedIndex = -1;
        }
      }
    }

    public override void FireIsSelectedChanged()
    {
      base.FireIsSelectedChanged();
      this.OnPropertyChanged("IsSelected", this.PropertyChanged);
    }
  }
}
