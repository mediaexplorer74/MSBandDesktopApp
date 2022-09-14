// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.WeightSensor
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Practices.ServiceLocation;
using System;

namespace Microsoft.Health.App.Core.Providers
{
  public class WeightSensor
  {
    public WeightSensor(DateTimeOffset timestamp, Weight weight)
    {
      this.Timestamp = timestamp;
      this.Weight = weight;
    }

    public bool IsCalculated { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public Weight Weight { get; set; }

    public string FormattedTimestamp => Formatter.FormatTimeWithSingleCharacterAMOrPM(this.Timestamp, true);

    public string FormattedWeight => (string) ServiceLocator.Current.GetInstance<IFormattingService>().FormatStat((object) this.Weight, StatValueType.Weight);

    public static WeightSensor FromCloudModel(Microsoft.Health.Cloud.Client.WeightSensor weightSensor) => new WeightSensor(weightSensor.TimeStamp.ToLocalTime(), Weight.FromGrams((double) weightSensor.Weight));

    public static Microsoft.Health.Cloud.Client.WeightSensor ToCloudModel(
      WeightSensor weightSensor)
    {
      return new Microsoft.Health.Cloud.Client.WeightSensor()
      {
        TimeStamp = (DateTimeOffset) weightSensor.Timestamp.UtcDateTime,
        Weight = (uint) weightSensor.Weight.TotalGrams
      };
    }
  }
}
