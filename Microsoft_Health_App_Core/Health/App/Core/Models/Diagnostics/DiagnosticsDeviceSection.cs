// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Diagnostics.DiagnosticsDeviceSection
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models.Diagnostics
{
  [DataContract]
  public class DiagnosticsDeviceSection
  {
    [DataMember(Name = "type")]
    public string Type { get; set; }

    [DataMember(Name = "manufacturer")]
    public string Manufacturer { get; set; }

    [DataMember(Name = "model")]
    public string Model { get; set; }

    [DataMember(Name = "version")]
    public string Version { get; set; }

    [DataMember(Name = "screenResolution")]
    public DiagnosticsScreenSize ScreenResolution { get; set; }

    [DataMember(Name = "language")]
    public string Language { get; set; }

    [DataMember(Name = "region")]
    public string Region { get; set; }

    [DataMember(Name = "timeZone")]
    public DiagnosticsTimeZone TimeZone { get; set; }
  }
}
