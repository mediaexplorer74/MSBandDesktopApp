// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Calories.CaloriesWeeklyViewModel
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

namespace Microsoft.Health.App.Core.ViewModels.Calories
{
  [PageTaxonomy(new string[] {"Fitness", "Calories", "Weekly"})]
  public class CaloriesWeeklyViewModel : StatWeeklyViewModelBase<CaloriesWeekViewModel>
  {
    public CaloriesWeeklyViewModel(
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

    protected override string TileIcon => "\uE009";

    protected override StyledSpan GetFormattedStyledHeader(int count) => Formatter.FormatCalories(new int?(count), true);

    protected override InsightDataUsedPivot InsightDataUsed => InsightDataUsedPivot.Calories;

    protected override int GetHistoryStatEntryCount(UserDailySummaryGroup userDailySummaryGroup) => userDailySummaryGroup.TotalCaloriesBurned;
  }
}
