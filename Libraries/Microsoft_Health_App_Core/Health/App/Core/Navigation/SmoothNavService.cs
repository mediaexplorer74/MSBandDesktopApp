// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Navigation.SmoothNavService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Navigation
{
  public class SmoothNavService : ISmoothNavService
  {
    private const int ViewModelCacheSize = 5;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Navigation\\SmoothNavService.cs");
    private readonly IMessageSender messageSender;
    private readonly IDispatchService dispatchService;
    private readonly IPageFactory pageFactory;
    private readonly INavigationHandler navigationHandler;
    private readonly object navigationSyncRoot = new object();
    private readonly Stack<JournalEntry> navStack;
    private readonly List<Type> navPanelBlockers;
    private readonly object navPanelLock = new object();

    public event EventHandler NavPanelEnabledStateChanged;

    public SmoothNavService(
      IMessageSender messageSender,
      IDispatchService dispatchService,
      IPageFactory pageFactory,
      INavigationHandler navigationHandler)
    {
      Assert.ParamIsNotNull((object) messageSender, nameof (messageSender));
      Assert.ParamIsNotNull((object) dispatchService, nameof (dispatchService));
      Assert.ParamIsNotNull((object) pageFactory, nameof (pageFactory));
      Assert.ParamIsNotNull((object) navigationHandler, nameof (navigationHandler));
      this.messageSender = messageSender;
      this.dispatchService = dispatchService;
      this.pageFactory = pageFactory;
      this.navigationHandler = navigationHandler;
      this.navStack = new Stack<JournalEntry>();
      this.navPanelBlockers = new List<Type>();
      this.messageSender.Register<InvalidateAllViewModelsOfTypeInBackCacheMessage>((object) this, new Action<InvalidateAllViewModelsOfTypeInBackCacheMessage>(this.OnInvalidateAllViewModelsOfType));
    }

    public event EventHandler<NavigationEventArguments> Navigating;

    public void Navigate(
      Type viewModelType,
      IDictionary<string, string> arguments = null,
      NavigationStackAction action = NavigationStackAction.None)
    {
      this.dispatchService.RunOnUIThreadAsync((Action) (() => this.NavigateInternal(viewModelType, arguments, action)));
    }

    public void Navigate<T>(IDictionary<string, string> arguments = null, NavigationStackAction action = NavigationStackAction.None) => this.Navigate(typeof (T), arguments, action);

    private async void NavigateInternal(
      Type viewModelType,
      IDictionary<string, string> arguments,
      NavigationStackAction action)
    {
      if ((object) viewModelType == null)
        throw new ArgumentNullException(nameof (viewModelType));
      SmoothNavService.Logger.Debug((object) string.Format("<START> send navigation request to view associated model {0} (parameters=[{1}])", new object[2]
      {
        (object) viewModelType.Name,
        (object) LoggingUtilities.DictionaryToString(arguments)
      }));
      bool flag1 = false;
      JournalEntry nextJournalEntry = (JournalEntry) null;
      NavigationType navigationType = NavigationType.Forward;
      JournalEntry fromEntry = (JournalEntry) null;
      lock (this.navigationSyncRoot)
      {
        if (this.navStack.Count > 0)
        {
          fromEntry = this.navStack.Peek();
          flag1 = this.AreSameLocation(fromEntry.ViewModelType, fromEntry.Arguments, viewModelType, arguments);
        }
        if (flag1)
        {
          navigationType = NavigationType.Duplicate;
          nextJournalEntry = this.CurrentJournalEntry;
        }
        else
        {
          if (action == NavigationStackAction.RemovePrevious && this.navStack.Count > 0)
            this.navStack.Pop();
          nextJournalEntry = SmoothNavService.BuildJournalEntry(viewModelType, arguments);
        }
      }
      if (!flag1)
      {
        bool flag2 = this.navStack.Count > 0;
        if (flag2)
          flag2 = await this.TriggerChangeIfNeededAsync(nextJournalEntry, fromEntry);
        if (flag2)
          navigationType = NavigationType.Update;
        else
          this.LoadJournalEntry(nextJournalEntry);
        this.PushJournalEntry(nextJournalEntry);
      }
      this.navigationHandler.Navigate(nextJournalEntry, navigationType);
      EventHandler<NavigationEventArguments> navigating = this.Navigating;
      if (navigating != null)
        navigating((object) this, new NavigationEventArguments(navigationType));
      ApplicationTelemetry.LogPageView(nextJournalEntry.ViewModel);
      SmoothNavService.Logger.Debug("<END> send navigation request to view associated model {0}", (object) viewModelType.Name);
    }

    public void GoBack() => this.GoBack(1);

    public void GoBack(int amount) => this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () => await this.GoBackInternalAsync(amount)));

    private async Task GoBackInternalAsync(int amount)
    {
      if (!this.CanGoBack)
        return;
      JournalEntry fromEntry = (JournalEntry) null;
      JournalEntry destination = (JournalEntry) null;
      lock (this.navigationSyncRoot)
      {
        fromEntry = this.navStack.Pop();
        for (int index = 0; index < amount - 1 && this.navStack.Count > 1; ++index)
          fromEntry = this.navStack.Pop();
        destination = this.navStack.Peek();
      }
      destination.IsPageCached = true;
      if (!await this.TriggerChangeIfNeededAsync(destination, fromEntry) && destination.ViewModel == null)
      {
        destination.IsPageCached = false;
        this.LoadJournalEntry(destination);
      }
      SmoothNavService.Logger.Debug((object) string.Format("Navigated BACK to {0}", new object[1]
      {
        (object) destination.ViewModelType
      }));
      this.navigationHandler.Navigate(destination, NavigationType.Backward);
      EventHandler<NavigationEventArguments> navigating = this.Navigating;
      if (navigating != null)
        navigating((object) this, new NavigationEventArguments(NavigationType.Backward));
      ApplicationTelemetry.LogPageView(destination.ViewModel, true);
      destination = (JournalEntry) null;
    }

    public bool CanGoBack => this.navStack.Count > 1;

    public int Depth => this.navStack.Count;

    public void ClearBackStack(bool keepCurrentEntryInStack = true)
    {
      SmoothNavService.Logger.Debug((object) "<FLAG> clearing the back stack");
      lock (this.navigationSyncRoot)
      {
        JournalEntry journalEntry = !keepCurrentEntryInStack || this.navStack.Count <= 0 ? (JournalEntry) null : this.navStack.Pop();
        this.navStack.Clear();
        if (journalEntry == null)
          return;
        this.navStack.Push(journalEntry);
      }
    }

    public JournalEntry CurrentJournalEntry
    {
      get
      {
        lock (this.navigationSyncRoot)
        {
          if (this.navStack.Count > 0)
            return this.navStack.Peek();
        }
        return (JournalEntry) null;
      }
    }

    public JournalEntry PopJournalEntry()
    {
      lock (this.navigationSyncRoot)
        return this.navStack.Count > 0 ? this.navStack.Pop() : (JournalEntry) null;
    }

    public bool IsNavPanelEnabled => this.navPanelBlockers.Count == 0;

    public JournalEntry PageWithIdentifier(Guid id)
    {
      lock (this.navigationSyncRoot)
        return this.navStack.FirstOrDefault<JournalEntry>((Func<JournalEntry, bool>) (e => e.Id == id));
    }

    public void Reset(JournalEntry entry)
    {
      lock (this.navigationSyncRoot)
      {
        this.navStack.Clear();
        this.navStack.Push(entry);
      }
    }

    public void GoHome() => this.Navigate(typeof (TilesViewModel), (IDictionary<string, string>) null, NavigationStackAction.None);

    public void OnInvalidateAllViewModelsOfType(
      InvalidateAllViewModelsOfTypeInBackCacheMessage message)
    {
      lock (this.navigationSyncRoot)
      {
        SmoothNavService.Logger.Debug("<START> invalidating all back cache page stores of type (type={0})", (object) message.Type.FullName);
        foreach (JournalEntry nav in this.navStack)
        {
          if ((object) nav.ViewModelType == (object) message.Type)
            nav.ClearCachedData();
        }
        SmoothNavService.Logger.Debug("<END> invalidating all back cache page stores of type (type={0})", (object) message.Type.FullName);
      }
    }

    private bool AreSameLocation(
      Type viewModelTypeA,
      IDictionary<string, string> argumentsA,
      Type viewModelTypeB,
      IDictionary<string, string> argumentsB)
    {
      if ((object) viewModelTypeA != (object) viewModelTypeB)
        return false;
      if ((argumentsA == null || argumentsA.Count == 0) && (argumentsB == null || argumentsB.Count == 0))
        return true;
      return argumentsA != null && argumentsB != null && argumentsA.Count == argumentsB.Count && argumentsA.Keys.All<string>((Func<string, bool>) (key => argumentsB.ContainsKey(key) && argumentsA[key] == argumentsB[key]));
    }

    public void DisableNavPanel(Type source)
    {
      lock (this.navPanelLock)
      {
        if (this.navPanelBlockers.Contains(source))
          return;
        this.navPanelBlockers.Add(source);
        if (this.navPanelBlockers.Count != 1)
          return;
        EventHandler enabledStateChanged = this.NavPanelEnabledStateChanged;
        if (enabledStateChanged == null)
          return;
        enabledStateChanged((object) this, (EventArgs) null);
      }
    }

    public void EnableNavPanel(Type source)
    {
      bool isNavPanelEnabled = this.IsNavPanelEnabled;
      lock (this.navPanelLock)
      {
        if ((object) source == null)
          this.navPanelBlockers.Clear();
        else if (this.navPanelBlockers.Contains(source))
          this.navPanelBlockers.Remove(source);
      }
      if (isNavPanelEnabled == this.IsNavPanelEnabled)
        return;
      EventHandler enabledStateChanged = this.NavPanelEnabledStateChanged;
      if (enabledStateChanged == null)
        return;
      enabledStateChanged((object) this, (EventArgs) null);
    }

    public JournalEntry PeekBack(int amount)
    {
      lock (this.navigationSyncRoot)
        return this.navStack.ElementAtOrDefault<JournalEntry>(amount);
    }

    private static JournalEntry BuildJournalEntry(
      Type viewModelType,
      IDictionary<string, string> arguments)
    {
      return new JournalEntry()
      {
        Arguments = arguments,
        ViewModelType = viewModelType
      };
    }

    private void LoadJournalEntry(JournalEntry entry)
    {
      SmoothNavService.Logger.Debug((object) string.Format("Creating and loading view model of type {0}.", new object[1]
      {
        (object) entry.ViewModelType
      }));
      object instance = ServiceLocator.Current.GetInstance(entry.ViewModelType);
      if (instance is ILoadableParameters loadableParameters)
        loadableParameters.LoadAsync(entry.Arguments);
      entry.ViewModel = instance;
      entry.Page = this.pageFactory.GetPage(entry.ViewModelType);
    }

    private async Task<bool> TriggerChangeIfNeededAsync(
      JournalEntry toEntry,
      JournalEntry fromEntry)
    {
      if ((object) toEntry.ViewModelType == (object) fromEntry.ViewModelType)
      {
        PageViewModelBase viewModel = fromEntry.ViewModel as PageViewModelBase;
        SmoothNavService.Logger.Debug((object) string.Format("changing view model {0}.", new object[1]
        {
          (object) viewModel
        }));
        if (viewModel != null && viewModel.SupportsChanges)
        {
          toEntry.ViewModel = (object) viewModel;
          toEntry.Page = fromEntry.Page;
          await viewModel.ChangeAsync(toEntry.Arguments);
          return true;
        }
      }
      return false;
    }

    private void PushJournalEntry(JournalEntry entry)
    {
      lock (this.navigationSyncRoot)
      {
        SmoothNavService.Logger.Debug((object) string.Format("Pushing journal entry with view model type {0} on to the navigation stack.", new object[1]
        {
          (object) entry.ViewModelType
        }));
        this.navStack.Push(entry);
        if (this.navStack.Count <= 5)
          return;
        this.navStack.Take<JournalEntry>(6).Last<JournalEntry>()?.ClearCachedData();
      }
    }
  }
}
