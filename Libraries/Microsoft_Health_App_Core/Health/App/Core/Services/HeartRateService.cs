// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.HeartRateService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class HeartRateService : IHeartRateService
  {
    private readonly IUserProfileService userProfileService;

    public HeartRateService(IUserProfileService userProfileService) => this.userProfileService = userProfileService;

    public int LastKnownEventMaximumHeartRate { get; private set; }

    public async Task<HeartRateZone> GetEventHeartRateZonesAsync(
      HRZones cloudHeartRateZones)
    {
      int? maxHr = cloudHeartRateZones.MaxHR;
      if (maxHr.HasValue)
      {
        maxHr = cloudHeartRateZones.MaxHR;
        this.LastKnownEventMaximumHeartRate = maxHr.Value;
        maxHr = cloudHeartRateZones.MaxHR;
        return new HeartRateZone(maxHr.Value);
      }
      int age = this.userProfileService.LastKnownAge;
      if (age < 0)
        age = (await this.userProfileService.GetUserProfileAsync(new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan).Token)).Birthdate.GetAge();
      return HeartRateZone.CreateHeartRateZoneWithAge(age);
    }
  }
}
