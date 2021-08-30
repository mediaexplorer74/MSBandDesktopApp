// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.HistoryProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.History;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class HistoryProvider : IHistoryProvider
  {
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IUserProfileService userProfileService;
    private readonly IFormattingService formatter;

    public HistoryProvider(
      IHealthCloudClient healthCloudClient,
      IUserProfileService userProfileService,
      IFormattingService formatterService)
    {
      this.healthCloudClient = healthCloudClient;
      this.userProfileService = userProfileService;
      this.formatter = formatterService;
    }

    public async Task<IList<HistoryEventViewModel<T>>> GetEventHistoryAsync<T>(
      EventType eventType,
      int top,
      DateTimeOffset? beforeDate)
      where T : UserEvent
    {
      return (IList<HistoryEventViewModel<T>>) (await this.healthCloudClient.GetEventsAsync<T>(eventType, top, beforeDate, (int) this.userProfileService.GetMileOrKilometerDistance().TotalCentimeters, CancellationToken.None)).Select<T, HistoryEventViewModel<T>>((Func<T, HistoryEventViewModel<T>>) (historyEvent => new HistoryEventViewModel<T>(historyEvent, this.formatter))).ToList<HistoryEventViewModel<T>>();
    }

    public Task<IList<UsersGoal>> GetBestGoalsAsync(GoalType type) => this.healthCloudClient.GetBestGoalsAsync(CancellationToken.None, type);

    public async Task<HistoryEventViewModel<T>> GetHistoryItemAsync<T>(
      string eventId)
      where T : UserEvent
    {
      T eventAsync = await this.healthCloudClient.GetEventAsync<T>(eventId, CancellationToken.None);
      return (object) eventAsync == null ? (HistoryEventViewModel<T>) null : new HistoryEventViewModel<T>(eventAsync, this.formatter);
    }
  }
}
