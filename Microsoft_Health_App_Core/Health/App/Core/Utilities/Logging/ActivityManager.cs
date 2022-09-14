// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Logging.ActivityManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities.Logging
{
  public class ActivityManager : IActivityManager
  {
    private const string ActivityStartedFormatString = "{0} has started...";
    private const string ActivityStartedWithParametersFormatString = "{0} has started... {1}";
    private const string ActivityCompletedFormatString = "{0} has completed.";
    private const string ActivityCompletedWithReturnFormatString = "{0} has completed. {1}";
    private const string ActivityFailedFormatString = "{0} has failed.";
    private readonly ILog log;

    public ActivityManager(ILog log) => this.log = log;

    public void Start(Activity activity, IDictionary<string, object> parameters = null)
    {
      if (!this.log.IsLevelEnabled(activity.LogLevel))
        return;
      if (parameters != null && parameters.Count > 0)
        this.log.LogFormat(activity.LogLevel, "{0} has started... {1}", (object) activity.Name, (object) parameters.ToDebugString<string, object>());
      else
        this.log.LogFormat(activity.LogLevel, "{0} has started...", (object) activity.Name);
    }

    public void Complete(Activity activity, IDictionary<string, object> parameters = null)
    {
      if (!this.log.IsLevelEnabled(activity.LogLevel))
        return;
      if (parameters != null && parameters.Count > 0)
        this.log.LogFormat(activity.LogLevel, "{0} has completed. {1}", (object) activity.Name, (object) parameters.ToDebugString<string, object>());
      else
        this.log.LogFormat(activity.LogLevel, "{0} has completed.", (object) activity.Name);
    }

    public void Fail(Activity activity)
    {
      if (!this.log.IsLevelEnabled(activity.LogLevel))
        return;
      this.log.LogFormat(activity.LogLevel, "{0} has failed.", (object) activity.Name);
    }

    public void RunAsActivity(Level loggingLevel, Func<string> name, Action action) => this.RunAsActivity(loggingLevel, name, (Func<IDictionary<string, object>>) null, action);

    public T RunAsActivity<T>(
      Level loggingLevel,
      Func<string> name,
      Func<T> function,
      bool logReturnValue)
    {
      return this.RunAsActivity<T>(loggingLevel, name, (Func<IDictionary<string, object>>) null, function, logReturnValue);
    }

    public Task RunAsActivityAsync(
      Level loggingLevel,
      Func<string> name,
      Func<Task> asyncFunction)
    {
      return this.RunAsActivityAsync(loggingLevel, name, (Func<IDictionary<string, object>>) null, asyncFunction);
    }

    public Task<T> RunAsActivityAsync<T>(
      Level loggingLevel,
      Func<string> name,
      Func<Task<T>> asyncFunction,
      bool logReturnValue)
    {
      return this.RunAsActivityAsync<T>(loggingLevel, name, (Func<IDictionary<string, object>>) null, asyncFunction, logReturnValue);
    }

    public void RunAsActivity(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Action action)
    {
      this.RunAsActivity<bool>(loggingLevel, name, parameters, (Func<bool>) (() =>
      {
        ActivityManager.TryRun(action);
        return true;
      }), false);
    }

    public Task RunAsActivityAsync(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Func<Task> asyncFunction)
    {
      return (Task) this.RunAsActivityAsync<bool>(loggingLevel, name, parameters, (Func<Task<bool>>) (async () =>
      {
        await ActivityManager.TryRunAsync(asyncFunction).ConfigureAwait(false);
        return true;
      }), false);
    }

    public T RunAsActivity<T>(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Func<T> function,
      bool logReturnValue)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (!this.log.IsLevelEnabled(loggingLevel))
        return function();
      Activity activity = new Activity(ActivityManager.TryGet(name), loggingLevel);
      this.Start(activity, ActivityManager.TryGet(parameters));
      T obj;
      try
      {
        obj = function();
      }
      catch (Exception ex)
      {
        this.Fail(activity);
        throw;
      }
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      if (logReturnValue)
        dictionary.Add("Return", (object) obj);
      this.Complete(activity, (IDictionary<string, object>) dictionary);
      return obj;
    }

    public async Task<T> RunAsActivityAsync<T>(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Func<Task<T>> asyncFunction,
      bool logReturnValue)
    {
      if (asyncFunction == null)
        throw new ArgumentNullException(nameof (asyncFunction));
      if (!this.log.IsLevelEnabled(loggingLevel))
        return await asyncFunction().ConfigureAwait(false);
      Activity activity = new Activity(ActivityManager.TryGet(name), loggingLevel);
      this.Start(activity, ActivityManager.TryGet(parameters));
      T returnValue;
      try
      {
        returnValue = await asyncFunction().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        this.Fail(activity);
        throw;
      }
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      if (logReturnValue)
        dictionary.Add("Return", (object) returnValue);
      this.Complete(activity, (IDictionary<string, object>) dictionary);
      return returnValue;
    }

    private static IDictionary<string, object> TryGet(
      Func<IDictionary<string, object>> activityValues)
    {
      try
      {
        IDictionary<string, object> dictionary = (IDictionary<string, object>) null;
        if (activityValues != null)
          dictionary = activityValues();
        return dictionary;
      }
      catch (Exception ex)
      {
        return (IDictionary<string, object>) new Dictionary<string, object>()
        {
          {
            "- failed to create this value string, see exception -",
            (object) ex
          }
        };
      }
    }

    private static string TryGet(Func<string> activityName)
    {
      try
      {
        string str = (string) null;
        if (activityName != null)
          str = activityName();
        return str;
      }
      catch (Exception ex)
      {
        return string.Format("- failed to create this activity title string, encountered the following exception: {0} -", new object[1]
        {
          (object) ex
        });
      }
    }

    private static Task TryRunAsync(Func<Task> func) => func != null ? func() : (Task) Task.FromResult<bool>(true);

    private static void TryRun(Action action)
    {
      if (action == null)
        return;
      action();
    }
  }
}
