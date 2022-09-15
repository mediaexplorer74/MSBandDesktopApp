// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfSyncViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"App", "Golf", "ProTip"})]
  public class GolfSyncViewModel : PageViewModelBase
  {
    private const uint CargoStatusGolfTileOpen = 2693660674;
    internal const string CourseIdKey = "CourseId";
    internal const string TeeIdKey = "TeeId";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfSyncViewModel.cs");
    private static readonly TimeSpan SyncTimeout = TimeSpan.FromMinutes(1.0);
    private readonly IGolfSyncService golfSyncService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IMessageSender messageSender;
    private readonly IMessageBoxService messageBoxService;

    public GolfSyncViewModel(
      INetworkService networkService,
      IGolfSyncService golfSyncService,
      IErrorHandlingService errorHandlingService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IMessageBoxService messageBoxService)
      : base(networkService)
    {
      this.golfSyncService = golfSyncService;
      this.errorHandlingService = errorHandlingService;
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.messageBoxService = messageBoxService;
    }

    protected override void OnNavigatedTo() => this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (message => message.CancelAction()));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      string courseId = this.GetStringParameter("CourseId");
      string teeId = this.GetStringParameter("TeeId");
      while (true)
      {
        try
        {
          using (CancellationTokenSource tokenSource = new CancellationTokenSource(GolfSyncViewModel.SyncTimeout))
            await this.golfSyncService.SyncCourseToBandAsync(courseId, teeId, tokenSource.Token);
          GolfSyncViewModel.Logger.Debug((object) "Golf course sync succeeded.");
          goto label_20;
        }
        catch (Exception ex)
        {
          BandOperationException operationException = ex.Find<BandOperationException>();
          bool flag;
          if (operationException != null && operationException.HResult == 2693660674U)
          {
            GolfSyncViewModel.Logger.Debug((object) "Golf tile is open.", ex);
            flag = await this.messageBoxService.ShowAsync(AppResources.GolfSyncTileOpenFailureAndRetry, AppResources.GolfSyncTryAgainTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK;
          }
          else
          {
            GolfSyncViewModel.Logger.Error((object) "Exception encountered during golf course sync.", ex);
            flag = await this.errorHandlingService.HandleExceptionWithRetryAsync(ex);
          }
          if (flag)
          {
            GolfSyncViewModel.Logger.Debug((object) "Retrying golf course sync.", ex);
            ex = (Exception) null;
          }
          else
            break;
        }
      }
      Exception exception = new Exception();
      GolfSyncViewModel.Logger.Debug((object) "Golf course sync retry declined.", exception);
label_20:
      this.smoothNavService.GoBack();
    }
  }
}
