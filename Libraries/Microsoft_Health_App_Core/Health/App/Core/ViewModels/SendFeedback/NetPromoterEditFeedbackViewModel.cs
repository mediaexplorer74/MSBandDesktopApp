// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SendFeedback.NetPromoterEditFeedbackViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.SendFeedback
{
  public class NetPromoterEditFeedbackViewModel : PageViewModelBase
  {
    private readonly INpsStore npsStore;
    private readonly ISmoothNavService smoothNavService;
    private HealthCommand backCommand;
    private HealthCommand cancelCommand;
    private string previousFeedback;

    public NetPromoterEditFeedbackViewModel(
      INetworkService networkService,
      INpsStore npsStore,
      ISmoothNavService smoothNavService)
      : base(networkService)
    {
      this.npsStore = npsStore;
      this.smoothNavService = smoothNavService;
    }

    protected override void OnNavigatedTo()
    {
      this.Feedback = this.npsStore.FeedbackText;
      this.previousFeedback = this.Feedback;
    }

    public string Feedback
    {
      get => this.npsStore.FeedbackText;
      set
      {
        this.npsStore.FeedbackText = value;
        this.RaisePropertyChanged(nameof (Feedback));
      }
    }

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand((Action) (() =>
    {
      this.Feedback = this.previousFeedback;
      this.smoothNavService.GoBack();
    })));
  }
}
