// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.UserProfileEdit
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DesktopSyncApp
{
  [DataContract]
  public class UserProfileEdit : INotifyPropertyChanged
  {
    public const ushort MinHeight = 1000;
    public const ushort MaxHeight = 2500;
    public const ushort MinHeightFeet = 3;
    public const ushort MinHeightInches = 4;
    public const ushort DefaultHeight = 1700;
    public const uint MinWeight = 35000;
    public const uint MaxWeight = 250000;
    public const uint MinWeightPounds = 78;
    public const uint MaxWeightPounds = 551;
    public const uint DefaultWeight = 70250;
    private UserProfileSurrogate source;
    private string deviceName;
    private bool deviceNameError;
    private string firstName;
    private bool firstNameError;
    private Gender? gender;
    private ushort? height;
    private uint? weight;
    private DateTime? birthdate;
    private DistanceUnitType? heightDisplayUnits;
    private MassUnitType? weightDisplayUnits;
    private TemperatureUnitType? temperatureDisplayUnits;
    private string zipCode;
    private string deviceNameSaved;
    private string firstNameSaved;
    private Gender genderSaved;
    private ushort heightSaved;
    private uint weightSaved;
    private DateTime? birthdateSaved;
    private DistanceUnitType heightDisplayUnitsSaved;
    private MassUnitType weightDisplayUnitsSaved;
    private TemperatureUnitType temperatureDisplayUnitsSaved;
    private string zipCodeSaved;
    private bool editing;
    private Command changeWeightDisplayUnits;
    private Command changeHeightDisplayUnits;

    public event PropertyChangedEventHandler PropertyChanged;

    public UserProfileEdit(UserProfileSurrogate source, bool edit, OobeConfiguration dynamicOobe)
    {
      this.source = source;
      this.deviceNameSaved = source.DeviceName;
      this.firstNameSaved = source.FirstName;
      this.genderSaved = source.Gender;
      this.heightSaved = source.Height;
      this.weightSaved = source.Weight;
      this.birthdateSaved = source.Birthdate;
      this.heightDisplayUnitsSaved = source.HeightDisplayUnits;
      this.weightDisplayUnitsSaved = source.WeightDisplayUnits;
      this.temperatureDisplayUnitsSaved = source.TemperatureDisplayUnits;
      this.zipCodeSaved = source.ZipCode;
      if (!source.HasCompletedOOBE)
      {
        this.heightDisplayUnitsSaved = dynamicOobe.Defaults.DistanceUnit == null ? (DistanceUnitType) 2 : (DistanceUnitType) 1;
        this.weightDisplayUnitsSaved = dynamicOobe.Defaults.WeightUnit == null ? (MassUnitType) 2 : (MassUnitType) 1;
        this.temperatureDisplayUnitsSaved = dynamicOobe.Defaults.TemperatureUnit == null ? (TemperatureUnitType) 2 : (TemperatureUnitType) 1;
        this.Height = (ushort) 1700;
        this.Weight = 70250U;
        this.Birthdate = new DateTime?(new DateTime(1980, 6, 15));
      }
      this.editing = edit;
    }

    public bool Editing
    {
      get => this.editing;
      set
      {
        if (this.editing == value)
          return;
        this.DeviceNameError = false;
        this.FirstNameError = false;
        this.editing = value;
        this.OnPropertyChanged(nameof (Editing), this.PropertyChanged);
      }
    }

    public string DeviceName
    {
      get => this.deviceName ?? this.deviceNameSaved;
      set
      {
        if (value != null)
        {
          int num = value == "" ? 1 : 0;
        }
        if (this.deviceName != value)
        {
          this.deviceName = value;
          this.OnPropertyChanged(nameof (DeviceName), this.PropertyChanged);
          this.OnPropertyChanged("DeviceNameOK", this.PropertyChanged);
          this.OnPropertyChanged("CanBeSaved", this.PropertyChanged);
        }
        this.DeviceNameError = !this.DeviceNameOK;
      }
    }

    public bool DeviceNameOK => this.DeviceName != null && this.DeviceName != "";

    public bool DeviceNameError
    {
      get => this.deviceNameError;
      set
      {
        if (this.deviceNameError == value)
          return;
        this.deviceNameError = value;
        this.OnPropertyChanged(nameof (DeviceNameError), this.PropertyChanged);
      }
    }

    public string FirstName
    {
      get => this.firstName ?? this.firstNameSaved;
      set
      {
        if (value != null)
        {
          int num = value == "" ? 1 : 0;
        }
        if (this.firstName != value)
        {
          this.firstName = value;
          this.OnPropertyChanged(nameof (FirstName), this.PropertyChanged);
          this.OnPropertyChanged("FirstNameOK", this.PropertyChanged);
          this.OnPropertyChanged("CanBeSaved", this.PropertyChanged);
        }
        this.FirstNameError = !this.FirstNameOK;
      }
    }

    public bool FirstNameOK => this.FirstName != null && this.FirstName != "";

    public bool FirstNameError
    {
      get => this.firstNameError;
      set
      {
        if (this.firstNameError == value)
          return;
        this.firstNameError = value;
        this.OnPropertyChanged(nameof (FirstNameError), this.PropertyChanged);
      }
    }

    public Gender Gender
    {
      get => this.gender ?? this.genderSaved;
      set
      {
        if (this.gender.HasValue)
        {
          Gender? gender1 = this.gender;
          Gender gender2 = value;
          if ((gender1.GetValueOrDefault() == gender2 ? (!gender1.HasValue ? 1 : 0) : 1) == 0)
            return;
        }
        this.gender = new Gender?(value);
        this.OnPropertyChanged(nameof (Gender), this.PropertyChanged);
      }
    }

    public ushort Height
    {
      get => this.height ?? this.heightSaved;
      set
      {
        if (this.height.HasValue)
        {
          ushort? height = this.height;
          int? nullable = height.HasValue ? new int?((int) height.GetValueOrDefault()) : new int?();
          int num = (int) value;
          if ((nullable.GetValueOrDefault() == num ? (!nullable.HasValue ? 1 : 0) : 1) == 0)
            return;
        }
        this.height = new ushort?(Math2.Between<ushort>(value, (ushort) 1000, (ushort) 2500));
        this.OnPropertyChanged(nameof (Height), this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedFeetValue", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedInchesValue", this.PropertyChanged);
        this.OnPropertyChanged("HeightConvertedCentimetersValue", this.PropertyChanged);
      }
    }

    public int HeightConvertedFeetValue
    {
      get => UnitConversion.MMToFeetPortion((int) this.Height);
      set => this.Height = (ushort) UnitConversion.FeetAndInchesToMM(value, UnitConversion.MMToInchesPortion((int) this.Height));
    }

    public int HeightConvertedInchesValue
    {
      get
      {
        int feet;
        int inches;
        UnitConversion.MMToFeetAndInches((int) this.Height, out feet, out inches);
        if (feet <= 3 && inches < 4)
          inches = 4;
        return inches;
      }
      set => this.Height = (ushort) UnitConversion.FeetAndInchesToMM(UnitConversion.MMToFeetPortion((int) this.Height), value);
    }

    public int? HeightConvertedCentimetersValue
    {
      get => new int?(UnitConversion.MMToCM((int) this.Height));
      set
      {
        if (!value.HasValue)
          return;
        this.Height = (ushort) UnitConversion.CMToMM(value.Value);
      }
    }

    public uint Weight
    {
      get => this.weight ?? this.weightSaved;
      set
      {
        if (this.weight.HasValue)
        {
          uint? weight = this.weight;
          uint num = value;
          if (((int) weight.GetValueOrDefault() == (int) num ? (!weight.HasValue ? 1 : 0) : 1) == 0)
            return;
        }
        this.weight = new uint?(Math2.Between<uint>(value, 35000U, 250000U));
        this.OnPropertyChanged(nameof (Weight), this.PropertyChanged);
        this.OnPropertyChanged("WeightConvertedPoundsValue", this.PropertyChanged);
        this.OnPropertyChanged("WeightConvertedKilogramsValue", this.PropertyChanged);
      }
    }

    public int? WeightConvertedPoundsValue
    {
      get => new int?((int) Math2.Between<uint>((uint) UnitConversion.GramsToPounds((int) this.Weight), 78U, 551U));
      set
      {
        if (!value.HasValue)
          return;
        this.Weight = (uint) UnitConversion.PoundsToGrams(value.Value);
      }
    }

    public int? WeightConvertedKilogramsValue
    {
      get => new int?(UnitConversion.GramsToKilograms((int) this.Weight));
      set
      {
        if (!value.HasValue)
          return;
        this.Weight = (uint) UnitConversion.KilogramsToGrams(value.Value);
      }
    }

    public DateTime? Birthdate
    {
      get => this.birthdate ?? this.birthdateSaved;
      set
      {
        DateTime date = DateTime.Now.Date;
        DateTime dateTime1 = new DateTime(date.Year - 109, date.Month, 1);
        DateTime dateTime2 = new DateTime(date.Year - 18, date.Month, 1);
        dateTime2 = dateTime2.AddMonths(1);
        DateTime dateTime3 = dateTime2.AddDays(-1.0);
        DateTime? nullable1 = value;
        DateTime dateTime4 = dateTime1;
        DateTime? nullable2;
        if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() < dateTime4 ? 1 : 0) : 0) != 0)
        {
          value = new DateTime?(new DateTime(date.Year - 109, date.Month, 15));
        }
        else
        {
          nullable2 = value;
          DateTime dateTime5 = dateTime3;
          if ((nullable2.HasValue ? (nullable2.GetValueOrDefault() > dateTime5 ? 1 : 0) : 0) != 0)
            value = new DateTime?(new DateTime(date.Year - 18, date.Month, 15));
        }
        if (this.birthdate.HasValue)
        {
          nullable2 = this.birthdate;
          DateTime? nullable3 = value;
          if ((nullable2.HasValue == nullable3.HasValue ? (nullable2.HasValue ? (nullable2.GetValueOrDefault() != nullable3.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
            return;
        }
        this.birthdate = value;
        this.OnPropertyChanged(nameof (Birthdate), this.PropertyChanged);
        this.OnPropertyChanged("BirthdateMonth", this.PropertyChanged);
        this.OnPropertyChanged("BirthdateYear", this.PropertyChanged);
      }
    }

    public int BirthdateMonth
    {
      get => !this.Birthdate.HasValue ? 1 : this.Birthdate.Value.Month;
      set
      {
        value = Math2.Between<int>(value, 1, 12);
        if (this.Birthdate.HasValue)
          this.Birthdate = new DateTime?(new DateTime(this.Birthdate.Value.Year, value, 15));
        else
          this.Birthdate = new DateTime?(new DateTime(1901, value, 15));
      }
    }

    public int? BirthdateYear
    {
      get => new int?(this.Birthdate.HasValue ? this.Birthdate.Value.Year : 1901);
      set
      {
        if (!value.HasValue)
          return;
        int year1 = Math2.Between<int>(value.Value, 0, int.MaxValue);
        int num1 = year1;
        DateTime now = DateTime.Now;
        int num2 = now.Year - 2000;
        if (num1 <= num2)
          year1 += 2000;
        else if (value.Value < 1000)
          year1 += 1900;
        DateTime? birthdate = this.Birthdate;
        if (birthdate.HasValue)
        {
          int year2 = year1;
          birthdate = this.Birthdate;
          now = birthdate.Value;
          int month = now.Month;
          this.Birthdate = new DateTime?(new DateTime(year2, month, 15));
        }
        else
          this.Birthdate = new DateTime?(new DateTime(year1, 1, 15));
      }
    }

    public DistanceUnitType HeightDisplayUnits
    {
      get => this.heightDisplayUnits ?? this.heightDisplayUnitsSaved;
      set
      {
        if (this.heightDisplayUnits.HasValue)
        {
          DistanceUnitType? heightDisplayUnits = this.heightDisplayUnits;
          DistanceUnitType distanceUnitType = value;
          if ((heightDisplayUnits.GetValueOrDefault() == distanceUnitType ? (!heightDisplayUnits.HasValue ? 1 : 0) : 1) == 0)
            return;
        }
        this.heightDisplayUnits = new DistanceUnitType?(value);
        this.OnPropertyChanged(nameof (HeightDisplayUnits), this.PropertyChanged);
      }
    }

    public Command ChangeHeightDisplayUnits => this.changeHeightDisplayUnits ?? (this.changeHeightDisplayUnits = new Command((ExecuteHandler) ((parameter, evetArgs) => this.HeightDisplayUnits = (DistanceUnitType) parameter)));

    public MassUnitType WeightDisplayUnits
    {
      get => this.weightDisplayUnits ?? this.weightDisplayUnitsSaved;
      set
      {
        if (this.weightDisplayUnits.HasValue)
        {
          MassUnitType? weightDisplayUnits = this.weightDisplayUnits;
          MassUnitType massUnitType = value;
          if ((weightDisplayUnits.GetValueOrDefault() == massUnitType ? (!weightDisplayUnits.HasValue ? 1 : 0) : 1) == 0)
            return;
        }
        this.weightDisplayUnits = new MassUnitType?(value);
        this.OnPropertyChanged(nameof (WeightDisplayUnits), this.PropertyChanged);
        this.OnPropertyChanged("WeightConvertedValue", this.PropertyChanged);
      }
    }

    public Command ChangeWeightDisplayUnits => this.changeWeightDisplayUnits ?? (this.changeWeightDisplayUnits = new Command((ExecuteHandler) ((parameter, evetArgs) => this.WeightDisplayUnits = (MassUnitType) parameter)));

    public TemperatureUnitType TemperatureDisplayUnits
    {
      get => this.temperatureDisplayUnits ?? this.temperatureDisplayUnitsSaved;
      set
      {
        if (this.temperatureDisplayUnits.HasValue)
        {
          TemperatureUnitType? temperatureDisplayUnits = this.temperatureDisplayUnits;
          TemperatureUnitType temperatureUnitType = value;
          if ((temperatureDisplayUnits.GetValueOrDefault() == temperatureUnitType ? (!temperatureDisplayUnits.HasValue ? 1 : 0) : 1) == 0)
            return;
        }
        this.temperatureDisplayUnits = new TemperatureUnitType?(value);
        this.OnPropertyChanged(nameof (TemperatureDisplayUnits), this.PropertyChanged);
      }
    }

    public string ZipCode
    {
      get => this.zipCode ?? this.zipCodeSaved;
      set
      {
        if (!(this.zipCode != value))
          return;
        this.zipCode = value;
        this.OnPropertyChanged(nameof (ZipCode), this.PropertyChanged);
      }
    }

    public bool CanBeSaved
    {
      get
      {
        bool deviceNameOk = this.DeviceNameOK;
        bool firstNameOk = this.FirstNameOK;
        this.DeviceNameError = !deviceNameOk;
        this.FirstNameError = !firstNameOk;
        return deviceNameOk & firstNameOk;
      }
    }

    public bool Save()
    {
      bool flag = false;
      if (this.deviceName != null && this.deviceName != this.deviceNameSaved)
      {
        this.source.DeviceName = this.deviceName;
        flag = true;
      }
      if (this.firstName != null && this.firstName != this.firstNameSaved)
      {
        this.source.FirstName = this.firstName;
        flag = true;
      }
      if (this.gender.HasValue)
      {
        Gender? gender = this.gender;
        Gender genderSaved = this.genderSaved;
        if ((gender.GetValueOrDefault() == genderSaved ? (!gender.HasValue ? 1 : 0) : 1) != 0)
        {
          this.source.Gender = this.gender.Value;
          flag = true;
        }
      }
      if (this.height.HasValue)
      {
        ushort? height = this.height;
        int? nullable = height.HasValue ? new int?((int) height.GetValueOrDefault()) : new int?();
        int heightSaved = (int) this.heightSaved;
        if ((nullable.GetValueOrDefault() == heightSaved ? (!nullable.HasValue ? 1 : 0) : 1) != 0)
        {
          this.source.Height = this.height.Value;
          flag = true;
        }
      }
      if (this.weight.HasValue)
      {
        uint? weight = this.weight;
        uint weightSaved = this.weightSaved;
        if (((int) weight.GetValueOrDefault() == (int) weightSaved ? (!weight.HasValue ? 1 : 0) : 1) != 0)
        {
          this.source.Weight = this.weight.Value;
          flag = true;
        }
      }
      if (this.birthdate.HasValue)
      {
        DateTime? birthdate = this.birthdate;
        DateTime? birthdateSaved = this.birthdateSaved;
        if ((birthdate.HasValue == birthdateSaved.HasValue ? (birthdate.HasValue ? (birthdate.GetValueOrDefault() != birthdateSaved.GetValueOrDefault() ? 1 : 0) : 0) : 1) != 0)
        {
          this.source.Birthdate = new DateTime?(this.birthdate.Value);
          flag = true;
        }
      }
      if (this.heightDisplayUnits.HasValue)
      {
        DistanceUnitType? heightDisplayUnits = this.heightDisplayUnits;
        DistanceUnitType displayUnitsSaved = this.heightDisplayUnitsSaved;
        if ((heightDisplayUnits.GetValueOrDefault() == displayUnitsSaved ? (!heightDisplayUnits.HasValue ? 1 : 0) : 1) != 0)
        {
          this.source.HeightDisplayUnits = this.heightDisplayUnits.Value;
          flag = true;
        }
      }
      if (this.weightDisplayUnits.HasValue)
      {
        MassUnitType? weightDisplayUnits = this.weightDisplayUnits;
        MassUnitType displayUnitsSaved = this.weightDisplayUnitsSaved;
        if ((weightDisplayUnits.GetValueOrDefault() == displayUnitsSaved ? (!weightDisplayUnits.HasValue ? 1 : 0) : 1) != 0)
        {
          this.source.WeightDisplayUnits = this.weightDisplayUnits.Value;
          flag = true;
        }
      }
      if (this.temperatureDisplayUnits.HasValue)
      {
        TemperatureUnitType? temperatureDisplayUnits = this.temperatureDisplayUnits;
        TemperatureUnitType displayUnitsSaved = this.temperatureDisplayUnitsSaved;
        if ((temperatureDisplayUnits.GetValueOrDefault() == displayUnitsSaved ? (!temperatureDisplayUnits.HasValue ? 1 : 0) : 1) != 0)
        {
          this.source.TemperatureDisplayUnits = this.temperatureDisplayUnits.Value;
          flag = true;
        }
      }
      if (this.zipCode != null && this.zipCode != this.zipCodeSaved)
      {
        this.source.ZipCode = this.zipCode;
        flag = true;
      }
      return flag;
    }

    public void Restore()
    {
      this.source.DeviceName = this.deviceNameSaved;
      this.source.FirstName = this.firstNameSaved;
      this.source.Gender = this.genderSaved;
      this.source.Height = this.heightSaved;
      this.source.Weight = this.weightSaved;
      this.source.Birthdate = this.birthdateSaved;
      this.source.HeightDisplayUnits = this.heightDisplayUnitsSaved;
      this.source.WeightDisplayUnits = this.weightDisplayUnitsSaved;
      this.source.TemperatureDisplayUnits = this.temperatureDisplayUnitsSaved;
      this.source.ZipCode = this.zipCodeSaved;
    }
  }
}
