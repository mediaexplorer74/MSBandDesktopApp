// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Steps.StepsDailyViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Health.App.Core.ViewModels.Steps
{
  [PageTaxonomy(new string[] {"Fitness", "Steps", "Daily"})]
  public class StepsDailyViewModel : StatDailyViewModelBase<StepsDayViewModel>
  {
    public StepsDailyViewModel(
      ISmoothNavService navigationService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      IServiceLocator serviceLocator,
      IUserProfileService profileManager,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider,
      IInsightsProvider insightsProvider)
      : base(navigationService, messageBoxService, networkService, serviceLocator, profileManager, dateTimeService, userDailySummaryProvider, insightsProvider)
    {
    }

    public override string DeviceId
    {
      get => base.DeviceId;
      set
      {
        base.DeviceId = value;
        this.DayViewModel.DeviceId = value;
      }
    }

    protected override string TileIcon => "\uE008";

    protected override StyledSpan GetFormattedStyledHeader(int count) => Formatter.FormatSteps(count, true);

    protected override InsightDataUsedPivot InsightDataUsed => InsightDataUsedPivot.Steps;

    protected override int GetStatHistoryEntryCount(UserDailySummaryGroup userDailySummaryGroup) => userDailySummaryGroup.TotalStepsTaken;
  }
}
