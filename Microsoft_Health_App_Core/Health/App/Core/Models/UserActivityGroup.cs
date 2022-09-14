// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.UserActivityGroup
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models
{
  public class UserActivityGroup
  {
    public IList<UserActivity> UserActivities { get; set; }

    public ActivityPeriod ActivityPeriod { get; set; }

    public IList<Sample> StepsSamples { get; set; }

    public IList<Sample> CaloriesSamples { get; set; }

    public IList<Sample> HeartRateSamples { get; set; }

    public IList<Sample> UvExposureSamples { get; set; }
  }
}
