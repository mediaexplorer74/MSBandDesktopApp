// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.EventTileViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public abstract class EventTileViewModelBase<TModel> : MetricTileViewModel where TModel : UserEvent
  {
    public const string EventIdParameter = "ID";
    protected static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\EventTileViewModelBase.cs");
    private readonly EventType eventType;
    private readonly IEventTrackingService eventViewTracker;
    private readonly LabeledItem<EventType> labeledEventType;
    private readonly IMessageSender messageSender;
    private readonly EventSummaryViewModelBase<TModel> summary;
    private TModel currentEvent;

    protected EventTileViewModelBase(
      IEventTrackingService eventViewTracker,
      EventType eventType,
      INetworkService networkUtils,
      IMessageSender messageSender,
      ISmoothNavService smoothNavService,
      EventSummaryViewModelBase<TModel> summary,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(networkUtils, smoothNavService, messageSender, firstTimeUse)
    {
      this.eventViewTracker = eventViewTracker;
      this.eventType = eventType;
      this.messageSender = messageSender;
      this.labeledEventType = LabeledItem<EventType>.FromEnumValue(eventType);
      this.summary = summary;
    }

    public TModel Event
    {
      get => this.currentEvent;
      set => this.SetProperty<TModel>(ref this.currentEvent, value, nameof (Event));
    }

    protected EventType EventType => this.eventType;

    protected override string LoadingMessage
    {
      get
      {
        if (this.OnDetailsPage)
          return AppResources.UpdatingTileMessage;
        return string.Format(AppResources.EventTileLoadingSubheader, new object[1]
        {
          (object) this.labeledEventType.Label
        });
      }
    }

    protected override void OnNavigatedBack()
    {
      base.OnNavigatedBack();
      if (this.Pivots.Any<PivotDefinition>() && !this.Pivots.Any<PivotDefinition>((Func<PivotDefinition, bool>) (pivot => pivot.IsSelected)))
        this.Pivots[0].IsSelected = true;
      this.summary.RefreshHistory();
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<EventChangedMessage>((object) this, new Action<EventChangedMessage>(this.OnEventChanged));
    }

    protected virtual void UpdateColorLevel(TimeSpan ageThreshold)
    {
      this.ColorLevel = TileColorLevel.Medium;
      if ((object) this.Event == null || !(this.Event.StartTime > DateTimeOffset.Now - ageThreshold) || this.eventViewTracker.WasViewed(this.Event.EventId))
        return;
      this.ColorLevel = TileColorLevel.High;
    }

    protected override void OnOpen()
    {
      base.OnOpen();
      if ((object) this.Event == null)
        return;
      this.eventViewTracker.ReportView(this.Event.EventId);
    }

    protected async void OnEventChanged(EventChangedMessage message)
    {
      if (message == null || message.Event == null)
        return;
      if (this.EventType != message.Target)
        return;
      try
      {
        if ((object) this.Event != null && message.Event.EventId == this.Event.EventId && message.Operation != EventOperation.Delete)
          await this.LoadAsync((IDictionary<string, string>) new Dictionary<string, string>()
          {
            ["ID"] = message.Event.EventId
          });
        else
          await this.LoadAsync((IDictionary<string, string>) null);
      }
      catch (Exception ex)
      {
        EventTileViewModelBase<TModel>.Logger.Error(ex, string.Format("Exception encountered during {0} changes.", new object[1]
        {
          (object) this.EventType
        }));
      }
    }
  }
}
