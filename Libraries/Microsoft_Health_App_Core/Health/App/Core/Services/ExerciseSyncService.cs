// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ExerciseSyncService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class ExerciseSyncService : IExerciseSyncService, IAppUpgradeListener
  {
    private const string ExerciseSettingsServiceCategory = "ExerciseSettingsService";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ExerciseSyncService.cs");
    public static readonly ConfigurationValue<bool> ExerciseListSyncNeededConfig = ConfigurationValue.CreateBoolean("ExerciseSettingsService", nameof (ExerciseListSyncNeeded), (Func<bool>) (() => true));
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IBandHardwareService bandHardwareService;
    private readonly IBandConnectionFactory bandConnectionFactory;
    private readonly IConfigurationService configurationService;

    public ExerciseSyncService(
      IHealthCloudClient healthCloudClient,
      IBandHardwareService bandHardwareService,
      IBandConnectionFactory bandConnectionFactory,
      IConfigurationService configurationService)
    {
      this.healthCloudClient = healthCloudClient;
      this.bandHardwareService = bandHardwareService;
      this.bandConnectionFactory = bandConnectionFactory;
      this.configurationService = configurationService;
    }

    public bool ExerciseListSyncNeeded
    {
      get => this.configurationService.GetValue<bool>(ExerciseSyncService.ExerciseListSyncNeededConfig);
      set => this.configurationService.SetValue<bool>((GenericConfigurationValue<bool>) ExerciseSyncService.ExerciseListSyncNeededConfig, value);
    }

    public async Task GetAndSyncExerciseTagsToBandAsync(CancellationToken token) => await this.SyncExerciseTagsToBandAsync(await this.healthCloudClient.GetExerciseTagsAsync(token), token);

    public async Task SaveExerciseTagsToCloudAsync(
      IList<ExerciseTag> tags,
      CancellationToken token)
    {
      await this.healthCloudClient.UpdateExerciseTagsAsync(tags, token);
    }

    public async Task SyncExerciseTagsToBandAsync(
      IList<ExerciseTag> tags,
      CancellationToken token)
    {
      if (await this.bandHardwareService.GetDeviceTypeAsync(token).ConfigureAwait(false) != BandClass.Envoy || tags == null)
        return;
      List<ExerciseTag> list = tags.Where<ExerciseTag>((Func<ExerciseTag, bool>) (tag => tag.IsChecked)).ToList<ExerciseTag>();
      if (list == null)
        return;
      IList<WorkoutActivity> workoutActivities = (IList<WorkoutActivity>) list.Select<ExerciseTag, WorkoutActivity>(new Func<ExerciseTag, WorkoutActivity>(ExerciseSyncService.ConvertToWorkoutActivity)).ToList<WorkoutActivity>();
      if (!token.IsCancellationRequested && workoutActivities != null)
      {
        using (IBandConnection cargoConnection = await this.bandConnectionFactory.CreateConnectionAsync(token))
          await cargoConnection.SetWorkoutActivitiesAsync(workoutActivities, token);
      }
      workoutActivities = (IList<WorkoutActivity>) null;
    }

    private static WorkoutActivity ConvertToWorkoutActivity(ExerciseTag exerciseTag) => new WorkoutActivity(exerciseTag.ExerciseTypeId, exerciseTag.Text)
    {
      Flags = exerciseTag.Flags,
      TrackingAlgorithmId = (byte) exerciseTag.Algorithm
    };

    Task IAppUpgradeListener.OnAppUpgradeAsync(
      Version newVersion,
      Version oldVersion,
      CancellationToken token)
    {
      this.ExerciseListSyncNeeded = true;
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
