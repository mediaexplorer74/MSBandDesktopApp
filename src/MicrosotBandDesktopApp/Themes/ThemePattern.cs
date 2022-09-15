// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ThemePattern
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DesktopSyncApp
{
  public class ThemePattern : SelectableThemeItem
  {
    public new event PropertyChangedEventHandler PropertyChanged;

    public ThemePattern(
      DeviceThemeManager manager,
      int index,
      int id,
      string name,
      string patternKey)
      : base(manager, index, id, name)
    {
      this.PatternKey = patternKey;
    }

    public string PatternKey { get; private set; }

    public BitmapImage Image
    {
      get
      {
        try
        {
          string.Format("{0}-{1}-{2}", (object) this.manager.CurrentBandClass.ToString().ToLower(), (object) this.manager.ColorSets.SelectedItem.ColorKey, (object) this.PatternKey);
          return (BitmapImage) Application.Current.Resources[(object) string.Format("{0}-{1}-{2}", (object) this.manager.CurrentBandClass.ToString().ToLower(), (object) this.manager.ColorSets.SelectedItem.ColorKey, (object) this.PatternKey)];
        }
        catch
        {
          return (BitmapImage) null;
        }
      }
    }

    public override bool IsSelected
    {
      get => this.manager.Patterns.SelectedIndex == this.Index;
      set
      {
        if (value)
        {
          this.manager.Patterns.SelectedIndex = this.Index;
        }
        else
        {
          if (!this.IsSelected)
            return;
          this.manager.Patterns.SelectedIndex = -1;
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
