// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.IHistoryProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.ViewModels.History;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public interface IHistoryProvider
  {
    Task<IList<HistoryEventViewModel<T>>> GetEventHistoryAsync<T>(
      EventType eventType,
      int top,
      DateTimeOffset? beforeDate)
      where T : UserEvent;

    Task<IList<UsersGoal>> GetBestGoalsAsync(GoalType type);

    Task<HistoryEventViewModel<T>> GetHistoryItemAsync<T>(string eventId) where T : UserEvent;
  }
}
