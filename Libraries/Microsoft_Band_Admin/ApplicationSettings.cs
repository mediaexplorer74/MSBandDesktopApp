// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.ApplicationSettings
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band.Admin
{
  public sealed class ApplicationSettings
  {
    public Guid ApplicationId { get; set; }

    public Guid PairedDeviceId { get; set; }

    public string Locale { get; set; }

    public bool? AllowPersonalization { get; set; }

    public bool? AllowToRunInBackground { get; set; }

    public TimeSpan? SyncInterval { get; set; }

    public string AvatarFileURL { get; set; }

    public string HomeScreenWallpaperURL { get; set; }

    public ApplicationThemeColor? ThemeColor { get; set; }

    public TemperatureUnitType? TemperatureDisplayType { get; set; }

    public MeasurementUnitType? MeasurementDisplayType { get; set; }

    public IDictionary<string, string> AdditionalSettings { get; set; }

    public string ThirdPartyPartnersPortalEndpoint { get; set; }

    public string PreferredLocale { get; set; }

    public string PreferredRegion { get; set; }
  }
}
