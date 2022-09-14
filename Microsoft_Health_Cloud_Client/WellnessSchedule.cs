// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WellnessSchedule
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class WellnessSchedule
  {
    public WellnessSchedule() => this.Reminders = (IList<Reminder>) new List<Reminder>();

    [DataMember]
    public BandSettings BandSettings { get; set; }

    [DataMember]
    public DateTimeOffset Start { get; set; }

    [DataMember]
    public bool EnableReminders { get; set; }

    [DataMember]
    public DateTimeOffset Expiration { get; set; }

    [DataMember]
    public IList<Reminder> Reminders { get; private set; }

    [DataMember]
    public bool HasActivePlans { get; set; }
  }
}
