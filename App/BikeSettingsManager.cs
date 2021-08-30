// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BikeSettingsManager
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace DesktopSyncApp
{
  public class BikeSettingsManager : BikeRunBaseManager
  {
    private StartStripManager parent;
    private CargoBikeDisplayMetrics originalBikeMetrics;
    private CargoBikeDisplayMetrics savedBikeMetrics;
    public IList<BikeDisplayMetricType> metricChoices;
    public IList<BikeDisplayMetricType> metricChoicesWithNone;
    private IList<LabeledItem<BikeDisplayMetricType>> metricComboOptions = (IList<LabeledItem<BikeDisplayMetricType>>) new List<LabeledItem<BikeDisplayMetricType>>();
    private IList<LabeledItem<BikeDisplayMetricType>> metricComboOptionsWithNone = (IList<LabeledItem<BikeDisplayMetricType>>) new List<LabeledItem<BikeDisplayMetricType>>();
    private IList<LabeledItem<int>> splitComboOptions = (IList<LabeledItem<int>>) new List<LabeledItem<int>>();
    private int originalSplit;
    private int savedSplit;

    public new event PropertyChangedEventHandler PropertyChanged;

    public BikeSettingsManager(
      StartStripManager parent,
      BandClass deviceType,
      CargoBikeDisplayMetrics metrics,
      int split,
      DistanceUnitType distanceUnitType)
      : base(deviceType, split, distanceUnitType)
    {
      this.parent = parent;
      this.SetBikeMetrics(metrics, split);
    }

    public LabeledItem<BikeDisplayMetricType> Metric1 { get; set; }

    public LabeledItem<BikeDisplayMetricType> Metric2 { get; set; }

    public LabeledItem<BikeDisplayMetricType> Metric3 { get; set; }

    public LabeledItem<BikeDisplayMetricType> Metric4 { get; set; }

    public LabeledItem<BikeDisplayMetricType> Metric5 { get; set; }

    public LabeledItem<BikeDisplayMetricType> Metric6 { get; set; }

    public LabeledItem<BikeDisplayMetricType> Metric7 { get; set; }

    public LabeledItem<int> MetricSplit { get; set; }

    public IList<LabeledItem<BikeDisplayMetricType>> MetricComboOptions => this.metricComboOptions;

    public IList<LabeledItem<BikeDisplayMetricType>> MetricComboOptionsWithNone => this.metricComboOptionsWithNone;

    public IList<LabeledItem<int>> SplitComboOptions => this.splitComboOptions;

    public CargoBikeDisplayMetrics BikeDisplayMetrics
    {
      get
      {
        this.LoadSavedBikeMetrics(this.savedBikeMetrics);
        return this.savedBikeMetrics;
      }
    }

    public int Split
    {
      get
      {
        this.savedSplit = this.MetricSplit.Value;
        return this.savedSplit;
      }
    }

    public void SetBikeMetrics(CargoBikeDisplayMetrics metrics, int split)
    {
      this.originalSplit = split;
      this.savedSplit = split;
      this.originalBikeMetrics = metrics.Clone();
      this.savedBikeMetrics = metrics.Clone();
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Clear();
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Clear();
      ((ICollection<LabeledItem<int>>) this.splitComboOptions).Clear();
      if (this.DeviceType == 1)
      {
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 1, Strings.Settings_Bike_DataPoint3));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 3, Strings.Settings_Bike_DataPoint8));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 2, Strings.Settings_Bike_DataPoint5));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 5, Strings.Settings_Bike_DataPoint7));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 4, Strings.Settings_Bike_DataPoint1));
      }
      else
      {
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 1, Strings.Settings_Bike_DataPoint3));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 3, Strings.Settings_Bike_DataPoint8));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 2, Strings.Settings_Bike_DataPoint5));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 7, Strings.Settings_Bike_DataPoint2));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 4, Strings.Settings_Bike_DataPoint1));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 5, Strings.Settings_Bike_DataPoint7));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 8, Strings.Settings_Bike_DataPoint6));
        ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 6, Strings.Settings_Bike_DataPoint4));
      }
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 0, Strings.Settings_Bike_DataPoint0));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 1, Strings.Settings_Bike_DataPoint3));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 3, Strings.Settings_Bike_DataPoint8));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 2, Strings.Settings_Bike_DataPoint5));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 7, Strings.Settings_Bike_DataPoint2));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 4, Strings.Settings_Bike_DataPoint1));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 5, Strings.Settings_Bike_DataPoint7));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 8, Strings.Settings_Bike_DataPoint6));
      ((ICollection<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<BikeDisplayMetricType>((BikeDisplayMetricType) 6, Strings.Settings_Bike_DataPoint4));
      int[] numArray = new int[10]
      {
        1,
        2,
        3,
        4,
        5,
        10,
        15,
        20,
        25,
        50
      };
      foreach (int num in numArray)
      {
        string str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} {1}", (object) num, (object) this.GetDistanceUnit(this.distanceUnitType));
        ((ICollection<LabeledItem<int>>) this.splitComboOptions).Add(new LabeledItem<int>(num, str));
      }
      this.Metric1 = this.FindMetricChoice(metrics.PrimaryMetric);
      this.Metric2 = this.FindMetricChoice(metrics.TopLeftMetric);
      this.Metric3 = this.FindMetricChoice(metrics.TopRightMetric);
      this.Metric4 = this.FindMetricChoice(metrics.DrawerTopLeftMetric);
      this.Metric5 = this.FindMetricChoiceWithNone(metrics.DrawerBottomLeftMetric);
      this.Metric6 = this.FindMetricChoiceWithNone(metrics.DrawerBottomRightMetric);
      this.Metric7 = this.FindMetricChoiceWithNone(metrics.Metric07);
      this.MetricSplit = this.FindSplitChoice(split);
      this.RaiseBikeMetricPropertiesChanged();
    }

    public void SaveBikeMetrics()
    {
      this.savedSplit = this.MetricSplit.Value;
      this.LoadSavedBikeMetrics(this.savedBikeMetrics);
      this.parent.NeedToSave = true;
      this.RaiseBikeMetricPropertiesChanged();
    }

    public void CancelBikeMetrics()
    {
      this.MetricSplit = this.FindSplitChoice(this.savedSplit);
      this.Metric1 = this.FindMetricChoice(this.savedBikeMetrics.PrimaryMetric);
      this.Metric2 = this.FindMetricChoice(this.savedBikeMetrics.TopLeftMetric);
      this.Metric3 = this.FindMetricChoice(this.savedBikeMetrics.TopRightMetric);
      this.Metric4 = this.FindMetricChoice(this.savedBikeMetrics.DrawerTopLeftMetric);
      this.Metric5 = this.FindMetricChoiceWithNone(this.savedBikeMetrics.DrawerBottomLeftMetric);
      this.Metric6 = this.FindMetricChoiceWithNone(this.savedBikeMetrics.DrawerBottomRightMetric);
      this.Metric7 = this.FindMetricChoiceWithNone(this.savedBikeMetrics.Metric07);
      this.RaiseBikeMetricPropertiesChanged();
    }

    private void RaiseBikeMetricPropertiesChanged()
    {
      this.OnPropertyChanged("Metric1", this.PropertyChanged);
      this.OnPropertyChanged("Metric2", this.PropertyChanged);
      this.OnPropertyChanged("Metric3", this.PropertyChanged);
      this.OnPropertyChanged("Metric4", this.PropertyChanged);
      this.OnPropertyChanged("Metric5", this.PropertyChanged);
      this.OnPropertyChanged("Metric6", this.PropertyChanged);
      this.OnPropertyChanged("Metric7", this.PropertyChanged);
      this.OnPropertyChanged("MetricSplit", this.PropertyChanged);
    }

    private void LoadSavedBikeMetrics(CargoBikeDisplayMetrics loadMetrics)
    {
      if (this.DeviceType == 1)
      {
        List<BikeDisplayMetricType> displayMetricTypeList1 = new List<BikeDisplayMetricType>();
        displayMetricTypeList1.Add(this.Metric1.Value);
        displayMetricTypeList1.Add(this.Metric2.Value);
        displayMetricTypeList1.Add(this.Metric3.Value);
        List<BikeDisplayMetricType> displayMetricTypeList2 = displayMetricTypeList1;
        List<BikeDisplayMetricType> list = ((IEnumerable<BikeDisplayMetricType>) new List<BikeDisplayMetricType>((IEnumerable<BikeDisplayMetricType>) Enum.GetValues(typeof (BikeDisplayMetricType)))).Except<BikeDisplayMetricType>((IEnumerable<BikeDisplayMetricType>) displayMetricTypeList2).ToList<BikeDisplayMetricType>();
        list.Remove((BikeDisplayMetricType) 0);
        loadMetrics.PrimaryMetric = displayMetricTypeList2[0];
        loadMetrics.TopLeftMetric = displayMetricTypeList2[1];
        loadMetrics.TopRightMetric = displayMetricTypeList2[2];
        loadMetrics.DrawerTopLeftMetric = list[0];
        loadMetrics.DrawerBottomLeftMetric = list[1];
        loadMetrics.DrawerBottomRightMetric = (BikeDisplayMetricType) 0;
        loadMetrics.Metric07 = (BikeDisplayMetricType) 0;
      }
      else
      {
        loadMetrics.PrimaryMetric = this.Metric1.Value;
        loadMetrics.TopLeftMetric = this.Metric2.Value;
        loadMetrics.TopRightMetric = this.Metric3.Value;
        loadMetrics.DrawerTopLeftMetric = this.Metric4.Value;
        loadMetrics.DrawerBottomLeftMetric = this.Metric5.Value;
        loadMetrics.DrawerBottomRightMetric = this.Metric6.Value;
        loadMetrics.Metric07 = this.Metric7.Value;
      }
    }

    public bool SplitChanged => this.originalSplit != 0 && this.originalSplit != this.Split;

    public bool BikeMetricsChanged
    {
      get
      {
        if (this.originalBikeMetrics == null)
          return false;
        List<BikeDisplayMetricType> displayMetricTypeList1 = new List<BikeDisplayMetricType>();
        displayMetricTypeList1.Add(this.Metric1.Value);
        displayMetricTypeList1.Add(this.Metric2.Value);
        displayMetricTypeList1.Add(this.Metric3.Value);
        displayMetricTypeList1.Add(this.Metric4.Value);
        displayMetricTypeList1.Add(this.Metric5.Value);
        displayMetricTypeList1.Add(this.Metric6.Value);
        displayMetricTypeList1.Add(this.Metric7.Value);
        List<BikeDisplayMetricType> displayMetricTypeList2 = displayMetricTypeList1;
        IList<BikeDisplayMetricType> bikeList = this.CreateBikeList(this.originalBikeMetrics);
        bool flag = false;
        for (int index = 0; index < 7; ++index)
        {
          if (displayMetricTypeList2[index] != bikeList[index])
          {
            flag = true;
            break;
          }
        }
        return flag;
      }
    }

    public bool ValidateBikeMetrics()
    {
      bool flag = false;
      for (int index = 0; index < 7; ++index)
        this.errors[index] = false;
      if (this.originalBikeMetrics == null)
        return !flag;
      CargoBikeDisplayMetrics bikeDisplayMetrics = new CargoBikeDisplayMetrics();
      this.LoadSavedBikeMetrics(bikeDisplayMetrics);
      IList<BikeDisplayMetricType> bikeList = this.CreateBikeList(bikeDisplayMetrics);
      for (int index1 = 0; index1 < ((ICollection<BikeDisplayMetricType>) bikeList).Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < ((ICollection<BikeDisplayMetricType>) bikeList).Count; ++index2)
        {
          if (bikeList[index1] != null && bikeList[index1] == bikeList[index2])
          {
            flag = true;
            this.errors[index1] = true;
            this.errors[index2] = true;
          }
        }
      }
      this.RaiseMetricErrorsPropertiesChanged();
      return !flag;
    }

    private LabeledItem<BikeDisplayMetricType> FindMetricChoice(
      BikeDisplayMetricType metricType)
    {
      return ((IEnumerable<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptions).Single<LabeledItem<BikeDisplayMetricType>>((Func<LabeledItem<BikeDisplayMetricType>, bool>) (c => c.Value == metricType));
    }

    private LabeledItem<BikeDisplayMetricType> FindMetricChoiceWithNone(
      BikeDisplayMetricType metricType)
    {
      return ((IEnumerable<LabeledItem<BikeDisplayMetricType>>) this.metricComboOptionsWithNone).Single<LabeledItem<BikeDisplayMetricType>>((Func<LabeledItem<BikeDisplayMetricType>, bool>) (c => c.Value == metricType));
    }

    private LabeledItem<int> FindSplitChoice(int splitValue) => ((IEnumerable<LabeledItem<int>>) this.splitComboOptions).Single<LabeledItem<int>>((Func<LabeledItem<int>, bool>) (c => c.Value == splitValue));

    private IList<BikeDisplayMetricType> CreateBikeList(
      CargoBikeDisplayMetrics bikeDisplayMetrics)
    {
      List<BikeDisplayMetricType> displayMetricTypeList = new List<BikeDisplayMetricType>();
      displayMetricTypeList.Add(bikeDisplayMetrics.PrimaryMetric);
      displayMetricTypeList.Add(bikeDisplayMetrics.TopLeftMetric);
      displayMetricTypeList.Add(bikeDisplayMetrics.TopRightMetric);
      displayMetricTypeList.Add(bikeDisplayMetrics.DrawerTopLeftMetric);
      displayMetricTypeList.Add(bikeDisplayMetrics.DrawerBottomLeftMetric);
      displayMetricTypeList.Add(bikeDisplayMetrics.DrawerBottomRightMetric);
      displayMetricTypeList.Add(bikeDisplayMetrics.Metric07);
      return (IList<BikeDisplayMetricType>) displayMetricTypeList;
    }
  }
}
