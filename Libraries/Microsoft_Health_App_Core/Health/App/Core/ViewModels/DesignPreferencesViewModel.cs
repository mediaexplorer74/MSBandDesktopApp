// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.DesignPreferencesViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Utilities;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class DesignPreferencesViewModel
  {
    private IList<LabeledItem<DistanceUnitType>> distanceUnitTypesList = LabeledItem<DistanceUnitType>.FromEnumValues();
    private IList<LabeledItem<TemperatureUnitType>> temperatureUnitTypesList = LabeledItem<TemperatureUnitType>.FromEnumValues();
    private IList<LabeledItem<MassUnitType>> massUnitTypesList = LabeledItem<MassUnitType>.FromEnumValues();

    public IList<LabeledItem<DistanceUnitType>> DistanceUnitTypes => this.distanceUnitTypesList;

    public IList<LabeledItem<TemperatureUnitType>> TemperatureUnitTypes => this.temperatureUnitTypesList;

    public IList<LabeledItem<MassUnitType>> MassUnitTypes => this.massUnitTypesList;

    public bool ShowGettingProfileProgress => false;

    public bool ShowSavingProfileProgress => false;

    public bool ShowPreferencesPanel => true;

    public LabeledItem<MassUnitType> SelectedMassUnitType => this.MassUnitTypes[0];

    public LabeledItem<TemperatureUnitType> SelectedTemperatureUnitType => this.TemperatureUnitTypes[0];

    public LabeledItem<DistanceUnitType> SelectedDistanceUnitType => this.DistanceUnitTypes[0];

    public bool IsLiveTileEnabled => true;
  }
}
