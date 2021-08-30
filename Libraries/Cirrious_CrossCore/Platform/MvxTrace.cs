// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Platform.MvxTrace
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using System;

namespace Cirrious.CrossCore.Platform
{
  public class MvxTrace : MvxSingleton<IMvxTrace>, IMvxTrace
  {
    public static readonly DateTime WhenTraceStartedUtc = DateTime.UtcNow;
    private readonly IMvxTrace _realTrace;

    public static string DefaultTag { get; set; }

    public static void Initialize()
    {
      if (MvxSingleton<IMvxTrace>.Instance != null)
        throw new MvxException("MvxTrace already initialized");
      MvxTrace.DefaultTag = "mvx";
      MvxTrace mvxTrace = new MvxTrace();
    }

    public static void TaggedTrace(
      MvxTraceLevel level,
      string tag,
      string message,
      params object[] args)
    {
      if (MvxSingleton<IMvxTrace>.Instance == null)
        return;
      MvxSingleton<IMvxTrace>.Instance.Trace(level, tag, MvxTrace.PrependWithTime(message), args);
    }

    public static void TaggedTrace(MvxTraceLevel level, string tag, Func<string> message)
    {
      if (MvxSingleton<IMvxTrace>.Instance == null)
        return;
      MvxSingleton<IMvxTrace>.Instance.Trace(level, tag, MvxTrace.PrependWithTime(message));
    }

    public static void Trace(MvxTraceLevel level, string message, params object[] args)
    {
      if (MvxSingleton<IMvxTrace>.Instance == null)
        return;
      MvxSingleton<IMvxTrace>.Instance.Trace(level, MvxTrace.DefaultTag, MvxTrace.PrependWithTime(message), args);
    }

    public static void Trace(MvxTraceLevel level, Func<string> message)
    {
      if (MvxSingleton<IMvxTrace>.Instance == null)
        return;
      MvxSingleton<IMvxTrace>.Instance.Trace(level, MvxTrace.DefaultTag, MvxTrace.PrependWithTime(message));
    }

    public static void TaggedTrace(string tag, string message, params object[] args) => MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, tag, message, args);

    public static void TaggedWarning(string tag, string message, params object[] args) => MvxTrace.TaggedTrace(MvxTraceLevel.Warning, tag, message, args);

    public static void TaggedError(string tag, string message, params object[] args) => MvxTrace.TaggedTrace(MvxTraceLevel.Error, tag, message, args);

    public static void TaggedTrace(string tag, Func<string> message) => MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, tag, message);

    public static void TaggedWarning(string tag, Func<string> message) => MvxTrace.TaggedTrace(MvxTraceLevel.Warning, tag, message);

    public static void TaggedError(string tag, Func<string> message) => MvxTrace.TaggedTrace(MvxTraceLevel.Error, tag, message);

    public static void Trace(string message, params object[] args) => MvxTrace.Trace(MvxTraceLevel.Diagnostic, message, args);

    public static void Warning(string message, params object[] args) => MvxTrace.Trace(MvxTraceLevel.Warning, message, args);

    public static void Error(string message, params object[] args) => MvxTrace.Trace(MvxTraceLevel.Error, message, args);

    public static void Trace(Func<string> message) => MvxTrace.Trace(MvxTraceLevel.Diagnostic, message);

    public static void Warning(Func<string> message) => MvxTrace.Trace(MvxTraceLevel.Warning, message);

    public static void Error(Func<string> message) => MvxTrace.Trace(MvxTraceLevel.Error, message);

    public MvxTrace()
    {
      this._realTrace = Mvx.Resolve<IMvxTrace>();
      if (this._realTrace == null)
        throw new MvxException("No platform trace service available");
    }

    void IMvxTrace.Trace(MvxTraceLevel level, string tag, Func<string> message) => this._realTrace.Trace(level, tag, message);

    void IMvxTrace.Trace(MvxTraceLevel level, string tag, string message) => this._realTrace.Trace(level, tag, message);

    void IMvxTrace.Trace(
      MvxTraceLevel level,
      string tag,
      string message,
      params object[] args)
    {
      this._realTrace.Trace(level, tag, message, args);
    }

    private static string PrependWithTime(string input) => string.Format("{0,6:0.00} {1}", new object[2]
    {
      (object) (DateTime.UtcNow - MvxTrace.WhenTraceStartedUtc).TotalSeconds,
      (object) input
    });

    private static Func<string> PrependWithTime(Func<string> input) => (Func<string>) (() => MvxTrace.PrependWithTime(input()));
  }
}
