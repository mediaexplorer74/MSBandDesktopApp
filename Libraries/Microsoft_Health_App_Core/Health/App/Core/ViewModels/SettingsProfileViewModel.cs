// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SettingsProfileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Settings", "User", "Profile"})]
  [PageMetadata(PageContainerType.HomeShell)]
  public class SettingsProfileViewModel : PageViewModelBase, IHomeShellViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\SettingsProfileViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService navService;
    private readonly IUserProfileService userProfileService;
    private HealthCommand cancelCommand;
    private HealthCommand gotFocusCommand;
    private HealthCommand lostFocusCommand;
    private HealthCommand updateCommand;
    private string loadingText;
    private bool isLoading;
    private bool isFocusedOnText;

    public SettingsProfileViewModel(
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IUserProfileService userProfileService,
      INetworkService networkService,
      IProfileFieldsViewModel profileFieldsViewModel,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.navService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.userProfileService = userProfileService;
      this.messageSender = messageSender;
      this.ProfileFields = profileFieldsViewModel;
    }

    public NavigationHeaderBackground NavigationHeaderBackground => NavigationHeaderBackground.Dark;

    public IProfileFieldsViewModel ProfileFields { get; set; }

    public ICommand UpdateCommand => (ICommand) this.updateCommand ?? (ICommand) (this.updateCommand = new HealthCommand(new Action(this.AttemptSave)));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand((Action) (() => this.ProfileFields.Revert())));

    public ICommand LostFocusCommand => (ICommand) this.lostFocusCommand ?? (ICommand) (this.lostFocusCommand = new HealthCommand(new Action(this.LostFocus), (Func<bool>) (() => this.IsActive)));

    private void LostFocus()
    {
      this.isFocusedOnText = false;
      this.RefreshAppBar();
    }

    public ICommand GotFocusCommand => (ICommand) this.gotFocusCommand ?? (ICommand) (this.gotFocusCommand = new HealthCommand(new Action(this.GotFocus), (Func<bool>) (() => this.IsActive)));

    private void GotFocus()
    {
      this.isFocusedOnText = true;
      this.RefreshAppBar();
    }

    public string LoadingText
    {
      get => this.loadingText;
      set => this.SetProperty<string>(ref this.loadingText, value, nameof (LoadingText));
    }

    public bool IsLoading
    {
      get => this.isLoading;
      set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      SettingsProfileViewModel.Logger.Debug((object) "<START> loading profile settings page");
      try
      {
        this.LoadingText = AppResources.CheckingProfileLoadingText;
        this.IsLoading = true;
        this.RefreshAppBar();
        await this.ProfileFields.LoadDataAsync((IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Origin",
            "Settings"
          }
        });
        this.IsLoading = false;
        SettingsProfileViewModel.Logger.Debug((object) "<END> loading profile settings page");
      }
      catch (Exception ex)
      {
        SettingsProfileViewModel.Logger.Error(ex, "<FAILED> loading profile settings page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.navService.GoBack();
      }
      finally
      {
        this.RefreshAppBar();
      }
    }

    protected override void OnNavigatedTo()
    {
      this.ProfileFields.PropertyChanged += new PropertyChangedEventHandler(this.OnFieldPropertyChanged);
      base.OnNavigatedTo();
    }

    protected override void OnNavigatedFrom()
    {
      this.ProfileFields.PropertyChanged -= new PropertyChangedEventHandler(this.OnFieldPropertyChanged);
      base.OnNavigatedFrom();
    }

    protected override void OnBackNavigation() => this.RefreshAppBar();

    private void OnFieldPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "IsDirty"))
        return;
      this.RefreshAppBar();
    }

    private async void AttemptSave()
    {
      SettingsProfileViewModel.Logger.Debug((object) "<START> saving profile");
      if (!await this.AttemptSaveInternalAsync(false))
      {
        SettingsProfileViewModel.Logger.Debug((object) "<RECOVERY> Attempt to save to cloud only.");
        int num = await this.AttemptSaveInternalAsync(true) ? 1 : 0;
      }
      SettingsProfileViewModel.Logger.Debug((object) "<END> saving profile");
    }

    private async Task<bool> AttemptSaveInternalAsync(bool useCloudOnly)
    {
      try
      {
        this.LoadingText = AppResources.UpdatingLoadingText;
        this.IsLoading = true;
        this.RefreshAppBar();
        if (this.ProfileFields != null && !this.ProfileFields.HasErrors)
        {
          if (!this.ProfileFields.Validate())
          {
            this.ProfileFields.RefreshProfileStatus();
            throw new ValidationException("Cannot get local profile if RequiredFieldsFilled is false.");
          }
          using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
            await this.userProfileService.SaveUserProfileAsync(this.ProfileFields.Profile, cancellationTokenSource.Token, useCloudOnly);
          ApplicationTelemetry.LogProfileUpdated();
          this.ProfileFields.Status = ProfileStatus.Success;
        }
        this.IsLoading = false;
        this.ProfileFields.MarkSaved();
      }
      catch (ValidationException ex)
      {
        SettingsProfileViewModel.Logger.Error((Exception) ex, "<FAILED> saving profile");
        this.ProfileFields.Status = ProfileStatus.Failure;
        this.IsLoading = false;
      }
      catch (BandIOException ex)
      {
        SettingsProfileViewModel.Logger.Warn((Exception) ex, "<WARNING> saving profile to cloud as Band cannot be found");
        return false;
      }
      catch (Exception ex)
      {
        this.IsLoading = false;
        this.ProfileFields.Status = ProfileStatus.Failure;
        SettingsProfileViewModel.Logger.Error(ex, "<FAILED> saving profile");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      finally
      {
        this.RefreshAppBar();
      }
      return true;
    }

    private void RefreshAppBar()
    {
      if (this.IsLoading)
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      else if (this.isFocusedOnText)
        this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[1]
        {
          new AppBarButton(AppResources.AppBarNext, AppBarIcon.Next, (ICommand) new HealthCommand(new Action(this.GoToNextInputField)))
        });
      else if (this.ProfileFields.IsDirty)
        this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
        {
          new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.UpdateCommand),
          new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
        });
      else
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
    }

    private void GoToNextInputField() => this.messageSender.Send<NextInputFieldMessage>(new NextInputFieldMessage());
  }
}
