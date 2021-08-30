// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SendFeedback.ReportProblemConfirmViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Diagnostics;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.SendFeedback
{
  [PageTaxonomy(new string[] {"App", "Feedback", "Summary"})]
  public class ReportProblemConfirmViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\SendFeedback\\ReportProblemConfirmViewModel.cs");
    private readonly IReportProblemStore reportProblemStore;
    private readonly IDiagnosticsService diagnosticsService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IEnvironmentService environmentService;
    private readonly IMessageSender messageSender;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private HealthCommand backCommand;
    private AsyncHealthCommand sendCommand;
    private bool includeDiagnosticLog;
    private bool isSending;

    public ReportProblemConfirmViewModel(
      INetworkService networkService,
      IReportProblemStore reportProblemStore,
      IDiagnosticsService diagnosticsService,
      ISmoothNavService smoothNavService,
      IEnvironmentService environmentService,
      IMessageSender messageSender,
      IErrorHandlingService errorHandlingService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService)
    {
      this.reportProblemStore = reportProblemStore;
      this.diagnosticsService = diagnosticsService;
      this.smoothNavService = smoothNavService;
      this.environmentService = environmentService;
      this.messageSender = messageSender;
      this.errorHandlingService = errorHandlingService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.includeDiagnosticLog = true;
    }

    protected override void OnNavigatedTo() => this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (message =>
    {
      if (!this.IsSending)
        return;
      message.CancelAction();
    }));

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() =>
    {
      if (this.IsSending)
        return;
      this.smoothNavService.GoBack();
    })));

    public ICommand SendCommand => (ICommand) this.sendCommand ?? (ICommand) (this.sendCommand = AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      string archiveId = string.Empty;
      try
      {
        Assert.IsTrue(this.reportProblemStore.UserFeedback != null, "User feedback is null while trying to upload.");
        this.IsSending = true;
        this.smoothNavService.DisableNavPanel(typeof (ReportProblemViewModel));
        Func<Task> emailFunc = (Func<Task>) null;
        IFile diagnosisFile = await this.diagnosticsService.CaptureDiagnosisPackageAsync(this.reportProblemStore.UserFeedback, (IEnumerable<IFile>) this.reportProblemStore.ImageFiles, this.environmentService.IsPublicRelease, this.IncludeDiagnosticLog);
        if (!this.environmentService.IsPublicRelease)
        {
          IFile diagnosisFileForEmail;
          IFile file = diagnosisFileForEmail;
          diagnosisFileForEmail = await this.CreateDiagnosisFileForEmailAsync(diagnosisFile);
          string feedbackText = this.reportProblemStore.UserFeedback.Text;
          emailFunc = (Func<Task>) (() => this.diagnosticsService.SendFeedbackEmailAsync(diagnosisFileForEmail, feedbackText));
        }
        archiveId = diagnosisFile.Name;
        if (!string.IsNullOrWhiteSpace(archiveId) && archiveId.LastIndexOf(".") > 0)
          archiveId = archiveId.Substring(0, archiveId.LastIndexOf("."));
        string cloudAsync;
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
          cloudAsync = await cargoConnection.UploadFileToCloudAsync(diagnosisFile, LogFileTypes.KAppLogs, CancellationToken.None);
        ApplicationTelemetry.LogFeedbackComplete(this.reportProblemStore.ImageFiles.Count > 0, this.IncludeDiagnosticLog, !string.IsNullOrEmpty(this.reportProblemStore.UserFeedback.Text), archiveId, cloudAsync);
        DiagnosticsCategory? feedbackCategory = this.reportProblemStore.UserFeedback.Category;
        this.reportProblemStore.Clear();
        if (emailFunc != null)
          await emailFunc();
        if (feedbackCategory.HasValue)
          this.smoothNavService.GoBack(2);
        else
          this.smoothNavService.GoBack(3);
        emailFunc = (Func<Task>) null;
        diagnosisFile = (IFile) null;
        feedbackCategory = new DiagnosticsCategory?();
      }
      catch (Exception ex)
      {
        ApplicationTelemetry.LogFeedbackSendFailure(archiveId);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      finally
      {
        this.IsSending = false;
        this.smoothNavService.EnableNavPanel(typeof (ReportProblemViewModel));
      }
    })));

    private async Task<IFile> CreateDiagnosisFileForEmailAsync(IFile diagnosisFile)
    {
      IFile copiedDiagnosisFile = await this.diagnosticsService.CreateDiagnosisFileAsync(diagnosisFile.Name, !this.environmentService.IsPublicRelease, CreationCollisionOption.GenerateUniqueName);
      await diagnosisFile.CopyToAsync(copiedDiagnosisFile);
      return copiedDiagnosisFile;
    }

    public string Title => this.reportProblemStore.Title ?? AppResources.ReportProblemPageTitle;

    public string Description => this.reportProblemStore.UserFeedback?.Text;

    public string ImageCountLabel
    {
      get
      {
        if (this.reportProblemStore.ImageFiles.Count <= 0)
          return (string) null;
        return string.Format(AppResources.ReportProblemAttachedImageCountLabel, new object[1]
        {
          (object) this.reportProblemStore.ImageFiles.Count
        });
      }
    }

    public bool IncludeDiagnosticLog
    {
      get => this.includeDiagnosticLog;
      set => this.SetProperty<bool>(ref this.includeDiagnosticLog, value, nameof (IncludeDiagnosticLog));
    }

    public bool IsSending
    {
      get => this.isSending;
      set => this.SetProperty<bool>(ref this.isSending, value, nameof (IsSending));
    }
  }
}
