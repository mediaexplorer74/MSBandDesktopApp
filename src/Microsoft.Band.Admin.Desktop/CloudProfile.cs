﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudProfile
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class CloudProfile
  {
    [DataMember(EmitDefaultValue = false)]
    internal string CreatedOn;
    [DataMember(EmitDefaultValue = true)]
    internal string LastKDKSyncUpdateOn;
    [DataMember(EmitDefaultValue = false)]
    internal string LastModifiedOn;
    [DataMember(EmitDefaultValue = false)]
    internal string LastUserUpdateOn;
    [DataMember(EmitDefaultValue = false)]
    internal string EndPoint;
    [DataMember(EmitDefaultValue = false)]
    internal string FUSEndPoint;
    [DataMember(EmitDefaultValue = false, Name = "ODSUserID")]
    internal Guid? UserID;
    [DataMember(EmitDefaultValue = false)]
    internal string FirstName;
    [DataMember(EmitDefaultValue = false)]
    internal string LastName;
    [DataMember(EmitDefaultValue = false)]
    internal string ZipCode;
    [DataMember(EmitDefaultValue = false)]
    internal string EmailAddress;
    [DataMember(EmitDefaultValue = false)]
    internal string SmsAddress;
    [DataMember(EmitDefaultValue = false, Name = "HeightInMM")]
    internal uint? Height;
    [DataMember(EmitDefaultValue = false, Name = "WeightInGrams")]
    internal uint? Weight;
    [DataMember(EmitDefaultValue = false)]
    internal bool? HasCompletedOOBE;
    [DataMember(EmitDefaultValue = true)]
    internal Gender Gender;
    [DataMember(EmitDefaultValue = false, Name = "DateOfBirth")]
    internal string Birthdate;
    [DataMember(EmitDefaultValue = false)]
    internal ulong? TotalCaloriesBurnedFromMotion;
    [DataMember(EmitDefaultValue = false)]
    internal ulong? TotalCaloriesBurnedWhileNotWorn;
    [DataMember(EmitDefaultValue = false)]
    internal ulong? TotalCaloriesBurnedFromHR;
    [DataMember(EmitDefaultValue = false)]
    internal ulong? TotalDistanceTravelledInM;
    [DataMember(EmitDefaultValue = false)]
    internal ulong? TotalDistanceMeasuredByPedometerInM;
    [DataMember(EmitDefaultValue = false)]
    internal ulong? TotalDistanceMeasuredByGPSInM;
    [DataMember(EmitDefaultValue = false)]
    internal ulong? TotalStepsCounted;
    [DataMember(EmitDefaultValue = false)]
    internal uint? HRGain;
    [DataMember(EmitDefaultValue = false)]
    internal uint? HRRecoveryTime;
    [DataMember(EmitDefaultValue = false)]
    internal uint? HRIntensity;
    [DataMember(EmitDefaultValue = false)]
    internal uint? HRResponseTime;
    [DataMember(EmitDefaultValue = false)]
    internal float? StrideLengthWhileWalking;
    [DataMember(EmitDefaultValue = false)]
    internal float? StrideLengthWhileRunning;
    [DataMember(EmitDefaultValue = false)]
    internal float? StrideLengthWhileJogging;
    [DataMember(EmitDefaultValue = false)]
    internal uint? RestingHR;
    [DataMember(EmitDefaultValue = false)]
    internal bool? RestingHROverride;
    [DataMember(EmitDefaultValue = false)]
    internal uint? MaxHR;
    [DataMember(EmitDefaultValue = false)]
    internal bool? MaxHROverride;
    [DataMember(EmitDefaultValue = false)]
    internal float? ActivityClass;

    [DataMember]
    internal CloudApplicationSettings ApplicationSettings { get; set; }

    [DataMember]
    internal CloudDeviceSettings DeviceSettings { get; set; }

    [DataMember]
    internal IDictionary<Guid, CloudDeviceSettings> AllDeviceSettings { get; set; }
  }
}
