// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.EventSummaryViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Sharing;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.History;
using Microsoft.Health.Cloud.Client;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class EventSummaryViewModelBase<TModel> : 
    PanelViewModelBase,
    IInsightModel,
    IShareImageViewModel
    where TModel : UserEvent
  {
    protected static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\EventSummaryViewModelBase.cs");
    private const int ResultsPageSize = 10;
    private readonly ISmoothNavService smoothNavService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IBestEventProvider bestEventProvider;
    private readonly IHistoryProvider historyProvider;
    private readonly IMessageBoxService messageBoxService;
    private readonly IShareService shareService;
    private readonly IMessageSender messageSender;
    private TModel model;
    private IList<BestEvent> bestEvents;
    private IList<HistoryEventViewModel<TModel>> allEvents;
    private string insightMessage;
    private string insightActionMessage;
    private bool showInsight;
    private GoalType goalType;
    private EventType eventType;
    private bool endReached;
    private bool showHistory;
    private bool isLoading;
    private bool canHaveBests = true;
    private int selectedItemIndex = -1;
    private int version;
    private string deletionMessage;
    private bool isBeingEdited;
    private ICommand shareButtonCommand;
    private IList<ActionViewModel> actions;
    private bool hasHistory = true;

    protected IHealthCloudClient HealthCloudClient { get; private set; }

    protected IFormattingService FormattingService { get; private set; }

    protected EventSummaryViewModelBase(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IBestEventProvider bestEventProvider,
      IHistoryProvider historyProvider,
      IMessageBoxService messageBoxService,
      IHealthCloudClient healthCloudClient,
      IShareService shareService,
      IFormattingService formattingService,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.bestEventProvider = bestEventProvider;
      this.historyProvider = historyProvider;
      this.messageBoxService = messageBoxService;
      this.HealthCloudClient = healthCloudClient;
      this.shareService = shareService;
      this.FormattingService = formattingService;
      this.messageSender = messageSender;
    }

    protected virtual IList<ActionViewModel> InitializeActions()
    {
      ObservableCollection<ActionViewModel> observableCollection = new ObservableCollection<ActionViewModel>();
      observableCollection.Add(new ActionViewModel(AppResources.ShareEventButtonText, this.ShareButtonCommand, (object) "Bottom"));
      observableCollection.Add(new ActionViewModel(AppResources.PanelDeleteButtonText, this.DeleteCommand));
      return (IList<ActionViewModel>) observableCollection;
    }

    public IList<ActionViewModel> Actions
    {
      get
      {
        if (this.actions == null)
          this.actions = this.InitializeActions();
        return this.actions;
      }
    }

    public TModel Model
    {
      get => this.model;
      protected set => this.SetProperty<TModel>(ref this.model, value, nameof (Model));
    }

    public IList<StatViewModel> Stats { get; } = (IList<StatViewModel>) new ObservableCollection<StatViewModel>();

    public bool IsLoading
    {
      get => this.isLoading;
      set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public GoalType GoalType
    {
      get => this.goalType;
      set => this.SetProperty<GoalType>(ref this.goalType, value, nameof (GoalType));
    }

    public EventType EventType
    {
      get => this.eventType;
      set => this.SetProperty<EventType>(ref this.eventType, value, nameof (EventType));
    }

    public IList<HistoryEventViewModel<TModel>> AllEvents
    {
      get => this.allEvents;
      set => this.SetProperty<IList<HistoryEventViewModel<TModel>>>(ref this.allEvents, value, nameof (AllEvents));
    }

    public string InsightMessage
    {
      get => this.insightMessage;
      set => this.SetProperty<string>(ref this.insightMessage, value, nameof (InsightMessage));
    }

    public string InsightActionMessage
    {
      get => this.insightActionMessage;
      set => this.SetProperty<string>(ref this.insightActionMessage, value, nameof (InsightActionMessage));
    }

    public bool ShowInsight
    {
      get => this.showInsight;
      set => this.SetProperty<bool>(ref this.showInsight, value, nameof (ShowInsight));
    }

    public bool CanHaveBests
    {
      get => this.canHaveBests;
      set => this.SetProperty<bool>(ref this.canHaveBests, value, nameof (CanHaveBests));
    }

    public bool HasHistory
    {
      get => this.hasHistory;
      set => this.SetProperty<bool>(ref this.hasHistory, value, nameof (HasHistory));
    }

    public bool ShowHistory
    {
      get => this.showHistory;
      set => this.SetProperty<bool>(ref this.showHistory, value, nameof (ShowHistory));
    }

    public string DeletionMessage
    {
      get => this.deletionMessage;
      set => this.SetProperty<string>(ref this.deletionMessage, value, nameof (DeletionMessage));
    }

    public IShareImageProvider ShareImageThumbnailProvider { get; set; }

    protected virtual bool IsShareImageIncluded => false;

    public bool IsBeingEdited
    {
      get => this.isBeingEdited;
      set => this.SetProperty<bool>(ref this.isBeingEdited, value, nameof (IsBeingEdited));
    }

    public ICommand ShareButtonCommand => this.shareButtonCommand ?? (this.shareButtonCommand = (ICommand) new AsyncHealthCommand<string>((Func<string, Task>) (parameter =>
    {
      if (parameter == "Top")
        return this.BeginShareOperationAsync(ShareButtonType.Top);
      if (parameter == "Bottom")
        return this.BeginShareOperationAsync(ShareButtonType.Bottom);
      EventSummaryViewModelBase<TModel>.Logger.Error("ShareButtonCommand parameter {0} invalid", (object) (parameter ?? "<null>"));
      return (Task) Task.FromResult<object>((object) null);
    }), false));

    private async Task BeginShareOperationAsync(ShareButtonType shareButtonType)
    {
      ApplicationTelemetry.LogShareButtonTap(this.EventType, ShareType.Enhanced, shareButtonType);
      this.IsLoading = true;
      try
      {
        ShareRequest shareRequestAsync = await this.CreateShareRequestAsync();
        shareRequestAsync.ButtonType = shareButtonType;
        EventSummaryViewModelBase<TModel>.Logger.Debug((object) "<FLAG> Preparing the share service for sharing.");
        await this.shareService.PrepareForSharingAsync(shareRequestAsync);
        this.shareService.Share();
      }
      finally
      {
        this.IsLoading = false;
      }
    }

    protected virtual async Task<ShareRequest> CreateShareRequestAsync()
    {
      ShareRequest request = new ShareRequest();
      if (this.IsShareImageIncluded)
      {
        IShareImageProvider thumbnailProvider = this.ShareImageThumbnailProvider;
        request.EventType = this.eventType;
        if (thumbnailProvider != null)
        {
          ShareRequest shareRequest = request;
          IFile imageThumbnailAsync = await thumbnailProvider.GetShareImageThumbnailAsync(CancellationToken.None);
          shareRequest.ShareImageThumbnail = imageThumbnailAsync;
          shareRequest = (ShareRequest) null;
        }
      }
      return request;
    }

    protected virtual void SetEventProperties() => this.PopulateInsight();

    protected void AddDurationStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelDuration,
      Glyph = "\uE025",
      Value = (object) this.Model.Duration,
      ValueType = StatValueType.DurationFull,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelStartTime,
        Value = (object) this.Model.StartTime.ToLocalTime(),
        ValueType = SubStatValueType.Time
      }
    });

    protected void AddUvExposureStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelUvExposure,
      Glyph = "\uE091",
      Value = (object) TimeSpan.FromMinutes((double) this.Model.UvExposure),
      ValueType = StatValueType.DurationWithTextWithoutSeconds
    });

    protected void AddCardioMinutesStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelCardioMinutes,
      Glyph = "\uE025",
      Value = (object) (this.Model.CardioScore / 60),
      ValueType = StatValueType.Integer,
      ShowNotAvailableOnZero = false,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.CoachingPlanActivitySubmetricTwoTitle,
        Value = (object) (this.Model.IntenseCardioSeconds / 60),
        ValueType = SubStatValueType.CardioBonusMinutes
      }
    });

    protected async Task SetHistoryIfValidAsync(IDictionary<string, string> parameters)
    {
      if (parameters != null)
      {
        if (parameters.ContainsKey("History"))
          this.HasHistory = bool.Parse(parameters["History"]);
      }
      else
        this.HasHistory = true;
      if (!this.HasHistory)
        return;
      await this.SetHistoryAsync();
    }

    private async Task SetHistoryAsync()
    {
      ++this.version;
      await this.SetBestsAsync();
      this.AllEvents = (IList<HistoryEventViewModel<TModel>>) new ObservableCollection<HistoryEventViewModel<TModel>>();
      await this.LoadNewEventsAsync();
    }

    private async Task SetBestsAsync()
    {
      IList<BestEvent> bestEventsAsync = await this.bestEventProvider.GetBestEventsAsync(this.goalType);
      if (bestEventsAsync == null)
        return;
      this.bestEvents = (IList<BestEvent>) bestEventsAsync.ToList<BestEvent>();
    }

    private void PopulateInsight() => InsightUtilities.PopulateInsightModel((IInsightModel) this, this.Model.Evidences.Where<Evidence>((Func<Evidence, bool>) (e => e.Role == InsightRole.Primary)).Select<Evidence, RaisedInsight>((Func<Evidence, RaisedInsight>) (e => e.Insight)));

    public async void RefreshHistory()
    {
      if (this.AllEvents == null || this.CanHaveBests && (!this.CanHaveBests || this.bestEvents == null))
        return;
      await this.LoadItemAtIndexAsync();
      if (this.AllEvents.Count != 0)
        return;
      this.ShowHistory = false;
    }

    private async Task LoadItemAtIndexAsync()
    {
      try
      {
        int localVersion = this.version;
        int localSelectedItemIndex = this.selectedItemIndex;
        if (localSelectedItemIndex >= this.AllEvents.Count || localSelectedItemIndex < 0)
          return;
        HistoryEventViewModel<TModel> localItem = this.AllEvents[localSelectedItemIndex];
        try
        {
          await this.SetBestsAsync();
        }
        catch (Exception ex)
        {
          EventSummaryViewModelBase<TModel>.Logger.Error(ex, "Unable to refresh the best events.");
        }
        if (localVersion != this.version)
          return;
        HistoryEventViewModel<TModel> serverItem;
        try
        {
          serverItem = await this.historyProvider.GetHistoryItemAsync<TModel>(localItem.EventId);
        }
        catch (Exception ex)
        {
          EventSummaryViewModelBase<TModel>.Logger.Error(ex, "Unable to retrieve item at history index of {0}", (object) localSelectedItemIndex);
          return;
        }
        if (localVersion != this.version)
          return;
        if (serverItem != null)
        {
          this.bestEventProvider.CheckForBest((HistoryEventViewModelBase) serverItem, (ICollection<BestEvent>) this.bestEvents);
          this.AllEvents[localSelectedItemIndex] = serverItem;
        }
        else
        {
          this.AllEvents.RemoveAt(localSelectedItemIndex);
          for (int index = 0; index < this.AllEvents.Count; ++index)
          {
            HistoryEventViewModel<TModel> allEvent = this.AllEvents[index];
            if (!allEvent.IsBest)
            {
              this.bestEventProvider.CheckForBest((HistoryEventViewModelBase) allEvent, (ICollection<BestEvent>) this.bestEvents);
              if (allEvent.IsBest)
              {
                this.AllEvents.RemoveAt(index);
                this.AllEvents.Insert(index, allEvent);
              }
            }
          }
        }
        localItem = (HistoryEventViewModel<TModel>) null;
        serverItem = (HistoryEventViewModel<TModel>) null;
      }
      finally
      {
        this.selectedItemIndex = -1;
      }
    }

    private async void Paginate()
    {
      if (this.IsLoading || this.endReached || !this.ShowHistory)
        return;
      this.IsLoading = true;
      await this.LoadNewEventsAsync();
      this.IsLoading = false;
    }

    private void SelectItem(HistoryEventViewModel<TModel> item)
    {
      if (item != null)
      {
        HistoryEventViewModel<TModel> historyEventViewModel = this.AllEvents.FirstOrDefault<HistoryEventViewModel<TModel>>((Func<HistoryEventViewModel<TModel>, bool>) (e => e.EventId == item.EventId));
        if (historyEventViewModel != null)
          item = historyEventViewModel;
      }
      this.selectedItemIndex = this.AllEvents.IndexOf(item);
      if (item == null)
        return;
      this.smoothNavService.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "ID",
          item.EventId
        },
        {
          "Type",
          item.EventType.ToString()
        },
        {
          "History",
          bool.FalseString
        }
      });
    }

    private async Task LoadNewEventsAsync()
    {
      this.endReached = false;
      try
      {
        DateTimeOffset? beforeDate = new DateTimeOffset?();
        if (this.AllEvents != null)
        {
          HistoryEventViewModel<TModel> historyEventViewModel = this.AllEvents.LastOrDefault<HistoryEventViewModel<TModel>>();
          if (historyEventViewModel != null)
            beforeDate = new DateTimeOffset?(historyEventViewModel.Event.StartTime);
        }
        if (!beforeDate.HasValue && (object) this.Model != null)
        {
          DateTimeOffset startTime = this.Model.StartTime;
          beforeDate = new DateTimeOffset?(this.Model.StartTime.ToLocalTime());
        }
        IList<HistoryEventViewModel<TModel>> eventHistoryAsync = await this.historyProvider.GetEventHistoryAsync<TModel>(this.eventType, 10, beforeDate);
        if (eventHistoryAsync != null && eventHistoryAsync.Any<HistoryEventViewModel<TModel>>())
        {
          if (eventHistoryAsync.Count < 10)
            this.endReached = true;
          this.bestEventProvider.PopulateAllEvents<TModel>((ICollection<HistoryEventViewModel<TModel>>) eventHistoryAsync, (ICollection<BestEvent>) this.bestEvents, (ICollection<HistoryEventViewModel<TModel>>) this.AllEvents);
        }
        else
          this.endReached = true;
      }
      catch (Exception ex)
      {
        EventSummaryViewModelBase<TModel>.Logger.Error(ex, "Loading of events failed for type {0}", (object) this.EventType);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.ShowHistory = this.AllEvents != null && this.AllEvents.Any<HistoryEventViewModel<TModel>>();
    }

    protected virtual async Task DeleteAsync()
    {
      if ((object) this.Model == null)
        return;
      if (await this.messageBoxService.ShowAsync(this.DeletionMessage, AppResources.DeleteTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
        return;
      try
      {
        await this.HealthCloudClient.DeleteEventAsync(this.Model.EventId, this.EventType, CancellationToken.None);
        this.messageSender.Send<EventChangedMessage>(new EventChangedMessage()
        {
          Event = (UserEvent) this.Model,
          Operation = EventOperation.Delete,
          Target = this.EventType,
          IsRefreshCanceled = !this.HasHistory
        });
      }
      catch (Exception ex)
      {
        EventSummaryViewModelBase<TModel>.Logger.Error(ex, "Exception encountered during deletion.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.smoothNavService.GoBack();
    }

    public ICommand DeleteCommand => (ICommand) new HealthCommand((Action) (async () => await this.DeleteAsync()));

    public ICommand LoadMoreCommand => (ICommand) new HealthCommand(new Action(this.Paginate));

    public ICommand SelectItemCommand => (ICommand) new HealthCommand<HistoryEventViewModel<TModel>>(new Action<HistoryEventViewModel<TModel>>(this.SelectItem));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await base.LoadDataAsync(parameters);
      this.Stats.Clear();
    }
  }
}
