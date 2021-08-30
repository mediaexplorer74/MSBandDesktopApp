// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mocks.MockInsightsProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Mocks
{
  public class MockInsightsProvider : IInsightsProvider
  {
    public Task<IList<RaisedInsight>> GetStatInsightsAsync(
      InsightDataUsedPivot dataUsed,
      InsightTimespanPivot timespan)
    {
      return Task.FromResult<IList<RaisedInsight>>((IList<RaisedInsight>) new List<RaisedInsight>()
      {
        new RaisedInsight()
        {
          IM_Msg = "You walked 15243 steps last week and met your 5000 step goal 3 time(s). This is Distance from Shishole Bay to Blake Island by boat. Excellent work!",
          IM_Action_Msg = "Increasing your step goal can encourage you to walk even more.",
          RaisedInsightId = "2519940419999489999",
          EffectiveDT = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(5.0)
        }
      });
    }

    public Task AcknowledgeInsightAsync(string insightId, CancellationToken token) => (Task) Task.FromResult<object>((object) null);

    public Task<IList<RaisedInsight>> GetWellnessPlanInsightsAsync(
      CancellationToken token)
    {
      return Task.FromResult<IList<RaisedInsight>>((IList<RaisedInsight>) new List<RaisedInsight>()
      {
        new RaisedInsight()
        {
          IM_Msg = "You're sleeping better now!",
          IM_Action_Msg = "Keep at it.",
          RaisedInsightId = "2519940419999489999",
          EffectiveDT = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(5.0)
        }
      });
    }

    public Task<IList<RaisedInsight>> GetSleepPlanGoalInsightsAsync(
      CancellationToken token)
    {
      return Task.FromResult<IList<RaisedInsight>>((IList<RaisedInsight>) new List<RaisedInsight>()
      {
        new RaisedInsight()
        {
          IM_Msg = "You're sleeping better now!",
          IM_Action_Msg = "Keep at it.",
          RaisedInsightId = "2519940419999489999",
          EffectiveDT = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(5.0)
        }
      });
    }
  }
}
