// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.EventWaitHandlerService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Health.App.Core.Services
{
  public class EventWaitHandlerService : IEventWaitHandleService
  {
    private static Dictionary<string, EventWaitHandle> waitHandleMap = new Dictionary<string, EventWaitHandle>();

    public EventWaitHandle GetEventWaitHandle(bool initialState, EventResetMode mode) => new EventWaitHandle(initialState, mode);

    public EventWaitHandle GetEventWaitHandle(
      bool initialState,
      EventResetMode mode,
      string name)
    {
      if (string.IsNullOrEmpty(name))
        return this.GetEventWaitHandle(initialState, mode);
      lock (EventWaitHandlerService.waitHandleMap)
      {
        EventWaitHandle result = (EventWaitHandle) null;
        if (this.TryOpenExisting(name, out result))
          return result;
        EventWaitHandle eventWaitHandle = this.GetEventWaitHandle(initialState, mode);
        EventWaitHandlerService.waitHandleMap.Add(name, eventWaitHandle);
        return eventWaitHandle;
      }
    }

    public EventWaitHandle OpenExisting(string name)
    {
      EventWaitHandle result = (EventWaitHandle) null;
      if (!this.TryOpenExisting(name, out result))
        throw new WaitHandleCannotBeOpenedException(string.Format("No EventWaitHandler exists with name {0}", new object[1]
        {
          (object) name
        }));
      return result;
    }

    public bool TryOpenExisting(string name, out EventWaitHandle result)
    {
      Assert.ParamIsNotNullOrEmpty(name, nameof (name));
      lock (EventWaitHandlerService.waitHandleMap)
        return EventWaitHandlerService.waitHandleMap.TryGetValue(name, out result);
    }
  }
}
