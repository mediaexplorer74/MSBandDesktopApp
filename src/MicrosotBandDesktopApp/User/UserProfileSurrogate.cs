// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.UserProfileSurrogate
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace DesktopSyncApp
{
  [DataContract]
  public class UserProfileSurrogate : INotifyPropertyChanged
  {
    private IUserProfile source;
    private string timeFormat;
    private bool updatingDevicePairing;
    private ErrorInfo lastDevicePairingError;
    private bool hasCompletedOOBE;

    public event PropertyChangedEventHandler PropertyChanged;

    public UserProfileSurrogate(IUserProfile source, ViewModel1 model)
    {
      this.source = source;
      if (!source.HasCompletedOOBE)
        this.SetOOBEDefaults();
      this.Updated = Stopwatch.StartNew();
      this.hasCompletedOOBE = source.HasCompletedOOBE;
      model.HeartBeat01Sec.Beat += new HeartBeatHandler(this.heartBeat_Beat);
    }

    public IUserProfile Source
    {
      get => this.source;
      set
      {
        this.source = value != null ? value : throw new ArgumentNullException(nameof (Source));
        if (!this.source.HasCompletedOOBE)
          this.SetOOBEDefaults();
        this.Updated = Stopwatch.StartNew();
        this.OnPropertyChanged("FirstName", this.PropertyChanged);
        this.OnPropertyChanged("LastName", this.PropertyChanged);
        this.OnPropertyChanged("Gender", this.PropertyChanged);
        this.OnPropertyChanged("Height", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedHighValue", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedLowValue", this.PropertyChanged);
        this.OnPropertyChanged("Weight", this.PropertyChanged);
        this.OnPropertyChanged("WeightConvertedValue", this.PropertyChanged);
        this.OnPropertyChanged("Birthdate", this.PropertyChanged);
        this.OnPropertyChanged("CalculatedCurrentAge", this.PropertyChanged);
        this.OnPropertyChanged("HeightDisplayUnits", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedHighValue", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedLowValue", this.PropertyChanged);
        this.OnPropertyChanged("WeightDisplayUnits", this.PropertyChanged);
        this.OnPropertyChanged("WeightConvertedValue", this.PropertyChanged);
        this.OnPropertyChanged("TemperatureDisplayUnits", this.PropertyChanged);
        this.OnPropertyChanged("ZipCode", this.PropertyChanged);
        this.OnPropertyChanged("DeviceName", this.PropertyChanged);
        this.OnPropertyChanged("PairedDeviceID", this.PropertyChanged);
      }
    }

    public Stopwatch Updated { get; private set; }

    public Guid UserID => this.source.UserID;

    public string FirstName
    {
      get => this.source.FirstName;
      set
      {
        if (!(this.source.FirstName != value))
          return;
        this.source.FirstName = value;
        this.OnPropertyChanged(nameof (FirstName), this.PropertyChanged);
      }
    }

    public string LastName
    {
      get => this.source.LastName;
      set
      {
        if (!(this.source.LastName != value))
          return;
        this.source.LastName = value;
        this.OnPropertyChanged(nameof (LastName), this.PropertyChanged);
      }
    }

    public Gender Gender
    {
      get => this.source.Gender;
      set
      {
        if (this.source.Gender == value)
          return;
        this.source.Gender = value;
        this.OnPropertyChanged(nameof (Gender), this.PropertyChanged);
      }
    }

    public ushort Height
    {
      get => this.source.Height;
      set
      {
        if ((int) this.source.Height == (int) value)
          return;
        this.source.Height = value;
        this.OnPropertyChanged(nameof (Height), this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedHighValue", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedLowValue", this.PropertyChanged);
      }
    }

    public int HeightConvertedHighValue
    {
      get
      {
        DistanceUnitType heightDisplayUnits = this.HeightDisplayUnits;
        return (int)heightDisplayUnits == 1 || (int)heightDisplayUnits != 2 
                    ? UnitConversion.MMToFeetPortion((int) this.Height) 
                    : UnitConversion.MMToCM((int) this.Height);
      }
    }

    public int HeightConvertedLowValue
    {
      get
      {
        DistanceUnitType heightDisplayUnits = this.HeightDisplayUnits;
        if ((int)heightDisplayUnits != 1 && (int)heightDisplayUnits == 2)
          return 0;
        int feet;
        int inches;
        UnitConversion.MMToFeetAndInches((int) this.Height, out feet, out inches);
        if (feet <= 3 && inches < 4)
          inches = 4;
        return inches;
      }
    }

    public uint Weight
    {
      get => this.source.Weight;
      set
      {
        if ((int) this.source.Weight == (int) value)
          return;
        this.source.Weight = value;
        this.OnPropertyChanged(nameof (Weight), this.PropertyChanged);
        this.OnPropertyChanged("WeightConvertedValue", this.PropertyChanged);
      }
    }

    public uint WeightConvertedValue
    {
      get
      {
        MassUnitType weightDisplayUnits = this.WeightDisplayUnits;
        return (int)weightDisplayUnits == 
        1 || (int)weightDisplayUnits != 2 
        ? Math2.Between<uint>((uint) UnitConversion.GramsToPounds((int) this.Weight), 78U, 551U) 
        : (uint) UnitConversion.GramsToKilograms((int) this.Weight);
      }
    }

    public DateTime? Birthdate
    {
      get => this.source.Birthdate > DateTime.MinValue ? new DateTime?(this.source.Birthdate) : new DateTime?();
      set
      {
        if (value.HasValue)
        {
          if (!(this.source.Birthdate != value.Value))
            return;
          this.source.Birthdate = value.Value;
          this.OnPropertyChanged(nameof (Birthdate), this.PropertyChanged);
          this.OnPropertyChanged("CalculatedCurrentAge", this.PropertyChanged);
        }
        else
        {
          if (!(this.source.Birthdate != DateTime.MinValue))
            return;
          this.source.Birthdate = DateTime.MinValue;
          this.OnPropertyChanged(nameof (Birthdate), this.PropertyChanged);
          this.OnPropertyChanged("CalculatedCurrentAge", this.PropertyChanged);
        }
      }
    }

    public DistanceUnitType HeightDisplayUnits
    {
      get => this.source.DeviceSettings.LocaleSettings.DistanceLongUnits;
      set
      {
        if (this.source.DeviceSettings.LocaleSettings.DistanceShortUnits == value && this.source.DeviceSettings.LocaleSettings.DistanceLongUnits == value)
          return;
        this.source.DeviceSettings.LocaleSettings.DistanceShortUnits = value;
        this.source.DeviceSettings.LocaleSettings.DistanceLongUnits = value;
        this.OnPropertyChanged(nameof (HeightDisplayUnits), this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedHighValue", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedLowValue", this.PropertyChanged);
      }
    }

    public MassUnitType WeightDisplayUnits
    {
      get => this.source.DeviceSettings.LocaleSettings.MassUnits;
      set
      {
        if (this.source.DeviceSettings.LocaleSettings.MassUnits == value)
          return;
        this.source.DeviceSettings.LocaleSettings.MassUnits = value;
        this.OnPropertyChanged(nameof (WeightDisplayUnits), this.PropertyChanged);
        this.OnPropertyChanged("WeightConvertedValue", this.PropertyChanged);
      }
    }

    public TemperatureUnitType TemperatureDisplayUnits
    {
      get => this.source.DeviceSettings.LocaleSettings.TemperatureUnits;
      set
      {
        if (this.source.DeviceSettings.LocaleSettings.TemperatureUnits == value)
          return;
        this.source.DeviceSettings.LocaleSettings.TemperatureUnits = value;
        this.OnPropertyChanged(nameof (TemperatureDisplayUnits), this.PropertyChanged);
      }
    }

    public string DeviceName
    {
      get => this.source.DeviceSettings.DeviceName;
      set
      {
        if (!(this.source.DeviceSettings.DeviceName != value))
          return;
        this.source.DeviceSettings.DeviceName = value;
        this.OnPropertyChanged(nameof (DeviceName), this.PropertyChanged);
      }
    }

    public string TimeFormat => this.timeFormat ?? (this.timeFormat = this.CreateTimeFormat());

    public DateTime? LastKDKSyncUpdateOn => !this.source.LastKDKSyncUpdateOn.HasValue ? new DateTime?() : new DateTime?(this.source.LastKDKSyncUpdateOn.Value.DateTime);

    public Guid PairedDeviceID => this.source.ApplicationSettings.PairedDeviceId;

    public bool HasCompletedOOBE
    {
      get => this.hasCompletedOOBE;
      set
      {
        if (this.hasCompletedOOBE == value)
          return;
        this.hasCompletedOOBE = value;
        this.source.HasCompletedOOBE = value;
        this.OnPropertyChanged(nameof (HasCompletedOOBE), this.PropertyChanged);
      }
    }

    public string ZipCode
    {
      get => this.source.ZipCode;
      set
      {
        if (!(this.source.ZipCode != value))
          return;
        this.source.ZipCode = value;
        this.OnPropertyChanged(nameof (ZipCode), this.PropertyChanged);
      }
    }

    public bool UpdatingDevicePairing
    {
      get => this.updatingDevicePairing;
      set
      {
        if (this.updatingDevicePairing == value)
          return;
        this.updatingDevicePairing = value;
        this.OnPropertyChanged(nameof (UpdatingDevicePairing), this.PropertyChanged);
      }
    }

    public ErrorInfo LastDevicePairingError
    {
      get => this.lastDevicePairingError;
      set
      {
        if (this.lastDevicePairingError == value)
          return;
        this.lastDevicePairingError = value;
        this.OnPropertyChanged(nameof (LastDevicePairingError), this.PropertyChanged);
      }
    }

    public void PairedDeviceIDUpdated() => this.OnPropertyChanged("PairedDeviceID", this.PropertyChanged);

    public void heartBeat_Beat(object sender, EventArgs e) => this.OnPropertyChanged("CalculatedCurrentAge", this.PropertyChanged);

    public string CreateTimeFormat()
    {
      switch ((int) this.source.DeviceSettings.LocaleSettings.TimeFormat)
      {
        case 1:
          return "HH:mm";
        case 2:
          return "H:mm";
        case 3:
          return "hh:mm";
        default:
          return "h:mm";
      }
    }

    public void Saved() => this.Updated = Stopwatch.StartNew();

    private void SetOOBEDefaults()
    {
      this.source.DeviceSettings.LocaleSettings.MassUnits = RegionInfo.CurrentRegion.IsMetric ? (MassUnitType) 2 : (MassUnitType) 1;
      this.source.DeviceSettings.LocaleSettings.TemperatureUnits = RegionInfo.CurrentRegion.IsMetric ? (TemperatureUnitType) 2 : (TemperatureUnitType) 1;
      this.source.DeviceSettings.LocaleSettings.DistanceLongUnits = RegionInfo.CurrentRegion.IsMetric ? (DistanceUnitType) 2 : (DistanceUnitType) 1;
      this.source.DeviceSettings.LocaleSettings.DistanceShortUnits = RegionInfo.CurrentRegion.IsMetric ? (DistanceUnitType) 2 : (DistanceUnitType) 1;
    }
  }
}
