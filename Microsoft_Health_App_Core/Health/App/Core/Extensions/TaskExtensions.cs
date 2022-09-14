// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.TaskExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class TaskExtensions
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Extensions\\TaskExtensions.cs");

    public static void IgnoreException(this Task task, string errorMessage, [CallerMemberName] string callerName = null) => task.ContinueWith((Action<Task>) (_ => TaskExtensions.LogException((Exception) task.Exception, errorMessage, callerName)), TaskContinuationOptions.OnlyOnFaulted);

    private static void LogException(Exception exception, string errorMessage, string callerName = null) => TaskExtensions.Logger.Warn(exception, "<WARNING> Task failed: {0} {1}", (object) (callerName ?? "Unknown"), (object) errorMessage);
  }
}
