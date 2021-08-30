// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudLocaleSettings
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal struct CloudLocaleSettings
  {
    [DataMember]
    internal string LocaleName;
    [DataMember(Name = "Locale")]
    internal ushort LocaleId;
    [DataMember]
    internal ushort Language;
    [DataMember]
    internal char DateSeparator;
    [DataMember]
    internal char NumberSeparator;
    [DataMember]
    internal char DecimalSeparator;
    [DataMember]
    internal byte TimeFormat;
    [DataMember]
    internal byte DateFormat;
    [DataMember(Name = "DisplaySizeUnit")]
    internal byte DistanceShortUnits;
    [DataMember(Name = "DisplayDistanceUnit")]
    internal byte DistanceLongUnits;
    [DataMember(Name = "DisplayWeightUnit")]
    internal byte MassUnits;
    [DataMember(Name = "DisplayVolumeUnit")]
    internal byte VolumeUnits;
    [DataMember(Name = "DisplayCaloriesUnit")]
    internal byte EnergyUnits;
    [DataMember(Name = "DisplayTemperatureUnit")]
    internal byte TemperatureUnits;
  }
}
