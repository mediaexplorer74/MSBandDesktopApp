// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.RouteBasedExerciseEventProviderBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public abstract class RouteBasedExerciseEventProviderBase<T> : IRouteBasedExerciseEventProvider<T>
    where T : RouteBasedExerciseEvent
  {
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IUserProfileService userProfileService;
    private readonly EventType eventType;

    public RouteBasedExerciseEventProviderBase(
      IHealthCloudClient healthCloudClient,
      IUserProfileService userProfileService,
      EventType eventType)
    {
      this.healthCloudClient = healthCloudClient;
      this.userProfileService = userProfileService;
      this.eventType = eventType;
    }

    public Task<IList<T>> GetEventsAsync(
      DateTimeOffset startDayId,
      DateTimeOffset endDayId)
    {
      return this.healthCloudClient.GetEventsBetweenAsync<T>(startDayId, endDayId, this.eventType, CancellationToken.None);
    }

    public Task<IList<T>> GetTopEventsAsync(int count) => this.healthCloudClient.GetTopEventsAsync<T>(count, this.eventType, CancellationToken.None);

    public async Task DeleteEventAsync(string eventId) => await this.healthCloudClient.DeleteEventAsync(eventId, this.eventType, CancellationToken.None);

    public async Task PatchEventAsync(string eventId, string name) => await this.healthCloudClient.PatchEventAsync(eventId, name, this.eventType, CancellationToken.None);

    public async Task<T> GetLastEventAsync() => (await this.GetTopEventsAsync(1)).FirstOrDefault<T>();

    public async Task<T> GetEventAsync(string eventId) => await this.healthCloudClient.GetRouteBasedExerciseEventDetailsAsync<T>(eventId, (int) this.userProfileService.GetMileOrKilometerDistance().TotalCentimeters, CancellationToken.None);
  }
}
