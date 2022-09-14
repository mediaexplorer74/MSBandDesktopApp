// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.DeviceLinkIds
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal struct DeviceLinkIds
  {
    [DataMember(EmitDefaultValue = false)]
    internal Guid ApplicationId;
    [DataMember(EmitDefaultValue = false)]
    internal Guid? PairedDeviceId;
    [DataMember(EmitDefaultValue = false)]
    internal Guid? DeviceId;
    [DataMember(EmitDefaultValue = false)]
    internal string SerialNumber;
  }
}
