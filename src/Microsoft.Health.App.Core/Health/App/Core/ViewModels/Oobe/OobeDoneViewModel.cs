// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Oobe.OobeDoneViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Oobe
{
  public class OobeDoneViewModel : PanelViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Oobe\\OobeDoneViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IOobeService oobeService;
    private bool showProgress;

    public OobeDoneViewModel(
      IErrorHandlingService errorHandlingService,
      INetworkService networkService,
      IOobeService oobeService)
      : base(networkService)
    {
      this.errorHandlingService = errorHandlingService;
      this.oobeService = oobeService;
    }

    public bool ShowProgress
    {
      get => this.showProgress;
      set => this.SetProperty<bool>(ref this.showProgress, value, nameof (ShowProgress));
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      while (true)
      {
        this.ShowProgress = true;
        Exception ex;
        try
        {
          await this.oobeService.CompleteStepAsync(OobeStep.Done, CancellationToken.None);
          break;
        }
        catch (Exception ex1)
        {
          ex = ex1;
          OobeDoneViewModel.Logger.Error(ex, "Unknown exception trying to complete OOBE");
        }
        await this.errorHandlingService.HandleExceptionAsync(ex);
        ex = (Exception) null;
      }
    }
  }
}
