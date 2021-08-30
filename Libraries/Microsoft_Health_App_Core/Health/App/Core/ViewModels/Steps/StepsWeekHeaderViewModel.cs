// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Steps.StepsWeekHeaderViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Steps
{
  public class StepsWeekHeaderViewModel : StatWeekHeaderViewModelBase
  {
    public StepsWeekHeaderViewModel(
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider)
      : base(dateTimeService, userDailySummaryProvider)
    {
    }

    public override string TileIcon => "\uE008";

    public override async Task LoadAsync(IDictionary<string, string> parameters = null)
    {
      await base.LoadAsync(parameters);
      this.Header = Formatter.FormatSteps(this.SummaryGroup?.TotalStepsTaken.GetValueOrDefault(), true).ToSerializedString();
      this.SubHeader = this.StatHistoryEntry?.DateText;
    }
  }
}
