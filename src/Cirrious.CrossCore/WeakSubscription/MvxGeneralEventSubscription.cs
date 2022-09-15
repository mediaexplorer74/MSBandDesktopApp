// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.WeakSubscription.MvxGeneralEventSubscription
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Reflection;

namespace Cirrious.CrossCore.WeakSubscription
{
  public class MvxGeneralEventSubscription : MvxWeakEventSubscription<object, EventArgs>
  {
    public MvxGeneralEventSubscription(
      object source,
      EventInfo eventInfo,
      EventHandler<EventArgs> eventHandler)
      : base(source, eventInfo, eventHandler)
    {
    }

    protected override Delegate CreateEventHandler() => (Delegate) new EventHandler(((MvxWeakEventSubscription<object, EventArgs>) this).OnSourceEvent);
  }
}
