// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WeightTracking.WeightThreeMonthViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.WeightTracking
{
  public class WeightThreeMonthViewModel : WeightViewModelBase
  {
    public WeightThreeMonthViewModel(
      INetworkService networkService,
      IWeightProvider weightProvider,
      IUserProfileService userProfileService,
      ISmoothNavService smoothNavService,
      IWeightSavingService weightSavingService)
      : base(networkService, weightProvider, userProfileService, smoothNavService, weightSavingService)
    {
    }

    public override string ChangedWeightHeader => AppResources.WeightTrackingChangeLabelThreeMonth;

    protected override Range<DateTimeOffset> GetTimeWindow()
    {
      DateTimeOffset timestamp = this.History.First<WeightSensor>().Timestamp;
      return Range.GetInclusive<DateTimeOffset>(timestamp.AddMonths(-3).RoundDown(TimeSpan.FromDays(1.0)), timestamp);
    }
  }
}
