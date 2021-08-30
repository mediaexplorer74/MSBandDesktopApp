// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Diagnostics.DiagnosticsContext
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models.Diagnostics
{
  [DataContract]
  public class DiagnosticsContext
  {
    [DataMember(Name = "application")]
    public DiagnosticsApplication Application { get; set; }

    [DataMember(Name = "operatingSystem")]
    public DiagnosticsOperatingSystem OperatingSystem { get; set; }

    [DataMember(Name = "device")]
    public DiagnosticsDeviceSection Device { get; set; }

    [DataMember(Name = "currentContext")]
    public DiagnosticsCurrentContext CurrentContext { get; set; }

    [DataMember(Name = "microsoftHealth")]
    public DiagnosticsHealth MicrosoftHealth { get; set; }
  }
}
