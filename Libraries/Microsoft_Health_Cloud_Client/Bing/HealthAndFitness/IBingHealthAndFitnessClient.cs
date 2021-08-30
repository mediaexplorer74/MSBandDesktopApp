// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.IBingHealthAndFitnessClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  public interface IBingHealthAndFitnessClient
  {
    IHttpTracingConfiguration Configuration { get; }

    Task<WorkoutSearch> SearchWorkoutsAsync(
      WorkoutSearchOptions options,
      CancellationToken cancellationToken);

    Task<WorkoutPlanDetail> GetWorkoutAsync(
      string workoutId,
      CancellationToken cancellationToken);

    Task<string> GetVideoUrlAsync(
      string id,
      double screenWidth,
      double screenHeight,
      CancellationToken cancellationToken);

    Task<string> GetGolfIntroVideoUrlAsync(
      double screenWidth,
      double screenHeight,
      CancellationToken cancellationToken);
  }
}
