// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.TelemetryObject
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Diagnostics
{
  [DataContract]
  public class TelemetryObject
  {
    [DataMember(Name = "type")]
    public string Type { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "properties")]
    public IDictionary<string, string> Properties { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "metrics")]
    public IDictionary<string, double> Metrics { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "sequence")]
    public string Sequence { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "timestamp")]
    public DateTimeOffset Timestamp { get; set; }
  }
}
