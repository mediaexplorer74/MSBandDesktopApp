// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.LocaleConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models
{
  [DataContract]
  public class LocaleConfiguration : ILocaleConfiguration
  {
    [DataMember(Name = "localeName")]
    private string localeName;
    [DataMember(Name = "localeLanguage")]
    private string localeLanguage;
    [DataMember(Name = "dateSeparator")]
    private char dateSeparator;
    [DataMember(Name = "numberSeparator")]
    private char numberSeparator;
    [DataMember(Name = "decimalSeparator")]
    private char decimalSeparator;
    [DataMember(Name = "displayTime")]
    private string displayTime;
    [DataMember(Name = "displayDate")]
    private string displayDate;
    [DataMember(Name = "displaySizeUnit")]
    private string displaySizeUnit;
    [DataMember(Name = "displayVolumeUnit")]
    private string displayVolumeUnit;
    [DataMember(Name = "displayCaloriesUnit")]
    private string displayCaloriesUnit;

    public string LocaleName => this.localeName;

    public char DateSeparator => this.dateSeparator;

    public char NumberSeparator => this.numberSeparator;

    public char DecimalSeparator => this.decimalSeparator;

    public DisplayTimeFormat DisplayTime
    {
      get
      {
        string displayTime = this.displayTime;
        if (displayTime == "HHmmss")
          return DisplayTimeFormat.HHmmss;
        if (displayTime == "Hmmss")
          return DisplayTimeFormat.Hmmss;
        if (displayTime == "hhmmss")
          return DisplayTimeFormat.hhmmss;
        return displayTime == "hmmss" ? DisplayTimeFormat.hmmss : DisplayTimeFormat.hmmss;
      }
    }

    public DisplayDateFormat DisplayDate
    {
      get
      {
        string displayDate = this.displayDate;
        if (displayDate == "YYYYMMDD")
          return DisplayDateFormat.yyyyMMdd;
        if (displayDate == "DDMMYYYY")
          return DisplayDateFormat.ddMMyyyy;
        if (displayDate == "DMMYYYY")
          return DisplayDateFormat.dMMyyyy;
        if (displayDate == "MMDDYYYY")
          return DisplayDateFormat.MMddyyyy;
        return displayDate == "MDYYYY" ? DisplayDateFormat.Mdyyyy : DisplayDateFormat.Mdyyyy;
      }
    }

    public VolumeUnitType DisplayVolumeUnit
    {
      get
      {
        string displayVolumeUnit = this.displayVolumeUnit;
        if (displayVolumeUnit == "metric")
          return VolumeUnitType.Metric;
        return displayVolumeUnit == "imperial" ? VolumeUnitType.Imperial : VolumeUnitType.Imperial;
      }
    }

    public EnergyUnitType DisplayCaloriesUnit
    {
      get
      {
        string displayCaloriesUnit = this.displayCaloriesUnit;
        if (displayCaloriesUnit == "metric")
          return EnergyUnitType.Metric;
        return displayCaloriesUnit == "imperial" ? EnergyUnitType.Imperial : EnergyUnitType.Imperial;
      }
    }
  }
}
