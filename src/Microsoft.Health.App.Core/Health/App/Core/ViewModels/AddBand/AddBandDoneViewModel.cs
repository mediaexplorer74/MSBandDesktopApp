// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.AddBand.AddBandDoneViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Oobe;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.AddBand
{
  [PageTaxonomy(new string[] {"App", "Add Band", "Finished"})]
  public class AddBandDoneViewModel : OobePageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\AddBand\\AddBandDoneViewModel.cs");
    private readonly IAddBandService addBandService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageSender messageSender;
    private readonly IMessageBoxService messageBoxService;
    private readonly INetPromoterService netPromoterService;
    private string loadingText;
    private bool somethingWentWrong;
    private ActionViewModel positiveAction;
    private ActionViewModel negativeAction;

    public AddBandDoneViewModel(
      INetworkService networkService,
      IAddBandService addBandService,
      IErrorHandlingService errorHandlingService,
      IMessageSender messageSender,
      INetPromoterService netPromoterService,
      IMessageBoxService messageBoxService)
      : base(networkService)
    {
      this.addBandService = addBandService;
      this.errorHandlingService = errorHandlingService;
      this.messageSender = messageSender;
      this.messageBoxService = messageBoxService;
      this.netPromoterService = netPromoterService;
    }

    public string LoadingText
    {
      get => this.loadingText;
      set => this.SetProperty<string>(ref this.loadingText, value, nameof (LoadingText));
    }

    public override string PageTitle => AppResources.ErrorPageTitle;

    public override string PageSubtitle => AppResources.GenericErrorMessage;

    public override ActionViewModel PositiveAction => this.positiveAction ?? (this.positiveAction = new ActionViewModel(AppResources.TryAgainButton, (ICommand) new HealthCommand((Action) (async () => await this.CompleteAddBandAsync()))));

    public override ActionViewModel NegativeAction => this.negativeAction ?? (this.negativeAction = new ActionViewModel(AppResources.Cancel, (ICommand) new HealthCommand((Action) (async () =>
    {
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        if (await this.messageBoxService.ShowAsync(AppResources.AddBandCancelDialogMessage, AppResources.ApplicationTitle, PortableMessageBoxButton.CasualBooleanChoice) == PortableMessageBoxResult.OK)
          await this.addBandService.ExitAsync(cancellationTokenSource.Token);
      }
    }))));

    public bool SomethingWentWrong
    {
      get => this.somethingWentWrong;
      private set => this.SetProperty<bool>(ref this.somethingWentWrong, value, nameof (SomethingWentWrong));
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null) => this.CompleteAddBandAsync();

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (message => message.CancelAction()));
    }

    private async Task CompleteAddBandAsync()
    {
      this.SomethingWentWrong = false;
      try
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5.0)))
        {
          await this.addBandService.NextAsync(cancellationTokenSource.Token, (IProgress<InitializationProgress>) new SimpleProgress<InitializationProgress>((Action<InitializationProgress>) (p => this.Report(p))));
          if (!this.netPromoterService.UserHasFilledNpsSurvey)
            this.netPromoterService.ResetAppUseCount();
        }
      }
      catch (Exception ex)
      {
        AddBandDoneViewModel.Logger.Error(ex, "Failed to complete adding a band.");
        this.SomethingWentWrong = true;
        ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
        {
          "App",
          "Add Band",
          "Something went wrong"
        });
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    }

    private void Report(InitializationProgress progress)
    {
      if (progress != InitializationProgress.GettingReady)
      {
        if (progress == InitializationProgress.AlmostThere)
          this.LoadingText = AppResources.EarlyUpdateSuccessTitle;
        else
          this.LoadingText = string.Empty;
      }
      else
        this.LoadingText = AppResources.GettingReadyLoadingText;
    }
  }
}
