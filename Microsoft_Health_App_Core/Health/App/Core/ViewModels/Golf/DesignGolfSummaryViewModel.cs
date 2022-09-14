// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.DesignGolfSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class DesignGolfSummaryViewModel
  {
    public int CaloriesBurned => 640;

    public TimeSpan Duration => TimeSpan.FromHours(8.25);

    public DateTimeOffset StartTime => new DateTimeOffset(2015, 3, 15, 8, 36, 48, TimeSpan.FromHours(7.0));

    public Length LongestDrive => Length.FromMiles(0.4);

    public int ParOrBetter => 4;

    public Length Distance => Length.FromMiles(3.6);

    public TimeSpan PlayPace => TimeSpan.FromHours(1.0);

    public int Steps => 12000;

    public int AverageHeartRate => 70;

    public int PeakHeartRate => 120;

    public int LowestHeartRate => 60;

    public bool EnableShare => true;

    public bool DisplayNamingTextBox => true;

    public string Name => "Best Golf Ever";

    public string GolfNameLabel => string.IsNullOrEmpty(this.Name) ? AppResources.PanelNameButtonText : AppResources.PanelRenameButtonText;
  }
}
