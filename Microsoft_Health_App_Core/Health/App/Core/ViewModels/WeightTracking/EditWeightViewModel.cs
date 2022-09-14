// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WeightTracking.EditWeightViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
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

namespace Microsoft.Health.App.Core.ViewModels.WeightTracking
{
  [PageTaxonomy(new string[] {"Weight", "Editor"})]
  public class EditWeightViewModel : PageViewModelBase
  {
    public const string WeightParameter = "WeightSensor.Weight";
    public const string TimeStampParameter = "WeightSensor.TimeStamp";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WeightTracking\\EditWeightViewModel.cs");
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IWeightProvider weightProvider;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IFormattingService formattingService;
    private StyledSpan weight;
    private string date;
    private string time;
    private Microsoft.Health.App.Core.Providers.WeightSensor weightSensor;
    private ICommand cancelCommand;
    private ICommand deleteCommand;

    public EditWeightViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IMessageBoxService messageBoxService,
      IWeightProvider weightProvider,
      IErrorHandlingService errorHandlingService,
      IFormattingService formattingService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.messageBoxService = messageBoxService;
      this.weightProvider = weightProvider;
      this.errorHandlingService = errorHandlingService;
      this.formattingService = formattingService;
    }

    public StyledSpan Weight => this.weight;

    public string Date => this.date;

    public string Time => this.time;

    public ICommand CancelCommand => this.cancelCommand ?? (this.cancelCommand = (ICommand) new HealthCommand(new Action(this.GoBack)));

    public ICommand DeleteCommand => this.deleteCommand ?? (this.deleteCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      PortableMessageBoxResult response = await this.messageBoxService.ShowAsync(AppResources.DeleteWeightBodyText, AppResources.DeleteWeightTitleText, PortableMessageBoxButton.CasualBooleanChoice);
      if (response == PortableMessageBoxResult.OK && this.LoadState != LoadState.Loading)
      {
        this.LoadState = LoadState.Loading;
        ApplicationTelemetry.LogWeightDeleted(true);
        try
        {
          using (CancellationTokenSource tokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
            await this.weightProvider.DeleteWeightAsync(this.weightSensor, tokenSource.Token);
          this.messageSender.Send<WeightChangedMessage>(new WeightChangedMessage((Microsoft.Health.App.Core.Providers.WeightSensor) null));
          this.GoBack();
        }
        catch (Exception ex)
        {
          this.LoadState = LoadState.Loaded;
          if (ex is NotAcceptableException)
          {
            int num = (int) await this.messageBoxService.ShowAsync(AppResources.LastWeightDeleteErrorBodyText, AppResources.ErrorTitleText, PortableMessageBoxButton.OK);
          }
          else
          {
            EditWeightViewModel.Logger.Error((object) "Failed to delete weight.", ex);
            await this.errorHandlingService.HandleExceptionAsync(ex);
          }
        }
      }
      else
      {
        if (response != PortableMessageBoxResult.Cancel)
          return;
        ApplicationTelemetry.LogWeightDeleted(false);
      }
    })));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(this.GetStringParameter("WeightSensor.TimeStamp"));
      Microsoft.Health.Cloud.Client.Weight weight = Microsoft.Health.Cloud.Client.Weight.FromGrams(this.GetDoubleParameter("WeightSensor.Weight"));
      this.weightSensor = new Microsoft.Health.App.Core.Providers.WeightSensor(dateTimeOffset, weight);
      this.time = Formatter.FormatTimeWithSingleCharacterAMOrPM(dateTimeOffset);
      this.date = Formatter.FormatShortDate(dateTimeOffset);
      this.weight = this.formattingService.FormatWeight(new Microsoft.Health.Cloud.Client.Weight?(weight));
      this.ShowAppBar();
      int num = await Task.FromResult<bool>(true) ? 1 : 0;
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[1]
    {
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private void GoBack() => this.smoothNavService.GoBack();
  }
}
