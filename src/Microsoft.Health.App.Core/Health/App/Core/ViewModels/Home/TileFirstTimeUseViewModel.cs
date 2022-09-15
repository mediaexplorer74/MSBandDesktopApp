// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.TileFirstTimeUseViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public class TileFirstTimeUseViewModel : HealthViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\TileFirstTimeUseViewModel.cs");
    private readonly ILauncherService promptService;
    private readonly IUserProfileService userProfileService;
    private bool alwaysShowLearnMoreCommand;
    private bool isSupported;
    private Lazy<HealthCommand> learnMoreCommand;
    private Action learnMoreCommandAction;
    private string learnMoreCommandText;
    private string message;
    private TileFirstTimeUseViewModel.FirstTimeUseType type;

    public TileFirstTimeUseViewModel(
      ILauncherService promptService,
      IUserProfileService userProfileService)
    {
      Assert.ParamIsNotNull((object) promptService, nameof (promptService));
      Assert.ParamIsNotNull((object) userProfileService, nameof (userProfileService));
      this.promptService = promptService;
      this.userProfileService = userProfileService;
      this.learnMoreCommandAction = new Action(this.OnLearnMore);
      this.learnMoreCommandText = AppResources.TileFirstTimeUseLearnMore;
      this.learnMoreCommand = new Lazy<HealthCommand>((Func<HealthCommand>) (() => new HealthCommand((Action) (() => this.learnMoreCommandAction()), (Func<bool>) (() => this.learnMoreCommandAction != null))));
    }

    public bool AlwaysShowLearnMoreCommand
    {
      get => this.alwaysShowLearnMoreCommand;
      set
      {
        this.SetProperty<bool>(ref this.alwaysShowLearnMoreCommand, value, nameof (AlwaysShowLearnMoreCommand));
        this.RaisePropertyChanged("ShowLearnMoreCommand");
      }
    }

    public bool ShowLearnMoreCommand => this.AlwaysShowLearnMoreCommand || !this.userProfileService.HasAnyBandBeenRegistered;

    public bool IsSupported
    {
      get => this.isSupported;
      set => this.SetProperty<bool>(ref this.isSupported, value, nameof (IsSupported));
    }

    public ICommand LearnMoreCommand => (ICommand) this.learnMoreCommand.Value;

    public Action LearnMoreCommandAction
    {
      get => this.learnMoreCommandAction;
      set
      {
        this.SetProperty<Action>(ref this.learnMoreCommandAction, value, nameof (LearnMoreCommandAction));
        this.learnMoreCommand.Value.RaiseCanExecuteChanged();
      }
    }

    public string LearnMoreCommandText
    {
      get => this.learnMoreCommandText;
      set => this.SetProperty<string>(ref this.learnMoreCommandText, value, nameof (LearnMoreCommandText));
    }

    public string Message
    {
      get => this.message;
      set => this.SetProperty<string>(ref this.message, value, nameof (Message));
    }

    public TileFirstTimeUseViewModel.FirstTimeUseType Type
    {
      get => this.type;
      set => this.SetProperty<TileFirstTimeUseViewModel.FirstTimeUseType>(ref this.type, value, nameof (Type));
    }

    private void OnLearnMore()
    {
      try
      {
        this.promptService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkID=532705"));
      }
      catch (Exception ex)
      {
        TileFirstTimeUseViewModel.Logger.Error(ex, "Unable to show a web browser for URI: {0}", (object) "https://go.microsoft.com/fwlink/?LinkID=532705");
      }
    }

    public enum FirstTimeUseType
    {
      Unknown,
      Bike,
      Exercise,
      GuidedWorkout,
      Run,
      Sleep,
      Golf,
      Hike,
    }
  }
}
