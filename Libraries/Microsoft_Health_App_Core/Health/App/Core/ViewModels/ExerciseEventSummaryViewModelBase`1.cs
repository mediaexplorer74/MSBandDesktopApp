// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.ExerciseEventSummaryViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class ExerciseEventSummaryViewModelBase<TModel> : 
    EventSummaryViewModelBase<TModel>,
    IRenameableEventViewModel
    where TModel : ExerciseEventBase
  {
    private ActionViewModel renameAction;
    private ICommand renameCommand;
    private ICommand assignNameCommand;
    private ICommand clearNameCommand;
    private bool displayNamingTextBox;
    private string name;

    public bool DisplayNamingTextBox
    {
      get => this.displayNamingTextBox;
      set => this.SetProperty<bool>(ref this.displayNamingTextBox, value, nameof (DisplayNamingTextBox));
    }

    public string Name
    {
      get => this.name;
      set
      {
        this.SetProperty<string>(ref this.name, value, nameof (Name));
        this.InvalidateRenameText();
      }
    }

    public ICommand RenameCommand => this.renameCommand ?? (this.renameCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.DisplayNamingTextBox = true;
      EventHandler renameRequested = this.RenameRequested;
      if (renameRequested == null)
        return;
      renameRequested((object) this, EventArgs.Empty);
    })));

    public ICommand AssignNameCommand => this.assignNameCommand ?? (this.assignNameCommand = (ICommand) AsyncHealthCommand.Create(new Func<Task>(this.AssignNameAsync)));

    public ICommand ClearNameCommand => this.clearNameCommand ?? (this.clearNameCommand = (ICommand) new HealthCommand((Action) (() => this.Name = string.Empty)));

    public virtual Task AssignNameAsync() => (Task) Task.FromResult<object>((object) null);

    public event EventHandler RenameRequested;

    protected ExerciseEventSummaryViewModelBase(
      INetworkService networkUtils,
      ISmoothNavService smoothNavService,
      IErrorHandlingService cargoExceptionUtils,
      IBestEventProvider bestEventProvider,
      IHistoryProvider historyProvider,
      IMessageBoxService messageBoxService,
      IHealthCloudClient healthCloudClient,
      IShareService shareService,
      IFormattingService formattingService,
      IMessageSender messageSender)
      : base(networkUtils, smoothNavService, cargoExceptionUtils, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, messageSender)
    {
    }

    protected override IList<ActionViewModel> InitializeActions()
    {
      this.renameAction = new ActionViewModel(string.Empty, this.RenameCommand);
      this.InvalidateRenameText();
      IList<ActionViewModel> actionViewModelList = base.InitializeActions();
      actionViewModelList.Add(this.renameAction);
      return actionViewModelList;
    }

    protected void AddCaloriesStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelCaloriesBurned,
      Glyph = "\uE009",
      Value = (object) this.Model.CaloriesBurned,
      ValueType = StatValueType.Integer,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelCaloriesFromFat,
        Value = (object) (int) this.Model.CaloriesFromFat,
        ValueType = SubStatValueType.Integer
      },
      SubStat2 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelCaloriesFromCarbs,
        Value = (object) (int) this.Model.CaloriesFromCarbs,
        ValueType = SubStatValueType.Integer
      }
    });

    protected void AddHeartRateStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelAverageHeartRate,
      Glyph = "\uE006",
      Value = (object) this.Model.AverageHeartRate,
      ValueType = StatValueType.Integer,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelPeakHeartRate,
        Value = (object) this.Model.PeakHeartRate,
        ValueType = SubStatValueType.Integer
      },
      SubStat2 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelLowestHeartRate,
        Value = (object) this.Model.LowestHeartRate,
        ValueType = SubStatValueType.Integer
      }
    });

    protected void AddEndingHeartRateStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelEndingHeartRate,
      Glyph = "\uE006",
      Value = (object) this.Model.FinishHeartRate,
      ValueType = StatValueType.Integer,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabel1MinRecovery,
        Value = (object) this.Model.RecoveryHeartRate1Minute,
        ValueType = SubStatValueType.Integer
      },
      SubStat2 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabel2MinRecovery,
        Value = (object) this.Model.RecoveryHeartRate2Minute,
        ValueType = SubStatValueType.Integer
      }
    });

    protected void AddRecoveryTimeStat()
    {
      StatViewModel statViewModel = new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelRecoveryTime,
        Glyph = "\uE025",
        Value = (object) this.Model.RecoveryTime,
        ValueType = StatValueType.DurationWithText
      };
      if (this.Model.RecoveryTime > TimeSpan.Zero)
      {
        DateTimeOffset dateTimeOffset = this.Model.StartTime.ToLocalTime() + this.Model.PausedTime + this.Model.Duration + this.Model.RecoveryTime;
        string str = string.Format(AppResources.PanelStatisticLabelRecoveryTimeFormat, new object[1]
        {
          (object) Formatter.GetFriendlyMonthDayString(dateTimeOffset)
        });
        statViewModel.SubStat1 = new SubStatViewModel()
        {
          Label = str,
          Value = (object) dateTimeOffset,
          ValueType = SubStatValueType.Time
        };
      }
      this.Stats.Add(statViewModel);
    }

    protected void AddFitnessBenefitStat()
    {
      string str = this.FormattingService.FormatFitnessBenefitValue(this.Model.FitnessBenefitMsg);
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelFitnessBenefit,
        Glyph = "\uE148",
        Value = (object) str,
        ValueType = StatValueType.SmallText
      });
    }

    private void InvalidateRenameText()
    {
      if (this.renameAction == null)
        return;
      this.renameAction.Text = string.IsNullOrEmpty(this.Name) ? AppResources.PanelNameButtonText : AppResources.PanelRenameButtonText;
    }
  }
}
