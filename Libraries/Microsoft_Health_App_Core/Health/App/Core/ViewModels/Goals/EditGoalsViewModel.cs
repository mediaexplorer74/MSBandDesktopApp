// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Goals.EditGoalsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Goals
{
  [PageMetadata(PageContainerType.FullScreen)]
  public class EditGoalsViewModel : PageViewModelBase, IPageTaxonomyProvider
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Goals\\EditGoalsViewModel.cs");
    private readonly IGoalsProvider goalsProvider;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IUserProfileService userProfileService;
    private readonly IFormattingService formattingService;
    private HealthCommand cancelCommand;
    private StyledSpan distance;
    private string goalLabel;
    private Gender? currentUserGender;
    private ushort? currentUserHeightMM;
    private bool isCalories;
    private bool isSteps;
    private bool isEditable;
    private int maxValue;
    private string message;
    private string gotPlanMessage;
    private int minValue;
    private HealthCommand saveCommand;
    private int stepSize;
    private GoalType type;
    private UsersGoal userGoal;
    private int value;
    private TimeSpan walkTime;

    public EditGoalsViewModel(
      IGoalsProvider goalsProvider,
      ISmoothNavService smoothNavService,
      IBandConnectionFactory cargoConnectionFactory,
      IUserProfileService userProfileService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      IMessageSender messageSender,
      IFormattingService formattingService)
      : base(networkService)
    {
      this.userProfileService = userProfileService;
      this.smoothNavService = smoothNavService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.goalsProvider = goalsProvider;
      this.messageBoxService = messageBoxService;
      this.messageSender = messageSender;
      this.formattingService = formattingService;
    }

    public GoalType Type
    {
      get => this.type;
      set => this.SetProperty<GoalType>(ref this.type, value, nameof (Type));
    }

    public int Value
    {
      get => this.value;
      set
      {
        this.SetProperty<int>(ref this.value, value, nameof (Value));
        this.RaisePropertyChanged("ValueString");
        this.CalculateValues();
      }
    }

    public string ValueString => Formatter.FormatGoalValue(this.Value);

    public int MaxValue
    {
      get => this.maxValue;
      set => this.SetProperty<int>(ref this.maxValue, value, nameof (MaxValue));
    }

    public int MinValue
    {
      get => this.minValue;
      set => this.SetProperty<int>(ref this.minValue, value, nameof (MinValue));
    }

    public int StepSize
    {
      get => this.stepSize;
      set => this.SetProperty<int>(ref this.stepSize, value, nameof (StepSize));
    }

    public StyledSpan Distance
    {
      get => this.distance;
      set => this.SetProperty<StyledSpan>(ref this.distance, value, nameof (Distance));
    }

    public TimeSpan WalkTime
    {
      get => this.walkTime;
      set => this.SetProperty<TimeSpan>(ref this.walkTime, value, nameof (WalkTime));
    }

    public bool IsCalories
    {
      get => this.isCalories;
      set => this.SetProperty<bool>(ref this.isCalories, value, nameof (IsCalories));
    }

    public bool IsSteps
    {
      get => this.isSteps;
      set => this.SetProperty<bool>(ref this.isSteps, value, nameof (IsSteps));
    }

    public bool IsEditable
    {
      get => this.isEditable;
      set => this.SetProperty<bool>(ref this.isEditable, value, nameof (IsEditable));
    }

    public string Message
    {
      get => this.message;
      set => this.SetProperty<string>(ref this.message, value, nameof (Message));
    }

    public string GoalLabel
    {
      get => this.goalLabel;
      set => this.SetProperty<string>(ref this.goalLabel, value, nameof (GoalLabel));
    }

    public ICommand SaveCommand
    {
      get
      {
        this.saveCommand = this.saveCommand ?? new HealthCommand(new Action(this.Save));
        return (ICommand) this.saveCommand;
      }
    }

    public ICommand CancelCommand
    {
      get
      {
        this.cancelCommand = this.cancelCommand ?? new HealthCommand(new Action(this.GoBack));
        return (ICommand) this.cancelCommand;
      }
    }

    private void GoBack() => this.smoothNavService.GoBack();

    private async void Save()
    {
      this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      this.LoadState = LoadState.Loading;
      if (this.userProfileService.IsRegisteredBandPaired)
      {
        try
        {
          using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
          {
            using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
              await cargoConnection.CheckConnectionWorkingAsync(cancellationTokenSource.Token);
          }
        }
        catch (Exception ex)
        {
          this.HandleSaveExceptions(ex, AppResources.BandErrorBody, AppResources.BandErrorTitle);
          return;
        }
      }
      try
      {
        await this.goalsProvider.SetGoalAsync(this.type, (object) this.Value);
      }
      catch (Exception ex)
      {
        this.HandleSaveExceptions(ex, AppResources.NetworkErrorBody, AppResources.NetworkErrorTitle);
        return;
      }
      if (this.userProfileService.IsRegisteredBandPaired)
      {
        try
        {
          Microsoft.Band.Admin.Goals goals = await this.CreateGoalAsync();
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
            await cargoConnection.SaveGoalsToBandAsync(goals);
          goals = (Microsoft.Band.Admin.Goals) null;
        }
        catch (Exception ex)
        {
          this.HandleSaveExceptions(ex, AppResources.GoalsSaveError, AppResources.BandErrorTitle);
          return;
        }
      }
      this.messageSender.Send<GoalsChangedMessage>(new GoalsChangedMessage(this.Type, this.Value));
      this.smoothNavService.GoBack();
    }

    private void HandleSaveExceptions(Exception ex, string messageBody, string messageTitle)
    {
      this.LoadState = LoadState.Loaded;
      EditGoalsViewModel.Logger.Error(ex, string.Format("An exception was thrown while saving a new goal for {0}, caused by {1}: {2}", new object[3]
      {
        (object) this.type,
        (object) messageTitle,
        (object) messageBody
      }));
      this.messageBoxService.ShowAsync(messageBody, messageTitle, PortableMessageBoxButton.OK);
      this.ShowSaveCancelButton();
    }

    private async Task<Microsoft.Band.Admin.Goals> CreateGoalAsync()
    {
      Microsoft.Band.Admin.Goals goals1;
      Microsoft.Band.Admin.Goals goals2;
      Microsoft.Band.Admin.Goals goals;
      if (this.type.Equals((object) GoalType.CalorieGoal))
      {
        goals2 = new Microsoft.Band.Admin.Goals();
        goals2.CaloriesEnabled = true;
        goals2.CaloriesGoal = (uint) this.value;
        goals2.StepsEnabled = true;
        goals1 = goals2;
        UsersGoal goalAsync = await this.goalsProvider.GetGoalAsync(GoalType.StepGoal);
        goals1.StepsGoal = (uint) goalAsync.Value;
        goals = goals2;
        goals1 = (Microsoft.Band.Admin.Goals) null;
        goals2 = (Microsoft.Band.Admin.Goals) null;
      }
      else if (this.type.Equals((object) GoalType.StepGoal))
      {
        goals1 = new Microsoft.Band.Admin.Goals();
        goals1.CaloriesEnabled = true;
        goals2 = goals1;
        UsersGoal goalAsync = await this.goalsProvider.GetGoalAsync(GoalType.CalorieGoal);
        goals2.CaloriesGoal = (uint) goalAsync.Value;
        goals1.StepsEnabled = true;
        goals1.StepsGoal = (uint) this.value;
        goals = goals1;
        goals2 = (Microsoft.Band.Admin.Goals) null;
        goals1 = (Microsoft.Band.Admin.Goals) null;
      }
      else
        throw new InvalidOperationException(string.Format("{0} is not a valid type", new object[1]
        {
          (object) this.type
        }));
      return goals;
    }

    private void SetupData()
    {
      switch (this.type)
      {
        case GoalType.StepGoal:
          this.MaxValue = 25000;
          this.MinValue = 1000;
          this.StepSize = 500;
          this.IsSteps = true;
          this.Message = AppResources.EditStepGoalMessage;
          this.gotPlanMessage = AppResources.EditStepGoalUserPartOfPlanMessage;
          this.GoalLabel = AppResources.EditStepGoalLabel;
          break;
        case GoalType.CalorieGoal:
          this.MaxValue = 10000;
          this.MinValue = 1000;
          this.StepSize = 100;
          this.IsCalories = true;
          this.Message = AppResources.EditCalorieGoalMessage;
          this.GoalLabel = AppResources.EditCalorieGoalLabel;
          break;
        default:
          this.MaxValue = 0;
          break;
      }
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      string s1 = (string) null;
      if (parameters != null && parameters.TryGetValue("type", out s1))
      {
        int num = int.Parse(s1);
        Assert.IsTrue((Enum.IsDefined(typeof (GoalType), (object) num) ? 1 : 0) != 0, string.Format("{0} in not a valid member of GoalType.", new object[1]
        {
          (object) num
        }));
        this.type = (GoalType) num;
        this.SetupData();
      }
      else
        this.type = GoalType.Unknown;
      Assert.IsTrue((uint) this.type > 0U, "Goal type must not be unknown.");
      EditGoalsViewModel editGoalsViewModel = this;
      UsersGoal userGoal = editGoalsViewModel.userGoal;
      UsersGoal goalAsync = await this.goalsProvider.GetGoalAsync(this.type);
      editGoalsViewModel.userGoal = goalAsync;
      editGoalsViewModel = (EditGoalsViewModel) null;
      if (this.userGoal == null)
        throw new NoDataException();
      string s2;
      parameters.TryGetValue("Value", out s2);
      long result;
      if (!long.TryParse(s2, out result))
        result = this.userGoal.Value;
      if (!string.IsNullOrEmpty(this.userGoal.ParentId))
      {
        this.Message = this.gotPlanMessage;
        this.IsEditable = false;
        this.SetStepValue(result);
        this.ShowCancelButton();
      }
      else
      {
        this.IsEditable = true;
        this.SetStepValue(result);
        this.ShowSaveCancelButton();
      }
    }

    protected override void OnNavigatedTo()
    {
      if (this.type != GoalType.Unknown)
        return;
      this.smoothNavService.GoBack();
    }

    private void SetStepValue(long initialValue) => this.Value = initialValue > (long) this.MinValue ? (int) initialValue : this.MinValue;

    private void ShowSaveCancelButton() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.SaveCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private void ShowCancelButton() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[1]
    {
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private void CalculateValues()
    {
      if (this.Type != GoalType.StepGoal)
        return;
      if (!this.currentUserGender.HasValue)
        this.currentUserGender = new Gender?(this.userProfileService.CurrentUserProfile.Gender);
      if (!this.currentUserHeightMM.HasValue)
        this.currentUserHeightMM = new ushort?(this.userProfileService.CurrentUserProfile.HeightMM);
      this.Distance = this.formattingService.FormatStat((object) Conversions.DistanceGivenSteps(this.Value, this.currentUserGender.Value, (float) this.currentUserHeightMM.Value), StatValueType.Distance);
      this.WalkTime = Conversions.WalkTimeGivenSteps(this.Value, this.currentUserGender.Value);
    }

    public IReadOnlyList<string> GetPageTaxonomy()
    {
      switch (this.Type)
      {
        case GoalType.StepGoal:
          return (IReadOnlyList<string>) new List<string>()
          {
            "Fitness",
            "Steps",
            "Edit Goal"
          };
        case GoalType.CalorieGoal:
          return (IReadOnlyList<string>) new List<string>()
          {
            "Fitness",
            "Calories",
            "Edit Goal"
          };
        default:
          DebugUtilities.Fail("Unable to determine a proper taxonomy for the edit goals page.");
          return (IReadOnlyList<string>) new List<string>()
          {
            this.GetType().Name
          };
      }
    }
  }
}
