// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.BandTileBandClassData
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models
{
  public class BandTileBandClassData : HealthObservableObject
  {
    private int defaultOrder;

    public BandTileBandClassData() => this.DefaultOrder = int.MaxValue;

    public int DefaultOrder
    {
      get => this.defaultOrder;
      set => this.SetProperty<int>(ref this.defaultOrder, value, nameof (DefaultOrder));
    }

    public IList<AppBandIcon> ExtraIcons { get; set; }
  }
}
