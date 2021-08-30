// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.ExerciseProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class ExerciseProvider : IExerciseProvider
  {
    private readonly IHealthCloudClient healthCloudClient;

    public ExerciseProvider(IHealthCloudClient healthCloudClient) => this.healthCloudClient = healthCloudClient;

    public Task<IList<ExerciseEvent>> GetExerciseEventsAsync(
      DateTimeOffset startDayId,
      DateTimeOffset endDayId)
    {
      return this.healthCloudClient.GetEventsBetweenAsync<ExerciseEvent>(startDayId, endDayId, EventType.Workout, CancellationToken.None);
    }

    public Task<IList<ExerciseEvent>> GetTopExerciseEventsAsync(
      int count,
      bool expand = false)
    {
      IHealthCloudClient healthCloudClient = this.healthCloudClient;
      int count1 = count;
      CancellationToken none = CancellationToken.None;
      bool flag = expand;
      int num1 = expand ? 1 : 0;
      int num2 = flag ? 1 : 0;
      int num3 = expand ? 1 : 0;
      return healthCloudClient.GetTopEventsAsync<ExerciseEvent>(count1, EventType.Workout, none, num1 != 0, num2 != 0, num3 != 0);
    }

    public async Task DeleteExerciseEventAsync(string eventId) => await this.healthCloudClient.DeleteEventAsync(eventId, EventType.Workout, CancellationToken.None);

    public async Task PatchExerciseEventAsync(string eventId, string name) => await this.healthCloudClient.PatchEventAsync(eventId, name, EventType.Workout, CancellationToken.None);

    public async Task<ExerciseEvent> GetLastExerciseEventAsync(bool expand = false)
    {
      IList<ExerciseEvent> exerciseEventsAsync = await this.GetTopExerciseEventsAsync(1, expand);
      return exerciseEventsAsync == null || exerciseEventsAsync.Count <= 0 ? (ExerciseEvent) null : exerciseEventsAsync[0];
    }

    public async Task<ExerciseEvent> GetExerciseEventAsync(string eventId) => await this.healthCloudClient.GetExerciseEventDetailsAsync(eventId, CancellationToken.None);
  }
}
