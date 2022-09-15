// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Logging.IActivityManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities.Logging
{
  public interface IActivityManager
  {
    void Start(Activity activity, IDictionary<string, object> parameters = null);

    void Complete(Activity activity, IDictionary<string, object> parameters = null);

    void Fail(Activity activity);

    void RunAsActivity(Level loggingLevel, Func<string> name, Action action);

    T RunAsActivity<T>(
      Level loggingLevel,
      Func<string> name,
      Func<T> function,
      bool logReturnValue);

    Task RunAsActivityAsync(Level loggingLevel, Func<string> name, Func<Task> asyncFunction);

    Task<T> RunAsActivityAsync<T>(
      Level loggingLevel,
      Func<string> name,
      Func<Task<T>> asyncFunction,
      bool logReturnValue);

    void RunAsActivity(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Action action);

    T RunAsActivity<T>(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Func<T> function,
      bool logReturnValue);

    Task RunAsActivityAsync(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Func<Task> asyncFunction);

    Task<T> RunAsActivityAsync<T>(
      Level loggingLevel,
      Func<string> name,
      Func<IDictionary<string, object>> parameters,
      Func<Task<T>> asyncFunction,
      bool logReturnValue);
  }
}
