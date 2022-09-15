// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Hike.ExpandableWaypointItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Hike
{
  public class ExpandableWaypointItemViewModel : HealthObservableObject
  {
    private bool visible = true;
    private bool expanded;
    private bool bottomLineVisible = true;
    private bool isUserWaypoint;
    private string waypointDesignator;
    private string waypointTitle;
    private string waypointSubtitle;
    private string waypointSubmetric;
    private string timeOfDayFormatted;
    private string distanceFormatted;
    private string waypointLatitudeSubmetric;
    private string waypointLongitudeSubmetric;
    private string waypointAltitudeGain;
    private string waypointAltitudeLoss;
    private string waypointClimbRate;
    private HealthCommand toggleExpandedCommand;
    private HealthCommand launchMapCommand;
    private HikeEventSequence current;

    public bool Visible
    {
      get => this.visible;
      set => this.SetProperty<bool>(ref this.visible, value, nameof (Visible));
    }

    public bool Expanded
    {
      get => this.expanded;
      set
      {
        this.SetProperty<bool>(ref this.expanded, value, nameof (Expanded));
        this.RaisePropertyChanged<string>((Expression<Func<string>>) (() => this.ExpandGlyph));
      }
    }

    public bool BottomLineVisible
    {
      get => this.bottomLineVisible;
      set => this.SetProperty<bool>(ref this.bottomLineVisible, value, nameof (BottomLineVisible));
    }

    public bool IsUserWaypoint
    {
      get => this.isUserWaypoint;
      set => this.SetProperty<bool>(ref this.isUserWaypoint, value, nameof (IsUserWaypoint));
    }

    public string ExpandGlyph => !this.Expanded ? "\uE100" : "\uE099";

    public ICommand ToggleExpandedCommand => (ICommand) this.toggleExpandedCommand ?? (ICommand) (this.toggleExpandedCommand = new HealthCommand((Action) (() => this.Expanded = !this.Expanded)));

    public ICommand LaunchMapCommand => (ICommand) this.launchMapCommand ?? (ICommand) (this.launchMapCommand = new HealthCommand((Action) (() =>
    {
      ILauncherService instance = ServiceLocator.Current.GetInstance<ILauncherService>();
      if (this.Current.Location == null)
        return;
      instance.MapGeoposition((double) this.Current.Location.Latitude, (double) this.Current.Location.Longitude, this.GetPoiTypeString(this.Current.PoiType));
    })));

    private string GetPoiTypeString(PointOfInterestType poiType)
    {
      switch (poiType)
      {
        case PointOfInterestType.UserGenerated:
          return AppResources.PointOfInterestUserGenerated;
        case PointOfInterestType.ElevationMax:
          return AppResources.PointOfInterestMaxElevation;
        case PointOfInterestType.ElevationMin:
          return AppResources.PointOfInterestMinElevation;
        case PointOfInterestType.TimeMidPoint:
          return AppResources.PointOfInterestMidpoint;
        case PointOfInterestType.PauseAuto:
          return AppResources.PointOfInterestPause;
        case PointOfInterestType.Sunrise:
          return AppResources.PointOfInterestSunrise;
        case PointOfInterestType.Sunset:
          return AppResources.PointOfInterestSunset;
        case PointOfInterestType.Start:
          return AppResources.PointOfInterestStart;
        case PointOfInterestType.End:
          return AppResources.PointOfInterestFinish;
        default:
          return string.Empty;
      }
    }

    public string Text { get; set; }

    public HikeEventSequence Current
    {
      get => this.current;
      set => this.SetProperty<HikeEventSequence>(ref this.current, value, nameof (Current));
    }

    public string TimeOfDayFormatted
    {
      get => this.timeOfDayFormatted;
      set => this.SetProperty<string>(ref this.timeOfDayFormatted, value, nameof (TimeOfDayFormatted));
    }

    public string WaypointDesignator
    {
      get => this.waypointDesignator;
      set => this.SetProperty<string>(ref this.waypointDesignator, value, nameof (WaypointDesignator));
    }

    public string WaypointSubtitle
    {
      get => this.waypointSubtitle;
      set => this.SetProperty<string>(ref this.waypointSubtitle, value, nameof (WaypointSubtitle));
    }

    public string WaypointSubmetric
    {
      get => this.waypointSubmetric;
      set => this.SetProperty<string>(ref this.waypointSubmetric, value, nameof (WaypointSubmetric));
    }

    public string WaypointTitle
    {
      get => this.waypointTitle;
      set => this.SetProperty<string>(ref this.waypointTitle, value, nameof (WaypointTitle));
    }

    public string WaypointLatitudeSubmetric
    {
      get => this.waypointLatitudeSubmetric;
      set => this.SetProperty<string>(ref this.waypointLatitudeSubmetric, value, nameof (WaypointLatitudeSubmetric));
    }

    public string WaypointLongitudeSubmetric
    {
      get => this.waypointLongitudeSubmetric;
      set => this.SetProperty<string>(ref this.waypointLongitudeSubmetric, value, nameof (WaypointLongitudeSubmetric));
    }

    public string WaypointAltitudeGain
    {
      get => this.waypointAltitudeGain;
      set => this.SetProperty<string>(ref this.waypointAltitudeGain, value, nameof (WaypointAltitudeGain));
    }

    public string WaypointAltitudeLoss
    {
      get => this.waypointAltitudeLoss;
      set => this.SetProperty<string>(ref this.waypointAltitudeLoss, value, nameof (WaypointAltitudeLoss));
    }

    public string WaypointClimbRate
    {
      get => this.waypointClimbRate;
      set => this.SetProperty<string>(ref this.waypointClimbRate, value, nameof (WaypointClimbRate));
    }

    public string DistanceFormatted
    {
      get => this.distanceFormatted;
      set => this.SetProperty<string>(ref this.distanceFormatted, value, nameof (DistanceFormatted));
    }
  }
}
