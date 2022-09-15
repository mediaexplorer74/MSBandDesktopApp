// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.UpdateExerciseTileTask
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Sync
{
  [SyncTask(SyncTaskType.Foreground)]
  public class UpdateExerciseTileTask : IBandSyncTask
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Sync\\UpdateExerciseTileTask.cs");
    private readonly IConfig config;
    private readonly IExerciseSyncService exerciseSyncService;

    public UpdateExerciseTileTask(IConfig config, IExerciseSyncService exerciseSyncService)
    {
      this.config = config;
      this.exerciseSyncService = exerciseSyncService;
    }

    public async Task RunAsync(
      BandSyncContext context,
      CancellationToken cancellationToken,
      SyncDebugResult debugResults = null)
    {
      UpdateExerciseTileTask.Logger.Info((object) "<START> scheduled task : UpdateExerciseTileTask");
      try
      {
        if (!this.config.IsExerciseEnabled || cancellationToken.IsCancellationRequested || !this.exerciseSyncService.ExerciseListSyncNeeded)
          return;
        UpdateExerciseTileTask.Logger.Debug((object) "<START> Update exercise tile.");
        await this.exerciseSyncService.GetAndSyncExerciseTagsToBandAsync(cancellationToken);
        this.exerciseSyncService.ExerciseListSyncNeeded = false;
        UpdateExerciseTileTask.Logger.Debug((object) "<END> Update exercise tile.");
      }
      catch (Exception ex)
      {
        UpdateExerciseTileTask.Logger.Error((object) "<FAILED> UpdateExerciseTileTask.", ex);
        throw;
      }
      finally
      {
        UpdateExerciseTileTask.Logger.Info((object) "<END> scheduled task : UpdateExerciseTileTask");
      }
    }
  }
}
