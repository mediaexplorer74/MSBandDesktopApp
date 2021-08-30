// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.IBestEventProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.App.Core.ViewModels.History;
using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public interface IBestEventProvider
  {
    Task<IList<BestEvent>> GetBestEventsAsync(GoalType goalType);

    Task<IList<HistoryEventViewModel<T>>> GetBestEventsDetailsAsync<T>(
      ICollection<string> eventIds)
      where T : UserEvent;

    Task<KeyValuePair<bool, string>> GetLabelForEventAsync(
      string eventId,
      EventType type);

    void PopulateAllEvents<T>(
      ICollection<HistoryEventViewModel<T>> items,
      ICollection<BestEvent> bests,
      ICollection<HistoryEventViewModel<T>> allEvents)
      where T : UserEvent;

    void CheckForBest(HistoryEventViewModelBase historyEvent, ICollection<BestEvent> bests);
  }
}
