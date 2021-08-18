// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.StartStripManager
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace DesktopSyncApp
{
  public class StartStripManager : INotifyPropertyChanged
  {
    private ObservableCollection<SelectableTile> displayStrip;
    private StartStrip currentStartStrip;
    private bool updatingStartStrip;
    private bool needToSave;
    private BandClass deviceType;

    public event PropertyChangedEventHandler PropertyChanged;

    public Color TileBackground { get; set; }

    public StartStripManager()
    {
      this.currentStartStrip = new StartStrip();
      this.displayStrip = new ObservableCollection<SelectableTile>();
      this.ResetDisplayStrip();
    }

    public StartStrip UpdateCurrentStartStrip()
    {
      this.currentStartStrip.Clear();
      for (int index = 0; index < this.displayStrip.Count; ++index)
      {
        this.displayStrip[index].Strapp.SetSettings(this.displayStrip[index].Strapp.Id, this.displayStrip[index].TileSettings);
        this.currentStartStrip.Add(this.displayStrip[index].Strapp);
      }
      return this.currentStartStrip;
    }

    public void ResetDisplayStrip()
    {
      this.displayStrip.Clear();
      for (int index = 0; index < this.currentStartStrip.Count; ++index)
        this.displayStrip.Add(new SelectableTile(this, this.currentStartStrip[index]));
      this.NeedToSave = false;
    }

    public ObservableCollection<SelectableTile> DisplayStrip => this.displayStrip;

    public StartStrip CurrentStartStrip
    {
      get => this.currentStartStrip;
      set
      {
        this.currentStartStrip = value;
        this.ResetDisplayStrip();
      }
    }

    public SelectableTile GetStrap(Guid strapId)
    {
      for (int index = 0; index < this.displayStrip.Count; ++index)
      {
        if (this.displayStrip[index].Strapp.Id == strapId)
          return this.displayStrip[index];
      }
      return (SelectableTile) null;
    }

    public bool UpdatingStartStrip
    {
      get => this.updatingStartStrip;
      set
      {
        if (this.updatingStartStrip == value)
          return;
        this.updatingStartStrip = value;
        this.OnPropertyChanged(nameof (UpdatingStartStrip), this.PropertyChanged);
      }
    }

    public bool NeedToSave
    {
      get => this.needToSave;
      set
      {
        if (this.needToSave == value)
          return;
        this.needToSave = value;
        this.OnPropertyChanged(nameof (NeedToSave), this.PropertyChanged);
      }
    }

    public BandClass DeviceType
    {
      get => this.deviceType;
      set
      {
        if (this.deviceType == value)
          return;
        this.deviceType = value;
        this.OnPropertyChanged(nameof (DeviceType), this.PropertyChanged);
        this.OnPropertyChanged("DeviceIsEnvoy", this.PropertyChanged);
      }
    }

    public BikeSettingsManager BikeManager { get; set; }

    public RunSettingsManager RunManager { get; set; }

    public StartStrip GetFakeStartStrip()
    {
      StartStrip startStrip = new StartStrip();
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "Calls", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "Run", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "Workout", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "sms", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "Facebook", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "mail", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "twitter", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "sleep", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "uv", (AdminTileSettings) 1));
      startStrip.Add(new AdminBandTile(Guid.NewGuid(), "weather", (AdminTileSettings) 1));
      return startStrip;
    }
  }
}
