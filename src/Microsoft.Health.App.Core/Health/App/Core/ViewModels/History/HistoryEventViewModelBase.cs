// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.History.HistoryEventViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;

namespace Microsoft.Health.App.Core.ViewModels.History
{
  public class HistoryEventViewModelBase
  {
    private readonly UserEvent eventModel;

    public HistoryEventViewModelBase(UserEvent userEvent) => this.eventModel = userEvent;

    public UserEvent Event => this.eventModel;

    public bool IsBest { get; set; }

    public EventType EventType => this.eventModel.EventType;

    public string EventId => this.eventModel.EventId;
  }
}
