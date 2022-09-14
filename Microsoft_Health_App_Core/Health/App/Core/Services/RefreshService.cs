// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.RefreshService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class RefreshService : IRefreshService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\RefreshService.cs");
    private static readonly string LastRefreshedTimeKey = "RefreshServiceLastRefreshedTime";
    private readonly IConfig config;
    private readonly IDateTimeService dateTimeService;
    private readonly Dictionary<WeakReference, Func<CancellationToken, Task>> subscriptions;

    public RefreshService(IConfig config, IDateTimeService dateTimeService)
    {
      this.config = config;
      this.dateTimeService = dateTimeService;
      this.subscriptions = new Dictionary<WeakReference, Func<CancellationToken, Task>>();
    }

    public void Subscribe(object subscriber, Func<CancellationToken, Task> callOnRefresh)
    {
      this.subscriptions.Add(new WeakReference(subscriber), callOnRefresh);
      RefreshService.Logger.Debug("<FLAG> object subcribed a refresh delegate (hashcode={0}, type={1})", (object) subscriber.GetHashCode(), (object) subscriber.GetType());
      if (this.subscriptions.Count <= 50)
        return;
      this.RemoveSubscriptionsFromDeadSubscribers();
    }

    public void Unsubscribe(object subscriber)
    {
      foreach (KeyValuePair<WeakReference, Func<CancellationToken, Task>> keyValuePair in new Dictionary<WeakReference, Func<CancellationToken, Task>>((IDictionary<WeakReference, Func<CancellationToken, Task>>) this.subscriptions))
      {
        if (keyValuePair.Key.IsAlive && keyValuePair.Key.Target == subscriber)
        {
          this.subscriptions.Remove(keyValuePair.Key);
          RefreshService.Logger.Debug("<FLAG> object unsubcribed (hashcode={0}, type={1})", (object) subscriber.GetHashCode(), (object) subscriber.GetType());
        }
      }
    }

    public async Task RefreshAsync(CancellationToken cancellationToken)
    {
      RefreshService.Logger.Debug("<START> calling refresh delegates for subscribers (totalSubscribed={0})", (object) this.subscriptions.Count);
      List<Task> tasks = new List<Task>();
      foreach (KeyValuePair<WeakReference, Func<CancellationToken, Task>> keyValuePair in new Dictionary<WeakReference, Func<CancellationToken, Task>>((IDictionary<WeakReference, Func<CancellationToken, Task>>) this.subscriptions))
      {
        if (keyValuePair.Key.IsAlive)
        {
          RefreshService.Logger.Debug("<FLAG> calling refresh delegate for object (hashcode={0}, type={1})", (object) keyValuePair.Key.Target.GetHashCode(), (object) keyValuePair.Key.Target.GetType());
          tasks.Add(keyValuePair.Value(cancellationToken));
        }
      }
      while (tasks.Count > 0)
      {
        cancellationToken.ThrowIfCancellationRequested();
        Task task = await Task.WhenAny((IEnumerable<Task>) tasks).ConfigureAwait(false);
        tasks.Remove(task);
      }
      this.config.LastCompletedRefresh = this.dateTimeService.Now;
      RefreshService.Logger.Debug("<END> calling refresh delegates for subscribers (totalSubscribed={0})", (object) this.subscriptions.Count);
    }

    public async void Refresh(CancellationToken cancellationToken) => await this.RefreshAsync(cancellationToken).ConfigureAwait(false);

    private void RemoveSubscriptionsFromDeadSubscribers()
    {
      RefreshService.Logger.Debug("<START> checking for dead subscribers (totalSubscribed={0})", (object) this.subscriptions.Count);
      foreach (KeyValuePair<WeakReference, Func<CancellationToken, Task>> keyValuePair in new Dictionary<WeakReference, Func<CancellationToken, Task>>((IDictionary<WeakReference, Func<CancellationToken, Task>>) this.subscriptions))
      {
        if (!keyValuePair.Key.IsAlive)
        {
          this.subscriptions.Remove(keyValuePair.Key);
          RefreshService.Logger.Debug((object) "<FLAG> removed subscription for dead subscriber");
        }
      }
      RefreshService.Logger.Debug("<END> checking for dead subscribers (totalSubscribed={0})", (object) this.subscriptions.Count);
    }
  }
}
