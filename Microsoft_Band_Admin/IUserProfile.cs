// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.IUserProfile
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band.Admin
{
  public interface IUserProfile
  {
    ushort Version { get; }

    DateTimeOffset? CreatedOn { get; set; }

    DateTimeOffset? LastKDKSyncUpdateOn { get; set; }

    Guid UserID { get; set; }

    string FirstName { get; set; }

    string LastName { get; set; }

    string EmailAddress { get; set; }

    string ZipCode { get; set; }

    string SmsAddress { get; set; }

    DateTime Birthdate { get; set; }

    uint Weight { get; set; }

    ushort Height { get; set; }

    Gender Gender { get; set; }

    bool HasCompletedOOBE { get; set; }

    byte RestingHeartRate { get; set; }

    ApplicationSettings ApplicationSettings { get; set; }

    DeviceSettings DeviceSettings { get; set; }

    IDictionary<Guid, DeviceSettings> AllDeviceSettings { get; set; }
  }
}
