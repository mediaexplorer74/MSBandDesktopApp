// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WeightTracking.WeightAllViewModel
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
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.WeightTracking
{
  public class WeightAllViewModel : WeightViewModelBase
  {
    public WeightAllViewModel(
      INetworkService networkService,
      IWeightProvider weightProvider,
      IUserProfileService userProfileService,
      ISmoothNavService smoothNavService,
      IWeightSavingService weightSavingService)
      : base(networkService, weightProvider, userProfileService, smoothNavService, weightSavingService)
    {
    }

    public override string ChangedWeightHeader => AppResources.WeightTrackingChangeLabelAll;

    protected override IList<WeightSensor> GetHistoryWindowPlusOne() => this.History;

    protected override Range<DateTimeOffset> GetTimeWindow()
    {
      DateTimeOffset timestamp = this.History.First<WeightSensor>().Timestamp;
      DateTimeOffset dateTime = this.History.Last<WeightSensor>().Timestamp;
      DateTimeOffset dateTimeOffset = timestamp.AddDays(-7.0);
      if (dateTimeOffset < dateTime)
        dateTime = dateTimeOffset;
      return Range.GetInclusive<DateTimeOffset>(dateTime.RoundDown(TimeSpan.FromDays(1.0)), timestamp);
    }
  }
}
