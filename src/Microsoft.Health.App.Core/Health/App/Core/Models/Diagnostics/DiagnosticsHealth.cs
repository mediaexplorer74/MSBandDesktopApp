// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Diagnostics.DiagnosticsHealth
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models.Diagnostics
{
  [DataContract]
  public class DiagnosticsHealth
  {
    public DiagnosticsHealth() => this.PairedDevices = (IList<DiagnosticsDevice>) new List<DiagnosticsDevice>();

    [DataMember(Name = "cloud")]
    public DiagnosticsHealthCloud Cloud { get; set; }

    [DataMember(Name = "pairedDevices")]
    public IList<DiagnosticsDevice> PairedDevices { get; private set; }
  }
}
