// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BikeRunBaseManager
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Resources;
using System.ComponentModel;

namespace DesktopSyncApp
{
  public class BikeRunBaseManager : INotifyPropertyChanged
  {
    private BandClass deviceType;
    protected DistanceUnitType distanceUnitType;
    protected bool[] errors;

    public event PropertyChangedEventHandler PropertyChanged;

    public BikeRunBaseManager(BandClass deviceType, int split, DistanceUnitType distanceUnitType)
    {
      this.errors = new bool[7];
      this.DeviceType = deviceType;
      this.distanceUnitType = distanceUnitType;
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

    public bool DeviceIsEnvoy => this.deviceType == 2;

    public string Metric1Title => string.Format(Strings.Settings_BikeRun_DataTitle, (object) 1);

    public string Metric2Title => string.Format(Strings.Settings_BikeRun_DataTitle, (object) 2);

    public string Metric3Title => string.Format(Strings.Settings_BikeRun_DataTitle, (object) 3);

    public string Metric4Title => string.Format(Strings.Settings_BikeRun_DataTitle, (object) 1);

    public string Metric5Title => string.Format(Strings.Settings_BikeRun_DataTitle, (object) 2);

    public string Metric6Title => string.Format(Strings.Settings_BikeRun_DataTitle, (object) 3);

    public string Metric7Title => string.Format(Strings.Settings_BikeRun_DataTitle, (object) 4);

    public string SplitMarkerMainTitle => AppResources.SplitMarkerHeader;

    public string SplitMarkerTitle => AppResources.SplitMarkerSubheader;

    public bool Metric1HasError => this.errors[0];

    public bool Metric2HasError => this.errors[1];

    public bool Metric3HasError => this.errors[2];

    public bool Metric4HasError => this.errors[3];

    public bool Metric5HasError => this.errors[4];

    public bool Metric6HasError => this.errors[5];

    public bool Metric7HasError => this.errors[6];

    protected string GetDistanceUnit(DistanceUnitType unit) => unit == 2 ? Strings.Settings_SplitMarkers_Metric : Strings.Settings_SplitMarkers_Imperial;

    protected void RaiseMetricErrorsPropertiesChanged()
    {
      this.OnPropertyChanged("Metric1HasError", this.PropertyChanged);
      this.OnPropertyChanged("Metric2HasError", this.PropertyChanged);
      this.OnPropertyChanged("Metric3HasError", this.PropertyChanged);
      this.OnPropertyChanged("Metric4HasError", this.PropertyChanged);
      this.OnPropertyChanged("Metric5HasError", this.PropertyChanged);
      this.OnPropertyChanged("Metric6HasError", this.PropertyChanged);
      this.OnPropertyChanged("Metric7HasError", this.PropertyChanged);
    }
  }
}
