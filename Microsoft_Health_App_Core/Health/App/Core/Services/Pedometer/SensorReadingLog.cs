// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Pedometer.SensorReadingLog
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Pedometer
{
  [DataContract]
  internal sealed class SensorReadingLog
  {
    public static readonly Uri CurrentSchema = new Uri("http://microsoft.com/Health/App/SensorCore/Log/2015/02");

    public SensorReadingLog()
      : this(Enumerable.Empty<SensorReading>())
    {
    }

    public SensorReadingLog(IEnumerable<SensorReading> readings)
    {
      this.Schema = SensorReadingLog.CurrentSchema;
      this.Readings = (IList<SensorReading>) new List<SensorReading>(readings);
    }

    [DataMember(Name = "$schema")]
    public Uri Schema { get; set; }

    [DataMember(Name = "readings")]
    public IList<SensorReading> Readings { get; private set; }
  }
}
