// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.RouteBasedExerciseEventSummaryViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class RouteBasedExerciseEventSummaryViewModelBase<TModel> : 
    ExerciseEventSummaryViewModelBase<TModel>
    where TModel : RouteBasedExerciseEvent
  {
    private readonly IBestEventProvider bestEventProvider;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IRouteBasedExerciseEventProvider<TModel> routeBasedExerciseEventProvider;
    private readonly IMessageSender messageSender;
    private HealthCommand assignNameCommand;
    private string id;
    private bool initialized;
    private string personalBestMessage;
    private bool isPersonalBest;
    private bool isLowPowerGps;
    private ArgbColor32 pathColor;
    private bool useMarkerForEnds;
    private bool useSatelliteImages;

    protected RouteBasedExerciseEventSummaryViewModelBase(
      INetworkService networkUtils,
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IBestEventProvider bestEventProvider,
      IHistoryProvider historyProvider,
      IMessageBoxService messageBoxService,
      IHealthCloudClient healthCloudClient,
      IShareService shareService,
      IFormattingService formattingService,
      IRouteBasedExerciseEventProvider<TModel> routeBasedExerciseEventProvider,
      IMessageSender messageSender,
      ArgbColor32 pathColor = null,
      bool useMarkersForEnds = false,
      bool useSatelliteImages = false)
      : base(networkUtils, smoothNavService, errorHandlingService, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, messageSender)
    {
      this.bestEventProvider = bestEventProvider;
      this.errorHandlingService = errorHandlingService;
      this.routeBasedExerciseEventProvider = routeBasedExerciseEventProvider;
      this.messageSender = messageSender;
      this.pathColor = pathColor;
      this.useMarkerForEnds = useMarkersForEnds;
      this.useSatelliteImages = useSatelliteImages;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await base.LoadDataAsync(parameters);
      if (parameters != null && parameters.TryGetValue("ID", out this.id))
      {
        this.Model = await this.routeBasedExerciseEventProvider.GetEventAsync(this.id);
      }
      else
      {
        this.id = (await this.routeBasedExerciseEventProvider.GetLastEventAsync() ?? throw new NoDataException(string.Format("failed to get the most recent {0} event", new object[1]
        {
          (object) this.EventType
        }))).EventId;
        this.Model = await this.routeBasedExerciseEventProvider.GetEventAsync(this.id);
      }
      if ((object) this.Model != null)
      {
        KeyValuePair<bool, string> labelForEventAsync = await this.bestEventProvider.GetLabelForEventAsync(this.id, this.EventType);
        this.IsPersonalBest = labelForEventAsync.Key;
        this.PersonalBestMessage = labelForEventAsync.Value;
        this.Name = this.Model.Name;
        this.IsLowPowerGps = this.Model.IsLowPowerGps;
        this.SetEventProperties();
        await this.SetHistoryIfValidAsync(parameters);
      }
      else
        throw new NoDataException(string.Format("failed to get {0} event", new object[1]
        {
          (object) this.EventType
        }));
    }

    protected void AddElevationGainStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelElevationGain,
      Glyph = "\uE103",
      Value = (object) this.Model.TotalAltitudeGain,
      ValueType = StatValueType.Elevation,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelElevationMax,
        Value = (object) this.Model.MaxAltitude,
        ValueType = SubStatValueType.Elevation
      }
    });

    protected void AddElevationLossStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelElevationLoss,
      Glyph = "\uE103",
      Value = (object) -this.Model.TotalAltitudeLoss,
      ValueType = StatValueType.Elevation,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelElevationMin,
        Value = (object) this.Model.MinAltitude,
        ValueType = SubStatValueType.Elevation
      }
    });

    protected string Id
    {
      get => this.id;
      set => this.SetProperty<string>(ref this.id, value, nameof (Id));
    }

    public bool Initialized
    {
      get => this.initialized;
      set => this.SetProperty<bool>(ref this.initialized, value, nameof (Initialized));
    }

    public string PersonalBestMessage
    {
      get => this.personalBestMessage;
      set => this.SetProperty<string>(ref this.personalBestMessage, value, nameof (PersonalBestMessage));
    }

    public bool IsPersonalBest
    {
      get => this.isPersonalBest;
      set => this.SetProperty<bool>(ref this.isPersonalBest, value, nameof (IsPersonalBest));
    }

    public bool IsLowPowerGps
    {
      get => this.isLowPowerGps;
      set => this.SetProperty<bool>(ref this.isLowPowerGps, value, nameof (IsLowPowerGps));
    }

    public ArgbColor32 PathColor
    {
      get => this.pathColor;
      set => this.SetProperty<ArgbColor32>(ref this.pathColor, value, nameof (PathColor));
    }

    public bool UseMarkerForEnds
    {
      get => this.useMarkerForEnds;
      set => this.SetProperty<bool>(ref this.useMarkerForEnds, value, nameof (UseMarkerForEnds));
    }

    public bool UseSatelliteImages
    {
      get => this.useSatelliteImages;
      set => this.SetProperty<bool>(ref this.useSatelliteImages, value, nameof (UseSatelliteImages));
    }

    public override async Task AssignNameAsync()
    {
      try
      {
        this.IsBeingEdited = true;
        await this.routeBasedExerciseEventProvider.PatchEventAsync(this.Model.EventId, this.Name);
        this.DisplayNamingTextBox = false;
        this.messageSender.Send<EventChangedMessage>(new EventChangedMessage()
        {
          Event = (UserEvent) this.Model,
          Operation = EventOperation.Rename,
          Target = this.Model.EventType,
          IsRefreshCanceled = !this.HasHistory
        });
      }
      catch (Exception ex)
      {
        EventSummaryViewModelBase<TModel>.Logger.Error(ex, "Exception encountered during naming.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      finally
      {
        this.IsBeingEdited = false;
      }
    }

    public abstract ICommand OpenFullMapCommand { get; }
  }
}
