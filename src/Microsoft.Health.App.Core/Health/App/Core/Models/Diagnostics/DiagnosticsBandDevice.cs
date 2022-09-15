// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Diagnostics.DiagnosticsBandDevice
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models.Diagnostics
{
  [DataContract]
  public class DiagnosticsBandDevice : DiagnosticsDevice
  {
    [DataMember(Name = "serialNumber")]
    public string SerialNumber { get; set; }

    [DataMember(Name = "uniqueId")]
    public string UniqueId { get; set; }

    [DataMember(Name = "versions")]
    public DiagnosticsBandDeviceVersions Versions { get; set; }
  }
}
