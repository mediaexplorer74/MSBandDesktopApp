// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Navigation.ISmoothNavService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Navigation
{
  public interface ISmoothNavService
  {
    event EventHandler<NavigationEventArguments> Navigating;

    bool CanGoBack { get; }

    JournalEntry CurrentJournalEntry { get; }

    JournalEntry PopJournalEntry();

    JournalEntry PageWithIdentifier(Guid identifier);

    void Navigate(
      Type viewModelType,
      IDictionary<string, string> arguments = null,
      NavigationStackAction action = NavigationStackAction.None);

    void Navigate<T>(IDictionary<string, string> arguments = null, NavigationStackAction action = NavigationStackAction.None);

    void GoBack();

    void GoBack(int amount);

    void ClearBackStack(bool keepCurrentEntryInStack = true);

    void Reset(JournalEntry entry);

    void GoHome();

    bool IsNavPanelEnabled { get; }

    event EventHandler NavPanelEnabledStateChanged;

    void DisableNavPanel(Type source);

    void EnableNavPanel(Type source);

    JournalEntry PeekBack(int amount);

    int Depth { get; }
  }
}
