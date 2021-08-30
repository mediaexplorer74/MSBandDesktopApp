// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Run.RunSplit
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;

namespace Microsoft.Health.App.Core.ViewModels.Run
{
  public class RunSplit
  {
    public string LapTime { get; set; }

    public SpeedMarker SpeedMarker { get; set; }

    public string SpeedGlyph => this.SpeedMarker.ToGlyph();

    public RunSummary SplitSensorData { get; set; }
  }
}
