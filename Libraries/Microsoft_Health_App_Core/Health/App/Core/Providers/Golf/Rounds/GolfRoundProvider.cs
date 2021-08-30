// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Rounds.GolfRoundProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Events.Golf;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers.Golf.Rounds
{
  internal sealed class GolfRoundProvider : IGolfRoundProvider
  {
    private readonly IGolfClient golfClient;

    public GolfRoundProvider(IGolfClient golfClient)
    {
      Assert.ParamIsNotNull((object) golfClient, nameof (golfClient));
      this.golfClient = golfClient;
    }

    public async Task<GolfRound> GetLastRoundAsync(CancellationToken token)
    {
      GolfEvent golfEvent = (await this.golfClient.GetTopEventsAsync(1, token, true).ConfigureAwait(false)).FirstOrDefault<GolfEvent>();
      return golfEvent == null ? (GolfRound) null : GolfRoundProvider.CreateRoundFromEvent(golfEvent);
    }

    public async Task<GolfRound> GetRoundAsync(string eventId, CancellationToken token)
    {
      GolfEvent eventAsync = await this.golfClient.GetEventAsync(eventId, CancellationToken.None, true, true, true);
      return eventAsync == null ? (GolfRound) null : GolfRoundProvider.CreateRoundFromEvent(eventAsync);
    }

    public async Task RenameRoundAsync(string eventId, string name, CancellationToken token) => await this.golfClient.RenameGolfEventAsync(eventId, name, token);

    private static GolfRound CreateRoundFromEvent(GolfEvent golfEvent) => new GolfRound(golfEvent.CourseName, golfEvent.Name, golfEvent.StartTime, golfEvent.TotalScore, golfEvent.TotalScore - golfEvent.ParForHolesPlayed, golfEvent.ParOrBetterCount, golfEvent.LongestDrive, golfEvent.PaceOfPlay, golfEvent.TotalStepCount, golfEvent.TotalDistanceWalkedInCm, golfEvent.AverageHeartRate, golfEvent.PeakHeartRate, golfEvent.LowestHeartRate, golfEvent.TeeNameSelected, golfEvent.Sequences.Select<GolfEventHoleSequence, GolfRoundHole>((Func<GolfEventHoleSequence, GolfRoundHole>) (sequence => GolfRoundProvider.CreateHoleFromSequence(sequence))), golfEvent);

    private static GolfRoundHole CreateHoleFromSequence(
      GolfEventHoleSequence holeSequence)
    {
      bool flag = (uint) holeSequence.UserScore > 0U;
      return new GolfRoundHole(holeSequence.HoleNumber, holeSequence.HolePar, holeSequence.DistanceToPin, holeSequence.HoleDifficultyIndex != 0 ? new int?(holeSequence.HoleDifficultyIndex) : new int?(), flag ? new int?(holeSequence.UserScore) : new int?(), flag ? new int?(holeSequence.UserScore - holeSequence.HolePar) : new int?(), flag ? new TimeSpan?(holeSequence.Duration) : new TimeSpan?(), flag ? new Length?(holeSequence.DistanceWalked) : new Length?(), flag ? new int?(holeSequence.StepCount) : new int?(), flag ? new int?(holeSequence.CaloriesBurned) : new int?(), flag ? new int?(holeSequence.LowestHeartRate) : new int?(), flag ? new int?(holeSequence.PeakHeartRate) : new int?(), flag ? new int?(holeSequence.AverageHeartRate) : new int?(), holeSequence.HoleShotOverlayImageUrl);
    }
  }
}
