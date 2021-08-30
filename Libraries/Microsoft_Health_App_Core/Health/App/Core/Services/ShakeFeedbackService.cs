// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ShakeFeedbackService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.ViewModels.SendFeedback;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class ShakeFeedbackService : IShakeFeedbackService
  {
    public const string ShakeFeedbackEnabledKey = "PhoneSettings.ShakeFeedback";
    private const bool DefaultShakeFeedbackSetting = true;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ShakeFeedbackService.cs");
    private readonly IConfigProvider configProvider;
    private readonly IAccelerometerService accelerometerService;
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IDispatchService dispatchService;
    private readonly IReportProblemStore reportProblemStore;
    private readonly IScreenshotService screenshotService;
    private bool isHandlingShakeEvent;
    private List<TypeInfo> ignoreOnViewModels = new List<TypeInfo>()
    {
      typeof (HelpAndFeedbackViewModel).GetTypeInfo(),
      typeof (ReportProblemConfirmViewModel).GetTypeInfo(),
      typeof (ReportProblemEditDescriptionViewModel).GetTypeInfo(),
      typeof (ReportProblemViewModel).GetTypeInfo()
    };

    public ShakeFeedbackService(
      ISmoothNavService smoothNavService,
      IDispatchService dispatchService,
      IMessageBoxService messageBoxService,
      IConfigProvider configProvider,
      IReportProblemStore reportProblemStore,
      IScreenshotService screenshotService,
      IAccelerometerService accelerometerService)
    {
      this.configProvider = configProvider;
      this.accelerometerService = accelerometerService;
      this.messageBoxService = messageBoxService;
      this.dispatchService = dispatchService;
      this.smoothNavService = smoothNavService;
      this.reportProblemStore = reportProblemStore;
      this.screenshotService = screenshotService;
    }

    public void Initialize() => this.SetAccelerometer(this.IsEnabled);

    private void SetAccelerometer(bool enable)
    {
      this.accelerometerService.Active = enable;
      if (enable)
        this.accelerometerService.Shaken += new EventHandler<EventArgs>(this.Shaken);
      else
        this.accelerometerService.Shaken -= new EventHandler<EventArgs>(this.Shaken);
      this.isHandlingShakeEvent = false;
    }

    private async void Shaken(object sender, EventArgs e)
    {
      if (!this.IsEnabled || this.isHandlingShakeEvent)
        return;
      JournalEntry currentJournalEntry = this.smoothNavService.CurrentJournalEntry;
      TypeInfo currentPageTypeInfo = currentJournalEntry != null ? currentJournalEntry.ViewModelType.GetTypeInfo() : (TypeInfo) null;
      if (currentPageTypeInfo == null || this.ignoreOnViewModels.Any<TypeInfo>((Func<TypeInfo, bool>) (vm => vm.IsAssignableFrom(currentPageTypeInfo))))
        return;
      this.isHandlingShakeEvent = true;
      try
      {
        PortableMessageBoxResult answer = await this.messageBoxService.ShowAsync(AppResources.ShakeFeedbackConfirmationMessage, AppResources.ShakeFeedbackConfirmationTitle, PortableMessageBoxButton.OKCancel, AppResources.ShakeFeedbackConfirmationOKButton, AppResources.Cancel);
        if (answer == PortableMessageBoxResult.OK)
        {
          await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () =>
          {
            this.reportProblemStore.Clear();
            foreach (IFile file in (IEnumerable<IFile>) await this.screenshotService.CaptureScreenshotsAsync(CancellationToken.None))
              this.reportProblemStore.ImageFiles.Add(file);
            this.smoothNavService.Navigate(typeof (HelpAndFeedbackViewModel));
          }));
          ApplicationTelemetry.LogFeedbackEntry("Shake");
        }
        Telemetry.LogEvent("App/Feedback/Shake dialog", (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Selection",
            answer.ToString()
          }
        });
      }
      catch (Exception ex)
      {
        ShakeFeedbackService.Logger.Error(ex, "Failed to navigate from ShakeFeedbackService");
      }
      finally
      {
        this.isHandlingShakeEvent = false;
      }
    }

    public bool IsEnabled
    {
      get => this.configProvider.Get<bool>("PhoneSettings.ShakeFeedback", true);
      set
      {
        if (this.configProvider.Get<bool>("PhoneSettings.ShakeFeedback", true) == value)
          return;
        this.configProvider.Set<bool>("PhoneSettings.ShakeFeedback", value);
        this.SetAccelerometer(value);
      }
    }
  }
}
