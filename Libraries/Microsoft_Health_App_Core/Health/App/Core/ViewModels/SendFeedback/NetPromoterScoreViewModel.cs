// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SendFeedback.NetPromoterScoreViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.NetPromoterScore;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.SendFeedback
{
  [PageTaxonomy(new string[] {"Net Promoter Score", "Description"})]
  public class NetPromoterScoreViewModel : PageViewModelBase
  {
    public const string ReferrerParameterName = "referrerSource";
    public const string ReferrerValuePrompt = "prompt";
    public const string ReferrerValueManual = "unprompt";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\SendFeedback\\NetPromoterScoreViewModel.cs");
    private readonly ISmoothNavService smoothNavService;
    private readonly INetPromoterScoreService netPromoterScoreService;
    private readonly INpsStore npsStore;
    private readonly IUserProfileService userProfileService;
    private string feedbackNotes;
    private int npsAppScore;
    private int npsBandScore;
    private bool submitButtonIsEnabled;
    private string pageTitle;
    private string appSectionSubtitle;
    private string bandSectionSubtitle;
    private bool pressedSubmit;
    private bool editingFeedback;
    private bool showPlaceholderText;
    private HealthCommand backCommand;
    private HealthCommand editDescriptionCommand;
    private HealthCommand submitNpsScoreCommand;
    private HealthCommand<int> chooseNpsAppScoreCommand;
    private HealthCommand<int> chooseNpsBandScoreCommand;
    private string referrerSource;
    private bool isBandPaired;

    public bool IsBandPaired
    {
      get => this.isBandPaired;
      set => this.SetProperty<bool>(ref this.isBandPaired, value, nameof (IsBandPaired));
    }

    public bool ShowPlaceholderText
    {
      get => this.showPlaceholderText;
      set => this.SetProperty<bool>(ref this.showPlaceholderText, value, nameof (ShowPlaceholderText));
    }

    public string FeedbackNotes
    {
      get => this.feedbackNotes;
      set => this.SetProperty<string>(ref this.feedbackNotes, value, nameof (FeedbackNotes));
    }

    public int NpsAppScore
    {
      get => this.npsAppScore;
      set => this.SetProperty<int>(ref this.npsAppScore, value, nameof (NpsAppScore));
    }

    public int NpsBandScore
    {
      get => this.npsBandScore;
      set => this.SetProperty<int>(ref this.npsBandScore, value, nameof (NpsBandScore));
    }

    public bool SubmitButtonIsEnabled
    {
      get => this.submitButtonIsEnabled;
      set
      {
        this.SetProperty<bool>(ref this.submitButtonIsEnabled, value, nameof (SubmitButtonIsEnabled));
        this.submitNpsScoreCommand?.RaiseCanExecuteChanged();
      }
    }

    public NetPromoterScoreViewModel(
      INetworkService networkService,
      ISmoothNavService navigationService,
      IUserProfileService userProfileService,
      INetPromoterScoreService netPromoterScoreService,
      INpsStore npsStore)
      : base(networkService)
    {
      this.smoothNavService = navigationService;
      this.netPromoterScoreService = netPromoterScoreService;
      this.npsStore = npsStore;
      this.userProfileService = userProfileService;
    }

    public ICommand SubmitNpsScoreCommand => (ICommand) this.submitNpsScoreCommand ?? (ICommand) (this.submitNpsScoreCommand = new HealthCommand((Action) (() =>
    {
      this.pressedSubmit = true;
      this.smoothNavService.GoBack();
      NetPromoterScoreViewModel.Logger.Debug((object) "Dismissing Net Promoter Score by Submit command.");
    })));

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() =>
    {
      this.smoothNavService.GoBack();
      NetPromoterScoreViewModel.Logger.Debug((object) "Dismissing Net Promoter Score by Back command.");
    })));

    public ICommand ChooseNpsAppScoreCommand => (ICommand) this.chooseNpsAppScoreCommand ?? (ICommand) (this.chooseNpsAppScoreCommand = new HealthCommand<int>(new Action<int>(this.ChooseNpsAppScore)));

    public ICommand ChooseNpsBandScoreCommand => (ICommand) this.chooseNpsBandScoreCommand ?? (ICommand) (this.chooseNpsBandScoreCommand = new HealthCommand<int>(new Action<int>(this.ChooseNpsBandScore)));

    public EmbeddedAsset HeaderImage => EmbeddedAsset.NpsBird;

    public string AppSectionSubtitle
    {
      get => this.appSectionSubtitle;
      set => this.SetProperty<string>(ref this.appSectionSubtitle, value, nameof (AppSectionSubtitle));
    }

    public string BandSectionSubtitle
    {
      get => this.bandSectionSubtitle;
      set => this.SetProperty<string>(ref this.bandSectionSubtitle, value, nameof (BandSectionSubtitle));
    }

    public string PageTitle
    {
      get => this.pageTitle;
      set => this.SetProperty<string>(ref this.pageTitle, value, nameof (PageTitle));
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null) => Task.Run((Action) (() =>
    {
      this.referrerSource = this.GetStringParameter("referrerSource");
      NetPromoterScoreViewModel.Logger.Debug((object) ("Page loading from " + this.referrerSource));
      this.PageTitle = AppResources.NetPromoterScorePageTitle;
      this.AppSectionSubtitle = string.Format(AppResources.NetPromoterScoreSectionSubtitleFormat, new object[1]
      {
        (object) AppResources.NetPromoterScoreAppSubtitleInsert
      });
      this.BandSectionSubtitle = string.Format(AppResources.NetPromoterScoreSectionSubtitleFormat, new object[1]
      {
        (object) AppResources.NetPromoterScoreBandSubtitleInsert
      });
      this.IsBandPaired = this.userProfileService.IsRegisteredBandPaired;
    }));

    private void ChooseNpsAppScore(int score)
    {
      this.npsAppScore = score;
      this.SubmitButtonIsEnabled = true;
      NetPromoterScoreViewModel.Logger.Debug((object) ("NPS App Score is now " + (object) this.npsAppScore));
    }

    private void ChooseNpsBandScore(int score)
    {
      this.npsBandScore = score;
      this.SubmitButtonIsEnabled = true;
      NetPromoterScoreViewModel.Logger.Debug((object) ("NPS Band Score is now " + (object) this.npsBandScore));
    }

    private void TelemetryTrackSubmission()
    {
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        this.netPromoterScoreService.PostNetPromoterScoreAsync(this.NpsAppScore, this.NpsBandScore, this.FeedbackNotes, this.referrerSource == "unprompt" ? SourceOfNps.Settings : SourceOfNps.Notification, cancellationTokenSource.Token);
        NetPromoterScoreViewModel.Logger.Info((object) string.Format("NPS Feedback submitted with a rating of {0} for App, {1} for Band.", new object[2]
        {
          (object) this.NpsAppScore,
          (object) this.NpsBandScore
        }));
      }
    }

    private void TelemetryTrackDismissal()
    {
      if (!(this.referrerSource == "prompt"))
        return;
      this.netPromoterScoreService.PostNetPromoterScorePromptDismissal();
      NetPromoterScoreViewModel.Logger.Info((object) "NPS Prompt dismissed.");
    }

    protected override void OnNavigatedTo()
    {
      this.FeedbackNotes = this.npsStore.FeedbackText;
      this.editingFeedback = false;
      if (string.IsNullOrEmpty(this.FeedbackNotes))
        this.ResetFeedback();
      base.OnNavigatedTo();
    }

    private void ResetFeedback()
    {
      this.npsStore.Clear();
      this.ShowPlaceholderText = true;
    }

    protected override void OnNavigateFromStarted()
    {
      if (!this.editingFeedback)
      {
        if (this.pressedSubmit)
          this.TelemetryTrackSubmission();
        else
          this.TelemetryTrackDismissal();
        this.ResetFeedback();
      }
      base.OnNavigateFromStarted();
    }

    public ICommand EditDescriptionCommand => (ICommand) this.editDescriptionCommand ?? (ICommand) (this.editDescriptionCommand = new HealthCommand((Action) (() =>
    {
      this.editingFeedback = true;
      this.npsStore.FeedbackText = this.FeedbackNotes;
      this.ShowPlaceholderText = false;
      this.smoothNavService.Navigate(typeof (NetPromoterEditFeedbackViewModel));
    })));
  }
}
