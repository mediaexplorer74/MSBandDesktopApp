// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.IWellnessPlanClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  public interface IWellnessPlanClient
  {
    Task<WellnessSchedule> GetScheduleAsync(
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      CancellationToken token,
      bool bypassCache = false);

    Task PutScheduleAsync(
      WellnessSchedule schedule,
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      CancellationToken token);

    Task<UserWellnessPlanProgress> GetPlanProgressAsync(
      string planId,
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      CancellationToken cancellationToken);

    Task UpdatePlanGoalAsync(
      string planId,
      string goalId,
      WellnessGoalUpdate update,
      CancellationToken token);

    Task<UserWellnessPlansResponse> GetPlansAsync(
      CancellationToken cancellationToken,
      WellnessPlanType? planType = null,
      IList<string> includes = null,
      WellnessPlanStatus? status = null);
  }
}
