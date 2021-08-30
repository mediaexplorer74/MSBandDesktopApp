// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.ExercisePendingTileSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.TileSettings
{
  public class ExercisePendingTileSettings : PendingTileSettings
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\TileSettings\\ExercisePendingTileSettings.cs");
    private readonly IExerciseSyncService exerciseSyncService;
    private readonly IHealthCloudClient healthCloudClient;
    private IList<ExerciseTag> exerciseTags;

    public ExercisePendingTileSettings(
      IExerciseSyncService exerciseSyncService,
      IHealthCloudClient healthCloudClient)
    {
      this.exerciseSyncService = exerciseSyncService;
      this.healthCloudClient = healthCloudClient;
    }

    public IList<ExerciseTag> ExerciseTags
    {
      get => this.exerciseTags;
      set
      {
        this.exerciseTags = value;
        this.IsChanged = true;
      }
    }

    public override async Task LoadSettingsAsync(CancellationToken token)
    {
      ExercisePendingTileSettings pendingTileSettings = this;
      IList<ExerciseTag> exerciseTags = pendingTileSettings.exerciseTags;
      IList<ExerciseTag> exerciseTagsAsync = await this.healthCloudClient.GetExerciseTagsAsync(token);
      pendingTileSettings.exerciseTags = exerciseTagsAsync;
      pendingTileSettings = (ExercisePendingTileSettings) null;
    }

    public override async Task ApplyChangesAsync()
    {
      await this.exerciseSyncService.SaveExerciseTagsToCloudAsync(this.ExerciseTags, CancellationToken.None);
      await this.exerciseSyncService.SyncExerciseTagsToBandAsync(this.ExerciseTags, CancellationToken.None);
    }
  }
}
