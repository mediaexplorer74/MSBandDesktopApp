// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.FullMapViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class FullMapViewModelBase<T> : PageViewModelBase where T : RouteBasedExerciseEvent
  {
    internal const string IdKey = "ID";
    internal const string SplitGroupSizeKey = "SplitGroupSize";
    internal const string IsLowPowerGpsKey = "IsLowPowerGps";
    private readonly ISmoothNavService navService;
    private readonly IRouteBasedExerciseEventProvider<T> eventProvider;
    private HealthCommand exitCommand;
    private string id;
    private bool initialized;
    private int splitGroupSize;
    private IList<MapPoint> mapPoints;
    private bool isLowPowerGps;
    private ArgbColor32 pathColor;
    private bool useMarkerForEnds;
    private bool useSatelliteImages;

    protected FullMapViewModelBase(
      INetworkService networkService,
      ISmoothNavService navService,
      IRouteBasedExerciseEventProvider<T> eventProvider,
      ArgbColor32 pathColor = null,
      bool useMarkersForEnds = false,
      bool useSatelliteImages = false)
      : base(networkService)
    {
      this.navService = navService;
      this.splitGroupSize = 1;
      this.eventProvider = eventProvider;
      this.pathColor = pathColor;
      this.useMarkerForEnds = useMarkersForEnds;
      this.useSatelliteImages = useSatelliteImages;
    }

    public IList<MapPoint> MapPoints
    {
      get => this.mapPoints;
      private set => this.SetProperty<IList<MapPoint>>(ref this.mapPoints, value, nameof (MapPoints));
    }

    public bool Initialized
    {
      get => this.initialized;
      set => this.SetProperty<bool>(ref this.initialized, value, nameof (Initialized));
    }

    public int SplitGroupSize
    {
      get => this.splitGroupSize;
      set => this.SetProperty<int>(ref this.splitGroupSize, value, nameof (SplitGroupSize));
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

    public ICommand ExitCommand => (ICommand) this.exitCommand ?? (ICommand) (this.exitCommand = new HealthCommand((Action) (() => this.navService.GoBack())));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters == null || !parameters.TryGetValue("ID", out this.id))
        throw new NoDataException();
      this.MapPoints = (await this.eventProvider.GetEventAsync(this.id)).MapPoints;
      if (parameters.ContainsKey("SplitGroupSize"))
        this.SplitGroupSize = int.Parse(parameters["SplitGroupSize"]);
      if (parameters.ContainsKey("IsLowPowerGps"))
        this.IsLowPowerGps = bool.Parse(parameters["IsLowPowerGps"]);
      this.Initialized = true;
    }
  }
}
