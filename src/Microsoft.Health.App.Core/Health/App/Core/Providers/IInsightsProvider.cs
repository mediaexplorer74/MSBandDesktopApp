// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.IInsightsProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public interface IInsightsProvider
  {
    Task<IList<RaisedInsight>> GetStatInsightsAsync(
      InsightDataUsedPivot dataUsed,
      InsightTimespanPivot timespan);

    Task AcknowledgeInsightAsync(string insightId, CancellationToken token);

    Task<IList<RaisedInsight>> GetWellnessPlanInsightsAsync(
      CancellationToken token);

    Task<IList<RaisedInsight>> GetSleepPlanGoalInsightsAsync(
      CancellationToken token);
  }
}
