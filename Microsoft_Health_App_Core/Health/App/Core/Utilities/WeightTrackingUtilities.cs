// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.WeightTrackingUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.Cloud.Client;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class WeightTrackingUtilities
  {
    public static readonly int MaxLbs = 551;
    public static readonly int MinLbs = 78;
    public static readonly int MaxKg = 250;
    public static readonly int MinKg = 35;

    public static bool IsWeightValid(Weight weight) => weight.TotalKilograms >= (double) WeightTrackingUtilities.MinKg && weight.TotalKilograms <= (double) WeightTrackingUtilities.MaxKg;

    public static string GetValidationErrorMessage(bool isMetric) => isMetric ? string.Format(AppResources.ProfileOutOfRangeErrorMessage, new object[3]
    {
      (object) WeightTrackingUtilities.MinKg,
      (object) WeightTrackingUtilities.MaxKg,
      (object) string.Format(" {0}", new object[1]
      {
        (object) AppResources.KilogramsAbbreviation
      })
    }) : string.Format(AppResources.ProfileOutOfRangeErrorMessage, new object[3]
    {
      (object) WeightTrackingUtilities.MinLbs,
      (object) WeightTrackingUtilities.MaxLbs,
      (object) string.Format(" {0}", new object[1]
      {
        (object) AppResources.PoundsPluralAbbreviation
      })
    });
  }
}
