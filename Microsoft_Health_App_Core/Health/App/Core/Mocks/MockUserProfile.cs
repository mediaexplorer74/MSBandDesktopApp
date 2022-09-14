// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mocks.MockUserProfile
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Mocks
{
  public class MockUserProfile : IUserProfile
  {
    public MockUserProfile(ushort version) => this.Version = version;

    public ushort Version { get; private set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? LastKDKSyncUpdateOn { get; set; }

    public Guid UserID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public string ZipCode { get; set; }

    public string SmsAddress { get; set; }

    public DateTime Birthdate { get; set; }

    public uint Weight { get; set; }

    public ushort Height { get; set; }

    public Gender Gender { get; set; }

    public bool HasCompletedOOBE { get; set; }

    public byte RestingHeartRate { get; set; }

    public ApplicationSettings ApplicationSettings { get; set; }

    public DeviceSettings DeviceSettings { get; set; }

    public IDictionary<Guid, DeviceSettings> AllDeviceSettings { get; set; }
  }
}
