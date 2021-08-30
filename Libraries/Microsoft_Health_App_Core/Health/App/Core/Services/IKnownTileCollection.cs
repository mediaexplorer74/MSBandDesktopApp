// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IKnownTileCollection
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services
{
  public interface IKnownTileCollection : IEnumerable<AppBandTile>, IEnumerable
  {
    AppBandTile StepsTile { get; }

    AppBandTile CaloriesTile { get; }

    AppBandTile MessagingTile { get; }

    AppBandTile MailTile { get; }

    AppBandTile CortanaTile { get; }

    AppBandTile PhoneTile { get; }

    AppBandTile CalendarTile { get; }

    AppBandTile RunTile { get; }

    AppBandTile BikeTile { get; }

    AppBandTile GolfTile { get; }

    AppBandTile ExerciseTile { get; }

    AppBandTile SleepTile { get; }

    AppBandTile AlarmTile { get; }

    AppBandTile GuidedWorkoutResultTile { get; }

    AppBandTile WeatherTile { get; }

    AppBandTile StocksTile { get; }

    AppBandTile UVTile { get; }

    AppBandTile StarbucksTile { get; }

    AppBandTile FacebookTile { get; }

    AppBandTile FBMessengerTile { get; }

    AppBandTile TwitterTile { get; }

    AppBandTile FeedTile { get; }

    AppBandTile WeightTile { get; }

    AppBandTile HikeTile { get; }

    IList<AppBandTile> DisabledTiles { get; }
  }
}
