// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Models.SensorData
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Models
{
  [DataContract]
  public class SensorData
  {
    public SensorData()
      : this(Enumerable.Empty<StepsSensor>())
    {
    }

    public SensorData(IEnumerable<StepsSensor> steps)
    {
      this.Steps = (IList<StepsSensor>) new List<StepsSensor>(steps);
      this.Calories = (IList<CaloriesSensor>) new List<CaloriesSensor>();
      this.Distances = (IList<DistanceSensor>) new List<DistanceSensor>();
      this.HeartRates = (IList<HeartRateSensor>) new List<HeartRateSensor>();
      this.TimezoneChanges = (IList<TimezoneSensor>) new List<TimezoneSensor>();
    }

    [DataMember]
    public long OperationId { get; set; }

    [DataMember]
    public string DeviceId { get; set; }

    [DataMember]
    public DateTimeOffset LogStartUtcTime { get; set; }

    [DataMember]
    public short UploadTimeZoneOffset { get; set; }

    [DataMember]
    public IList<StepsSensor> Steps { get; private set; }

    [DataMember]
    public IList<CaloriesSensor> Calories { get; private set; }

    [DataMember]
    public IList<DistanceSensor> Distances { get; private set; }

    [DataMember]
    public IList<HeartRateSensor> HeartRates { get; private set; }

    [DataMember]
    public IList<TimezoneSensor> TimezoneChanges { get; private set; }
  }
}
