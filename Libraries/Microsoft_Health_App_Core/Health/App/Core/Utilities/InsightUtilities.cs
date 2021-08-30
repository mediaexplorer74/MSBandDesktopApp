// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.InsightUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class InsightUtilities
  {
    public static void PopulateInsightModel(
      IInsightModel model,
      IEnumerable<RaisedInsight> insights)
    {
      List<RaisedInsight> list = insights.Where<RaisedInsight>((Func<RaisedInsight, bool>) (i =>
      {
        if (i.TonePivot == InsightTonePivot.Worse)
          return false;
        DateTimeOffset? expirationDt = i.ExpirationDT;
        if (!expirationDt.HasValue)
          return true;
        expirationDt = i.ExpirationDT;
        return expirationDt.Value > DateTimeOffset.UtcNow;
      })).ToList<RaisedInsight>();
      if (list.Count == 0)
      {
        model.ShowInsight = false;
      }
      else
      {
        list.Sort((IComparer<RaisedInsight>) new InsightUtilities.InsightSorter());
        RaisedInsight raisedInsight = list[0];
        model.InsightMessage = raisedInsight.IM_Msg;
        model.InsightActionMessage = raisedInsight.IM_Action_Msg;
        model.ShowInsight = !string.IsNullOrWhiteSpace(model.InsightMessage) || !string.IsNullOrWhiteSpace(model.InsightActionMessage);
      }
    }

    private class InsightSorter : IComparer<RaisedInsight>
    {
      public int Compare(RaisedInsight x, RaisedInsight y)
      {
        if (x.TonePivot == InsightTonePivot.Caution && y.TonePivot != InsightTonePivot.Caution)
          return -1;
        if (x.TonePivot != InsightTonePivot.Caution && y.TonePivot == InsightTonePivot.Caution)
          return 1;
        if (x.ComparisonPivot == InsightComparisonPivot.Self && y.ComparisonPivot != InsightComparisonPivot.Self)
          return -1;
        if (x.ComparisonPivot != InsightComparisonPivot.Self && y.ComparisonPivot == InsightComparisonPivot.Self)
          return 1;
        if (x.ComparisonPivot == InsightComparisonPivot.PeopleLikeYou && y.ComparisonPivot != InsightComparisonPivot.PeopleLikeYou)
          return -1;
        if (x.ComparisonPivot != InsightComparisonPivot.PeopleLikeYou && y.ComparisonPivot == InsightComparisonPivot.PeopleLikeYou)
          return 1;
        if (x.EffectiveDT > y.EffectiveDT)
          return -1;
        return x.EffectiveDT < y.EffectiveDT ? 1 : 0;
      }
    }
  }
}
