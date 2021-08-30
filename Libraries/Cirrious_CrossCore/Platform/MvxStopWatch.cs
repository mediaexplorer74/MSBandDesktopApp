// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Platform.MvxStopWatch
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Platform
{
  public class MvxStopWatch : IDisposable
  {
    private readonly string _message;
    private readonly int _startTickCount;
    private readonly string _tag;

    private MvxStopWatch(string tag, string text, params object[] args)
    {
      this._tag = tag;
      this._startTickCount = Environment.TickCount;
      this._message = string.Format(text, args);
    }

    public void Dispose()
    {
      MvxTrace.TaggedTrace(this._tag, "{0} - {1}", (object) (Environment.TickCount - this._startTickCount), (object) this._message);
      GC.SuppressFinalize((object) this);
    }

    public static MvxStopWatch CreateWithTag(
      string tag,
      string text,
      params object[] args)
    {
      return new MvxStopWatch(tag, text, args);
    }

    public static MvxStopWatch Create(string text, params object[] args) => MvxStopWatch.CreateWithTag("mvxStopWatch", text, args);
  }
}
