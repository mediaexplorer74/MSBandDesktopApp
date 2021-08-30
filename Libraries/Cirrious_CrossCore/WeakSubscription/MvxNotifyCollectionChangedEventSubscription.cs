// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.WeakSubscription.MvxNotifyCollectionChangedEventSubscription
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Collections.Specialized;
using System.Reflection;

namespace Cirrious.CrossCore.WeakSubscription
{
  public class MvxNotifyCollectionChangedEventSubscription : 
    MvxWeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
  {
    private static readonly EventInfo EventInfo = ReflectionExtensions.GetEvent(typeof (INotifyCollectionChanged), "CollectionChanged");

    public static void LinkerPleaseInclude(INotifyCollectionChanged iNotifyCollectionChanged) => iNotifyCollectionChanged.CollectionChanged += (NotifyCollectionChangedEventHandler) ((sender, e) => { });

    public MvxNotifyCollectionChangedEventSubscription(
      INotifyCollectionChanged source,
      EventHandler<NotifyCollectionChangedEventArgs> targetEventHandler)
      : base(source, MvxNotifyCollectionChangedEventSubscription.EventInfo, targetEventHandler)
    {
    }

    protected override Delegate CreateEventHandler() => (Delegate) new NotifyCollectionChangedEventHandler(((MvxWeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>) this).OnSourceEvent);
  }
}
