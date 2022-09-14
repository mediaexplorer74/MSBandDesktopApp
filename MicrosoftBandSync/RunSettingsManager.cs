// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.RunSettingsManager
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DesktopSyncApp
{
  public class RunSettingsManager : BikeRunBaseManager
  {
    private StartStripManager parent;
    private CargoRunDisplayMetrics originalRunMetrics;
    private CargoRunDisplayMetrics savedRunMetrics;
    public IList<RunDisplayMetricType> metricChoices;
    public IList<RunDisplayMetricType> metricChoicesWithNone;
    private IList<LabeledItem<RunDisplayMetricType>> metricComboOptions = (IList<LabeledItem<RunDisplayMetricType>>) new List<LabeledItem<RunDisplayMetricType>>();
    private IList<LabeledItem<RunDisplayMetricType>> metricComboOptionsWithNone = (IList<LabeledItem<RunDisplayMetricType>>) new List<LabeledItem<RunDisplayMetricType>>();

    public new event PropertyChangedEventHandler PropertyChanged;

    public RunSettingsManager(
      StartStripManager parent,
      BandClass deviceType,
      CargoRunDisplayMetrics metrics,
      DistanceUnitType distanceUnitType)
      : base(deviceType, -1, distanceUnitType)
    {
      this.parent = parent;
      this.SetRunMetrics(metrics);
    }

    public LabeledItem<RunDisplayMetricType> Metric1 { get; set; }

    public LabeledItem<RunDisplayMetricType> Metric2 { get; set; }

    public LabeledItem<RunDisplayMetricType> Metric3 { get; set; }

    public LabeledItem<RunDisplayMetricType> Metric4 { get; set; }

    public LabeledItem<RunDisplayMetricType> Metric5 { get; set; }

    public LabeledItem<RunDisplayMetricType> Metric6 { get; set; }

    public LabeledItem<RunDisplayMetricType> Metric7 { get; set; }

    public IList<LabeledItem<RunDisplayMetricType>> MetricComboOptions => this.metricComboOptions;

    public IList<LabeledItem<RunDisplayMetricType>> MetricComboOptionsWithNone => this.metricComboOptionsWithNone;

    public CargoRunDisplayMetrics RunDisplayMetrics
    {
      get
      {
        this.LoadSavedRunMetrics(this.savedRunMetrics);
        return this.savedRunMetrics;
      }
    }

    public void SetRunMetrics(CargoRunDisplayMetrics metrics)
    {
      this.originalRunMetrics = metrics.Clone();
      this.savedRunMetrics = metrics.Clone();
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Clear();
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Clear();
      if (this.DeviceType == 1)
      {
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 0, Strings.Settings_Run_DataPoint3));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 2, Strings.Settings_Run_DataPoint8));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 1, Strings.Settings_Run_DataPoint5));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 4, Strings.Settings_Run_DataPoint7));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 3, Strings.Settings_Run_DataPoint1));
      }
      else
      {
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 0, Strings.Settings_Run_DataPoint3));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 2, Strings.Settings_Run_DataPoint8));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 1, Strings.Settings_Run_DataPoint5));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 7, Strings.Settings_Run_DataPoint2));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 3, Strings.Settings_Run_DataPoint1));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 4, Strings.Settings_Run_DataPoint7));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 8, Strings.Settings_Run_DataPoint6));
        ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 6, Strings.Settings_Run_DataPoint4));
      }
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 5, Strings.Settings_Run_DataPoint0));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 0, Strings.Settings_Run_DataPoint3));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 2, Strings.Settings_Run_DataPoint8));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 1, Strings.Settings_Run_DataPoint5));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 7, Strings.Settings_Run_DataPoint2));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 3, Strings.Settings_Run_DataPoint1));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 4, Strings.Settings_Run_DataPoint7));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 8, Strings.Settings_Run_DataPoint6));
      ((ICollection<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Add(new LabeledItem<RunDisplayMetricType>((RunDisplayMetricType) 6, Strings.Settings_Run_DataPoint4));
      this.Metric1 = this.FindMetricChoice(metrics.PrimaryMetric);
      this.Metric2 = this.FindMetricChoice(metrics.TopLeftMetric);
      this.Metric3 = this.FindMetricChoice(metrics.TopRightMetric);
      this.Metric4 = this.FindMetricChoice(metrics.LeftDrawerMetric);
      this.Metric5 = this.FindMetricChoiceWithNone(metrics.RightDrawerMetric);
      this.Metric6 = this.FindMetricChoiceWithNone(metrics.Metric06);
      this.Metric7 = this.FindMetricChoiceWithNone(metrics.Metric07);
      this.RaiseRunMetricsPropertiesChanged();
    }

    public void SaveRunMetrics()
    {
      this.LoadSavedRunMetrics(this.savedRunMetrics);
      this.parent.NeedToSave = true;
      this.RaiseRunMetricsPropertiesChanged();
    }

    public void CancelRunMetrics()
    {
      this.Metric1 = this.FindMetricChoice(this.savedRunMetrics.PrimaryMetric);
      this.Metric2 = this.FindMetricChoice(this.savedRunMetrics.TopLeftMetric);
      this.Metric3 = this.FindMetricChoice(this.savedRunMetrics.TopRightMetric);
      this.Metric4 = this.FindMetricChoice(this.savedRunMetrics.LeftDrawerMetric);
      this.Metric5 = this.FindMetricChoiceWithNone(this.savedRunMetrics.RightDrawerMetric);
      this.Metric6 = this.FindMetricChoiceWithNone(this.savedRunMetrics.Metric06);
      this.Metric7 = this.FindMetricChoiceWithNone(this.savedRunMetrics.Metric07);
      this.RaiseRunMetricsPropertiesChanged();
    }

    private void RaiseRunMetricsPropertiesChanged()
    {
      this.OnPropertyChanged("Metric1", this.PropertyChanged);
      this.OnPropertyChanged("Metric2", this.PropertyChanged);
      this.OnPropertyChanged("Metric3", this.PropertyChanged);
      this.OnPropertyChanged("Metric4", this.PropertyChanged);
      this.OnPropertyChanged("Metric5", this.PropertyChanged);
      this.OnPropertyChanged("Metric6", this.PropertyChanged);
      this.OnPropertyChanged("Metric7", this.PropertyChanged);
    }

    private void LoadSavedRunMetrics(CargoRunDisplayMetrics loadMetrics)
    {
      if (this.DeviceType == 1)
      {
        List<RunDisplayMetricType> displayMetricTypeList1 = new List<RunDisplayMetricType>();
        displayMetricTypeList1.Add(this.Metric1.Value);
        displayMetricTypeList1.Add(this.Metric2.Value);
        displayMetricTypeList1.Add(this.Metric3.Value);
        List<RunDisplayMetricType> displayMetricTypeList2 = displayMetricTypeList1;
        List<RunDisplayMetricType> list = ((IEnumerable<RunDisplayMetricType>) new List<RunDisplayMetricType>((IEnumerable<RunDisplayMetricType>) Enum.GetValues(typeof (RunDisplayMetricType)))).Except<RunDisplayMetricType>((IEnumerable<RunDisplayMetricType>) displayMetricTypeList2).ToList<RunDisplayMetricType>();
        loadMetrics.PrimaryMetric = displayMetricTypeList2[0];
        loadMetrics.TopLeftMetric = displayMetricTypeList2[1];
        loadMetrics.TopRightMetric = displayMetricTypeList2[2];
        loadMetrics.LeftDrawerMetric = list[0];
        loadMetrics.RightDrawerMetric = list[1];
        loadMetrics.Metric06 = (RunDisplayMetricType) 5;
        loadMetrics.Metric07 = (RunDisplayMetricType) 5;
      }
      else
      {
        loadMetrics.PrimaryMetric = this.Metric1.Value;
        loadMetrics.TopLeftMetric = this.Metric2.Value;
        loadMetrics.TopRightMetric = this.Metric3.Value;
        loadMetrics.LeftDrawerMetric = this.Metric4.Value;
        loadMetrics.RightDrawerMetric = this.Metric5.Value;
        loadMetrics.Metric06 = this.Metric6.Value;
        loadMetrics.Metric07 = this.Metric7.Value;
      }
    }

    public bool RunMetricsChanged
    {
      get
      {
        if (this.originalRunMetrics == null)
          return false;
        List<RunDisplayMetricType> displayMetricTypeList1 = new List<RunDisplayMetricType>();
        displayMetricTypeList1.Add(this.Metric1.Value);
        displayMetricTypeList1.Add(this.Metric2.Value);
        displayMetricTypeList1.Add(this.Metric3.Value);
        displayMetricTypeList1.Add(this.Metric4.Value);
        displayMetricTypeList1.Add(this.Metric5.Value);
        displayMetricTypeList1.Add(this.Metric6.Value);
        displayMetricTypeList1.Add(this.Metric7.Value);
        List<RunDisplayMetricType> displayMetricTypeList2 = displayMetricTypeList1;
        IList<RunDisplayMetricType> runList = this.CreateRunList(this.originalRunMetrics);
        bool flag = false;
        for (int index = 0; index < 7; ++index)
        {
          if (displayMetricTypeList2[index] != runList[index])
          {
            flag = true;
            break;
          }
        }
        return flag;
      }
    }

    public bool ValidateRunMetrics()
    {
      bool flag = false;
      for (int index = 0; index < 7; ++index)
        this.errors[index] = false;
      if (this.originalRunMetrics == null)
        return !flag;
      CargoRunDisplayMetrics runDisplayMetrics = new CargoRunDisplayMetrics();
      this.LoadSavedRunMetrics(runDisplayMetrics);
      IList<RunDisplayMetricType> runList = this.CreateRunList(runDisplayMetrics);
      for (int index1 = 0; index1 < ((ICollection<RunDisplayMetricType>) runList).Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < ((ICollection<RunDisplayMetricType>) runList).Count; ++index2)
        {
          if (runList[index1] != 5 && runList[index1] == runList[index2])
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

    private LabeledItem<RunDisplayMetricType> FindMetricChoice(
      RunDisplayMetricType metricType)
    {
      return ((IEnumerable<LabeledItem<RunDisplayMetricType>>) this.metricComboOptions).Single<LabeledItem<RunDisplayMetricType>>((Func<LabeledItem<RunDisplayMetricType>, bool>) (c => c.Value == metricType));
    }

    private LabeledItem<RunDisplayMetricType> FindMetricChoiceWithNone(
      RunDisplayMetricType metricType)
    {
      return ((IEnumerable<LabeledItem<RunDisplayMetricType>>) this.metricComboOptionsWithNone).Single<LabeledItem<RunDisplayMetricType>>((Func<LabeledItem<RunDisplayMetricType>, bool>) (c => c.Value == metricType));
    }

    private IList<RunDisplayMetricType> CreateRunList(
      CargoRunDisplayMetrics runDisplayMetrics)
    {
      List<RunDisplayMetricType> displayMetricTypeList = new List<RunDisplayMetricType>();
      displayMetricTypeList.Add(runDisplayMetrics.PrimaryMetric);
      displayMetricTypeList.Add(runDisplayMetrics.TopLeftMetric);
      displayMetricTypeList.Add(runDisplayMetrics.TopRightMetric);
      displayMetricTypeList.Add(runDisplayMetrics.LeftDrawerMetric);
      displayMetricTypeList.Add(runDisplayMetrics.RightDrawerMetric);
      displayMetricTypeList.Add(runDisplayMetrics.Metric06);
      displayMetricTypeList.Add(runDisplayMetrics.Metric07);
      return (IList<RunDisplayMetricType>) displayMetricTypeList;
    }
  }
}
