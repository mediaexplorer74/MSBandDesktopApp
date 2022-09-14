// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.InsightsProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class InsightsProvider : IInsightsProvider
  {
    private readonly IHealthCloudClient healthCloudClient;

    public InsightsProvider(IHealthCloudClient healthCloudClient) => this.healthCloudClient = healthCloudClient;

    public Task<IList<RaisedInsight>> GetStatInsightsAsync(
      InsightDataUsedPivot dataUsed,
      InsightTimespanPivot timespan)
    {
      return this.healthCloudClient.GetRaisedInsightsAsync(CancellationToken.None, (IEnumerable<InsightDataUsedPivot>) new InsightDataUsedPivot[1]
      {
        dataUsed
      }, (IEnumerable<InsightTimespanPivot>) new InsightTimespanPivot[1]
      {
        timespan
      }, (IEnumerable<InsightScopePivot>) new InsightScopePivot[1]
      {
        InsightScopePivot.Global
      });
    }

    public Task<IList<RaisedInsight>> GetWellnessPlanInsightsAsync(
      CancellationToken token)
    {
      InsightCategoryPivot[] insightCategoryPivotArray = new InsightCategoryPivot[5]
      {
        InsightCategoryPivot.SleepCarePlanProgress,
        InsightCategoryPivot.SleepCareNewGoal,
        InsightCategoryPivot.ActivityPlanNewGoal,
        InsightCategoryPivot.ActivityPlanProgress,
        InsightCategoryPivot.SleepMetrics
      };
      return this.healthCloudClient.GetRaisedInsightsAsync(token, category: ((IEnumerable<InsightCategoryPivot>) insightCategoryPivotArray));
    }

    public Task<IList<RaisedInsight>> GetSleepPlanGoalInsightsAsync(
      CancellationToken token)
    {
      return this.healthCloudClient.GetRaisedInsightsAsync(token, category: ((IEnumerable<InsightCategoryPivot>) new InsightCategoryPivot[1]
      {
        InsightCategoryPivot.SleepCareNewGoal
      }));
    }

    public Task AcknowledgeInsightAsync(string insightId, CancellationToken token) => this.healthCloudClient.AcknowledgeInsightAsync(insightId, token);
  }
}
