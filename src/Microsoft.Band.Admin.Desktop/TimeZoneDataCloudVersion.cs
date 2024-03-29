﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TimeZoneDataCloudVersion
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class TimeZoneDataCloudVersion
  {
    private static readonly string[] DateTimeFormats = new string[9]
    {
      "o",
      "yyyy-MM-ddTHH:mm:sszzz",
      "yyyy-MM-ddTHH:mm:ss.fzzz",
      "yyyy-MM-ddTHH:mm:ss.ffzzz",
      "yyyy-MM-ddTHH:mm:ss.fffzzz",
      "yyyy-MM-ddTHH:mm:ssZ",
      "yyyy-MM-ddTHH:mm:ss.fZ",
      "yyyy-MM-ddTHH:mm:ss.ffZ",
      "yyyy-MM-ddTHH:mm:ss.fffZ"
    };
    private DateTime? lastModifiedDateTime;
    private DateTime? lastModifiedDateTimeDevice;
    private DateTime? lastCloudCheckDateTime;

    [DataMember]
    internal string Url { get; set; }

    [DataMember(Name = "LastModifiedDateTime")]
    private string LastModifiedDateTimeSerialized
    {
      get => this.GetSerializedDateTimeValue(this.lastModifiedDateTime);
      set => this.SetDeserializedDateTimeValue(value, out this.lastModifiedDateTime);
    }

    public DateTime? LastModifiedDateTime
    {
      get => this.lastModifiedDateTime;
      set => this.lastModifiedDateTime = value;
    }

    [DataMember(Name = "LastModifiedDateTimeDevice")]
    private string LastModifiedDateTimeDeviceSerialized
    {
      get => this.GetSerializedDateTimeValue(this.lastModifiedDateTimeDevice);
      set => this.SetDeserializedDateTimeValue(value, out this.lastModifiedDateTimeDevice);
    }

    public DateTime? LastModifiedDateTimeDevice
    {
      get => this.lastModifiedDateTimeDevice;
      set => this.lastModifiedDateTimeDevice = value;
    }

    [DataMember(Name = "LastCloudCheckDateTime")]
    private string LastCloudCheckDateTimeSerialized
    {
      get => this.GetSerializedDateTimeValue(this.lastCloudCheckDateTime);
      set => this.SetDeserializedDateTimeValue(value, out this.lastCloudCheckDateTime);
    }

    public DateTime? LastCloudCheckDateTime
    {
      get => this.lastCloudCheckDateTime;
      set => this.lastCloudCheckDateTime = value;
    }

    [DataMember(Name = "Language")]
    public LocaleLanguage Language { get; set; }

    private string GetSerializedDateTimeValue(DateTime? deserialized) => !deserialized.HasValue ? (string) null : deserialized.Value.ToString(TimeZoneDataCloudVersion.DateTimeFormats[0]);

    private void SetDeserializedDateTimeValue(string serialized, out DateTime? deserialized)
    {
      DateTime result;
      if (DateTime.TryParseExact(serialized, TimeZoneDataCloudVersion.DateTimeFormats, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result))
        deserialized = new DateTime?(result);
      else
        deserialized = new DateTime?();
    }
  }
}
