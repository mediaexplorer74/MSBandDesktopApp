// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Oobe.OobeProfileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Oobe
{
  [PageTaxonomy(new string[] {"App", "OOBE", "HWAG"})]
  public class OobeProfileViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Oobe\\OobeProfileViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IOobeService oobeService;
    private readonly IPerfLogger perfLogger;
    private readonly IMessageSender messageSender;
    private ActionViewModel doneAction;
    private HealthCommand gotFocusCommand;
    private bool isLoading;
    private HealthCommand lostFocusCommand;
    private string originalName;

    public OobeProfileViewModel(
      INetworkService networkService,
      IPerfLogger perfLogger,
      IErrorHandlingService errorHandlingService,
      IOobeService oobeService,
      IProfileFieldsViewModel profileFieldsViewModel,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.perfLogger = perfLogger;
      this.errorHandlingService = errorHandlingService;
      this.oobeService = oobeService;
      this.messageSender = messageSender;
      this.ProfileFields = profileFieldsViewModel;
      this.ProfileFields.ShowWelcomeMessage = true;
      this.PrepareLoad();
    }

    public IProfileFieldsViewModel ProfileFields { get; set; }

    public ActionViewModel DoneAction => this.doneAction ?? (this.doneAction = new ActionViewModel(AppResources.OobeProfileNextButtonText, (ICommand) new HealthCommand(new Action(this.AttemptSave))));

    private async void AttemptSave() => await this.AttemptSaveAsync();

    public ICommand LostFocusCommand => (ICommand) this.lostFocusCommand ?? (ICommand) (this.lostFocusCommand = new HealthCommand(new Action(this.LostFocus)));

    private void LostFocus() => this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;

    public ICommand GotFocusCommand => (ICommand) this.gotFocusCommand ?? (ICommand) (this.gotFocusCommand = new HealthCommand(new Action(this.GotFocus)));

    private void GotFocus() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[1]
    {
      new AppBarButton(AppResources.AppBarNext, AppBarIcon.Next, (ICommand) new HealthCommand(new Action(this.GoToNextInputField)))
    });

    private void GoToNextInputField() => this.messageSender.Send<NextInputFieldMessage>(new NextInputFieldMessage());

    public bool IsLoading
    {
      get => this.isLoading;
      set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    private async void PrepareLoad()
    {
      this.IsLoading = true;
      while (true)
      {
        try
        {
          await this.ProfileFields.LoadDataAsync();
          break;
        }
        catch (Exception ex)
        {
          await this.errorHandlingService.HandleExceptionAsync(ex);
        }
      }
      this.IsLoading = false;
      this.originalName = this.ProfileFields.FirstName.Value;
      this.perfLogger.Mark("FreLoad", "End");
    }

    private async Task AttemptSaveAsync()
    {
      this.IsLoading = true;
      if (this.ProfileFields == null)
        return;
      try
      {
        if (!this.ProfileFields.HasErrors)
        {
          if (!this.ProfileFields.Validate())
          {
            this.ProfileFields.RefreshProfileStatus();
            throw new ValidationException("Cannot get local profile if RequiredFieldsFilled is false.");
          }
          this.oobeService.SetCloudUserProfile(this.ProfileFields.Profile);
          this.ProfileFields.Status = ProfileStatus.Success;
        }
      }
      catch (ValidationException ex)
      {
        OobeProfileViewModel.Logger.Error((Exception) ex, "<FAILED> saving profile");
        this.ProfileFields.Status = ProfileStatus.Failure;
      }
      catch (Exception ex)
      {
        OobeProfileViewModel.Logger.Error(ex, "<FAILED> saving profile");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.ProfileFields.Status = ProfileStatus.Failure;
      }
      this.IsLoading = false;
      if (this.ProfileFields.Status != ProfileStatus.Success)
        return;
      int num = this.ProfileFields.Profile.FirstName != this.originalName ? 1 : 0;
      await this.oobeService.CompleteStepAsync(OobeStep.Profile, CancellationToken.None);
    }
  }
}
