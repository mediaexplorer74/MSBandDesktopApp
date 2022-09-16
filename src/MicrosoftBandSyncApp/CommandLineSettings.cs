// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.CommandLineSettings
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.ComponentModel;

namespace DesktopSyncApp
{
  public class CommandLineSettings : INotifyPropertyChanged
  {
    private bool hidden;
    private string language;

    public event PropertyChangedEventHandler PropertyChanged;

    public bool Hidden
    {
      get => this.hidden;
      set
      {
        if (this.hidden == value)
          return;
        this.hidden = value;
        this.OnPropertyChanged(nameof (Hidden), this.PropertyChanged);
      }
    }

    public string Language
    {
      get => this.language;
      set => this.language = value;
    }
  }
}
