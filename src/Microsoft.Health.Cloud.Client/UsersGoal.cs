// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UsersGoal
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UsersGoal : DeserializableObjectBase
  {
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Owner { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public GoalCategory Category { get; set; }

    [DataMember]
    public GoalType Type { get; set; }

    [DataMember]
    public GoalCheckIn CheckInCadence { get; set; }

    [DataMember]
    public DateTimeOffset StartTime { get; set; }

    [DataMember]
    public DateTimeOffset EndTime { get; set; }

    [DataMember]
    public GoalStatus Status { get; set; }

    [DataMember]
    public DateTimeOffset LastUpdateTime { get; set; }

    [DataMember]
    public IList<GoalValueSummary> ValueSummary { get; set; }

    [DataMember]
    public IList<GoalValueHistory> ValueHistory { get; set; }

    [DataMember]
    public string ParentId { get; set; }

    public long Value => (long) this.ValueSummary.First<GoalValueSummary>().ValueTemplate.Threshold;

    public double GetPercentComplete(long value) => this.Value <= 0L ? 0.0 : (double) value / (double) this.Value * 100.0;

    public long GetRoundedPercentCompletedOn(DateTimeOffset dateTimeOffset, long value)
    {
      if (this.ValueHistory == null || !this.ValueHistory.Any<GoalValueHistory>())
        return 0;
      GoalValueHistory goalValueHistory = this.ValueHistory.First<GoalValueHistory>();
      if (goalValueHistory.HistoryThresholds == null || !goalValueHistory.HistoryThresholds.Any<GoalValueRecord>())
        return 0;
      DateTimeOffset date = dateTimeOffset.AddDays(1.0);
      IOrderedEnumerable<GoalValueRecord> source = this.ValueHistory.First<GoalValueHistory>().HistoryThresholds.Where<GoalValueRecord>((Func<GoalValueRecord, bool>) (goal => (DateTimeOffset) goal.UpdateTime.LocalDateTime <= date)).OrderByDescending<GoalValueRecord, DateTimeOffset>((Func<GoalValueRecord, DateTimeOffset>) (goal => goal.UpdateTime));
      if (source.Any<GoalValueRecord>())
      {
        GoalValueRecord goalValueRecord = source.First<GoalValueRecord>();
        if (goalValueRecord != null && goalValueRecord.Value != null)
        {
          double num = double.Parse(goalValueRecord.Value.ToString());
          return num > 0.0 ? (long) Math.Floor((double) value / num * 100.0) : 0L;
        }
      }
      return 0;
    }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNullOrWhiteSpace("Id", this.Id);
      JsonAssert.PropertyIsNotNullOrWhiteSpace("Name", this.Name);
      JsonAssert.PropertyIsNotNull("StartTime", (object) this.StartTime);
      JsonAssert.PropertyIsNotNull("EndTime", (object) this.EndTime);
      JsonAssert.IsTrue(this.ValueHistory != null || this.ValueSummary != null, "ValueHistory or ValueSummary have to be present");
    }
  }
}
