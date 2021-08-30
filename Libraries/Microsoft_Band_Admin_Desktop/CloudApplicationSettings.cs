// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudApplicationSettings
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class CloudApplicationSettings
  {
    [DataMember(EmitDefaultValue = false)]
    internal string PreferredLocale;
    [DataMember(EmitDefaultValue = false)]
    internal string PreferredRegion;

    [DataMember]
    internal Guid? ApplicationId { get; set; }

    [DataMember]
    internal Guid? PairedDeviceId { get; set; }

    [DataMember]
    internal string Locale { get; set; }

    [DataMember]
    internal bool? AllowPersonalization { get; set; }

    [DataMember]
    internal bool? AllowToRunInBackground { get; set; }

    [DataMember]
    internal TimeSpan? SyncInterval { get; set; }

    [DataMember]
    internal string AvatarFileURL { get; set; }

    [DataMember]
    internal string HomeScreenWallpaperURL { get; set; }

    [DataMember]
    internal ApplicationThemeColor? ThemeColor { get; set; }

    [DataMember]
    internal TemperatureUnitType? TemperatureDisplayType { get; set; }

    [DataMember]
    internal MeasurementUnitType? MeasurementDisplayType { get; set; }

    [DataMember]
    internal string ThirdPartyPartnersPortalEndpoint { get; set; }

    [DataMember]
    internal IDictionary<string, string> AdditionalSettings { get; set; }
  }
}
